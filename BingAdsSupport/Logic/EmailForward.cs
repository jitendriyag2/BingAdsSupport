using BingAdsSupport.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using BingAdsSupport.Data;
using BingAdsSupport.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.Encodings.Web;
using System.Net;

namespace BingAdsSupport.Logic
{
    public class EmailForward
    {

        IConfiguration _config;
        ITicketRepository _dbContext;

        public EmailForward(IConfiguration config, ITicketRepository dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }

        public void SendICMEmail(TicketInfo ticket)
        {
            if (ticket != null)
            {
                OwnerInfo owner = _dbContext.GetTicketInfo(ticket.TicketNumber).OwnerInfo;

                string FromAddress = _config["EmailClientConfig:UserAccount"];
                string FromAdressTitle = "ICM Ticket Escalation Team";
                //To Address 
                //string ToAddress = owner.Email1;
                string ToAddress = "solution4all.in@gmail.com";
                //string CcAddress = owner.ManagerEmail;
                string CcAddress = "jitendriyag2a@gmail.com";
                string ToAdressTitle = $"{ticket.OwnerInfo.FirstName} {ticket.OwnerInfo.LastName}";
                string Subject = $"UCM Ticket Escalation Email On Ticket No: {ticket.TicketNumber}  {ticket.TicketSubject}";
                string BodyContent = $"Hi {ticket.OwnerInfo.FirstName} {ticket.OwnerInfo.LastName}," +
                    $"<br/> We are sorry to inform you that your Resolved Ticket no: " +
                    $@"<a href='https://ucm.bingads.microsoft.com/Ticketing/EditTicket/{ticket.TicketNumber}/IssueDetails'>" +
                    $" { ticket.TicketNumber } : {ticket.TicketSubject} </a> " +
                    $"was rated below average, could you please coopearte with customer to help more." +
                    $"<br/><br/> Thanks Regards, <br/> Escalation Team";

                //Smtp Server 
                string SmtpServer = _config["EmailClientConfig:SmtpServer"];
                //Smtp Port Number 
                int SmtpPortNumber = Convert.ToInt32(_config["EmailClientConfig:SmtpPortNumber"]);

                MailKitEmailSend(SmtpServer, SmtpPortNumber, FromAdressTitle, FromAddress,  ToAdressTitle,  ToAddress,  CcAddress, Subject, BodyContent).Wait();
                //SendGridEmailSend(FromAdressTitle, FromAddress, ToAdressTitle, ToAddress, CcAddress, Subject, BodyContent).Wait();
            }



        }

        public async Task MailKitEmailSend(string SmtpServer, int SmtpPortNumber, string FromAdressTitle, string FromAddress, string ToAdressTitle, string ToAddress, string CcAddress, string Subject, string BodyContent)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
            mimeMessage.Cc.Add(new MailboxAddress(CcAddress));
            mimeMessage.Subject = Subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = BodyContent

            };

            using (var client = new SmtpClient())
            {
                client.Connect(SmtpServer, SmtpPortNumber, false);
                // Note: only needed if the SMTP server requires authentication 
                // Error 5.5.1 Authentication  
                var Password = _config["EmailClientConfig:Password"];
                await client.AuthenticateAsync(FromAddress, Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }


        public async Task SendGridEmailSend(string FromAdressTitle, string FromAddress, string ToAdressTitle, string ToAddress, string CcAddress, string Subject, string BodyContent)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(FromAddress, FromAdressTitle);
            var subject = new List<string>() { Subject, Subject };

            var tos = new List<EmailAddress>()
            {
                new EmailAddress(ToAddress, ToAdressTitle),
                new EmailAddress(CcAddress)
            };

            //var to = new EmailAddress(ToAddress, ToAdressTitle);
            var plainTextContent = " ";
            var htmlContent = BodyContent;
            try
            {
                var msg = MailHelper.CreateMultipleEmailsToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent, new List<Dictionary<string, string>>() {   });
                var response = await client.SendEmailAsync(msg);
            }
            catch(Exception ex)
            { }
        }
    }
}