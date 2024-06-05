using Core.DataClasses;
using Core.Models.UserModels;

namespace BLL.Abstractions.Interfaces.UserInterfaces
{
    public interface ILoginService
    {
        Task<OptionalResult<(string Token, Guid UserId)>> Login(UserLoginModel userData);
    }
}
