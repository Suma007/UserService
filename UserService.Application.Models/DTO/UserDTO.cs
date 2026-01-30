namespace UserService.Application.Models.DTO
{
    /// <summary>
    /// DTO model of user
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Email"></param>
    /// <param name="Role"></param>
    /// <param name="UserName"></param>
    /// <param name="CreatedBy"></param>
    /// <param name="UpdatedBy"></param>
    /// <param name="CreatedDate"></param>
    /// <param name="UpdatedDate"></param>
    public record UserDto(
        string Id,
        string Name,
        string? Email,
        string Role,
        string UserName,
        string CreatedBy,
        string UpdatedBy,
        DateTime CreatedDate,
        DateTime UpdatedDate);
}
