using System.ComponentModel.DataAnnotations;
using Core.Models.RoomModels;

namespace Core.Models.UserModels
{
    public class UserModel : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        public bool IsActive { get; set; }

        public float Height { get; set; }

        public bool Sex { get; set; }

        public DateTime BirthDate { get; set; }

        public string Information { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}
