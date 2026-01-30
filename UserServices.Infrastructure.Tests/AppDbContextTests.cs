using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Services.Tests
{
    public class AppDbContextTests
    {
        [Fact]
        public async Task Can_save_and_fetch_user_using_sqlite_in_memory()
        {
            // SQLite in-memory DB must keep the connection open for the life of the context
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            // Create schema in memory
            await using (var setupContext = new AppDbContext(options))
            {
                await setupContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
            }

            var userId = Guid.NewGuid().ToString();

            // Act: insert value to memory
            await using (var writeContext = new AppDbContext(options))
            {
                writeContext.Users.Add(new User
                {
                    Id = userId,
                    Name = "Alice",
                    Email = "alice@example.com",
                    Role = UserRole.Customer,
                    UserName = "alice_1",
                    CreatedBy = "tester",
                    UpdatedBy = "tester",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                });

                await writeContext.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            // Assert: read from memory
            await using var readContext = new AppDbContext(options);
            var saved = await readContext.Users.SingleAsync(u => u.Id == userId, cancellationToken: TestContext.Current.CancellationToken);

            Assert.Equal("Alice", saved.Name);
            Assert.Equal("alice@example.com", saved.Email);
            Assert.Equal("Customer", saved.Role.ToString());
            Assert.Equal("alice_1", saved.UserName);
        }
    }
}
