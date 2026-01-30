namespace UserService.Application.Models.Requests
{
    /// <summary>
    /// The create user request
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Email"></param>
    /// <param name="Role"></param>
    /// <param name="UserName"></param>
    /// <param name="CreatedBy"></param>
    public record CreateUserRequest(
        /// <summary>The name of the user</summary>
        string Name,

        /// <summary>The email of the user</summary>
        string? Email,

        /// <summary>The role of the user</summary>
        string Role,

        /// <summary>The username of the user</summary>
        string UserName,

        /// <summary>The detail of who created the user</summary>
        string CreatedBy
    );
}
