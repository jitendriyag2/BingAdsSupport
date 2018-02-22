using BingAdsSupport.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BingAdsSupport.ViewModels
{
    public class Ticket
    {
        //[Required, StringLength(16, MinimumLength = 16, ErrorMessage = "Invalid Ticket ID")]
        //[Remote(action: "ValidateTicketNo", controller: "Validation", AdditionalFields = nameof(TicketID))]
        public string TicketID { get; set; }
        [Required(ErrorMessage = "Please provide a valuable rating"), Range(1, 5, ErrorMessage = "Please provide rating between 1 to 5")]
        public int SupportExperience { get; set; }
        [Required(ErrorMessage = "Please provide a valuable rating"), Range(1, 5, ErrorMessage = "Please provide rating between 1 to 5")]
        public int Solution { get; set; }
        [Required(ErrorMessage = "Please provide a valuable rating"), Range(1, 5, ErrorMessage = "Please provide rating between 1 to 5")]
        public int ResponseTime { get; set; }
        [Required(ErrorMessage = "Please provide a valuable rating"), Range(1, 5, ErrorMessage = "Please provide rating between 1 to 5")]
        public int Accuracy { get; set; }
        [Required,StringLength(500, ErrorMessage = "Please enter your Valuable Feedback")]
        public string Feedback { get; set; }
        public TicketInfo TicketInfo { get; set; }
    }
}
