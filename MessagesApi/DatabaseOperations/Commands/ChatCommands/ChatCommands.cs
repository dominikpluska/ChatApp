using MessagesApi.Data;
using MessagesApi.Dto;
using MessagesApi.Models;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                ArgumentNullException.ThrowIfNull(chat);
                await _mongoDBService.ChatCollection.InsertOneAsync(chat);
                return chat.ChatId.ToString();
            }
            catch(ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResult> AcceptChatRequest(ObjectId chatId, string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId) &
                             Builders<BsonDocument>.Filter.Eq("ChatParticipants.UserId", userId);

                var chatParticipantsUpdate = Builders<BsonDocument>.Update.Set("ChatParticipants.$.IsAccepted", true);

                await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, chatParticipantsUpdate);


                return Results.Ok("Chat Updated");
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResult> Insert(Chat chat)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(chat);

                await _context.Chats.AddAsync(chat);
                await _context.SaveChangesAsync();
                return Results.Ok("Chat added");
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IResult> InsertNewMessage(ObjectId chatId, Message message)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(message);

                var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
                var insert = Builders<BsonDocument>.Update.Push("Messages", message);


                var result = await _mongoDBService.ChatCollectionBson.FindOneAndUpdateAsync(filter, insert);

                return Results.Ok(result);

            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResult> UpdateMessage(MessageUpdateDto messageUpdateDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(messageUpdateDto);

                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(messageUpdateDto.ChatId)) &
                             Builders<BsonDocument>.Filter.Eq("Messages._id", ObjectId.Parse(messageUpdateDto.MessageId));

                var update = Builders<BsonDocument>.Update.Set("Messages.$.TextMessage", messageUpdateDto.TextMessage);

                await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, update);

                return Results.Ok("Message updated!");

            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResult> DeleteMessage(ObjectId chatId, ObjectId messageId)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
                var messageFilter = Builders<BsonDocument>.Filter.Eq("_id", messageId);

                var delete = Builders<BsonDocument>.Update.PullFilter("Messages", messageFilter);

                await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, delete);

                return Results.Ok("Item delted!");
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResult> RemoveChatParticipant(ObjectId chatId, string userId)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
                var userFilter = Builders<BsonDocument>.Filter.Eq("UserId", userId);

                var delete = Builders<BsonDocument>.Update.PullFilter("ChatParticipants", userFilter);

                await _mongoDBService.ChatCollectionBson.UpdateOneAsync(filter, delete);

                return Results.Ok("Item delted!");
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResult> DropChat(ObjectId chatId)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", chatId);
                await _mongoDBService.ChatCollectionBson.DeleteOneAsync(filter);
                return Results.Ok("Chat has been dropped!");
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
