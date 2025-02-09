using AuthApi.Data;
using AuthApi.DataBaseOperations.Commands.RoleCommands;
using AuthApi.DataBaseOperations.Commands.UserAccountsCommands;
using AuthApi.DataBaseOperations.Repository.RoleRepository;
using AuthApi.DataBaseOperations.Repository.UserAccountsRepository;
using AuthApi.Dto;
using AuthApi.Helper;
using AuthApi.Models;

namespace AuthApi.Managers.AdminManager
{
    public class AdminManager : IAdminManager
    {
        private readonly IUserAccountsCommands _userAccountsCommands;
        private readonly IUserAccountsRepository _userAccountsRepository;
        private readonly IRoleCommands _roleCommands;
        private readonly IRoleRepository _roleRepository;
        private readonly int _passwordLength = 8;

        public AdminManager(IUserAccountsCommands userAccountsCommands, IUserAccountsRepository userAccountsRepository, IRoleCommands roleCommands, IRoleRepository roleRepository)
        {
            _userAccountsCommands = userAccountsCommands;
            _userAccountsRepository = userAccountsRepository;
            _roleCommands = roleCommands;
            _roleRepository = roleRepository;
        }

        public async Task<IResult> RegisterNewUser(UserAdminRegistrationDto userDto)
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
                var role = await _roleRepository.GetRoleByName(userDto.Role);

                UserRegistration userRegistration = new()
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    PasswordHash = password,
                    RoleId = role.RoleId,
                    ProfilePicturePath = "",
                };

                await _userAccountsCommands.AddNewUser(userRegistration);

                return Results.Ok("User has been added!");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> ChangeUserPassword(UserAdminPasswordChangeDto userPasswordChangeDto)
        {
            try
            {
                var userFromDb = await _userAccountsRepository.GetUser(userPasswordChangeDto.UserAccountId);
                if (userFromDb == null)
                {
                    return Results.Problem("UserId doesn't exist!");
                }

                if (userPasswordChangeDto.Password.Length < _passwordLength)
                {
                    return Results.Problem($"Password must contain minimum of {_passwordLength} characters!");
                }
                if (!userPasswordChangeDto.Password.PasswordChecker())
                {
                    return Results.Problem($"Password must contain low, upper character as well as a number and a special character such as ! or *");
                }

                var password = BCrypt.Net.BCrypt.HashPassword(userPasswordChangeDto.Password);
                UserPasswordChange userPasswordChange = new()
                {
                    UserAccountId = userPasswordChangeDto.UserAccountId,
                    PasswordHash = password
                };

                await _userAccountsCommands.UpdatePassword(userPasswordChange);

                return Results.Ok("Password has been updated!");
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> UpdateUserAccount(UserAdminAccountUpdateDto userDto)
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
                    IsActive = userDto.IsActive,
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

        public async Task<IResult> DeleteUser(string userId)
        {
            try
            {
                await _userAccountsCommands.DeleteUser(userId);
                return Results.Ok("User has been deleted");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> GetAllUsers()
        {
            try
            {
                var resutls = await _userAccountsRepository.GetAllUsers();
                return Results.Ok(resutls);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> GetUser(string userId)
        {
            try
            {
                var result = await _userAccountsRepository.GetUser(userId);
                return Results.Ok(result);
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> AddNewRole(string roleName)
        {
            try
            {
                var roleFromDb = _roleRepository.GetRoleById(roleName);

                if(roleFromDb != null)
                {
                    return Results.Problem("Role name already exist!");
                }

                await _roleCommands.AddNewRole(roleName);
                return Results.Ok("Role has been added!");
            }
            catch(Exception ex) 
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> UpdateRole(RoleDto roleDto)
        {
            try
            {
                var roleFromDb = _roleRepository.GetRoleById(roleDto.RoleId);
                if (roleFromDb == null)
                {
                    return Results.Problem("Role does not exist!");
                }
                await _roleCommands.UpdateRole(roleDto);
                return Results.Ok("Role has been added!");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> DeleteRole(string roleId)
        {
            try
            {
                await _roleCommands.DeleteRole(roleId);
                return Results.Ok("Role has been deleted!");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> GetRoles()
        {
            try
            {
                var results = await _roleRepository.GetRoles();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> GetRoleById(string roleId)
        {
            try
            {
                var result = await _roleRepository.GetRoleById(roleId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
