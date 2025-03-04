using AuthApi.Data;
using AuthApi.DataBaseOperations.Commands.RoleCommands;
using AuthApi.DataBaseOperations.Commands.UserAccountsCommands;
using AuthApi.DataBaseOperations.Repository.RoleRepository;
using AuthApi.DataBaseOperations.Repository.UserAccountsRepository;
using AuthApi.Dto;
using AuthApi.Helper;
using AuthApi.Models;
using AutoMapper;

namespace AuthApi.Managers.AdminManager
{
    public class AdminManager : IAdminManager
    {
        private readonly IUserAccountsCommands _userAccountsCommands;
        private readonly IUserAccountsRepository _userAccountsRepository;
        private readonly IRoleCommands _roleCommands;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public AdminManager(IUserAccountsCommands userAccountsCommands, IUserAccountsRepository userAccountsRepository, IRoleCommands roleCommands, IRoleRepository roleRepository,
            IMapper mapper)
        {
            _userAccountsCommands = userAccountsCommands;
            _userAccountsRepository = userAccountsRepository;
            _roleCommands = roleCommands;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IResult> RegisterNewUser(UserAdminRegistrationDto userDto, CancellationToken cancellationToken)
        {
            try
            {
                var checkIfUserNameExists = await _userAccountsRepository.GetUserByName(userDto.UserName, cancellationToken);
                if (checkIfUserNameExists != null)
                {
                    return Results.Problem("Username already exists! Choose a different user name!");
                }

                var checkIfEmailExists = await _userAccountsRepository.GetUserByEmail(userDto.Email, cancellationToken);

                if (checkIfEmailExists != null)
                {
                    return Results.Problem("An account to which provided email is bound already exists! Please choose a different one or contact support for more details!");
                }

                if (!userDto.UserName.UserNameChecker())
                {
                    return Results.Problem("Username must not contain any white spaces nor any special characters!");
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

                var user = _mapper.Map<UserAccount>(userRegistration);
                await _userAccountsCommands.AddNewUser(user);

                return Results.Ok("User has been added!");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> ChangeUserPassword(UserAdminPasswordChangeDto userPasswordChangeDto, CancellationToken cancellationToken)
        {
            try
            {
                var userFromDb = await _userAccountsRepository.GetUser(userPasswordChangeDto.UserAccountId, cancellationToken);
                if (userFromDb == null)
                {
                    return Results.Problem("UserId doesn't exist!");
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

        public async Task<IResult> UpdateUserAccount(UserAdminAccountUpdateDto userDto, CancellationToken cancellationToken)
        {
            try
            {
                var userFromDb = await _userAccountsRepository.GetUser(userDto.UserAccountId, cancellationToken);
                var checkIfUserNameExists = await _userAccountsRepository.GetUserByName(userDto.UserName, cancellationToken);

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

        public async Task<IResult> DeleteUser(string userId, CancellationToken cancellationToken)
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

        public async Task<IResult> GetAllUsers(CancellationToken cancellationToken)
        {
            try
            {
                var resutls = await _userAccountsRepository.GetAllUsers(cancellationToken);
                return Results.Ok(resutls);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> GetUser(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userAccountsRepository.GetUser(userId, cancellationToken);
                return Results.Ok(result);
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> AddNewRole(string roleName, CancellationToken cancellationToken)
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

        public async Task<IResult> UpdateRole(RoleDto roleDto, CancellationToken cancellationToken)
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

        public async Task<IResult> DeleteRole(string roleId, CancellationToken cancellationToken)
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

        public async Task<IResult> GetRoles(CancellationToken cancellationToken)
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

        public async Task<IResult> GetRoleById(string roleId, CancellationToken cancellationToken)
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
