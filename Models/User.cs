using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlannerTwo.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        // First Name
        [Required]
        [Display(Name="First Name:")]
        public string FirstName { get; set; }

        // Last Name
        [Required]
        [Display(Name="Last Name:")]
        public string LastName { get; set; }

        // Email
        [Required]
        [Display(Name="Email:")]
        [EmailAddress]
        public string Email { get; set; }

        // Password
        [Required]
        [Display(Name="Password:")]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Confirm Password
        [NotMapped]
        [Required]
        [Display(Name="Confirm Password:")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirm { get; set; }

        //Navigational Prop - One To Many - A user can create many Weddings
        List<Wedding> MyWeddings { get; set; }

        // Navigational Prop - Many To Many - A user can attend many Weddings
        List<RSVP> Attending { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}