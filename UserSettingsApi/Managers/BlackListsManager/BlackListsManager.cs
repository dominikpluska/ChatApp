using UserSettingsApi.DatabaseOperations.Commands.BlackListCommands;
using UserSettingsApi.DatabaseOperations.Repository.BlackListRepository;
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
        private readonly IUserAccessor _userAccessor;

        public BlackListsManager(IUserAccessor userAccessor, IBlackListCommands blackListCommands,
            IBlackListRepository blackListRepository, IAuthenticationService authenticationService)
        {
            _userAccessor = userAccessor;
            _blackListRepository = blackListRepository;
            _blackListCommands = blackListCommands;
            _authenticationService = authenticationService;
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

                var results = _blackListRepository.GetBlackList(userProperties.UserAccountId);
                return Results.Ok(results.Result);
            }

            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
