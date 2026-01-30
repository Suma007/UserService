using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Application.Models.DTO;
using UserService.Application.Models.Error;
using UserService.Application.Models.Requests;
using UserService.Application.Models.Responses;
using UserService.Application.Services.Interfaces;
using UserService.WebApi.Controllers;

namespace UserService.WebApi.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IGetUserService> _getUserService = new();
        private readonly Mock<ICreateUserService> _createUserService = new();
        private readonly Mock<IUpdateUserService> _updateUserService = new();
        private readonly Mock<ILogger<UserController>> _logger = new();

        private UserController CreateController()
            => new(
                _getUserService.Object,
                _createUserService.Object,
                _updateUserService.Object,
                _logger.Object);


        #region GetUserById
        [Fact]
        public async Task GetUserById_WhenUserExists_ReturnsOk()
        {
            var response = new UserDto(
                "1",
                "John",
                "john@gmail.com",
                "Admin",
                "john_1",
                "seed",
                "seed",
                DateTime.UtcNow,
                DateTime.UtcNow);

            _getUserService
                .Setup(s => s.GetUserAsync("1"))
                .ReturnsAsync(response);

            var controller = CreateController();

            var result = await controller.GetUserById("1");

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(response, ok.Value);
        }

        [Fact]
        public async Task GetUserById_WhenUserNotFound_ReturnsNotFound()
        {
            _getUserService
                .Setup(s => s.GetUserAsync("missing"))
                .ThrowsAsync(new UserNotFoundException());

            var controller = CreateController();

            var result = await controller.GetUserById("missing");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetUserById_WhenBadRequest_ReturnsBadRequest()
        {
            _getUserService
                .Setup(s => s.GetUserAsync(It.IsAny<string>()))
                .ThrowsAsync(new BadRequestException("bad"));

            var controller = CreateController();

            var result = await controller.GetUserById("bad-id");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region CreateUser

        [Fact]
        public async Task CreateUser_WhenValid_ReturnsCreated()
        {
            var request = new CreateUserRequest(
              "John",
              "john@gmail.com",
              "Admin",
              "john_1",
              "system"
              );

            var response = new CreateUserResponse("user created sucessfully");

            _createUserService
                .Setup(s => s.CreateUserAsync(request))
                .ReturnsAsync(response);

            var controller = CreateController();

            var result = await controller.CreateUser(request);

            var created = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, created.StatusCode);
            Assert.Equal(response, created.Value);
        }

        [Fact]
        public async Task CreateUser_WhenDuplicate_ReturnsConflict()
        {
            var request = new CreateUserRequest(
              "John",
              "john@gmail.com",
              "Admin",
              "john_1",
              "system"
              );

            _createUserService
                .Setup(s => s.CreateUserAsync(It.IsAny<CreateUserRequest>()))
                .ThrowsAsync(new DuplicateUserException("User already exists"));

            var controller = CreateController();

            var result = await controller.CreateUser(request);

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_WhenBadRequest_ReturnsBadRequest()
        {
            var request = new CreateUserRequest(
                  "John$#%^",
                  "john@gmail.com",
                  "Admin",
                  "john_1",
                  "system"
                  );
            _createUserService
                .Setup(s => s.CreateUserAsync(It.IsAny<CreateUserRequest>()))
                .ThrowsAsync(new BadRequestException("bad request"));

            var controller = CreateController();

            var result = await controller.CreateUser(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region update user

        [Fact]
        public async Task UpdateUser_WhenValid_ReturnsNoContent()
        {
            var request = new UpdateUserRequest(
                Guid.NewGuid().ToString(),
                "Jimmy",
                "jimmy@gmail.com",
                "Admin",
                "jimmy_1",
                "admin"
             );
            _updateUserService
                .Setup(s => s.UpdateUserAsync(It.IsAny<UpdateUserRequest>()))
                .ReturnsAsync((string?)null);

            var controller = CreateController();


            var result = await controller.UpdateUser(request);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_WhenUserNotFound_ReturnsNotFound()
        {
            var request = new UpdateUserRequest(
                Guid.NewGuid().ToString(),
                "Jimmy",
                "jimmy@gmail.com",
                "Admin",
                "jimmy_1",
                "admin"
             );
            _updateUserService
                .Setup(s => s.UpdateUserAsync(It.IsAny<UpdateUserRequest>()))
                .ThrowsAsync(new UserNotFoundException());

            var controller = CreateController();

            var result = await controller.UpdateUser(request);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateUser_WhenDuplicate_ReturnsConflict()
        {
            var request = new UpdateUserRequest(
                Guid.NewGuid().ToString(),
                "Jimmy",
                "jimmy@gmail.com",
                "Admin",
                "jimmy_1",
                "admin"
             );
            _updateUserService
                .Setup(s => s.UpdateUserAsync(It.IsAny<UpdateUserRequest>()))
                .ThrowsAsync(new DuplicateUserException("User already exists"));

            var controller = CreateController();

            var result = await controller.UpdateUser(request);

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task UpdateUser_WhenBadRequest_ReturnsBadRequest()
        {
            var request = new UpdateUserRequest(
                Guid.NewGuid().ToString(),
                "Jimmy@13",
                "jimmy@gmail.com",
                "Admin",
                "jimmy_1",
                "admin"
             );
            _updateUserService
                .Setup(s => s.UpdateUserAsync(It.IsAny<UpdateUserRequest>()))
                .ThrowsAsync(new BadRequestException("bad request"));

            var controller = CreateController();

            var result = await controller.UpdateUser(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion
    }
}
