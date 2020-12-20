using System.ComponentModel.DataAnnotations;

namespace PlannerTwo.Models
{
    public class RSVP
    {
        [Key]
        public int RSVPId { get; set; }

        public int UserId { get; set; }

        public int WeddingId { get; set; }

        public User Attendee { get; set; }

        public Wedding Marriage { get; set; }
    }
}