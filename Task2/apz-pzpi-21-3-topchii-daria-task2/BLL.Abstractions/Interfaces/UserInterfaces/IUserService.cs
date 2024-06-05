using System.Linq.Expressions;
using Core.DataClasses;
using Core.DTOs;
using Core.Models.UserModels;

namespace BLL.Abstractions.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        Task<OptionalResult<UserModel>> CreateNonActiveUser(UserCreateModel user);

        Task<OptionalResult<UserModel>> ActivateUser(Guid id);

        Task<ExceptionalResult> Delete(Guid id);

        Task<OptionalResult<UserModel>> UpdateProfilePicture(Guid id, byte[] profilePicture);

        Task<OptionalResult<UserModel>> Update(UserUpdateModel user, UserHeightDTO userHeightDTO);

        Task<IEnumerable<UserModel>> GetByConditions(params Expression<Func<UserModel, bool>>[] conditions);

        Task<IEnumerable<UserModel>> GetActiveUsers(params Expression<Func<UserModel, bool>>[] additionalConditions);

        Task<UserModel> GetUserById(Guid id);
    }
}
