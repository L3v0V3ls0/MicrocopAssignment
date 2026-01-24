using Core.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DTOs
{
    public class CreateUserRequestTests
    {
        [Theory]
        [InlineData(null, "Valid FullName", "test@email.com", "+1234567890", "en", "en-US", "Password123", false, "UserName is required")]
        [InlineData("", "Valid FullName", "test@email.com", "+1234567890", "en", "en-US", "Password123", false, "UserName is required")]
        [InlineData("ab", "Valid FullName", "test@email.com", "+1234567890", "en", "en-US", "Password123", false, "UserName too short (2 chars)")]
        [InlineData("usr", "Valid FullName", "test@email.com", "+1234567890", "en", "en-US", "Password123", true, "UserName min length (3 chars)")]
        [InlineData("validusername123", "Valid FullName", "test@email.com", "+1234567890", "en", "en-US", "Password123", true, "Valid UserName")]
        [InlineData("verylongusername12345678901234567890123456789012345678901", "Valid FullName", "test@email.com", "+1234567890", "en", "en-US", "Password123", false, "UserName too long (51 chars)")]
        [InlineData("validuser", null, "test@email.com", "+1234567890", "en", "en-US", "Password123", false, "FullName is required")]
        [InlineData("validuser", "J", "test@email.com", "+1234567890", "en", "en-US", "Password123", false, "FullName too short (1 char)")]
        [InlineData("validuser", "Jo", "test@email.com", "+1234567890", "en", "en-US", "Password123", true, "FullName min length (2 chars)")]
        [InlineData("validuser", "verylongfullname1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901", "test@email.com", "+1234567890", "en", "en-US", "Password123", false, "FullName too long (101 chars)")]
        [InlineData("validuser", "John Doe", "notanemail", "+1234567890", "en", "en-US", "Password123", false, "Invalid email format")]
        [InlineData("validuser", "John Doe", "valid@email.com", "invalidphone", "en", "en-US", "Password123", false, "Invalid phone format")]
        [InlineData("validuser", "John Doe", "valid@email.com", "+1234567890", "engi", "en-US", "Password123", false, "Language too long (4 chars)")]
        [InlineData("validuser", "John Doe", "valid@email.com", "+1234567890", "en", "en-US-EXTRA", "12334556678",false, "Culture too long (11 chars)")]
        [InlineData("validuser", "John Doe", "valid@email.com", "+1234567890", null, null, "Password123", true, "Language and culture can be null, defaults applied")]
        public void CreateUserRequest_Validation(
           string userName, string fullName, string email, string mobileNumber,
           string language, string culture, string password, bool isValid, string description)
        {
            // Arrange
            var request = new CreateUserRequest
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                MobileNumber = mobileNumber,
                Language = language,
                Culture = culture,
                Password = password
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
        public void CreateUserRequest_DefaultValues_AreSet()
        {
            // Arrange & Act
            var request = new CreateUserRequest();

            // Assert
            Assert.Equal("en", request.Language);
            Assert.Equal("en-US", request.Culture);
        }

        [Fact]
        public void CreateUserRequest_ValidObject_NoValidationErrors()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                UserName = "johndoe123",
                FullName = "John Doe",
                Email = "john.doe@example.com",
                MobileNumber = "+1-555-123-4567",
                Language = "en",
                Culture = "en-US",
                Password = "SecurePass123!"
            };

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);

            // Assert
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("6L4E%xk_e?p2v!1w95*,n+z!TL/tH#9-U46wL}(+W1pS#H&,&hCrUMS8w{jM0*d6,@T1e6qGASJR-C=R0:P.D;9H-Ghf3e;D" +
            ".(Tv()kfuG{y.UWwHC:-{,D_z$5aCwt72S1@x%#GGB_XCUVD4(3hi@YqaZEyD$-[);=z(Hidc6j.,kqWUr(:8gT[Trz)PkA=N/eW_zq" +
            "P*2iBE/%NhE5X6/kRJM}X[x=iM:.ZSfR!Fmh2aMEibYF3XmCH1}7", false, "Email too long (256 chars)")]
        [InlineData("test@6L4E%xk_e?p2v!1w95*,n+z!TL/tH#9-U46wL}(+W1pS#H&,&hCrUMS8w{jM0*d6,@T1e6qGASJR-C=R0:P.D;9H-Ghf3e;D" +
            ".(Tv()kfuG{y.UWwHC:-{,D_z$5aCwt72S1@x%#GGB_XCUVD4(3hi@YqaZEyD$-[);=z(Hidc6j.,kqWUr(:8gT[Trz)PkA=N/eW_zq" +
            "P*2iBE/%NhE5X6/kRJM}X[x=iM:.ZSfR!Fmh2aMEibYF3XmCH1}7.com", false, "Email too long (259 chars)")]
        [InlineData("normal@email.com", true, "Valid email length")]
        public void CreateUserRequest_EmailLengthValidation(string email, bool isValid, string description)
        {
            // Arrange
            var request = new CreateUserRequest
            {
                UserName = "validuser",
                FullName = "John Doe",
                Email = email,
                MobileNumber = "+1234567890",
                Password = "Password123"
            };

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);
            var hasEmailError = validationResults.Any(v => v.MemberNames.Contains("Email"));

            // Assert
            if (isValid)
            {
                Assert.Empty(validationResults.Where(v => v.MemberNames.Contains("Email")));
            }
            else
            {
                Assert.Contains(validationResults, v => v.MemberNames.Contains("Email"));
            }
        }

        [Fact]
        public void CreateUserRequest_MobileNumberMaxLength_Valid()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                UserName = "validuser",
                FullName = "John Doe",
                Email = "test@email.com",
                MobileNumber = "+1234567890123456789", // 20 chars
                Password = "Password123"
            };

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);
            var mobileNumberErrors = validationResults.Where(v => v.MemberNames.Contains("MobileNumber"));

            // Assert
            Assert.Empty(mobileNumberErrors);
        }

        [Fact]
        public void CreateUserRequest_MobileNumberExceedsMaxLength_Invalid()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                UserName = "validuser",
                FullName = "John Doe",
                Email = "test@email.com",
                MobileNumber = "+12345678901234567890", // 21 chars > max
                Password = "Password123"
            };

            // Act
            var validationResults = DTOTestHelpers.ValidateModel(request);
            var mobileNumberErrors = validationResults.Where(v => v.MemberNames.Contains("MobileNumber"));

            // Assert
            Assert.NotEmpty(mobileNumberErrors);
        }
    }
}
