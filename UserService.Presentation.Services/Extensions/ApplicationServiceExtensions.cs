using System.Runtime.CompilerServices;
using UserService.Application.Services.CreateUser;
using UserService.Application.Services.GetUser;
using UserService.Application.Services.Interfaces;
using UserService.Application.Services.UpdateUser;

namespace UserService.WebApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationServiceExtensions(this IServiceCollection services)
        {
            services.AddScoped<IGetUserService, GetUserService>();
            services.AddScoped<ICreateUserService, CreateUserService>();
            services.AddScoped<IUpdateUserService, UpdateUserService>();
        }
    }
}
