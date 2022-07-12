using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CarRental.Models
{
    public partial class Car
    {
        public Car()
        {
            Rentals = new HashSet<Rental>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Make car is required")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Model car is required")]
        public string Model { get; set; }

        [Display(Name = "Registration number")]
        [Required(ErrorMessage = "Registration number is required")]
        public string RegistrationNumber { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
