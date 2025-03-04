using AuthApi.Cookies;
using AuthApi.DataBaseOperations.Commands.UserAccountsCommands;
using AuthApi.DataBaseOperations.Repository.RoleRepository;
using AuthApi.DataBaseOperations.Repository.UserAccountsRepository;
using AuthApi.Dto;
using AuthApi.Helper;
using AuthApi.Jwt;
using AuthApi.Managers.UserManager;
using AuthApi.Models;
using AuthApi.Services;
using AuthApi.UserAccessor;
using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tests.AuthApiTests.Data;
using Tests.AuthApiTests.TestAttributes;


namespace Tests.AuthApiTests
{
    public class UserManagerTests
    {
        private readonly Mock<IUserManager> _userManager;
        private readonly Mock<IUserAccountsRepository> _userAccountsRepository;
        private readonly Mock<IUserAccountsCommands> _userAccountsCommands;
        private readonly Mock<IRoleRepository> _roleRepository;
        private readonly Mock<IJwt> _jwt;
        private readonly Mock<ICookieGenerator> _cookieGenerator;
        private readonly Mock<IUserAccessor> _userAccessor;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUserSettingsService> _userSettingsService;
        private readonly CancellationToken cancellationToken;

        //Next tests : UpdateAccount

        public UserManagerTests()
        {
            _userManager = new Mock<IUserManager>();
            _userAccountsRepository = new Mock<IUserAccountsRepository>();
            _userAccountsCommands = new Mock<IUserAccountsCommands>();
            _roleRepository = new Mock<IRoleRepository>();
            _jwt = new Mock<IJwt>();
            _cookieGenerator = new Mock<ICookieGenerator>();
            _mapper = new Mock<IMapper>();
            _userSettingsService = new Mock<IUserSettingsService>();
        }


        #region RegisterNewUserTests
        [Fact]
        public async Task RegisterNewUser_GivenNullUserRegistrationDto_ThrowErrorAndReturnProblem()
        {
            //Arrange
            var ex = new ArgumentNullException();
            _userManager.Setup(x => x.RegisterNewUser(null, cancellationToken)).ReturnsAsync(Results.Problem("Argument Null Exception!", 
                ex.Message));

            //Assert
            var result = await _userManager.Object.RegisterNewUser(null, cancellationToken);

            //Act

            Assert.IsType<ProblemHttpResult>(result);


        }

        [Theory]
        [UserAccountDto]
        public async Task RegisterNewUser_GivenAlreadyExistingUser_ReturnProblemResult(UserRegistrationDto userDto)
        {
            //Arrange

            UserAccount userAccount = new()
            {
                UserName = "TestUserName",
                Email = "Test@test.com",
                PasswordHash = "(D(WA*(D*WA(",
                RoleId = "AAAA-AAAA-AAAA-AAAA",
            };

            _userAccountsRepository.Setup(x => x.GetUserByName(userDto.UserName, cancellationToken)).ReturnsAsync(userAccount);
            

            //Assert
            var doesUserExist = await _userAccountsRepository.Object.GetUserByName(userDto.UserName, cancellationToken);
            var httpResult = doesUserExist != null ? Results.Problem("Username already exists! Choose a different user name!") : null;
            _userManager.Setup(x => x.RegisterNewUser(userDto, cancellationToken)).ReturnsAsync(httpResult!);

            var result = await _userManager.Object.RegisterNewUser(userDto, cancellationToken);

            //Act

            Assert.IsType<ProblemHttpResult>(result);
        }

