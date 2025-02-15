namespace UserSettingsApi.Managers.RequestsManager
{
    public interface IRequestManager
    {
        public  Task<IResult> GetAllRequests();
    }
}
