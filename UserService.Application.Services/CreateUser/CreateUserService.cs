using Microsoft.Extensions.Logging;
using UserService.Application.Models.Error;
using UserService.Application.Models.Requests;
using UserService.Application.Models.Responses;
using UserService.Application.Models.Validations;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Services;

namespace UserService.Application.Services.CreateUser
{
    /// <summary>
    /// Create user service
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public class CreateUserService(AppDbContext dbContext,ILogger<CreateUserService> logger) : ICreateUserService
    {
        public async Task<CreateUserResponse?> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            logger.LogInformation("CreateUserAsync - Create user process started for name {Name}", createUserRequest.Name);

            //validate the user input
            if (UserRequestValidator.ValidateCreate(createUserRequest))
            {
                // If the same user with name exists in DB then raise an error
                var exists = dbContext.Users.Any(u => u.UserName == createUserRequest.UserName);
                if (exists)
                {
                    throw new DuplicateUserException("A user with the same user name already exists.");
                }
                if (!Enum.TryParse<UserRole>(createUserRequest.Role, ignoreCase: true, out UserRole role))
                {
                    throw new BadRequestException("The role is not valid");
                }


                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createUserRequest.Name,
                    Email = createUserRequest.Email,
                    Role = role,
                    UserName = createUserRequest.UserName,
                    CreatedBy = createUserRequest.CreatedBy,
                    UpdatedBy = createUserRequest.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("CreateUserAsync - New user with Name {Name} created", user.Name);

                var createdResponse = new CreateUserResponse($"User with Id {user.Id} created successfully");

                return createdResponse;
            }
            return null;
        }
    }
}