        [Theory]
        [UserAccountDto]
        public async Task RegisterNewUser_GivenEmailAlreadyExist_ReturnProblemResult(UserRegistrationDto userDto)
        {
            //Arrange

            UserAccount userAccount = new()
            {
                UserName = "TestUserName1",
                Email = "Test@test.com",
                PasswordHash = "(D(WA*(D*WA(",
                RoleId = "AAAA-AAAA-AAAA-AAAA",
            };

            _userAccountsRepository.Setup(x => x.GetUserByEmail(userDto.Email, cancellationToken)).ReturnsAsync(userAccount);

            //Assert
            var doesUserExist = await _userAccountsRepository.Object.GetUserByEmail(userDto.Email, cancellationToken);
            var httpResult = doesUserExist != null ? Results.Problem("An account to which provided email is bound already exists! Please choose a different one or contact support for more details!") : null;
            _userManager.Setup(x => x.RegisterNewUser(userDto, cancellationToken)).ReturnsAsync(httpResult!);

            var result = await _userManager.Object.RegisterNewUser(userDto, cancellationToken);

            //Act

            Assert.IsType<ProblemHttpResult>(result);
        }

        [Theory]
        [UserAccountDtoInvalid]
        public async Task RegisterNewUser_CheckUserName(UserRegistrationDto userDto)
        {
            //Arrange

            var result = userDto.UserName.UserNameChecker();
            var expectedResult = result ? Results.Ok("") : Results.Problem("");

            //Assert
            _userManager.Setup(x => x.RegisterNewUser(userDto, cancellationToken)).ReturnsAsync(expectedResult);
            var httpResult = await _userManager.Object.RegisterNewUser(userDto, cancellationToken);

            //Act 
            Assert.True(httpResult == expectedResult);
        }

        [Theory]
        [UserAccountDtoInvalid]
        public async Task RegisterNewUser_CheckPassword(UserRegistrationDto userDto)
        {
            //Arrange

            var result = userDto.Password.PasswordChecker();
            var expectedResult = result ? Results.Ok("") : Results.Problem("");

            //Assert
            _userManager.Setup(x => x.RegisterNewUser(userDto, cancellationToken)).ReturnsAsync(expectedResult);
            var httpResult = await _userManager.Object.RegisterNewUser(userDto, cancellationToken);

            //Act 
            Assert.True(httpResult == expectedResult);
        }

        [Theory]
        [UserAccountDtoInvalid]
        public async Task RegisterNewUser_CheckEmail(UserRegistrationDto userDto)
        {
            //Arrange

            var result = userDto.Email.EmailChecker();
            var expectedResult = result ? Results.Ok("") : Results.Problem("");

            //Assert
            _userManager.Setup(x => x.RegisterNewUser(userDto, cancellationToken)).ReturnsAsync(expectedResult);
            var httpResult = await _userManager.Object.RegisterNewUser(userDto, cancellationToken);

            //Act 
            Assert.True(httpResult == expectedResult);
        }

        [Theory]
        [UserAccountDto]
        public async Task RegisterNewUser_GivenValidAccount(UserRegistrationDto userDto)
        {
            //Arrange
            UserRegistration userRegistration = new()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = "NewPasswordHash",
                RoleId = "Test-Defaul-Role",
                ProfilePicturePath = "",
            };

            _mapper.Setup(x => x.Map<UserAccount>(userRegistration)).Returns(new UserAccount() 
            {
                UserName = userRegistration.UserName,
                Email = userRegistration.Email,
                PasswordHash = userRegistration.PasswordHash,
                RoleId = userRegistration.RoleId, 
                PicturePath = userRegistration.ProfilePicturePath
            });

            var user = _mapper.Object.Map<UserAccount>(userRegistration);

            _userManager.Setup(x => x.RegisterNewUser(userDto, cancellationToken)).ReturnsAsync(Results.Ok(""));
            _userAccountsCommands.Setup(x => x.AddNewUser(user)).ReturnsAsync("New-Object-Id");


            //Assert

            var userId = await _userAccountsCommands.Object.AddNewUser(user);
            var httpResponse = await _userManager.Object.RegisterNewUser(userDto, cancellationToken);

            var result = Assert.IsType<string>(userId);
            var httpResult = Assert.IsType<Ok<string>>(httpResponse);

            //Act
            Assert.True(result is string && httpResult is Ok<string>);

        }
        #endregion

