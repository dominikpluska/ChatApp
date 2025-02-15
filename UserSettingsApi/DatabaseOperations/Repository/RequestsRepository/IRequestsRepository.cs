using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.FriendRequestsRepository
{
    public interface IRequestsRepository
    {
        public Task<Request> GetRequests(ObjectId requestId);
        public Task<IEnumerable<Request>> GetAllRequests(string userId);
    }
}
