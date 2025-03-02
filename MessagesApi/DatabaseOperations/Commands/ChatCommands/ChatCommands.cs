using MessagesApi.Data;
using MessagesApi.Dto;
using MessagesApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MessagesApi.DatabaseOperations.Commands.ChatCommands
{
    public class ChatCommands : IChatCommands
    {
        private readonly ApplicationDbContext _context;
        private readonly MongoDBService _mongoDBService;

        public ChatCommands(ApplicationDbContext context, MongoDBService mongoDBService)
        {
            _context = context;
            _mongoDBService = mongoDBService;
        }

        public async Task<string> CreateChat(Chat chat)
        {
           ArgumentNullException.ThrowIfNull(chat);
           await _mongoDBService.ChatCollection.InsertOneAsync(chat);
           return chat.ChatId.ToString();
        }

        public async Task<IResult> AcceptChatRequest(ObjectId chatId, string userId)
        {

                ArgumentNullException.ThrowIfNull(userId);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId) &
                             Builders<BsonDocument>.Filter.Eq("ChatParticipants.UserId", userId);

                var chatParticipantsUpdate = Builders<BsonDocument>.Update.Set("ChatParticipants.$.IsAccepted", true);

                await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, chatParticipantsUpdate);


                return Results.Ok("Chat Updated");
        }

        public async Task<IResult> Insert(Chat chat)
        {
            ArgumentNullException.ThrowIfNull(chat);

            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
            return Results.Ok("Chat added");
        }

        public async Task<IResult> InsertNewMessage(ObjectId chatId, Message message)
        {
            ArgumentNullException.ThrowIfNull(message);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
            var insert = Builders<BsonDocument>.Update.Push("Messages", message);


            var result = await _mongoDBService.ChatCollectionBson.FindOneAndUpdateAsync(filter, insert);

            return Results.Ok(result);
        }

        public async Task<IResult> UpdateMessage(MessageUpdateDto messageUpdateDto)
        {
             ArgumentNullException.ThrowIfNull(messageUpdateDto);

             var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(messageUpdateDto.ChatId)) &
                          Builders<BsonDocument>.Filter.Eq("Messages._id", ObjectId.Parse(messageUpdateDto.MessageId));

             var update = Builders<BsonDocument>.Update.Set("Messages.$.TextMessage", messageUpdateDto.TextMessage);

             await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, update);

             return Results.Ok("Message updated!");
        }

        public async Task<IResult> DeleteMessage(ObjectId chatId, ObjectId messageId)
        {

            var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
            var messageFilter = Builders<BsonDocument>.Filter.Eq("_id", messageId);

            var delete = Builders<BsonDocument>.Update.PullFilter("Messages", messageFilter);

            await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, delete);

            return Results.Ok("Item delted!");
        }

        public async Task<IResult> RemoveChatParticipant(ObjectId chatId, string userId)
        {

           var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
           var userFilter = Builders<BsonDocument>.Filter.Eq("UserId", userId);

           var delete = Builders<BsonDocument>.Update.PullFilter("ChatParticipants", userFilter);

           await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, delete);

           return Results.Ok("Item delted!");

        }

        public async Task<IResult> DropChat(ObjectId chatId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
            await _mongoDBService.ChatCollectionBson.DeleteOneAsync(filter);
            return Results.Ok("Chat has been dropped!");
        }
    }
}
