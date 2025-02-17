using UserSettingsApi.Dto;

namespace UserSettingsApi.Managers.FriendsListsManager
{
    public interface IFriendManager
    {
        public Task<IResult> CreateFriendsListTable(string userId);
        public Task<IResult> SendFriendRequests(string friendId);
        public Task<IResult> GetFriendsList();
        public Task<IResult> AcceptFriendRequest(string requestId);
        public Task<IResult> RejectFriendRequest(string requestId);
        public Task<IResult> RemoveFriend(string friendId);

    }
}
