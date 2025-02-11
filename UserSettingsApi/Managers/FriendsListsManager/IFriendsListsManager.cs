using UserSettingsApi.Dto;

namespace UserSettingsApi.Managers.FriendsListsManager
{
    public interface IFriendsListsManager
    {
        public Task<IResult> CreateFriendsListTable(UserSettingsDto userSettingsDto);
        public Task<IResult> AddNewFriend(UserSettingsDto userSettingsDto);
        public Task<IResult> GetFriendsList();
    }
}
