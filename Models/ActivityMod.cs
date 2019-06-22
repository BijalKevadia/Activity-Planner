using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace activityPlanner.Models {
    public class ActivityMod {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        [Display(Name = "Title:")]
        public string Title { get; set; }
        
        [Required]
        [Display(Name = "Description:")]
        public string Description { get; set; }
        public string Planner { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Activity Date:")]
        public DateTime ActivityDate { get; set; }
        
        [Required]
        [Display(Name = "Duration:")]
        public int Duration { get; set; }
        
        public string DurationType { get; set; }
        
        [Required]
        [Display(Name= "Time:")]
        [DataType(DataType.Duration)]
        public DateTime Time { get; set; }
        
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        public List<Rsvp> Guests { get; set; }
        
        public ActivityMod() {
            Guests = new List<Rsvp>();
        }
    }
}