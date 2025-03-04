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

        public async Task<IResult> CreateChatsTable(Chat chat , CancellationToken cancellationToken)
        {

           ArgumentNullException.ThrowIfNull(chat);
           await _mongoDbService.ChatsCollection.InsertOneAsync(chat, cancellationToken: cancellationToken);
           return Results.Ok("Chat has been created");

        }
    }
}
