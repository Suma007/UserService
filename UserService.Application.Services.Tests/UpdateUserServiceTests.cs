using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Models.Error;
using UserService.Application.Models.Requests;
using UserService.Application.Services.UpdateUser;
using UserService.Domain.Models;

namespace UserService.Application.Services.Tests
{
    public class UpdateUserServiceTests
    {

        [Fact]
        public async Task UpdateUserAsync_WhenValidRequest_UpdatesUser()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            await using var db = await Utilities.CreateDbAsync(connection);

            string id = Guid.NewGuid().ToString();
            var existingUser = new User
            {
                Id = id,
                Name = "John",
                Email = "John@gmail.com",
                Role = UserRole.Customer,
                UserName = "john_1",
                CreatedBy = "seed",
                UpdatedBy = "seed",
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                UpdatedDate = DateTime.UtcNow.AddDays(-1)
            };

            db.Users.Add(existingUser);
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new UpdateUserService(db);

            var request = new UpdateUserRequest(
                id,
                "Jimmy",
                "jimmy@gmail.com",
                "Admin",
                "jimmy_1",
                "admin"
                );

            // Act
            var result = await service.UpdateUserAsync(request);

            // Assert
            Assert.Null(result);

            var updatedUser = await db.Users.SingleAsync(u => u.Id == id, cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal("Jimmy", updatedUser.Name);
            Assert.Equal("jimmy@gmail.com", updatedUser.Email);
            Assert.Equal(UserRole.Admin, updatedUser.Role);
            Assert.Equal("jimmy_1", updatedUser.UserName);
            Assert.Equal("admin", updatedUser.UpdatedBy);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            await using var db = await Utilities.CreateDbAsync(connection);

            var service = new UpdateUserService(db);

            var request = new UpdateUserRequest(
                new Guid().ToString(),
                "Jimmy",
                "jimmy@example.com",
                "Admin",
                "jimmy_1",
                "admin"
                );

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                service.UpdateUserAsync(request));
        }

        [Fact]
        public async Task UpdateUserAsync_WhenDuplicateUserNameExists_ThrowsDuplicateUserException()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            await using var db = await Utilities.CreateDbAsync(connection);
            string trackingId = Guid.NewGuid().ToString();

            db.Users.AddRange(
                new User
                {
                    Id = trackingId,
                    Name = "John",
                    Email = "John@gmail.com",
                    Role = UserRole.Customer,
                    UserName = "john_1",
                    CreatedBy = "seed",
                    UpdatedBy = "seed",
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    UpdatedDate = DateTime.UtcNow.AddDays(-1)
                },
                new User
                {
                    Id = new Guid().ToString(),
                    Name = "Kelly",
                    Email = "kelly@gmail.com",
                    Role = UserRole.Customer,
                    UserName = "kelly_1",
                    CreatedBy = "seed",
                    UpdatedBy = "seed",
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    UpdatedDate = DateTime.UtcNow.AddDays(-1)
                }
            );

            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new UpdateUserService(db);

            var request = new UpdateUserRequest(
                            trackingId,
                            "Kelly",
                            "jimmy@example.com",
                            "Admin",
                            "kelly_1",
                            "admin"
                            );


            // Act and Assert
            await Assert.ThrowsAsync<DuplicateUserException>(() =>
                service.UpdateUserAsync(request));
        }

        [Fact]
        public async Task UpdateUserAsync_WhenRoleInvalid_ThrowsBadRequestException()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            await using var db = await Utilities.CreateDbAsync(connection);
            string trackingId = Guid.NewGuid().ToString();

            db.Users.Add(new User
            {
                Id = trackingId,
                Name = "John",
                Email = "John@gmail.com",
                Role = UserRole.Customer,
                UserName = "john_1",
                CreatedBy = "seed",
                UpdatedBy = "seed",
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                UpdatedDate = DateTime.UtcNow.AddDays(-1)
            });

            await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            var service = new UpdateUserService(db);

            var request = new UpdateUserRequest(
                            trackingId,
                            "Kelly",
                            "jimmy@example.com",
                            "invalidRole",
                            "jimmy_1",
                            "admin"
                            );

            // Act and Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                service.UpdateUserAsync(request));
        }
    }
}
