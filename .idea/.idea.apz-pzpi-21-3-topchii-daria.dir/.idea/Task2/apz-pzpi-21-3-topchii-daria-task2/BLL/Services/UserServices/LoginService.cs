using BLL.Abstractions.Interfaces.UserInterfaces;
using Core.DataClasses;
using Core.Models.UserModels;

namespace BLL.Services.UserServices
{
    internal class LoginService : ILoginService
    {
        private readonly IUserService userService;

        private readonly IJwtService jwtService;

        private readonly IHashingService hashingService;

        public LoginService(IUserService userService, IJwtService jwtService, IHashingService hashingService)
        {
            this.userService = userService;
            this.jwtService = jwtService;
            this.hashingService = hashingService;
        }

        public async Task<OptionalResult<(string Token, Guid UserId)>> Login(UserLoginModel userData)
        {
            var user = (await this.userService.GetActiveUsers(x => x.Email == userData.Email)).FirstOrDefault();
            if (user is null || this.hashingService.Hash(userData.Password) != user.HashedPassword)
            {
                return new OptionalResult<(string, Guid)>(false, $"User with given credentials does not exist.");
            }

            var token = this.jwtService.GenerateJwt(user);
            return new OptionalResult<(string, Guid)>((token, user.Id));
        }
    }
}
