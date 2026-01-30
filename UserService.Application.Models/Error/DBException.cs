
namespace UserService.Application.Models.Error
{
    public class DBException(string? message) : Exception(message ?? "DBException occured")
    {
    }
}
