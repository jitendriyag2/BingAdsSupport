using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BingAdsSupport.Entity
{
    public class ICMTicket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required, MaxLength(16)]
        public string TicketID { get; set; }
        [Range(1,5)]
        public int SupportExperience { get; set; }
        [Range(1, 5)]
        public int Solution { get; set; }
        [Range(1, 5)]
        public int ResponseTime { get; set; }
        [Range(1, 5)]
        public int Accuracy { get; set; }
        [MaxLength(500)]
        public string Feedback { get; set; }
    }
}
