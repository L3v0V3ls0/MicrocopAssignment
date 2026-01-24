using Core.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DTOs
{
    public class ChangePasswordRequestTest
    {
            [Theory]
            [InlineData(null, false, "Password is required")]
            [InlineData("", false, "Password is required")]
            [InlineData(" ", false, "Password is required")]
            [InlineData("1234567", false, "Password too short (7 chars)")] // 7 chars < min
            [InlineData("12345678", true, "Valid password (8 chars)")] // min length
            [InlineData("Password123", true, "Valid password (11 chars)")] // valid length
            [InlineData("Pwd12_3!", true, "Valid password (8 chars with special char)")]
            [InlineData("VeryLongPassword1234567890123456789012345678901234567890123456789012345678901234567890123456789012345", false, "Password too long (101 chars)")] // 101 chars > max
            [InlineData("VeryLongPassword123456789012345678901234567890123456789012345678901234567890123456789012345678901234", true, "Valid password (100 chars)")] // max length
            public void ChangePasswordRequest_Validation(string password, bool isValid, string description)
            {
                // Arrange
                var request = new ChangePasswordRequest { Password = password };

                // Act
                var validationResults = DTOTestHelpers.ValidateModel(request);
                var hasPasswordError = validationResults.Any(v => v.MemberNames.Contains("Password"));

                // Assert
                if (isValid)
                {
                    Assert.Empty(validationResults);
                }
                else
                {
                    Assert.NotEmpty(validationResults);
                    Assert.True(hasPasswordError, $"Password validation failed: {description}");
                }
            }

            [Fact]
            public void ChangePasswordRequest_ValidObject_NoValidationErrors()
            {
                // Arrange
                var request = new ChangePasswordRequest
                {
                    Password = "ValidPassword123!"
                };

                // Act
                var validationResults = DTOTestHelpers.ValidateModel(request);

                // Assert
                Assert.Empty(validationResults);
            }
    }
}
