using UserSettingsApi.DatabaseOperations.Commands.FriendsLisiCommands;
using UserSettingsApi.DatabaseOperations.Repository.FriendsListRepository;
using UserSettingsApi.Dto;
using UserSettingsApi.Models;
using UserSettingsApi.Services;
using UserSettingsApi.UserAccessor;

namespace UserSettingsApi.Managers.FriendsListsManager
{
    public class FriendsListsManager : IFriendsListsManager
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IFriendListCommands _friendListCommands;
        private readonly IFriendsListRepository _friendsListRepository;
        private readonly IAuthenticationService _authenticationService;

        public FriendsListsManager(IUserAccessor userAccessor, IFriendListCommands friendListCommands, 
            IFriendsListRepository friendsListRepository, IAuthenticationService authenticationService)
        {
            _userAccessor = userAccessor;
            _friendListCommands = friendListCommands;
            _friendsListRepository = friendsListRepository;
            _authenticationService = authenticationService;
        }

        //Must be modified!
        public async Task<IResult> GetFriendsList()
        {
            try
            {
                var userId = _userAccessor.UserId;
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null)
                {
                    return Results.Problem("User does not exist!");
                }

                if(!userProperties.IsActive)
                {
                    return Results.Problem("User is inactive!");
                }

                var result = await _friendsListRepository.GetFriendsList(userProperties.UserAccountId);

                // transform results, reach out to the Auth SQL database and download usernames based on userIDs
                // If entity framework can't do it then use Dapper
                //From this point on, the rest of this function is only for the testing purposes

                List<FriendsListDto> friendsListDto = new();

                foreach(var friend in result.Friends)
                {
                    friendsListDto.Add(new FriendsListDto { UserId = friend});
                }

                return Results.Ok(friendsListDto);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        //Change the second part of the function. I must add another table which is going to keep track of friend  requests
        public async Task<IResult> AddNewFriend(UserSettingsDto userSettingsDto)
        {
            try
            {
                var userId = _userAccessor.UserId;
                //var userId = "529886c5-2396-4c5b-a711-0c1400286261";
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null)
                {
                    return Results.Problem("User does not exist!");
                }

                if (!userProperties.IsActive)
                {
                    return Results.Problem("User is inactive!");
                }

                if(userSettingsDto.UserId == userProperties.UserAccountId)
                {
                    return Results.Problem("You can't add yourself as a friend silly!");
                }

                var friendListId = await _friendsListRepository.GetFriendsListId(userProperties.UserAccountId);

                if(friendListId == null)
                {
                    return Results.Problem("User has no friends list!");
                }

                
                var newFriendProperties = await _authenticationService.GetAccountProperties(userSettingsDto.UserId);

                if (newFriendProperties == null)
                {
                    return Results.Problem("User does not exist!");
                }

                if (!newFriendProperties.IsActive)
                {
                    return Results.Problem("User is inactive!");
                }

                await _friendListCommands.AddNewFriend(friendListId, newFriendProperties.UserAccountId);

                return Results.Ok("Friend has been added!");    

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> CreateFriendsListTable(UserSettingsDto userSettingsDto)
        {
            try
            {
                FriendsList friendsList = new()
                {
                    UserAccountId = userSettingsDto.UserId,
                };

                await _friendListCommands.CreateFriendList(friendsList);
                return Results.Ok("Friends has been table created");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
