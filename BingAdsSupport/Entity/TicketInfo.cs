using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BingAdsSupport.Entity
{
    [Table("TicketReportView")]
    public partial class TicketInfo
    {
        [Key]
        public long TicketId { get; set; }
        public string TicketNumber { get; set; }
        public Int16 TicketPriority { get; set; }
        public string TicketSubject { get; set; }
        public string RequesterEmail { get; set; }
        public DateTime TicketCreatedDateTime { get; set; }
        public DateTime TicketModifiedDateTime { get; set; }
        public string ResolveReason { get; set; }
        public string CloseReason { get; set; }
        //public string AssignedToAlias { get; set; }
        public OwnerInfo OwnerInfo { get; set; }
        [Column("AssignedToAlias")]
        public virtual Int32 OwnerInfoOwnerSK { get; set; }
        public Int64 TimeToResolve { get; set; }

        public string RootCauseLevel1 { get; set; }
        public string RootCauseLevel2 { get; set; }
        public string RootCauseLevel3 { get; set; }

    } 
}