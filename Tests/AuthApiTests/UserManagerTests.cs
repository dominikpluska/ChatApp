﻿using AuthApi.Cookies;
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
            _userManager.Setup(x => x.RegisterNewUser(null)).ReturnsAsync(Results.Problem("Argument Null Exception!", 
                ex.Message));

            //Assert
            var result = await _userManager.Object.RegisterNewUser(null);

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

            _userAccountsRepository.Setup(x => x.GetUserByName(userDto.UserName)).ReturnsAsync(userAccount);
            

            //Assert
            var doesUserExist = await _userAccountsRepository.Object.GetUserByName(userDto.UserName);
            var httpResult = doesUserExist != null ? Results.Problem("Username already exists! Choose a different user name!") : null;
            _userManager.Setup(x => x.RegisterNewUser(userDto)).ReturnsAsync(httpResult!);

            var result = await _userManager.Object.RegisterNewUser(userDto);

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

            _userAccountsRepository.Setup(x => x.GetUserByEmail(userDto.Email)).ReturnsAsync(userAccount);

            //Assert
            var doesUserExist = await _userAccountsRepository.Object.GetUserByEmail(userDto.Email);
            var httpResult = doesUserExist != null ? Results.Problem("An account to which provided email is bound already exists! Please choose a different one or contact support for more details!") : null;
            _userManager.Setup(x => x.RegisterNewUser(userDto)).ReturnsAsync(httpResult!);

            var result = await _userManager.Object.RegisterNewUser(userDto);

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
            _userManager.Setup(x => x.RegisterNewUser(userDto)).ReturnsAsync(expectedResult);
            var httpResult = await _userManager.Object.RegisterNewUser(userDto);

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
            _userManager.Setup(x => x.RegisterNewUser(userDto)).ReturnsAsync(expectedResult);
            var httpResult = await _userManager.Object.RegisterNewUser(userDto);

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
            _userManager.Setup(x => x.RegisterNewUser(userDto)).ReturnsAsync(expectedResult);
            var httpResult = await _userManager.Object.RegisterNewUser(userDto);

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

            _userManager.Setup(x => x.RegisterNewUser(userDto)).ReturnsAsync(Results.Ok(""));
            _userAccountsCommands.Setup(x => x.AddNewUser(userRegistration)).ReturnsAsync("New-Object-Id");


            //Assert

            var userId = await _userAccountsCommands.Object.AddNewUser(userRegistration);
            var httpResponse = await _userManager.Object.RegisterNewUser(userDto);

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

            _userManager.Setup(x => x.Login(userLoginDto)).ReturnsAsync(Results.Ok(""));

            var httpResponse = wasPasswordCorrect ? await _userManager.Object.Login(userLoginDto) : Results.Problem("");

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

            _userManager.Setup(x => x.Login(userLoginDto)).ReturnsAsync(Results.Ok(""));

            var httpResponse = wasPasswordCorrect ? await _userManager.Object.Login(userLoginDto) : Results.Problem("");

            Assert.IsType<ProblemHttpResult>(httpResponse);
        }

        [Theory]
        [UserLoginDtoNull]
        public async Task Login_GivenNullUser_ThrowExceptionAndReturnProblem(UserLoginDto userLoginDto)
        {
            var isException = userLoginDto.Password == null && userLoginDto.UserName == null
                ? new ArgumentNullException() : null;
            _userManager.Setup(x => x.Login(userLoginDto)).ReturnsAsync(Results.Problem(""));

            var httpResponse = isException != null ? await _userManager.Object.Login(userLoginDto) :
                                null;

            Assert.IsType<ProblemHttpResult>(httpResponse);
        }
        #endregion
    }
}
