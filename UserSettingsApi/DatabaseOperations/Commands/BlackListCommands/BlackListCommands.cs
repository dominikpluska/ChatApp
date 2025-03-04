using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using UserSettingsApi.Data;
using UserSettingsApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UserSettingsApi.DatabaseOperations.Commands.BlackListCommands
{
    public class BlackListCommands : IBlackListCommands
    {
        private readonly MongoDBService _mongoDbService;

        public BlackListCommands(MongoDBService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IResult> CreateBlackList(BlackList blackList, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(blackList);
            await _mongoDbService.BlackListsCollection.InsertOneAsync(blackList, cancellationToken: cancellationToken);
            return Results.Ok("Black List has been created");
        }

        public async Task<IResult> AddToBlackList(ObjectId blackListId ,string userId, CancellationToken cancellationToken)
        {

           ArgumentNullException.ThrowIfNull(userId);

           var filter = Builders<BlackList>.Filter.Eq(x => x.BlackListId, blackListId);
           var update = Builders<BlackList>.Update.Push(x => x.BlockedAccounts, userId);

           var result = await _mongoDbService.BlackListsCollection.FindOneAndUpdateAsync(filter, update, cancellationToken: cancellationToken);

           return Results.Ok(result);

        }

        public async Task<IResult> RemoveFromBlackList(ObjectId blackListId, string userId, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(userId);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", blackListId);
            var delete = Builders<BsonDocument>.Update.Pull("BlockedAccounts", userId);

            var result = await _mongoDbService.BlackListsCollectionBson.UpdateOneAsync(filter, delete, cancellationToken: cancellationToken);
            return Results.Ok(result);

        }
    }
}
