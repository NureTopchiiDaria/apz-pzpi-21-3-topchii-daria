using Core.Models.UserModels;

namespace Core.Models.RoomModels
{
    public class RoomUpdateModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserModel> Users { get; set; }
    }
}
