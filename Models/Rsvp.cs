using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace activityPlanner.Models {
    public class Rsvp {
        [Key]
        public int RsvpId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ActivityId { get; set; }
        public ActivityMod Activity { get; set; }
    }
}