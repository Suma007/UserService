using Microsoft.EntityFrameworkCore;
using UserService.Application.Models.Error;
using UserService.Application.Models.Requests;
using UserService.Application.Models.Validations;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Services;

namespace UserService.Application.Services.UpdateUser
{
    /// <summary>
    /// UpdateUserService
    /// </summary>
    /// <param name="dbContext"></param>
    public class UpdateUserService(AppDbContext dbContext) : IUpdateUserService
    {

        public async Task<string?> UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
            //validate the user input
            if (UserRequestValidator.ValidateUpdate(updateUserRequest))
            {
                // Fetch the user
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == updateUserRequest.Id) ?? throw new UserNotFoundException();
                
                // If the same user with name exists in DB then raise an error
                var exists = dbContext.Users.Any(u => 
                        u.Id != updateUserRequest.Id && 
                        u.UserName == updateUserRequest.UserName);

                if (exists)
                {
                    throw new DuplicateUserException("A user with the same user name already exists.");
                }

                if (!Enum.TryParse<UserRole>(updateUserRequest.Role, ignoreCase: true, out UserRole role))
                {
                    throw new BadRequestException("The role is not valid");
                }

                user.Name = updateUserRequest.Name;
                user.Email = updateUserRequest.Email;
                user.Role = role;
                user.UserName = updateUserRequest.UserName;
                user.UpdatedBy = updateUserRequest.UpdatedBy;
                user.UpdatedDate = DateTime.UtcNow;

                await dbContext.SaveChangesAsync();

            }
            return null;
        }
        
    }
}
