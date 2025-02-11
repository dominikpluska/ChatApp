using UserSettingsApi.DatabaseOperations.Commands.BlackListCommands;
using UserSettingsApi.Dto;
using UserSettingsApi.Models;
using UserSettingsApi.UserAccessor;

namespace UserSettingsApi.Managers.BlackListsManager
{
    public class BlackListsManager : IBlackListsManager
    {
        private readonly IBlackListCommands _blackListCommands;
        private readonly IUserAccessor _userAccessor;

        public BlackListsManager(IUserAccessor userAccessor, IBlackListCommands blackListCommands)
        {
            _userAccessor = userAccessor;
            _blackListCommands = blackListCommands;
        }

        public async Task<IResult> CreateBlackListTable(UserSettingsDto userSettingsDto)
        {
            try
            {
                BlackList blackList = new()
                {
                    UserAccountId = userSettingsDto.UserId
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
    }
}
