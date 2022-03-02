using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking_System_API.Data.Entities
{
    public class Vehicle
    {
        [Key, Required]
        public string PlateNumberId { get; set; }

        public string BrandName { get; set; }

        public string SubCategory { get; set; }

        public string Color { get; set; }

        public DateTime startSubscription { get; set; }

        public DateTime endSubscription { get; set; }

        public bool IsPresent { get; set; }


    }
}
