namespace UserService.Application.Models.Error
{
    public class DuplicateUserException(string? message) : Exception(message ?? "Duplicate user found")
    {
    }
}
