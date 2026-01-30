using UserService.Application.Models.DTO;

namespace UserService.Application.Services.Interfaces
{
    /// <summary>
    /// IGetUserService
    /// </summary>
    public interface IGetUserService
    {
        /// <summary>
        /// Fetch User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDto> GetUserAsync(string userId);
    }
}
