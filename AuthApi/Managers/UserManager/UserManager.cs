using AuthApi.Cookies;
using AuthApi.DataBaseOperations.Commands.UserAccountsCommands;
using AuthApi.DataBaseOperations.Repository.RoleRepository;
using AuthApi.DataBaseOperations.Repository.UserAccountsRepository;
using AuthApi.Dto;
using AuthApi.Helper;
using AuthApi.Jwt;
using AuthApi.Models;
using AuthApi.Services;
using AuthApi.UserAccessor;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AuthApi.Managers.UserManager
{
    public class UserManager : IUserManager
    {
        private readonly IUserAccountsRepository _userAccountsRepository; 
        private readonly IUserAccountsCommands _userAccountsCommands;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwt _jwt;
        private readonly ICookieGenerator _cookieGenerator;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly IUserSettingsService _userSettingsService;
        private readonly int _passwordLength = 8;
        private readonly int _pagination = 20;

        public UserManager(IUserAccountsRepository userAccountsRepository, IUserAccountsCommands userAccountsCommands, IJwt jwt, ICookieGenerator cookieGenerator,
            IRoleRepository roleRepository, IUserAccessor userAccessor, IMapper mapper, IUserSettingsService userSettingsService)
        {
            _userAccountsRepository = userAccountsRepository;
            _userAccountsCommands = userAccountsCommands;
            _roleRepository = roleRepository;
            _jwt = jwt;
            _cookieGenerator = cookieGenerator;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _userSettingsService = userSettingsService;
        }

        public async Task<IResult> RegisterNewUser(UserRegistrationDto userDto)
        {
            try
            {
                var checkIfUserNameExists = await _userAccountsRepository.GetUserByName(userDto.UserName);
                if (checkIfUserNameExists != null)
                {
                    return Results.Problem("Username already exists! Choose a different user name!");
                }

                var checkIfEmailExists = await _userAccountsRepository.GetUserByEmail(userDto.Email);

                if (checkIfEmailExists != null)
                {
                    return Results.Problem("An account to which provided email is bound already exists! Please choose a different one or contact support for more details!");
                }

                if (!userDto.UserName.UserNameChecker())
                {
                    return Results.Problem("Username must not contain any white spaces nor any special characters!");
                }

                if (userDto.Password.Length < _passwordLength)
                {
                    return Results.Problem($"Password must contain minimum of {_passwordLength} characters!");
                }
                if (!userDto.Password.PasswordChecker())
                {
                    return Results.Problem($"Password must contain low, upper character as well as a number and a special character such as ! or *");
                }
                if (!userDto.Email.EmailChecker())
                {
                    return Results.Problem("Email is not valid!");
                }

                var password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                var defaultRole = await _roleRepository.GetRoleByName("User");

                UserRegistration userRegistration = new()
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    PasswordHash = password,
                    RoleId = defaultRole.RoleId,
                    ProfilePicturePath = "",
                };

                var userAccountId = await _userAccountsCommands.AddNewUser(userRegistration);

                //var userFromDb = await _userAccountsRepository.GetUserByName(userDto.UserName);

                //If any error occurs in the line below, drop username from db

                await _userSettingsService.CreateUserSettings(userAccountId);

                return Results.Ok("User has been registered!");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }

        }

        public async Task<IResult> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var userFromDb = await _userAccountsRepository.GetUserByName(userLoginDto.UserName);

                if (userFromDb == null)
                {
                    return Results.Problem("Login or password were incorrect!");
                }

                else
                {
                    if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, userFromDb!.PasswordHash))
                    {
                        return Results.NotFound("Login or password were incorrect!");
                    }
                    else
                    {
                        var cookie = _cookieGenerator.GenerateCookie(DateTime.Now.AddHours(2));
                        _userAccessor.SetCookie("ChatApp", _jwt.GenerateToken(userFromDb.UserName, userFromDb.RoleId), cookie);
                        _userAccessor.SetCookie("ChatAppUserId", userFromDb.UserAccountId, cookie);
                        return Results.Ok("Login successful");
                    }
                }
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex.Message}");
            }
        }

        public async Task<IResult> UpdatePassword(UserPasswordChangeDto userPasswordChangeDto)
        {
            try
            {
                var userFromDb = await _userAccountsRepository.GetUser(userPasswordChangeDto.UserAccountId);

                if (userFromDb == null)
                {
                    return Results.Problem("UserId doesn't exist!");
                }

                if (!BCrypt.Net.BCrypt.Verify(userPasswordChangeDto.OldPassword, userFromDb!.PasswordHash))
                {
                    return Results.NotFound("The old password was invalid!");
                }

                if (userPasswordChangeDto.NewPasswordConfirmed.Length < _passwordLength)
                {
                    return Results.Problem($"Password must contain minimum of {_passwordLength} characters!");
                }
                if (!userPasswordChangeDto.NewPasswordConfirmed.PasswordChecker())
                {
                    return Results.Problem($"Password must contain low, upper character as well as a number and a special character such as ! or *");
                }

                var password = BCrypt.Net.BCrypt.HashPassword(userPasswordChangeDto.NewPasswordConfirmed);
                UserPasswordChange userPasswordChange = new()
                {
                    UserAccountId = userPasswordChangeDto.UserAccountId,
                    PasswordHash = password
                };

                await _userAccountsCommands.UpdatePassword(userPasswordChange);

                return Results.Ok("Password has been updated!");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message); 
            }
        }

        public async Task<IResult> UpdateAccount(UserAccountUpdateDto userDto)
        {
            try
            {
                var userFromDb = await _userAccountsRepository.GetUser(userDto.UserAccountId);
                var checkIfUserNameExists = await _userAccountsRepository.GetUserByName(userDto.UserName);

                if (userFromDb != null)
                {
                    return Results.Problem("Invalid request! User does not exist");
                }

                if (checkIfUserNameExists.UserName == userDto.UserName && checkIfUserNameExists.UserAccountId != userDto.UserAccountId)
                {
                    return Results.Problem("Username already exists!");
                }
                if (checkIfUserNameExists.Email == userDto.Email && checkIfUserNameExists.UserAccountId != userDto.UserAccountId)
                {
                    return Results.Problem("An account to which provided email is bound already exists! Please choose a different one or contact support for more details!");
                }
                if (!userDto.UserName.UserNameChecker())
                {
                    return Results.Problem("Username must not contain any white spaces nor any special characters!");
                }
                if (!userDto.Email.UserNameChecker())
                {
                    return Results.Problem("Email is not valid!");
                }

                UserAccountDto userAccountDto = new()
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    PicturePath = "",

                };

                await _userAccountsCommands.UpdateUser(userAccountDto);

                return Results.Ok("Account has been updated!");
            }
            catch (Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> CheckAuthentication()
        {
            try
            {
                var token = _userAccessor.TokenString;
                var userName = _userAccessor.UserName;
                var userId =_userAccessor.UserId;
                var user = await _userAccountsRepository.GetUser(userId);

                if (token != null && user != null)
                {
                    return Results.Ok(new { message = "Authenticated", user = user.UserName, userId = user.UserAccountId });
                }
                else
                {
                    return Results.Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex.Message}");
            }
        }

        public async Task<IResult> GetAccountProperties(string userId)
        {
            try
            {
                var userFromDb = await _userAccountsRepository.GetUser(userId);

                if(userFromDb == null)
                {
                    return Results.Problem("User not found!");
                }
                var roleFromDb = await _roleRepository.GetRoleById(userFromDb.RoleId);

                if(roleFromDb == null)
                {
                    return Results.Problem("Role not found!");
                }

                UserAccountDto account = new UserAccountDto();
                var userAccount = _mapper.Map(userFromDb, account);

                if (roleFromDb.RoleName != "Admin")
                {
                    if(userId != userFromDb.UserAccountId)
                    {
                        return Results.Unauthorized();
                    }

                    else
                    {
                        return Results.Ok(userAccount);
                    }
                }
                else
                {
                    return Results.Ok(userAccount);
                }
            }
            catch(ArgumentNullException ex)
            {
                return Results.Problem("User Id must not be null!", ex.Message);
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> LogOut()
        {
            try
            {
                await Task.Run(() => 
                {
                    var cookieOptions = _cookieGenerator.GenerateCookie(DateTime.Now.AddDays(-1));
                    _userAccessor.SetCookie("ChatApp", "", cookieOptions);
                    _userAccessor.SetCookie("ChatAppUserId", "", cookieOptions);
                    _userAccessor.SetCookie("ChatAppUserName", "", cookieOptions);
                    
                });
                return Results.Ok("Logged out!");
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> GetActiveUserList(int itemsToSkip = 0)
        {
            try
            {
                var activeUsersCount = await _userAccountsRepository.GetActiveUsersCount() / _pagination;

                var result = await _userAccountsRepository.GetTopActiveUsersOrderedAlphabetically(itemsToSkip * _pagination);
                List<UserAccountDto> userAccountDtos = new();
                var userList = _mapper.Map(result, userAccountDtos);

                return Results.Ok(new { userList, activeUsersCount});
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        //Implement pagination here as well
        public async Task<IResult> SearchForUser(string userName)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userName);

                userName = userName.Trim();

                var resutls = await _userAccountsRepository.Search(userName);
                var activeUsersCount = resutls.Count() / _pagination;

                List<UserAccountDto> userAccountDtos = new();
                var userList = _mapper.Map(resutls, userAccountDtos);

                return Results.Ok(userList);

            }
            catch(ArgumentNullException ex)
            {
                return Results.Problem(ex.Message);
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
