using UserSettingsApi.Dto;

namespace UserSettingsApi.Managers.FriendsListsManager
{
    public interface IFriendManager
    {
        public Task<IResult> CreateFriendsListTable(string userId, CancellationToken cancellationToken);
        public Task<IResult> SendFriendRequests(string friendId, CancellationToken cancellationToken);
        public Task<IResult> GetFriendsList(CancellationToken cancellationToken);
        public Task<IResult> AcceptFriendRequest(string requestId, CancellationToken cancellationToken);
        public Task<IResult> RejectFriendRequest(string requestId, CancellationToken cancellationToken);
        public Task<IResult> RemoveFriend(string friendId, CancellationToken cancellationToken);

    }
}
