
using AutoMapper;
using UserSettingsApi.DatabaseOperations.Repository.RequestsRepository;
using UserSettingsApi.Dto;
using UserSettingsApi.Models;
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

        public async Task<IResult> GetAllRequests(CancellationToken cancellationToken)
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

                var results = await _requestsRepository.GetAllRequests(userProperties.UserAccountId, cancellationToken);

                if (results.Count() <= 0)
                {
                    return Results.Ok(null);
                }

                IdRequestsDto idRequestsDtos = new()
                {
                    Ids = results.Select(x => x.RequestorId)
                };

                var userList = await _authenticationService.GetUserListByIds(idRequestsDtos);

                var userRequestDto = new List<RequestDto>();

                var mappedResults = _mapper.Map(results, userRequestDto);

                var mergedList = mappedResults.GroupJoin(userList, result => result.RequestorId, user => user.UserAccountId, (result, users) =>
                {
                    var matchingUser = users.FirstOrDefault();
                    result.UserName = matchingUser?.UserName!;
                    return result;
                }).ToList();

                return Results.Ok(mergedList);
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
