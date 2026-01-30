using UserService.Application.Models.Requests;

namespace UserService.Application.Services.Interfaces
{
    public interface IUpdateUserService
    {
        Task<string?> UpdateUserAsync(UpdateUserRequest updateUserRequest);
    }
}
