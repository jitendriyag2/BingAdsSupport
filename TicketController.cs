using BingAdsSupport.Data;
using BingAdsSupport.Entity;
using BingAdsSupport.Logic;
using BingAdsSupport.Services;
using BingAdsSupport.ViewModels;
using Microsoft.AspNetCore.Http;
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
            _emailClient = new EmailForward(_config, _dbContext);
        }
        [Route("{TicketID?}/{feedbackRating?}", Name = "defaultFeedback")]
        [Route("Ticket/AddFeedback/{TicketID?}")]
        
        [HttpGet]
        public IActionResult AddFeedback(string TicketId = "UCM000000567513", int feedbackRating = 0)
        {

            TicketInfo ticketInfo = new TicketInfo();
            try
            {
                ticketInfo = _dbContext.GetTicketInfo(TicketId);
            }
            catch (Exception ex)
            {
                ticketInfo = null;
            }

            Ticket ticket = new Ticket() {
                TicketID = TicketId,
                SupportExperience = feedbackRating,
                TicketInfo = ticketInfo
            };

            return View(ticket);
            //return View(ticket);
        }
        [HttpPost]
        [Route("{TicketID?}", Name = "defaultFeedbackPost")]
        [Route("Ticket/AddFeedback/{TicketID?}")]
        //[ValidateAntiForgeryToken]
        public IActionResult AddFeedback(Ticket ticket)
        {
            string Error = "";

            TryValidateModel(ticket);

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
                            "Your feedback encourage us to deliver better" +
                            "";
                    }
                    else if (ticket.SupportExperience < 3)
                    {
                        Message = " <img src='../images/dissatisfied.png' /> <br/> " +
                            "Thanks for your valuable Feedback.<br/> " +
                            "We are sorry, we already escalate the Ticket and will assign concern team to help you better.";

                        ticket.TicketInfo = _dbContext.GetTicketInfo(_ticket.TicketID);
                        try {
                            _emailClient.SendICMEmail(ticket.TicketInfo);
                        }
                        catch(Exception ex)
                        {
                            ModelState.AddModelError("EmailError", "Unable to send Email");
                        }
                    }

                    TempData["Error"] = Message;
                    return RedirectToAction("Success");

                }


            }

            ModelState.AddModelError("Error", Error);
            return View(OldTicket);

        }
        [Route("Feedback/Save/{TicketID}/{feedbackRating}")]
        [HttpGet]
        public IActionResult SaveFeedback(string TicketID, int feedbackRating)
        {
            string Error = "";

            Ticket OldTicket = new Ticket()
            {
                TicketID = TicketID,
                TicketInfo = _dbContext.GetTicketInfo(TicketID)
            };


            if (ModelState.IsValid)
            {
                ICMTicket _ticket = new ICMTicket()
                {
                    TicketID = TicketID,
                    ResponseTime = 0,
                    Solution = 0,
                    Accuracy = 0,
                    SupportExperience = feedbackRating,
                    Feedback = "",
                };

                if (_dbContext.AddFeedback(_ticket, out Error) > 0)
                {
                    ModelState.AddModelError("Error", Error);
                    string Message = "";
                    if (_ticket.SupportExperience <= 5 && _ticket.SupportExperience >= 3)
                    {
                        Message = "<img src='../images/satisfied.png'/><br/>" +
                            "Thanks for your Positive Feedback. <br/> " +
                            "We are happy to serve you better.<br/> " +
                            "Your feedback encourage us to deliverer better" +
                            "";
                    }
                    else if (_ticket.SupportExperience < 3)
                    {
                        Message = " <img src='../images/dissatisfied.png' /> <br/> " +
                            "Thanks for your valuable Feedback.<br/> " +
                            "We are sorry, we already escalate the Ticket and will assign concern team to help you better.";

                        _emailClient = new EmailForward(_config, _dbContext);
                        _emailClient.SendICMEmail(OldTicket.TicketInfo);
                    }

                    TempData["Error"] = Message;
                    return RedirectToRoute("defaultFeedback", new { _ticket.TicketID, feedbackRating });

                }


            }

            ModelState.AddModelError("Error", Error);
            return View(OldTicket);

        }
        [Route("Test/{TicketId?}", Name = "TestDefault")]
        [Route("Ticket/Test/{TicketId?}")]
        public IActionResult Test(string TicketId = "UCM000000567513")
        {
            ViewData["content"] = _emailClient.ReturnEmailContent(TicketId);
            return View();

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

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Ticket ticket)
        {
            if(!ModelState.IsValid)
            {

            }

            return NoContent();
        }

    }
}
