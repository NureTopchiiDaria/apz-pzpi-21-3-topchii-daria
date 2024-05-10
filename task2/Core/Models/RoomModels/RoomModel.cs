using System.ComponentModel.DataAnnotations;
using Core.Models.UserModels;

namespace Core.Models.RoomModels
{
    public class RoomModel : BaseModel
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        public bool IsApproved { get; set; }
    }
}
