using AuthApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.AuthApiTests.Data
{
    public class InvalidTestData
    {
        public static IEnumerable<object[]> NullUserRegistrationDto =>
        new List<object[]>
        {
            new object[]
            {
                new UserRegistrationDto
                {
                    UserName = null,
                    Email = null,
                    Password = null,
                    ProfilePicture = null
                }
            }
        };

        public static IEnumerable<object[]> NullUserLoginDto =>
            new List<object[]>
            {
                new object[]
                {
                    new UserLoginDto
                    {
                        UserName = null,
                        Password = null
                    }
                }
            };

    }
}
