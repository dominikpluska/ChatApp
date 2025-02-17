using MongoDB.Bson;
using UserSettingsApi.DatabaseOperations.Commands.FriendRequestCommands;
using UserSettingsApi.DatabaseOperations.Commands.FriendsLisiCommands;
using UserSettingsApi.DatabaseOperations.Repository.BlackListRepository;
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
        private readonly IRequestCommands _friendRequestCommands;
        private readonly IRequestsRepository _requestsRepository;
        private readonly IBlackListRepository _blackListRepository;
        public FriendManager(IUserAccessor userAccessor, IFriendListCommands friendListCommands, 
            IFriendsListRepository friendsListRepository, IAuthenticationService authenticationService,
            IRequestCommands friendRequestCommands, IRequestsRepository requestsRepository, IBlackListRepository blackListRepository)
        {
            _userAccessor = userAccessor;
            _friendListCommands = friendListCommands;
            _friendsListRepository = friendsListRepository;
            _authenticationService = authenticationService;
            _friendRequestCommands = friendRequestCommands;
            _requestsRepository = requestsRepository;
            _blackListRepository = blackListRepository;
        }
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

                IdRequestsDto idRequestsDtos = new()
                {
                    Ids = result.Friends
                };

                var friendList = await _authenticationService.GetUserListByIds(idRequestsDtos);


                return Results.Ok(friendList);
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

                var blackListIdUser = await _blackListRepository.GetBlackListId(userProperties.UserAccountId);
                var isOnUserList = await _blackListRepository.GetBlockedUser(blackListIdUser, newFriendProperties.UserAccountId);

                if (isOnUserList != null)
                {
                    return Results.Problem("User is already on the black list! Remove them to continue the operation");
                }

                var blackListIdFriend = await _blackListRepository.GetBlackListId(newFriendProperties.UserAccountId);
                var isOnFriendList = await _blackListRepository.GetBlockedUser(blackListIdFriend, userProperties.UserAccountId);


                if (isOnFriendList != null)
                {
                    return Results.Problem("This user is currently blocking you! Operation aborted!");
                }

                var checkRequest = await _requestsRepository.GetRequest(userProperties.UserAccountId, newFriendProperties.UserAccountId);

                if (checkRequest != null)
                {
                    return Results.Problem("You have already sent a friend request to this user!");
                }

                var checkRequest2 = await _requestsRepository.GetRequest(newFriendProperties.UserAccountId, userProperties.UserAccountId);

                if (checkRequest2 != null)
                {
                    return Results.Problem("You already have a friend request pending from this user! Please check your requests list!");
                }

                Request friendRequest = new()
                {
                    RequestorId = userProperties.UserAccountId,
                    RequesteeId = newFriendProperties.UserAccountId

                };

                await _friendRequestCommands.InsertRequests(friendRequest);

                return Results.Ok("Friend request has been sent!");
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

                var friendRequest = await _requestsRepository.GetRequest(requestIdParsed);

                if(friendRequest.RequesteeId != userId)
                {
                    return Results.Problem("User Id does not match the one saved in the database!");
                }

                await _friendRequestCommands.AcceptRequest(friendRequest.RequestId);


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

        public async Task<IResult> RemoveFriend(string friendId)
        {
            try
            {
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

                var friendProperties = await _authenticationService.GetAccountProperties(friendId);

                if (friendProperties == null)
                {
                    return Results.Problem("User does not exist!");
                }

                var friendsListIdRequestor = await _friendsListRepository.GetFriendsListId(userProperties.UserAccountId);

                await _friendListCommands.RemoveFriend(friendsListIdRequestor, friendProperties.UserAccountId);

                var friendsListIdFriend = await _friendsListRepository.GetFriendsListId(friendProperties.UserAccountId);


                await _friendListCommands.RemoveFriend(friendsListIdFriend, userProperties.UserAccountId);

                return Results.Ok("Friend has been removed!");

            }
            catch(ArgumentNullException ex)
            {
                return Results.Problem("Argument null excetion", ex.Message);
            }
            catch (Exception ex)
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
