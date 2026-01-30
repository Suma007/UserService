namespace UserService.Application.Models.Error
{
    public class BadRequestException(string? message) :Exception(message ?? "The request is not valid")
    {
    }
}
