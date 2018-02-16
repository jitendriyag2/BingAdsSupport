using BingAdsSupport.Data;
using BingAdsSupport.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BingAdsSupport.Logic;
using Microsoft.EntityFrameworkCore;

namespace BingAdsSupport.Services
{
    public class TicketRepository  : ITicketRepository
    {
        private AppDbContext _dbContext;
        public TicketRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TicketInfo GetTicketInfo(string TicketID)
        {
            //return _dbContext.TicketInfos.FromSql("select distinct TicketId, AssignedToAlias, TicketNumber,OwnerSK as OwnerInfoOwnerSK, TicketPriority,TicketSubject,TicketCreatedDateTime,RequesterEmail ,ResolveReason,CloseReason,Alias,Email1, ManagerAlias,ManagerEmail,TicketModifiedDateTime from dbo.TicketReportView t inner join dbo.DimOwner d on t.AssignedToAlias = d.OwnerSK where TicketNumber = '{0}' ", TicketID ).FirstOrDefault();
            return _dbContext.TicketInfos.Include(x => x.OwnerInfo).FirstOrDefault(x => x.TicketNumber == TicketID);
        }

        public int AddFeedback(ICMTicket ticket, out string Error)
        {
            Validation v = new Validation(_dbContext);
            if (v.ValidateTicketNo(ticket.TicketID,out Error))
            {
                //if (v.ICMExistInDbOrNot(ticket.TicketID))
                //{
                //    Error = $"Feedback for {ticket.TicketID} Ticket Already given";
                //    return 0;
                //}

                var result = _dbContext.ICMTickets.Add(ticket);

                
                _dbContext.SaveChanges();
                if (result == null)
                {
                    Error = "There might be some issue while storing the data";
                    return 0;
                }
                Error = "Success";
                return 1;
            }
            return 0;
        }

    }
}
