using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Models.DTO;
using UserService.Application.Models.Error;
using UserService.Application.Services.GetUser;
using UserService.Domain.Models;
using UserService.Infrastructure.Services;

namespace UserService.Application.Services.Tests
{
    public class GetUserServiceTests
    {

        [Fact]
        public async Task GetUserAsync_WhenUserExists_ReturnsUserDto()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            await using var db = await Utilities.CreateDbAsync(connection);

            var user = new User
            {
                Id = "1",
                Name = "Alice",
                Email = "alice@example.com",
                Role = UserRole.Admin,
                UserName = "alice_1",
                CreatedBy = "seed",
                UpdatedBy = "seed",
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                UpdatedDate = DateTime.UtcNow
            };

            db.Users.Add(user);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new GetUserService(db);

            // Act
            UserDto result = await service.GetUserAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("Alice", result.Name);
            Assert.Equal("alice@example.com", result.Email);
            Assert.Equal("Admin", result.Role);
            Assert.Equal("alice_1", result.UserName);
            Assert.Equal("seed", result.CreatedBy);
            Assert.Equal("seed", result.UpdatedBy);
        }

        [Fact]
        public async Task GetUserAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            await using var db = await Utilities.CreateDbAsync(connection);

            var service = new GetUserService(db);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                service.GetUserAsync("doesn't-exists"));
        }
    }
}
