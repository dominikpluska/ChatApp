using AuthApi.Dto;
using AuthApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Tests.AuthApiTests.TestAttributes
{
    public class UserAccountDtoAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                new UserRegistrationDto()
                {
                    UserName = "TestUserName",
                    Email = "Test@test.com",
                    Password = "NewTestPassword",
                }
            };
        }
    }
}
