using BingAdsSupport.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BingAdsSupport.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }
        public DbSet<ICMTicket> ICMTickets { get; set; }
        public DbSet<TicketInfo> TicketInfos { get; set; }
        public DbSet<OwnerInfo> OwnerInfos { get; set; }
    }
}
