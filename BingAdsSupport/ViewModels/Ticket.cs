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
        [Required, MaxLength(16)]
        [Remote(action: "ValidateTicketNo", controller: "Validation", AdditionalFields = nameof(TicketID))]
        public string TicketID { get; set; }
        public int SupportExperience { get; set; }
        public int Solution { get; set; }
        public int ResponseTime { get; set; }
        public int Accuracy { get; set; }
        [MaxLength(500)]
        public string Feedback { get; set; }

        public TicketInfo TicketInfo { get; set; }
    }
}
