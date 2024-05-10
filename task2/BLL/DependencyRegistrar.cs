using BLL.Abstractions.Interfaces;
using BLL.Abstractions.Interfaces.RoomInterfaces;
using BLL.Abstractions.Interfaces.UserInterfaces;
using BLL.Services;
using BLL.Services.RoomServices;
using BLL.Services.UserServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class DependencyRegistrar
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureUserServices(services);
            ConfigureRoomServices(services);
            DAL.DependencyRegistrar.ConfigureServices(services, configuration);
        }

        private static void ConfigureUserServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IHashingService, SHA256HashingService>();
            services.AddScoped<IUserValidationService, UserValidationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IAccountActivationService, AccountActivationService>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEditUserInfoService, EditUserInfoService>();
        }

        private static void ConfigureRoomServices(IServiceCollection services)
        {
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoomPointsService, RoomPointsService>();
            services.AddScoped<IUserRoomsService, UserRoomsService>();
            services.AddScoped<IRoomApprovalService, RoomApprovalService>();
        }
    }
}
