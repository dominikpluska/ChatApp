using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.BlackListCommands
{
    public class BlackListCommands : IBlackListCommands
    {
        private readonly MongoDBService _mongoDbService;

        public BlackListCommands(MongoDBService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IResult> CreateBlackList(BlackList blackList)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(blackList);
                await _mongoDbService.BlackListsCollection.InsertOneAsync(blackList);
                return Results.Ok("Black List has been created");
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
