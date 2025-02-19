
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using UserSettingsApi.Services;
using UserSettingsApi.UserAccessor;

namespace UserSettingsApi.UserSettingsHub
{
    public class UserSettingsHub : Hub
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IAuthenticationService _authenticationService;
        private static readonly ConcurrentDictionary<string, string> _connections = new();

        public UserSettingsHub(IUserAccessor userAccessor, IAuthenticationService authenticationService)
        {
            _userAccessor = userAccessor;
            _authenticationService = authenticationService;
        }

        public override async Task OnConnectedAsync()
        {

            var userId = _userAccessor.UserId;
            var userProperties = await _authenticationService.GetAccountProperties(userId);

            if (userProperties == null)
            {
                await Clients.Caller.SendAsync("User does not exist!");
                return;
            }

            if (!userProperties!.IsActive)
            {
                await Clients.Caller.SendAsync("User is inactive!");
                return;
            }

            var context = Context.ConnectionId;

            if (_connections.ContainsKey(userId))
            {
                await Clients.Caller.SendAsync("Reconnected");
            }
            else
            {
                _connections.TryAdd(userId, context);
                await Clients.Caller.SendAsync("Connected to the setting's backed!");
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _userAccessor.UserId;

            if (_connections.ContainsKey(userId))
            {
                _connections.TryRemove(userId, out var userName);
            }

            await Clients.Caller.SendAsync("Connected to the setting's backed!");
        }

        //public async Task RequestOperation(string requestId)
        //{
        //    await Clients.Caller.SendAsync("onRequestOperation", requestId);
        //}

        public static string? IsConnected(string userId)
        {
            return _connections.TryGetValue(userId, out string connection) ? connection : null;
        }
    }
}
