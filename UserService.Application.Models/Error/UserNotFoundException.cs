namespace UserService.Application.Models.Error
{
    public class UserNotFoundException(string? message = null) : Exception(message ?? "User details not found")
    {
    }
}
