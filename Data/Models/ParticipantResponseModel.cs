﻿namespace Parking_System_API.Data.Models
{
    public class ParticipantResponseModel
    {
        public long ParticipantId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool DoProvidePhoto { get; set; }

        public bool DoDetected { get; set; }

        public bool Status
        { //0 : Not Activated , 1: Activated
            get; set;
        }
    }
}
