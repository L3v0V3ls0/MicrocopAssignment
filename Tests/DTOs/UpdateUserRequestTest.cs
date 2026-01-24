using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.EntityConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.TestingHelpers;

namespace Tests.DTOs
{
    public  class UpdateUserRequestTest
    {
        [Fact]
        public void UpdateUserRequest_AllNull_Valid()
        {
            // Arrange
            var request = new UpdateUserRequest();

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);

            // Assert
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("ab", null, null, null, null, null, false, "UserName too short (2 chars)")]
        [InlineData("usr", null, null, null, null, null, true, "UserName valid (3 chars)")]
        [InlineData("validusername123", null, null, null, null, null, true, "UserName valid")]
        [InlineData("verylongusername12345678901234567890123456789012345678901", null, null, null, null, null, false, "UserName too long (51 chars)")]
        [InlineData(null, "J", null, null, null, null, false, "FullName too short (1 char)")]
        [InlineData(null, "Jo", null, null, null, null, true, "FullName valid (2 chars)")]
        [InlineData(null, "verylongfullname1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901", null, null, null, null, false, "FullName too long (101 chars)")]
        [InlineData(null, null, "notanemail", null, null, null, false, "Invalid email format")]
        [InlineData(null, null, "valid@email.com", null, null, null, true, "Valid email")]
        [InlineData(null, null, null, "invalidphone", null, null, false, "Invalid phone format")]
        [InlineData(null, null, null, "+1234567890", null, null, true, "Valid phone")]
        [InlineData(null, null, null, null, "engi", null, false, "Language too long (4 chars)")]
        [InlineData(null, null, null, null, "en", null, true, "Language valid (2 chars)")]
        [InlineData(null, null, null, null, null, "en-US-EXTRA", false, "Culture too long (11 chars)")]
        [InlineData(null, null, null, null, null, "en-US", true, "Culture valid (5 chars)")]
        public void UpdateUserRequest_IndividualFieldValidation(
            string userName, string fullName, string email, string mobileNumber,
            string language, string culture, bool isValid, string description)
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                MobileNumber = mobileNumber,
                Language = language,
                Culture = culture
            };

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);

            // Assert
            if (isValid)
            {
                Assert.Empty(validationResults);
            }
            else
            {
                Assert.NotEmpty(validationResults);
            }
        }

        [Fact]
        public void UpdateUserRequest_PartialUpdate_Valid()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                FullName = "Jane Smith",
                Email = "jane.smith@example.com"
                // Other properties remain null
            };

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);

            // Assert
            Assert.Empty(validationResults);
        }

        [Fact]
        public void UpdateUserRequest_AllFieldsUpdated_Valid()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                UserName = "janesmith",
                FullName = "Jane Smith",
                Email = "jane.smith@example.com",
                MobileNumber = "+1-555-987-6543",
                Language = "fr",
                Culture = "fr-FR"
            };

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);

            // Assert
            Assert.Empty(validationResults);
        }

        [Fact]
        public void UpdateUserRequest_EmailMaxLength_Valid()
        {
            string address = "@test.com";
            // Arrange
            var longEmail = TestDataBuilder.GenerateRandomNumberString(GeneralConstraints.EmailMaxLength - address.Length - 1) + address; // Should be 254 chars or less
            var request = new UpdateUserRequest
            {
                Email = longEmail
            };

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);

            // Assert
            Assert.Empty(validationResults.Where(v => v.MemberNames.Contains("Email")));
        }

        [Fact]
        public void UpdateUserRequest_MobileNumberMaxLength_Valid()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                MobileNumber = "+" + TestDataBuilder.GenerateRandomNumberString(19) // 20 chars, max length
            };

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);

            // Assert
            Assert.Empty(validationResults.Where(v => v.MemberNames.Contains("MobileNumber")));
        }
    }
}
