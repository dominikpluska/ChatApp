using Microsoft.AspNetCore.SignalR;
using UserSettingsApi.DatabaseOperations.Commands.BlackListCommands;
using UserSettingsApi.DatabaseOperations.Commands.FriendsLisiCommands;
using UserSettingsApi.DatabaseOperations.Repository.BlackListRepository;
using UserSettingsApi.DatabaseOperations.Repository.FriendsListRepository;
using UserSettingsApi.Dto;
using UserSettingsApi.Models;
using UserSettingsApi.Services;
using UserSettingsApi.UserAccessor;

namespace UserSettingsApi.Managers.BlackListsManager
{
    public class BlackListsManager : IBlackListsManager
    {
        private readonly IBlackListCommands _blackListCommands;
        private readonly IBlackListRepository _blackListRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IFriendListCommands _friendListCommands;
        private readonly IFriendsListRepository _friendsListRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IHubContext<UserSettingsHub.UserSettingsHub> _hubContext;

        public BlackListsManager(IUserAccessor userAccessor, IBlackListCommands blackListCommands,
            IBlackListRepository blackListRepository, IAuthenticationService authenticationService,
            IFriendListCommands friendListCommands, IFriendsListRepository friendsListRepository,
            IHubContext<UserSettingsHub.UserSettingsHub> hubContext)
        {
            _userAccessor = userAccessor;
            _blackListRepository = blackListRepository;
            _blackListCommands = blackListCommands;
            _authenticationService = authenticationService;
            _friendListCommands = friendListCommands;
            _friendsListRepository = friendsListRepository;
            _hubContext = hubContext;
        }

        public async Task<IResult> CreateBlackListTable(string userId)
        {
            try
            {
                BlackList blackList = new()
                {
                    UserAccountId = userId
                };
                await _blackListCommands.CreateBlackList(blackList);

                return Results.Ok("Black list created!");
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> AddUserToBlackList(string blockedUserId)
        {

            try
            {
                ArgumentNullException.ThrowIfNull(blockedUserId);

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

                if(userProperties.UserAccountId == blockedUserId)
                {
                    return Results.Problem("You may not block yourself silly!");
                }

                var newBlockedUser = await _authenticationService.GetAccountProperties(blockedUserId);

                if (newBlockedUser == null)
                {
                    return Results.Problem("User does not exist!");
                }

                if (!newBlockedUser.IsActive)
                {
                    return Results.Problem("User is inactive!");
                }

                var blackListId = await _blackListRepository.GetBlackListId(userProperties.UserAccountId);
                var isOnList = await _blackListRepository.GetBlockedUser(blackListId, blockedUserId);

                if(isOnList != null)
                {
                    return Results.Problem("User is already on the black list!");
                }

                await _blackListCommands.AddToBlackList(blackListId, blockedUserId);

                var userFriendsListId = await _friendsListRepository.GetFriendsListId(userProperties.UserAccountId);

                await _friendListCommands.RemoveFriend(userFriendsListId, blockedUserId);

                var blockedFriendsListId = await _friendsListRepository.GetFriendsListId(blockedUserId);

                await _friendListCommands.RemoveFriend(blockedFriendsListId, userProperties.UserAccountId);

                var userConnectionId = UserSettingsHub.UserSettingsHub.IsConnected(userProperties.UserAccountId);
                if (userConnectionId != null)
                {
                    UserAccountLightDto userAccountLightDto = new()
                    {
                        UserAccountId = newBlockedUser.UserAccountId,
                        UserName = newBlockedUser.UserName
                    };
                    await _hubContext.Clients.Client(userConnectionId).SendAsync("OnBlockUser", userAccountLightDto);
                    await _hubContext.Clients.Client(userConnectionId).SendAsync("OnRemoveFriend", newBlockedUser.UserAccountId);
                }

                var blockedUserConnectionId = UserSettingsHub.UserSettingsHub.IsConnected(newBlockedUser.UserAccountId);

                if(blockedUserConnectionId != null)
                {
                    await _hubContext.Clients.Client(blockedUserConnectionId).SendAsync("OnRemoveFriend", userProperties.UserAccountId);
                }

                return Results.Ok("User has been blocked!");

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

        public async Task<IResult> GetUserFromBlackList(string userId, string chatterId)
        {
            try
            {

                var blackListId = await _blackListRepository.GetBlackListId(chatterId);

                if(blackListId == null)
                {
                    return Results.Problem("Black list is empty! Fix this issue!");
                }

                var checkIfBlocked = await _blackListRepository.GetBlockedUser(blackListId, userId);

                return Results.Ok(checkIfBlocked);
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

        public async Task<IResult> RemoveFromBlackList(string blockedUserId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(blockedUserId);

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

                var newBlockedUser = await _authenticationService.GetAccountProperties(blockedUserId);

                if (newBlockedUser == null)
                {
                    return Results.Problem("User does not exist!");
                }

                var blackListId = await _blackListRepository.GetBlackListId(userProperties.UserAccountId);

                await _blackListCommands.RemoveFromBlackList(blackListId, blockedUserId);


                var userConnectionId = UserSettingsHub.UserSettingsHub.IsConnected(userProperties.UserAccountId);
                if (userConnectionId != null)
                {
                    await _hubContext.Clients.Client(userConnectionId).SendAsync("OnRemoveBlockedUser", blockedUserId);
                }

                return Results.Ok("User has been removed from the blacklist!");

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

        public async Task<IResult> GetBlackList()
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

                var results = await _blackListRepository.GetBlackList(userProperties.UserAccountId);

                IdRequestsDto idRequestsDtos = new()
                {
                    Ids = results.BlockedAccounts
                };

                var blockedList = await _authenticationService.GetUserListByIds(idRequestsDtos);

                return Results.Ok(blockedList);
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
