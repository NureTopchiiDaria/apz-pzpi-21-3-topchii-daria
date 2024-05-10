using Core.DataClasses;
using Core.Models.UserModels;

namespace BLL.Abstractions.Interfaces.UserInterfaces
{
    public interface ITokenGeneratorService
    {
        ExceptionalResult CheckToken(UserModel user, string token);

        OptionalResult<Guid> GetIdFromBase64(string base64Id);

        string GetToken(UserModel user);

        string GetUidb64(UserModel user);
    }
}
