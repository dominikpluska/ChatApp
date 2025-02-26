using AuthApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.AuthApiTests.Data
{
    public class TestData
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

    }
}
