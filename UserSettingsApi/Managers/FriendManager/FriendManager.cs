using MongoDB.Bson;
using UserSettingsApi.DatabaseOperations.Commands.FriendRequestCommands;
using UserSettingsApi.DatabaseOperations.Commands.FriendsLisiCommands;
using UserSettingsApi.DatabaseOperations.Repository.FriendRequestsRepository;
using UserSettingsApi.DatabaseOperations.Repository.FriendsListRepository;
using UserSettingsApi.Dto;
using UserSettingsApi.Models;
using UserSettingsApi.Services;
using UserSettingsApi.UserAccessor;

namespace UserSettingsApi.Managers.FriendsListsManager
{
    public class FriendManager : IFriendManager
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IFriendListCommands _friendListCommands;
        private readonly IFriendsListRepository _friendsListRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IFriendRequestCommands _friendRequestCommands;
        private readonly IRequestsRepository _friendRequestsRepository;

        public FriendManager(IUserAccessor userAccessor, IFriendListCommands friendListCommands, 
            IFriendsListRepository friendsListRepository, IAuthenticationService authenticationService,
            IFriendRequestCommands friendRequestCommands, IRequestsRepository friendRequestsRepository)
        {
            _userAccessor = userAccessor;
            _friendListCommands = friendListCommands;
            _friendsListRepository = friendsListRepository;
            _authenticationService = authenticationService;
            _friendRequestCommands = friendRequestCommands;
            _friendRequestsRepository = friendRequestsRepository;
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

        public async Task<IResult> SendFriendRequests(string friendId)
        {
            try 
            {
                ArgumentNullException.ThrowIfNull(friendId);

                var userId = _userAccessor.UserId;
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null)
                {
                    return Results.Problem("User does not exist!");
                }

                if (!userProperties.IsActive)
                {
                    return Results.Problem("User is inactive!");
                }

                if (friendId == userProperties.UserAccountId)
                {
                    return Results.Problem("You can't add yourself as a friend silly!");
                }

                var newFriendProperties = await _authenticationService.GetAccountProperties(friendId);

                if (newFriendProperties == null)
                {
                    return Results.Problem("User does not exist!");
                }

                if (!newFriendProperties.IsActive)
                {
                    return Results.Problem("User is inactive!");
                }

                //Check if user is already on the friend list

                Request friendRequest = new()
                {
                    RequestorId = userProperties.UserAccountId,
                    RequesteeId = newFriendProperties.UserAccountId

                };

                await _friendRequestCommands.InsertFriendRequests(friendRequest);

                return Results.Ok("Friend requests has been sent!");
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> AcceptFriendRequest(string requestId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(requestId);

                var requestIdParsed = ObjectId.Parse(requestId);

                var userId = _userAccessor.UserId;
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null)
                {
                    return Results.Problem("User does not exist!");
                }

                if (!userProperties.IsActive)
                {
                    return Results.Problem("User is inactive!");
                }

                var friendRequest = await _friendRequestsRepository.GetRequests(requestIdParsed);

                if(friendRequest.RequesteeId != userId)
                {
                    return Results.Problem("User Id does not match the one saved in the database!");
                }

                await _friendRequestCommands.AcceptFriendRequest(friendRequest.RequestId);


                var friendListRequestee = await _friendsListRepository.GetFriendsList(userId);
                await _friendListCommands.AddNewFriend(friendListRequestee.FriendsListId, friendRequest.RequestorId);

                var friendListRequestor = await _friendsListRepository.GetFriendsList(friendRequest.RequestorId);
                await _friendListCommands.AddNewFriend(friendListRequestor.FriendsListId, friendRequest.RequesteeId);

                return Results.Ok("Friend Request has been accepted!");
            }
            catch(ArgumentNullException ex)
            {
                return Results.Problem("Argument Null exception!", ex.Message);
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> CreateFriendsListTable(string userId)
        {
            try
            {
                FriendsList friendsList = new()
                {
                    UserAccountId = userId,
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
