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

        [Required]
        public bool IsActive { get; set; }

        public float Height { get; set; }
    }
}
