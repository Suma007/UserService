using System.Text.RegularExpressions;
using UserService.Application.Models.Error;
using UserService.Application.Models.Requests;
using UserService.Domain.Models;

namespace UserService.Application.Models.Validations
{
    /// <summary>
    /// Shared user request validator for Create + Update.
    /// </summary>
    public static partial class UserRequestValidator
    {
        /// <summary>
        /// Va;odator for create req
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="BadRequestException"></exception>
        public static bool ValidateCreate(CreateUserRequest request)
        {
            if (request is null)
                throw new BadRequestException("The create request is empty");

            // required fields for create
            RequireValidString(request.Name, nameof(request.Name));
            RequireValidString(request.UserName, nameof(request.UserName));
            RequireValidString(request.Role, nameof(request.Role));
            RequireValidString(request.CreatedBy, nameof(request.CreatedBy));
            // optional email chexck
            ValidateEmailIfProvided(request.Email);

            return true;
        }

        /// <summary>
        /// Validator for update req
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="BadRequestException"></exception>
        public static bool ValidateUpdate(UpdateUserRequest request)
        {
            if (request is null)
            {
                throw new BadRequestException("The update request is empty");
            }
            // Id check
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                throw new BadRequestException($"Invalid Id '{request.Id}'");
            }
            // required fields for update
            RequireValidString(request.Name, nameof(request.Name));
            RequireValidString(request.UpdatedBy, nameof(request.UpdatedBy));
            RequireValidString(request.UserName, nameof(request.UserName));
            RequireValidString(request.Role, nameof(request.Role));

            // optional email chexck
            ValidateEmailIfProvided(request.Email);
            return true;
        }

        private static void RequireValidString(string value, string fieldName)
        {
            if (!(!string.IsNullOrWhiteSpace(value) && ValidStringRegex().IsMatch(value)))
            {
                throw new BadRequestException($"The {fieldName} '{value}' is invalid");
            }
        }

        private static void ValidateEmailIfProvided(string? email)
        {
            if (!string.IsNullOrWhiteSpace(email) && !EmailRegex().IsMatch(email))
            {
                throw new BadRequestException($"The Email '{email}' is invalid");
            }
        }

        // Email
        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
        private static partial Regex EmailRegex();

        // Validstring
        [GeneratedRegex(@"^[a-zA-Z0-9_]+$", RegexOptions.Compiled)]
        private static partial Regex ValidStringRegex();
    }
}