        #region LoginTests
        [Theory]
        [UserLogin]
        public async Task Login_GivenTheCorrectCredentials_ReturnResultOK(UserLoginDto userLoginDto)
        {
            UserLoginDto userLoginCorrectCredentials = new()
            {
                UserName = "TestUserName",
                Password = "VeryStrongPassword2025!"
            };

            var wasPasswordCorrect = userLoginCorrectCredentials.Password == userLoginDto.Password;

            _userManager.Setup(x => x.Login(userLoginDto, cancellationToken)).ReturnsAsync(Results.Ok(""));

            var httpResponse = wasPasswordCorrect ? await _userManager.Object.Login(userLoginDto, cancellationToken) : Results.Problem("");

            Assert.IsType<Ok<string>>(httpResponse);
        }
        [Theory]
        [UserLogin]
        public async Task Login_GivenIncorrectCredentials_ReturnResultsProblem(UserLoginDto userLoginDto)
        {
            UserLoginDto userLoginCorrectCredentials = new()
            {
                UserName = "TestUserName",
                Password = "VeryStrongPassword2025!!"
            };

            var wasPasswordCorrect = userLoginCorrectCredentials.Password == userLoginDto.Password;

            _userManager.Setup(x => x.Login(userLoginDto, cancellationToken)).ReturnsAsync(Results.Ok(""));

            var httpResponse = wasPasswordCorrect ? await _userManager.Object.Login(userLoginDto, cancellationToken) : Results.Problem("");

            Assert.IsType<ProblemHttpResult>(httpResponse);
        }

        [Theory]
        [UserLoginDtoNull]
        public async Task Login_GivenNullUser_ThrowExceptionAndReturnProblem(UserLoginDto userLoginDto)
        {
            var isException = userLoginDto.Password == null && userLoginDto.UserName == null
                ? new ArgumentNullException() : null;
            _userManager.Setup(x => x.Login(userLoginDto, cancellationToken)).ReturnsAsync(Results.Problem(""));

            var httpResponse = isException != null ? await _userManager.Object.Login(userLoginDto, cancellationToken) :
                                null;

            Assert.IsType<ProblemHttpResult>(httpResponse);
        }
        #endregion

        #region AuthenticationTests
        [Fact]
        public async Task CheckAuthentication_GivenNullTokenString_ReturnsUnathorized()
        {
            string tokenString = null;
            string userId = "TestUser";

            var httpResponse = (tokenString != null & userId != null) ? Results.Ok() : Results.Unauthorized();
            var methodResultSetup = _userManager.Setup(x => x.CheckAuthentication(cancellationToken)).ReturnsAsync(httpResponse);

            var methodResullt = await _userManager.Object.CheckAuthentication(cancellationToken);

            Assert.IsType<UnauthorizedHttpResult>(methodResullt);
        }

        [Fact]
        public async Task CheckAuthentication_GivenNullUserId_ReturnsUnathorized()
        {
            string tokenString = "TestUser";
            string userId = null;

            var httpResponse = (tokenString != null & userId != null) ? Results.Ok() : Results.Unauthorized();
            var methodResultSetup = _userManager.Setup(x => x.CheckAuthentication(cancellationToken)).ReturnsAsync(httpResponse);

            var methodResullt = await _userManager.Object.CheckAuthentication(cancellationToken);

            Assert.IsType<UnauthorizedHttpResult>(methodResullt);
        }

        [Fact]
        public async Task CheckAuthentication_GivenValidIdAndToken_ReturnsOk()
        {
            string tokenString = "TestUser";
            string userId = "TestUser";

            var httpResponse = (tokenString != null & userId != null) ? Results.Ok() : Results.Unauthorized();
            var methodResultSetup = _userManager.Setup(x => x.CheckAuthentication(cancellationToken)).ReturnsAsync(httpResponse);

            var methodResullt = await _userManager.Object.CheckAuthentication(cancellationToken);

            Assert.IsType<Ok>(methodResullt);
        }

        [Fact]
        public async Task CheckAuthentication_ThrowsException()
        {
            _userManager.Setup(x => x.CheckAuthentication(cancellationToken)).ThrowsAsync(new Exception());
            await Assert.ThrowsAsync<Exception>(() => _userManager.Object.CheckAuthentication(cancellationToken));
        }


        #endregion
    }
}
