using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using UserSettingsApi.DatabaseOperations.Commands.FriendRequestCommands;
using UserSettingsApi.DatabaseOperations.Commands.FriendsLisiCommands;
using UserSettingsApi.DatabaseOperations.Repository.BlackListRepository;
using UserSettingsApi.DatabaseOperations.Repository.FriendsListRepository;
using UserSettingsApi.DatabaseOperations.Repository.RequestsRepository;
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
        private readonly IHubContext<UserSettingsHub.UserSettingsHub> _hubContext;
        private readonly IMapper _mapper;
        public FriendManager(IUserAccessor userAccessor, IFriendListCommands friendListCommands, 
            IFriendsListRepository friendsListRepository, IAuthenticationService authenticationService,
            IRequestCommands friendRequestCommands, IRequestsRepository requestsRepository, IBlackListRepository blackListRepository,
            IHubContext<UserSettingsHub.UserSettingsHub> hubContext, IMapper mapper)
        {
            _userAccessor = userAccessor;
            _friendListCommands = friendListCommands;
            _friendsListRepository = friendsListRepository;
            _authenticationService = authenticationService;
            _friendRequestCommands = friendRequestCommands;
            _requestsRepository = requestsRepository;
            _blackListRepository = blackListRepository;
            _hubContext = hubContext;
            _mapper = mapper;
        }
        public async Task<IResult> GetFriendsList(CancellationToken cancellationToken)
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

                var result = await _friendsListRepository.GetFriendsList(userProperties.UserAccountId, cancellationToken);

                IdRequestsDto idRequestsDtos = new()
                {
                    Ids = result.Friends
                };

                var friendList = await _authenticationService.GetUserListByIds(idRequestsDtos);


                return Results.Ok(friendList);
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> SendFriendRequests(string friendId, CancellationToken cancellationToken)
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

                var blackListIdUser = await _blackListRepository.GetBlackListId(userProperties.UserAccountId, cancellationToken);
                var isOnUserList = await _blackListRepository.GetBlockedUser(blackListIdUser, newFriendProperties.UserAccountId, cancellationToken);

                if (isOnUserList != null)
                {
                    return Results.Problem("User is already on the black list! Remove them to continue the operation");
                }

                var blackListIdFriend = await _blackListRepository.GetBlackListId(newFriendProperties.UserAccountId, cancellationToken);
                var isOnFriendList = await _blackListRepository.GetBlockedUser(blackListIdFriend, userProperties.UserAccountId, cancellationToken);


                if (isOnFriendList != null)
                {
                    return Results.Problem("This user is currently blocking you! Operation aborted!");
                }

                var checkRequest = await _requestsRepository.GetRequest(userProperties.UserAccountId, newFriendProperties.UserAccountId, cancellationToken);

                if (checkRequest != null)
                {
                    return Results.Problem("You have already sent a friend request to this user!");
                }

                var checkRequest2 = await _requestsRepository.GetRequest(newFriendProperties.UserAccountId, userProperties.UserAccountId, cancellationToken);

                if (checkRequest2 != null)
                {
                    return Results.Problem("You already have a friend request pending from this user! Please check your requests list!");
                }

                Request friendRequest = new()
                {
                    RequestorId = userProperties.UserAccountId,
                    RequesteeId = newFriendProperties.UserAccountId

                };

                await _friendRequestCommands.InsertRequests(friendRequest, cancellationToken);


                var connectionId = UserSettingsHub.UserSettingsHub.IsConnected(newFriendProperties.UserAccountId);
                if (connectionId != null)
                {
                    RequestDto requestDto = new();
                    requestDto = _mapper.Map(friendRequest, requestDto);
                    requestDto.UserName = userProperties.UserName;

                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", $"Friend request has been sent by {userProperties.UserName}");
                    await _hubContext.Clients.Client(connectionId).SendAsync("OnRequestReceived", requestDto);
                }

                return Results.Ok($"Friend request has been sent to {newFriendProperties.UserName}");
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> AcceptFriendRequest(string requestId, CancellationToken cancellationToken)
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

                var friendRequest = await _requestsRepository.GetRequest(requestIdParsed, cancellationToken);

                var friendProperties = await _authenticationService.GetAccountProperties(friendRequest.RequestorId);

                if (friendRequest.RequesteeId != userId)
                {
                    return Results.Problem("User Id does not match the one saved in the database!");
                }

                await _friendRequestCommands.AcceptRequest(friendRequest.RequestId, cancellationToken);


                var friendListRequestee = await _friendsListRepository.GetFriendsList(userId, cancellationToken);
                await _friendListCommands.AddNewFriend(friendListRequestee.FriendsListId, friendRequest.RequestorId , cancellationToken);

                var friendListRequestor = await _friendsListRepository.GetFriendsList(friendRequest.RequestorId, cancellationToken);
                await _friendListCommands.AddNewFriend(friendListRequestor.FriendsListId, friendRequest.RequesteeId , cancellationToken);

                await _friendRequestCommands.DeleteRequest(requestIdParsed, cancellationToken);

                var connectionId = UserSettingsHub.UserSettingsHub.IsConnected(friendRequest.RequestorId);
                if (connectionId != null)
                {

                    UserAccountLightDto newFriendLightDto = new()
                    {
                        UserAccountId = userProperties.UserAccountId,
                        UserName = userProperties.UserName
                    };


                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", $"Friend request has been accepted by : {userProperties.UserName}");
                    await _hubContext.Clients.Client(connectionId).SendAsync("OnRequestRemoved", friendRequest);
                    await _hubContext.Clients.Client(connectionId).SendAsync("OnAddFriend", newFriendLightDto);
                }

                UserAccountLightDto userAccountLightDto = new()
                {
                    UserAccountId = friendRequest.RequestorId,
                    UserName = friendProperties.UserName
                };

                var userConnectionId = UserSettingsHub.UserSettingsHub.IsConnected(userProperties.UserAccountId);
                await _hubContext.Clients.Client(userConnectionId!).SendAsync("OnRequestRemoved", requestId);
                await _hubContext.Clients.Client(userConnectionId!).SendAsync("OnAddFriend", userAccountLightDto);

                return Results.Ok("Friend Request has been accepted");
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> RejectFriendRequest(string requestId, CancellationToken cancellationToken)
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

                var friendRequest = await _requestsRepository.GetRequest(requestIdParsed, cancellationToken);

                if(friendRequest == null)
                {
                    return Results.Problem("The request is null!");
                }

                await _friendRequestCommands.DeleteRequest(friendRequest.RequestId, cancellationToken);

                var requestorConnectionId = UserSettingsHub.UserSettingsHub.IsConnected(friendRequest.RequestorId);

                if(requestorConnectionId != null)
                {
                    await _hubContext.Clients.Client(requestorConnectionId).SendAsync("ReceiveNotification", $"{userProperties.UserName} has rejected your friend request!");
                }

                var userConnectionId = UserSettingsHub.UserSettingsHub.IsConnected(userProperties.UserAccountId);
                await _hubContext.Clients.Client(userConnectionId!).SendAsync("OnRequestRemoved", requestId);

                return Results.Ok("Friend request has been rejected!");
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> RemoveFriend(string friendId, CancellationToken cancellationToken)
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

                var friendsListIdRequestor = await _friendsListRepository.GetFriendsListId(userProperties.UserAccountId, cancellationToken);

                await _friendListCommands.RemoveFriend(friendsListIdRequestor, friendProperties.UserAccountId, cancellationToken);

                var friendsListIdFriend = await _friendsListRepository.GetFriendsListId(friendProperties.UserAccountId, cancellationToken);


                await _friendListCommands.RemoveFriend(friendsListIdFriend, userProperties.UserAccountId, cancellationToken);

                var userConnectionId = UserSettingsHub.UserSettingsHub.IsConnected(userProperties.UserAccountId);

                if(userConnectionId != null)
                {
                    await _hubContext.Clients.Client(userConnectionId).SendAsync("OnRemoveFriend", friendId);
                }

                var friendConnectionId = UserSettingsHub.UserSettingsHub.IsConnected(friendProperties.UserAccountId);

                if(friendConnectionId != null)
                {
                    await _hubContext.Clients.Client(friendConnectionId).SendAsync("OnRemoveFriend", friendId);
                }

                return Results.Ok("Friend has been removed!");

            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }

        }

        public async Task<IResult> CreateFriendsListTable(string userId, CancellationToken cancellationToken)
        {
            try
            {
                FriendsList friendsList = new()
                {
                    UserAccountId = userId,
                };

                await _friendListCommands.CreateFriendList(friendsList, cancellationToken);
                return Results.Ok("Friends has been table created");
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
