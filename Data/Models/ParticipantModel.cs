using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class ParticipantModel
    {
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        /*
         Attribute To Send Image
         
         */
    }
}
