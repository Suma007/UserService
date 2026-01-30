namespace UserService.Application.Models.Requests
{
    /// <summary>
    /// Update User Request
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Email"></param>
    /// <param name="Role"></param>
    /// <param name="UserName"></param>
    /// <param name="UpdatedBy"></param>
    public record UpdateUserRequest(
        /// <summary>Id of the user</summary>
        string Id,

        /// <summary>The name of the user</summary>
        string Name,

        /// <summary>The email of the user</summary>
        string? Email,

        /// <summary>The role of the user</summary>
        string Role,

        /// <summary>The username of the user</summary>
        string UserName,

        /// <summary>The detail of who updated the user</summary>
        string UpdatedBy
    );
}
