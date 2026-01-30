using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserService.Application.Models.DTO;
using UserService.Application.Models.Error;
using UserService.Application.Models.Requests;
using UserService.Application.Models.Responses;
using UserService.Application.Services.Interfaces;

namespace UserService.WebApi.Controllers
{
    /// <summary>
    /// Usercontroller
    /// </summary>
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController(IGetUserService getUserService, ICreateUserService createUserService, IUpdateUserService updateUserService, ILogger<UserController> logger) : ControllerBase
    {
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Fetch user", Description = "Fetch User by Id")]
        [SwaggerResponse(StatusCodes.Status200OK, "User details fetched successfully", typeof(UserDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request- Id is mpt proper")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Intersernal server error")]
        [ProducesResponseType(type: typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var user = await getUserService.GetUserAsync(userId);
                return Ok(user);
            }
            catch (BadRequestException ex)
            {
                logger.LogError($"Bad request while trying to fetch the user details: {ex.Message}");
                return BadRequest("The request is not valid");
            }
            catch (UserNotFoundException)
            {
                logger.LogInformation("User {UserId} not found", userId);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred");
                return StatusCode(500, "There is some error processing your request. Please contact the IT support team");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user", Description = "Create a new user based on request")]
        [SwaggerResponse(StatusCodes.Status201Created, "User  created successfully", typeof(CreateUserResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "User exists with the same name")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Intersernal server error")]
        [ProducesResponseType(type: typeof(CreateUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var user = await createUserService.CreateUserAsync(createUserRequest);
                return Created(string.Empty, user);
            }
            catch (DuplicateUserException)
            {
                logger.LogError("Duplicate user found in DB");
                return Conflict("The user exists with the same name");
            }
            catch (BadRequestException ex)
            {
                logger.LogError(ex, "Bad request while trying to fetch the user details");
                return BadRequest(ex.Message ?? "The request is not valid");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred");
                return StatusCode(500, "There is some error processing your request. Please contact the IT support team");
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Updates the new user", Description = "Updates the user based on request")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User  updated successfully", typeof(CreateUserResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Intersernal server error")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var user = await updateUserService.UpdateUserAsync(updateUserRequest);
                return NoContent();
            }
            catch (DuplicateUserException)
            {
                logger.LogError("Duplicate user found in DB");
                return Conflict("The user exists with the same name");
            }
            catch (BadRequestException ex)
            {
                logger.LogError(ex, "Bad request while trying to fetch the user details");
                return BadRequest(ex.Message ?? "The request is not valid");
            }
            catch (UserNotFoundException)
            {
                logger.LogInformation("User {UserId} not found", updateUserRequest.Id);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred");
                return StatusCode(500, "There is some error processing your request. Please contact the IT support team");
            }
        }
    }
}
