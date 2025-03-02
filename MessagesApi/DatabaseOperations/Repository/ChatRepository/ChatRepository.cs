using AutoMapper;
using AutoMapper.QueryableExtensions;
using MessagesApi.Data;
using MessagesApi.Dto;
using MessagesApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Text.Json;

namespace MessagesApi.DatabaseOperations.Repository.ChatRepository
{
    public class ChatRepository : IChatRepository
    {
        private readonly MongoDBService _mongoDBService;

        public ChatRepository(MongoDBService mongoDBService)
        {

            _mongoDBService = mongoDBService;
        }

        public async Task<ObjectId> CheckChat(ObjectId chatId)
        {

            var filter = Builders<Chat>.Filter.Eq(x => x.ChatId, chatId);
            var projection = Builders<Chat>.Projection
                .Include(x => x.ChatId)
                .Exclude(x => x.Messages)
                .Exclude(x => x.ChatParticipants);

            var result = await _mongoDBService.ChatCollection.Find(filter).Project(projection).FirstOrDefaultAsync();

            var selectedId = result.GetValue("_id");
            var deserializedResult = BsonSerializer.Deserialize<ObjectId>(selectedId.ToJson());

            return deserializedResult;

        }

        public async Task<string> FindChat(string userId, string chatterId)
        {

            ArgumentNullException.ThrowIfNull(userId, chatterId);

            var filter = Builders<Chat>.Filter.All(x => x.ChatParticipants, new[] {userId, chatterId});
            var projection = Builders<Chat>.Projection
                .Exclude(x => x.ChatParticipants)
                .Exclude(x => x.Messages);

            var document = await _mongoDBService.ChatCollection.Find(filter).Project(projection).FirstOrDefaultAsync();

            if(document == null)
            {
                return null!;
            }

            var selectedId = document.GetValue("_id");
            var deserializedResult = BsonSerializer.Deserialize<ObjectId>(selectedId.ToJson());

            return deserializedResult.ToString();

        }

        public async Task<IEnumerable<string>> GetChatParticipants(ObjectId chatId)
        {

            var filter = Builders<Chat>.Filter.Eq(x => x.ChatId, chatId);
            var projection = Builders<Chat>.Projection
                .Exclude(x => x.Messages)
                .Exclude(x => x.ChatId);


            var results = await _mongoDBService.ChatCollection.Find(filter).Project(projection).FirstOrDefaultAsync();

            var chatParticipantsBson = results.GetValue("ChatParticipants");
            var deserializedResults = BsonSerializer.Deserialize<IEnumerable<string>>(chatParticipantsBson.ToJson());

            return deserializedResults;

        } 

        //Limit about of messages in the future updates and work on the frontend to implement that feature
        public async Task<IEnumerable<Message>> GetChatMessages(ObjectId chatId)
        {

            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("_id", chatId)),
                new BsonDocument("$unwind", "$Messages"),
                new BsonDocument("$sort", new BsonDocument("Messages.PostedDate", -1)),
                //new BsonDocument("$limit", 100),
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 0 },    
                    { "Messages", 1 } 
                })
            };

            var results = await _mongoDBService.ChatCollectionBson.Aggregate<BsonDocument>(pipeline).ToListAsync();

            var deserializedResults = results
                .Select(result => BsonSerializer.Deserialize<Message>(result["Messages"].AsBsonDocument)) 
                .ToList();

            return deserializedResults!;
        }

        public async Task<Message> GetChatMessage(ObjectId chatId, ObjectId messageId)
        {

           var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
                        //Builders<BsonDocument>.Filter.Eq("Messages._id", messageId);

           var projection = Builders<BsonDocument>.Projection
                           .ElemMatch<BsonDocument>("Messages", Builders<BsonDocument>.Filter.Eq("Messages._id", messageId));

           var results = await _mongoDBService.ChatCollectionBson.Find(filter).FirstOrDefaultAsync();
           var messagesBson = results.GetValue("Messages").AsBsonArray;

           var deserializedResults = messagesBson.Select(x => BsonSerializer.Deserialize<Message>(x.AsBsonDocument)).FirstOrDefault();

           return deserializedResults!;

        }

    }
}
