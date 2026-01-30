namespace UserService.Domain.Models
{
    /// <summary>
    /// The User model
    /// </summary>
    public class User
    {
        /// <summary>
        /// The id of the user
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// The name of the user
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The email of the user
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// The role of the user
        /// </summary>
        public UserRole Role { get; set; } 
        /// <summary>
        /// The username of the user
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// The detail of who created the user
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;
        /// <summary>
        /// The detail of who last updated the user record
        /// </summary>
        public string UpdatedBy { get; set; } = string.Empty;
        /// <summary>
        /// Time when the user was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Time when the user was last updated
        /// </summary>
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    }
}
