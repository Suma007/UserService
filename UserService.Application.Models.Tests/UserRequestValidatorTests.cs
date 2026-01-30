using UserService.Application.Models.Error;
using UserService.Application.Models.Requests;
using UserService.Application.Models.Validations;

namespace UserService.Application.Models.Tests
{
    public class UserRequestValidatorTests
    {
        #region CreateUserRequestValidator
        [Fact]
        public void ValidateCreate_WithValidRequest_DoesNotThrow()
        {
            var request = new CreateUserRequest(
                "John",
                "john@gmail.com",
                "Admin",
                "John",
                "system"
                );

            var response = UserRequestValidator.ValidateCreate(request);

            Assert.True(response);
        }

        [Fact]
        public void ValidateCreate_WhenRequestIsNull_ThrowsBadRequestException()
        {
            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateCreate(null!));
        }

        [Fact]
        public void ValidateCreate_WhenNameIsNull_ThrowsBadRequestException()
        {
            var request = new CreateUserRequest(
              "",
              "john@gmail.com",
              "Admin",
              "John",
              "system"
              );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateCreate(request));
        }

        [Fact]
        public void ValidateCreate_WhenNameIsInvalid_ThrowsBadRequestException()
        {
            var request = new CreateUserRequest(
              "invalid()*",
              "john@gmail.com",
              "Admin",
              "John",
              "system"
              );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateCreate(request));
        }

        [Fact]
        public void ValidateCreate_WhenRoleNull_ThrowsBadRequestException()
        {
            var request = new CreateUserRequest(
                "John",
                "john@gmail.com",
                "",
                "John",
                "system"
            );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateCreate(request));
        }

        [Fact]
        public void ValidateCreate_WhenRoleInvalid_ThrowsBadRequestException()
        {
            var request = new CreateUserRequest(
                "John",
                "john@gmail.com",
                "Role$%^",
                "John",
                "system"
            );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateCreate(request));
        }

        [Fact]
        public void ValidateCreate_WhenEmailInvalid_ThrowsBadRequestException()
        {
            var request = new CreateUserRequest(
                "John",
                "invalid-emalls",
                "Admin",
                "John",
                "system"
               );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateCreate(request));
        }

        [Fact]
        public void ValidateCreate_WhenCreatedByNull_ThrowsBadRequestException()
        {
            var request = new CreateUserRequest(
                "John",
                "john@gmail.com",
                "admin",
                "John",
                ""
            );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateCreate(request));
        }

        [Fact]
        public void ValidateCreate_WhenCreatedByInvalid_ThrowsBadRequestException()
        {
            var request = new CreateUserRequest(
                "John",
                "john@gmail.com",
                "Role",
                "John",
                "system%^&"
            );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateCreate(request));
        }

        #endregion

        #region Updaterequest validator
        [Fact]
        public void ValidateUpdate_WithValidRequest_DoesNotThrow()
        {
            var request = new UpdateUserRequest(
                 Guid.NewGuid().ToString(),
                "John",
                "john@gmail.com",
                "Role",
                "John",
                "system");

            var response = UserRequestValidator.ValidateUpdate(request);

            Assert.True(response);
        }

        [Fact]
        public void ValidateUpdate_WhenRequestIsNull_ThrowsBadRequestException()
        {
            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateUpdate(null!));
        }

        [Fact]
        public void ValidateUpdate_WhenIdInvalid_ThrowsBadRequestException()
        {
            var request = new UpdateUserRequest(
                "",
                "John",
                "john@gmail.com",
                "Role",
                "John",
                "system");

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateUpdate(request));
        }


        [Fact]
        public void ValidateUpdate_WhenNameIsNull_ThrowsBadRequestException()
        {
            var request = new UpdateUserRequest(
               Guid.NewGuid().ToString(),
              "",
              "john@gmail.com",
              "Admin",
              "John",
              "system"
              );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateUpdate(request));
        }

        [Fact]
        public void ValidateUpdate_WhenNameIsInvalid_ThrowsBadRequestException()
        {
            var request = new UpdateUserRequest(
               Guid.NewGuid().ToString(),
              "invalid()*",
              "john@gmail.com",
              "Admin",
              "John",
              "system"
              );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateUpdate(request));
        }

        [Fact]
        public void ValidateUpdate_WhenRoleNull_ThrowsBadRequestException()
        {
            var request = new UpdateUserRequest( Guid.NewGuid().ToString(), "John", "john@gmail.com", "", "John", "system");

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateUpdate(request));
        }

        [Fact]
        public void ValidateUpdate_WhenRoleInvalid_ThrowsBadRequestException()
        {
            var request = new UpdateUserRequest(
                 Guid.NewGuid().ToString(),
                "John",
                "john@gmail.com",
                "Role$%^",
                "John",
                "system"
            );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateUpdate(request));
        }

        [Fact]
        public void ValidateUpdate_WhenEmailInvalid_ThrowsBadRequestException()
        {
            var request = new UpdateUserRequest(
                 Guid.NewGuid().ToString(),
                "John",
                "invalid-emalls",
                "Admin",
                "John",
                "system"
               );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateUpdate(request));
        }

        [Fact]
        public void ValidateUpdate_WhenUpdatedByNull_ThrowsBadRequestException()
        {
            var request = new UpdateUserRequest(
                 Guid.NewGuid().ToString(),
                "John",
                "john@gmail.com",
                "admin",
                "John",
                ""
            );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateUpdate(request));
        }

        [Fact]
        public void ValidateUpdate_WhenUpdatedByInvalid_ThrowsBadRequestException()
        {
            var request = new UpdateUserRequest(
                 Guid.NewGuid().ToString(),
                "John",
                "john@gmail.com",
                "Role",
                "John",
                "system%^&"
            );

            Assert.Throws<BadRequestException>(() =>
                UserRequestValidator.ValidateUpdate(request));
        }

        #endregion
    }
}
