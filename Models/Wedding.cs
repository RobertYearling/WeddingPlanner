using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PlannerTwo.Validations;

namespace PlannerTwo.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }

        // Brides Name
        [Required(ErrorMessage="Bride's Name is Required")]
        [Display(Name="Bride's Name:")]
        public string Bride { get; set; }

        // Grooms Name
        [Required(ErrorMessage="Groom's Name is Required")]
        [Display(Name="Groom's Name:")]
        public string Groom { get; set; }

        // Date
        [Required(ErrorMessage="Date is required")]
        [Display(Name="Date:")]
        [PastDate] //Name of file in Validations folder Minus Attribute
        public DateTime Date { get; set; }

        // Address
        [Required(ErrorMessage="Address is required")]
        [Display(Name="Address:")]
        public string Address { get; set; }

        // Foreign Key - One To Many
        public int UserId { get; set; }

        // Navigational Property - One To Many - Wedding can only be created by One User
        public User Creator { get; set; }

        // Naviagtional Property - Many To Many - A Wedding can have many attendeess.
        public List<RSVP> Attendees { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}