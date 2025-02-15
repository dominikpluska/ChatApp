
using AutoMapper;
using UserSettingsApi.DatabaseOperations.Repository.FriendRequestsRepository;
using UserSettingsApi.Dto;
using UserSettingsApi.Services;
using UserSettingsApi.UserAccessor;

namespace UserSettingsApi.Managers.RequestsManager
{
    public class RequestManager : IRequestManager
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IRequestsRepository _requestsRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public RequestManager(IUserAccessor userAccessor, IRequestsRepository requestsRepository,
            IAuthenticationService authenticationService, IMapper mapper)
        {
            _userAccessor = userAccessor;
            _requestsRepository = requestsRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<IResult> GetAllRequests()
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

                var results = await _requestsRepository.GetAllRequests(userProperties.UserAccountId);

                var userRequestDto = new List<RequestDto>();

                _mapper.Map(results, userRequestDto);

                return Results.Ok(userRequestDto);
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
