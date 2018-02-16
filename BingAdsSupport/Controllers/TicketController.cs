using BingAdsSupport.Data;
using BingAdsSupport.Entity;
using BingAdsSupport.Logic;
using BingAdsSupport.Services;
using BingAdsSupport.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BingAdsSupport.Controllers
{
    public class TicketController : Controller
    {
        IConfiguration _config;
        private ITicketRepository _dbContext;
        public EmailForward _emailClient;

        public TicketController(ITicketRepository dbContext, IConfiguration config, AppDbContext context)
        {
            _dbContext = dbContext;
            _config = config;
        }

        [HttpGet]
        public  IActionResult  AddFeedback(string TicketId = "UCM000000567513")
        {
            TicketInfo ticketInfo = new TicketInfo();
            try
            {
                ticketInfo = _dbContext.GetTicketInfo(TicketId);
            }
            catch(Exception ex)
            {
                ticketInfo = null;
            }

            Ticket ticket = new Ticket() {
                TicketID = TicketId,
                TicketInfo = ticketInfo
            };

            return View(ticket);
            //return View(ticket);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddFeedback(Ticket ticket)
        {
            string Error = "";

            Ticket OldTicket = new Ticket()
            {
                TicketID = ticket.TicketID,
                TicketInfo = _dbContext.GetTicketInfo(ticket.TicketID)
            };

            if (ModelState.IsValid)
            {
                ICMTicket _ticket = new ICMTicket()
            {
                TicketID = ticket.TicketID,
                ResponseTime = ticket.ResponseTime,
                Solution = ticket.Solution,
                Accuracy = ticket.Accuracy,
                SupportExperience = ticket.SupportExperience,
                Feedback = ticket.Feedback,
            };
           
                if (_dbContext.AddFeedback(_ticket, out Error) > 0)
                {
                    ModelState.AddModelError("Error", Error);
                    string Message = "";
                    if (ticket.SupportExperience <= 5 && ticket.SupportExperience >= 3)
                    {
                        Message = "<img src='../images/satisfied.png'/><br/>" +
                            "Thanks for your Positive Feedback. <br/> " +
                            "We are happy to serve you better.<br/> " +
                            "Your feedback encourage us to deliverer better";
                    }
                    else if (ticket.SupportExperience < 3)
                    {
                        Message = " <img src='../images/dissatisfied.png' /> <br/> " +
                            "Thanks for your valuable Feedback.<br/> " +
                            "We are sorry, we already escalate the Ticket and will assign concern team to help you better.";

                        ticket.TicketInfo = _dbContext.GetTicketInfo(_ticket.TicketID);
                        _emailClient = new EmailForward(_config, _dbContext);
                        _emailClient.SendICMEmail(ticket.TicketInfo);
                    }

                    TempData["Error"] = Message;
                    return RedirectToAction("Success");

                }
                

            }

            ModelState.AddModelError("Error", Error);
            return View( OldTicket);

        }

        public IActionResult Success()
        {
            return View();
        }

        /*Old AddFeedback Action Method
         * 
        [HttpPost]
        public IActionResult AddFeedback(string TicketID, int ResponseTime, int Solution, int Accuracy, int SupportExperience, string Feedback)
        {
            ICMTicket _ticket = new ICMTicket()
            {
                TicketID = TicketID,
                ResponseTime = ResponseTime,
                Solution = Solution,
                Accuracy = Accuracy,
                SupportExperience = SupportExperience,
                Feedback = Feedback
            };

            string Error = "";
            _dbContext.AddFeedback(_ticket, out Error);
            if (Error != "Success")
            {
                ModelState.AddModelError("Error", Error);
                return BadRequest(ModelState);
            }
            return Ok();
        }



    [HttpPost]
    public IActionResult AddFeedback()
    {
        ICMTicket _ticket = new ICMTicket()
        {
            TicketID = Request.Form["TicketID"],
            ResponseTime = Convert.ToInt16(Request.Form["ResponseTime"]),
            Solution = Convert.ToInt16(Request.Form["Solution"]),
            Accuracy = Convert.ToInt16(Request.Form["Accuracy"]),
            SupportExperience = Convert.ToInt16(Request.Form["SupportExperience"]),
            Feedback = Request.Form["Feedback"]
        };

        string Error = "";
        _dbContext.AddFeedback(_ticket, out Error);
        if (Error != "Success")
        {
            ModelState.AddModelError("Error", Error);
            return BadRequest(ModelState);
        }
        return Ok();
    }
    */

    }
}
