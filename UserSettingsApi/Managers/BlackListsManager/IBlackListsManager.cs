using UserSettingsApi.Dto;

namespace UserSettingsApi.Managers.BlackListsManager
{
    public interface IBlackListsManager
    {
        public Task<IResult> CreateBlackListTable(UserSettingsDto userSettingsDto);
    }
}
