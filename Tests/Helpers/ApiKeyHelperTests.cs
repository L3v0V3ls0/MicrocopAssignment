using Core.Helpers;
using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Tests.Helpers
{
    public class ApiKeyHelperTests
    {
        [Fact]
        public void HashApiKey_ValidInput_ProducesDeterministicHash()
        {
            // Arrange
            var apiKey = "test-api-key-12345";

            // Act
            var hash1 = ApiKeyHelper.HashApiKey(apiKey);
            var hash2 = ApiKeyHelper.HashApiKey(apiKey);

            // Assert
            Assert.Equal(hash1, hash2);
            Assert.NotEmpty(hash1);
        }
        [Fact]
        public void HashApiKey_DifferentInput_DifferentHash()
        {
            // Arrange
            var apiKey1 = "test-api-key-12345";
            var apiKey2 = "test-api-key-54321";

            // Act
            var hash1 = ApiKeyHelper.HashApiKey(apiKey1);
            var hash2 = ApiKeyHelper.HashApiKey(apiKey2);

            // Assert
            Assert.NotEqual(hash1, hash2);
            Assert.NotEmpty(hash1);
        }


        [Theory]
        [InlineData("")]
        [InlineData("simple-key")]
        [InlineData("very-long-api-key-with-many-characters-that-should-still-work-correctly")]
        [InlineData("key-with-special-chars-!@#$%^&*()")]
        [InlineData("key-with-unicode-测试-🚀")]
        public void HashApiKey_VariousInputs_ProducesValidHash(string apiKey)
        {
            // Act
            var hash = ApiKeyHelper.HashApiKey(apiKey);

            // Assert
            Assert.NotEmpty(hash);
            Assert.True(IsValidBase64(hash));
            // SHA256 produces 32 bytes = 44 Base64 characters (with padding)
            Assert.True(hash.Length == 43 || hash.Length == 44);
        }

        // Helper methods
        private static bool IsValidBase64(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            try
            {
                Convert.FromBase64String(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
