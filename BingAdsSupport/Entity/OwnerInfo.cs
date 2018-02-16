using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BingAdsSupport.Entity
{
    [Table("DimOwner")]
    public class OwnerInfo
    {

        [Column("OwnerSK")]
        [Key]
        public Int32 OwnerInfoOwnerSK { get; set; }
        public string  FirstName { get; set; }
        public string LastName { get; set; }
        public string Alias { get; set; }
        public string Email1 { get; set; }
        public string TeamName { get; set; }
        public string ManagerAlias { get; set; }
        public string ManagerEmail { get; set; }

        
    }
}

