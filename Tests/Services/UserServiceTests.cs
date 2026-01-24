using Core.DTOs.Request;
using Core.Entities;
using Core.Errors;
using Core.Services.Implementations;
using Data.Repositories.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Tests.TestingHelpers;
using Xunit;

namespace Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockRepository.Object);
        }

        #region CreateUserAsync Tests


        [Fact]
        public async Task CreateUserAsync_DuplicateActiveUserName_ThrowsException()
        {
            // Arrange
            var request = TestDataBuilder.CreateUserRequest("existinguser");
            var existingUser = TestDataBuilder.CreateUser(userName: "existinguser", isActive: true);
            
            _mockRepository.Setup(r => r.GetByUserNameAsync("existinguser"))
                          .ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _userService.CreateUserAsync(request));
            
            Assert.Equal(UserMessages.UsernameExists, exception.Message);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_DuplicateInactiveUserName_AllowsCreation()
        {
            // Arrange
            var request = TestDataBuilder.CreateUserRequest("inactiveuser");
            var inactiveUser = TestDataBuilder.CreateUser(userName: "inactiveuser", isActive: false);
            
            _mockRepository.Setup(r => r.GetByUserNameAsync("inactiveuser"))
                          .ReturnsAsync(inactiveUser);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<User>()))
                          .ReturnsAsync((User u) => u);

            // Act
            var result = await _userService.CreateUserAsync(request);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        #endregion

        #region GetUserByIdAsync Tests


        [Fact]
        public async Task GetUserByIdAsync_InactiveUser_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = TestDataBuilder.CreateUser(id: userId, isActive: false);
            
            _mockRepository.Setup(r => r.GetByIdAsync(userId))
                          .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }


        #endregion


        #region GetUserByUserNameAsync Tests

        [Fact]
        public async Task GetUserByUserNameAsync_InactiveUser_ReturnsNull()
        {
            // Arrange
            var userName = "inactiveuser";
            var user = TestDataBuilder.CreateUser(userName: userName, isActive: false);
            
            _mockRepository.Setup(r => r.GetByUserNameAsync(userName))
                          .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByUserNameAsync(userName);

            // Assert
            Assert.Null(result);
        }


        #endregion

        #region UpdateUserAsync Tests


        [Fact]
        public async Task UpdateUserAsync_UserNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateRequest = TestDataBuilder.UpdateUserRequest();

            _mockRepository.Setup(r => r.GetByIdAsync(userId))
                          .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _userService.UpdateUserAsync(userId, updateRequest));
            
            Assert.Equal(UserMessages.NoUser, exception.Message);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUserAsync_InactiveUser_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = TestDataBuilder.CreateUser(id: userId, isActive: false);
            var updateRequest = TestDataBuilder.UpdateUserRequest();

            _mockRepository.Setup(r => r.GetByIdAsync(userId))
                          .ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _userService.UpdateUserAsync(userId, updateRequest));
            
            Assert.Equal(UserMessages.NoUser, exception.Message);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }


        #endregion

        #region DeleteUserAsync Tests

        [Fact]
        public async Task DeleteUserAsync_ActiveUser_SoftDeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = TestDataBuilder.CreateUser(id: userId, isActive: true);
            User? updatedUser = null;

            _mockRepository.Setup(r => r.GetByIdAsync(userId))
                          .ReturnsAsync(user);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                          .Callback<User>(u => updatedUser = u)
                          .Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.False(updatedUser.IsActive);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_UserNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(userId))
                          .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _userService.DeleteUserAsync(userId));
            
            Assert.Equal(UserMessages.NoUserToDelete, exception.Message);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserAsync_AlreadyDeleted_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = TestDataBuilder.CreateUser(id: userId, isActive: false);

            _mockRepository.Setup(r => r.GetByIdAsync(userId))
                          .ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _userService.DeleteUserAsync(userId));
            
            Assert.Equal(UserMessages.NoUserToDelete, exception.Message);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        #endregion

        #region ValidatePasswordAsync Tests

        [Fact]
        public async Task ValidatePasswordAsync_ValidPassword_ReturnsTrue()
        {
            // Arrange
            var userName = "testuser";
            var password = "CorrectPassword123!";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = TestDataBuilder.CreateUser(userName: userName, isActive: true, passwordHash: passwordHash);

            _mockRepository.Setup(r => r.GetByUserNameAsync(userName))
                          .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidatePasswordAsync(userName, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidatePasswordAsync_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var userName = "testuser";
            var correctPassword = "CorrectPassword123!";
            var wrongPassword = "WrongPassword123!";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(correctPassword);
            var user = TestDataBuilder.CreateUser(userName: userName, isActive: true, passwordHash: passwordHash);

            _mockRepository.Setup(r => r.GetByUserNameAsync(userName))
                          .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidatePasswordAsync(userName, wrongPassword);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidatePasswordAsync_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var userName = "nonexistent";
            _mockRepository.Setup(r => r.GetByUserNameAsync(userName))
                          .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.ValidatePasswordAsync(userName, "anypassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidatePasswordAsync_InactiveUser_ReturnsFalse()
        {
            // Arrange
            var userName = "inactiveuser";
            var user = TestDataBuilder.CreateUser(userName: userName, isActive: false);

            _mockRepository.Setup(r => r.GetByUserNameAsync(userName))
                          .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidatePasswordAsync(userName, "anypassword");

            // Assert
            Assert.False(result);
        }

        #endregion

        #region ChangePasswordAsync Tests

        [Fact]
        public async Task ChangePasswordAsync_UserNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(userId))
                          .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _userService.ChangePasswordAsync(userId, "NewPassword123!"));
            
            Assert.Equal(UserMessages.NoUser, exception.Message);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ChangePasswordAsync_InactiveUser_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = TestDataBuilder.CreateUser(id: userId, isActive: false);

            _mockRepository.Setup(r => r.GetByIdAsync(userId))
                          .ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _userService.ChangePasswordAsync(userId, "NewPassword123!"));
            
            Assert.Equal(UserMessages.NoUser, exception.Message);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        #endregion
    }
}
