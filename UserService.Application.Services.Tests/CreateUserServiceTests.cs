using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Application.Models.Error;
using UserService.Application.Models.Requests;
using UserService.Application.Services.CreateUser;
using UserService.Domain.Models;

namespace UserService.Application.Services.Tests
{
    public class CreateUserServiceTests
    {

        [Fact]
        public async Task CreateUserAsync_WhenValidRequest_CreatesUserAndReturnsResponse()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            await using var db = await Utilities.CreateDbAsync(connection);

            var logger = new Mock<ILogger<CreateUserService>>();

            var service = new CreateUserService(db, logger.Object);

            var request = new CreateUserRequest(
                "John",
                "john@gmail.com",
                "Admin",
                "john_1",
                "system"
                );

            // Act
            var response = await service.CreateUserAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Contains("created successfully", response.Message); 

            var savedUser = await db.Users.SingleAsync(u => u.Name == "John", cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal("john@gmail.com", savedUser.Email);
            Assert.Equal(UserRole.Admin, savedUser.Role);
            Assert.Equal("john_1", savedUser.UserName);
            Assert.Equal("system", savedUser.CreatedBy);
            Assert.Equal("system", savedUser.UpdatedBy);
        }

        [Fact]
        public async Task CreateUserAsync_WhenDuplicateUserName_ThrowsDuplicateUserException()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            await using var db = await Utilities.CreateDbAsync(connection);

            // Seed existing user
            db.Users.Add(new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "John",
                Email = "existing@gmail.com",
                Role = UserRole.Admin,
                UserName = "john_1",
                CreatedBy = "seed",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            });
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var logger = new Mock<ILogger<CreateUserService>>();
            var service = new CreateUserService(db, logger.Object);

            // Create duplicate user
            var request = new CreateUserRequest(
                "John",
                "john@gmail.com",
                "Admin",
                "john_1",
                "system"
                );

            // Act and Assert
            await Assert.ThrowsAsync<DuplicateUserException>(() => service.CreateUserAsync(request));
        }

        [Fact]
        public async Task CreateUserAsync_WhenRoleInvalid_ThrowsBadRequestException()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            await using var db = await Utilities.CreateDbAsync(connection);

            var logger = new Mock<ILogger<CreateUserService>>();
            var service = new CreateUserService(db, logger.Object);

            var request = new CreateUserRequest(
                "Jacob",
                "john@gmail.com",
                "invalid",
                "John",
                "system"
                );

            // Act and Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.CreateUserAsync(request));
            Assert.Contains("role", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

    }
}
