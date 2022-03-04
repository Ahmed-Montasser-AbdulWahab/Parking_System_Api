﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{
    public class Participant
    {
        [Key, Required]
        public long ParticipantId { get; set; }

        public string Name { get; set; }

        [Required ]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public bool DoProvidePhoto { get; set; }
        [Required]
        public bool DoDetected { get; set; }
        [Required]
        public bool Status { //0 : Not Activated , 1: Activated
            get; set;
        }


        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<ParkingTransaction> ParkingTransactions { get; set; }
    }
}
