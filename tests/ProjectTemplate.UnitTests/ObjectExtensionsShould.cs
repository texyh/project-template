using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ProjectTemplate.Domain.Helpers;

namespace ProjectTemplate.UnitTests
{
    public class ObjectExtensionsShould
    {

        [Theory]
        [InlineData("123456", "**3456")]
        [InlineData("1234", "1234")]
        [InlineData("5564876598743467", "************3467")]
        public void Mask_String_Leaving_Last_Four_Characters(string number, string result)
        {
            result.Should().Be(number.Mask());
        }
    }
}
