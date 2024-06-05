using System;
using System.Threading.Tasks;
using Core.DataClasses;

namespace BLL.Abstractions.Interfaces.RoomInterfaces
{
    public interface IUserRoomManagementService
    {
        Task<ExceptionalResult> AddUserToRoom(Guid userId, Guid roomId, bool isAdmin = false);

        Task<ExceptionalResult> RemoveUserFromRoom(Guid userId, Guid roomId);

        Task<ExceptionalResult> MakeUserAdminInRoom(Guid userId, Guid roomId);

        Task<ExceptionalResult> RevokeAdminStatus(Guid userId, Guid roomId);
    }
}