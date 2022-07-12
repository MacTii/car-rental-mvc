using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CarRental.Models
{
    public partial class Rental
    {
        public int Id { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        [Required(ErrorMessage = "Rent date is required")]
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Comments { get; set; }
        public virtual Car Car { get; set; }
        public virtual Driver Driver { get; set; }
    }
}
