using Core.Models.RoomModels;

namespace Core.Models.UserModels
{
    public class UserUpdateModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public float Height { get; set; }

        public bool Sex { get; set; }

        public DateTime BirthDate { get; set; }

        public string Information { get; set; }
    }
}
