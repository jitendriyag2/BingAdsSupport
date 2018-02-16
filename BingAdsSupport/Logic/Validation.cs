using BingAdsSupport.Data;
using BingAdsSupport.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BingAdsSupport.Logic
{
    public class Validation
    {
        private AppDbContext _db;

        public Validation(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Method to validate the ICM Ticket no while receiving from client Request using QueryString
        /// </summary>
        /// <param name="icm"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        
        public bool ValidateTicketNo(string icmNo, out string ErrorMessage)
        {
            ErrorMessage = "";
            //Checking the ICM Ticket is valid or not
            if (icmNo.StartsWith("UCM"))//Checking the ICM Ticket started with UCM or not
            {
                if (icmNo.Length == 15)//Checking the ICM Ticket length
                {
                    return true;
                }
                else
                {
                    ErrorMessage = "Invalid ICM No.";
                    return false;
                }
            }
            else
            {
                ErrorMessage = "ICM No. is not in proper format";
                return false;
            }
        }

        //[AcceptVerbs("Get", "Post")]
        //public IActionResult ValidateTicketNo(string icmNo)
        //{
        //    //Checking the ICM Ticket is valid or not
        //    if (icmNo.StartsWith("UCM"))//Checking the ICM Ticket started with UCM or not
        //    {
        //        if (icmNo.Length == 15)//Checking the ICM Ticket length
        //        {
        //            return Json(data: true);
        //        }
        //        else
        //        {
        //            return Json(data: "Invalid ICM No.");
        //        }
        //    }
        //    else
        //    {
        //        return Json(data: "ICM No. is not in proper format");
        //    }
        //}

        public bool ICMExistInDbOrNot(string ICMTicketNo)
        {
            return _db.ICMTickets.FirstOrDefault( x=> x.TicketID == ICMTicketNo) == null  ? false : true;
        }

    }


}
