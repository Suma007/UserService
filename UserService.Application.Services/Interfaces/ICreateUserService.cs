using UserService.Application.Models.Requests;
using UserService.Application.Models.Responses;

namespace UserService.Application.Services.Interfaces
{
    public interface ICreateUserService
    {
        Task<CreateUserResponse?> CreateUserAsync(CreateUserRequest createUserRequest);
    }
}
