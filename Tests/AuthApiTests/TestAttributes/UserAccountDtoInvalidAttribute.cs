using AuthApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Tests.AuthApiTests.TestAttributes
{
    class UserAccountDtoInvalidAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                new UserRegistrationDto()
                {
                    UserName = "a",
                    Email = "Test@test.com",
                    Password = "NewTestPassword123!",
                }
            };
            yield return new object[]
            {
                new UserRegistrationDto()
                {
                    UserName = "TestAccount",
                    Email = "aa",
                    Password = "NewTestPassword",
                }
            };
            yield return new object[]
            {
                new UserRegistrationDto()
                {
                    UserName = "TestAccount",
                    Email = "Test@test.com",
                    Password = "123",
                }
            };
        }
    }
}
