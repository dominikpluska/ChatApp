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
    class UserLoginDtoNull : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[]
            {
                    new UserLoginDto
                    {
                        UserName = null,
                        Password = null
                    }
            };
        }
    }
}
