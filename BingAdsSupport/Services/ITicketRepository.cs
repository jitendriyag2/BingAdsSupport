using BingAdsSupport.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BingAdsSupport.Services
{
    public interface ITicketRepository
    {
        TicketInfo GetTicketInfo(string TicketID);
        int AddFeedback(ICMTicket ticket, out string Error);
    }
}
