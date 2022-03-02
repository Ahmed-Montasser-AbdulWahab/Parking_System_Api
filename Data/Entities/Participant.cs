using System;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{
    public class Participant
    {
        [Key, Required]
        public int ParticipantId { get; set; }
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Salt { get; set; }

        public bool Status { get; set; }
    }
}
