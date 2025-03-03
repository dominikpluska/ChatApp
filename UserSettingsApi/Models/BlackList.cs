﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserSettingsApi.Models
{
    public class BlackList
    {
        [BsonId]
        public ObjectId BlackListId { get; set; }
        public required string UserAccountId { get; set; }
        public List<string> BlockedAccounts { get; set; } = new List<string>();
    }
}
