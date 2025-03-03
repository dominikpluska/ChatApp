﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Buffers;
using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.BlackListRepository
{
    public class BlackListRepository : IBlackListRepository
    {
        private readonly MongoDBService _mongoDBService;

        public BlackListRepository(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<BlackList> GetBlackList(string userId, CancellationToken cancellationToken)
        {

            var filter = Builders<BlackList>.Filter.Eq(x => x.UserAccountId, userId);
            var chatsList = await _mongoDBService.BlackListsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return chatsList;

        }

        public async Task<ObjectId> GetBlackListId(string userId, CancellationToken cancellationToken)
        {

            var matchedItem = _mongoDBService.BlackListsCollection.AsQueryable()
                    .Where(x => x.UserAccountId == userId)
                    .Select(x => x.BlackListId);

            return await matchedItem.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task<string> GetBlockedUser(ObjectId blackListId, string blockedId, CancellationToken cancellationToken)
        {

           var matchedItem = _mongoDBService.BlackListsCollection.AsQueryable()
                                   .Where(x => x.BlackListId == blackListId && x.BlockedAccounts.Contains(blockedId))
                                   .Select(x => x.BlockedAccounts.FirstOrDefault(j => j == blockedId));

           var result = await matchedItem.FirstOrDefaultAsync(cancellationToken: cancellationToken);

           return result;
        }
    }
}
