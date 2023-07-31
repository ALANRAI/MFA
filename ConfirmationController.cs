using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using static MFA_assignment.Models.Confirmation;
using MFA_assignment.Models;

namespace MFA_assignment.Controllers
{
    public class ConfirmationController : ApiController
    {
        private static Dictionary<string, string> activeConfirmationCodes = new Dictionary<string, string>();
        private readonly CodeFileManager codeFileManager = new CodeFileManager("codes.txt");

        // POST /send-code
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("send-code")]
        public IHttpActionResult SendConfirmationCode([FromBody] SendConfirmationCodeRequest request)
        {
            if (string.IsNullOrEmpty(request.Phone))
            {
                return BadRequest("Phone number is required.");
            }

            // Check if the phone already has a confirmation code active
            if (activeConfirmationCodes.ContainsKey(request.Phone))
            {
                return BadRequest("A confirmation code is already sent for this phone number.");
            }

            // Generate a random confirmation code
            var confirmationCode = GenerateConfirmationCode();

            // Save the confirmation code to the active codes list (you can use a database in a real application)
            activeConfirmationCodes[request.Phone] = confirmationCode;
           
            // In a real application, you would send the code to the user's phone here
            // For this example, we'll just log it
            Console.WriteLine($"Confirmation code sent to {request.Phone}: {confirmationCode}");

            return Ok(new ApiResponse { Sent = true, GetThisCodeonPhone = confirmationCode });
        }

        // POST /check-code
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("check-code")]
        public IHttpActionResult CheckConfirmationCode([FromBody] CheckConfirmationCodeRequest request)
        {
            if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.Code))
            {
                return BadRequest("Phone number and confirmation code are required.");
            }

            // Check if the phone has an active confirmation code
            if (!activeConfirmationCodes.ContainsKey(request.Phone))
            {
                return BadRequest("No active confirmation code found for this phone number.");
            }


            // Get the active confirmation code for the phone
            var activeCode = activeConfirmationCodes[request.Phone];

            // Check if the received code matches the active code
            var isValid = string.Equals(request.Code, activeCode);          

            // Clean up the active confirmation code (you can set a time limit in a real application)
            activeConfirmationCodes.Remove(request.Phone);

            return Ok(new ApiResponse { Valid = isValid });
        }

        // Helper method to generate a random 6-digit confirmation code
        private string GenerateConfirmationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }

}