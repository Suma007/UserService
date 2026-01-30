using Microsoft.EntityFrameworkCore;
using UserService.Application.Models.DTO;
using UserService.Application.Models.Error;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Services;

namespace UserService.Application.Services.GetUser
{
    /// <summary>
    /// Fetching the User based on Id
    /// </summary>
    /// <param name="db"></param>
    public class GetUserService(AppDbContext db) : IGetUserService
    {
        public async Task<UserDto> GetUserAsync(string userId)
        {
            User? user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId) ?? throw new UserNotFoundException("User not found");
            return new UserDto(user.Id,
                user.Name,
                user.Email,
                user.Role.ToString(),
                user.UserName,
                user.CreatedBy,
                user.UpdatedBy,
                user.CreatedDate,
                user.UpdatedDate);
        }
    }
}
