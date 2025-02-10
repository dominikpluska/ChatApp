using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.ChatsCommands
{
    public class ChatsCommands : IChatsCommands
    {
        private readonly MongoDBService _mongoDbService;

        public ChatsCommands(MongoDBService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IResult> CreateChatsTable(Chat chat)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(chat);
                await _mongoDbService.ChatsCollection.InsertOneAsync(chat);
                return Results.Ok("Chat has been created");
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
