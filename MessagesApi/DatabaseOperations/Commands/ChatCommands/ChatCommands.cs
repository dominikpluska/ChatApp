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

        public async Task<string> CreateChat(Chat chat, CancellationToken cancellationToken)
        {
           ArgumentNullException.ThrowIfNull(chat);
           await _mongoDBService.ChatCollection.InsertOneAsync(chat, cancellationToken :cancellationToken);
           return chat.ChatId.ToString();
        }

        public async Task<IResult> AcceptChatRequest(ObjectId chatId, string userId, CancellationToken cancellationToken)
        {

                ArgumentNullException.ThrowIfNull(userId);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId) &
                             Builders<BsonDocument>.Filter.Eq("ChatParticipants.UserId", userId);

                var chatParticipantsUpdate = Builders<BsonDocument>.Update.Set("ChatParticipants.$.IsAccepted", true);

                await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, chatParticipantsUpdate, cancellationToken: cancellationToken);


                return Results.Ok("Chat Updated");
        }

        public async Task<IResult> Insert(Chat chat, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(chat);

            await _context.Chats.AddAsync(chat, cancellationToken: cancellationToken);
            await _context.SaveChangesAsync();
            return Results.Ok("Chat added");
        }

        public async Task<IResult> InsertNewMessage(ObjectId chatId, Message message, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(message);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
            var insert = Builders<BsonDocument>.Update.Push("Messages", message);


            var result = await _mongoDBService.ChatCollectionBson.FindOneAndUpdateAsync(filter, insert, cancellationToken: cancellationToken);

            return Results.Ok(result);
        }

        public async Task<IResult> UpdateMessage(MessageUpdateDto messageUpdateDto, CancellationToken cancellationToken)
        {
             ArgumentNullException.ThrowIfNull(messageUpdateDto);

             var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(messageUpdateDto.ChatId)) &
                          Builders<BsonDocument>.Filter.Eq("Messages._id", ObjectId.Parse(messageUpdateDto.MessageId));

             var update = Builders<BsonDocument>.Update.Set("Messages.$.TextMessage", messageUpdateDto.TextMessage);

             await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

             return Results.Ok("Message updated!");
        }

        public async Task<IResult> DeleteMessage(ObjectId chatId, ObjectId messageId, CancellationToken cancellationToken)
        {

            var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
            var messageFilter = Builders<BsonDocument>.Filter.Eq("_id", messageId);

            var delete = Builders<BsonDocument>.Update.PullFilter("Messages", messageFilter);

            await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, delete, cancellationToken: cancellationToken);

            return Results.Ok("Item delted!");
        }

        public async Task<IResult> RemoveChatParticipant(ObjectId chatId, string userId, CancellationToken cancellationToken)
        {

           var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
           var userFilter = Builders<BsonDocument>.Filter.Eq("UserId", userId);

           var delete = Builders<BsonDocument>.Update.PullFilter("ChatParticipants", userFilter);

           await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, delete, cancellationToken: cancellationToken);

           return Results.Ok("Item delted!");

        }

        public async Task<IResult> DropChat(ObjectId chatId, CancellationToken cancellationToken)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
            await _mongoDBService.ChatCollectionBson.DeleteOneAsync(filter, cancellationToken: cancellationToken);
            return Results.Ok("Chat has been dropped!");
        }
    }
}
