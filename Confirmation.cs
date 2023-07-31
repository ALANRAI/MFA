using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFA_assignment.Models
{
    public class Confirmation
    {
        // Request model for sending confirmation code
        public class SendConfirmationCodeRequest
        {
            public string Phone { get; set; }
        }

        // Request model for checking confirmation code
        public class CheckConfirmationCodeRequest
        {
            public string Phone { get; set; }
            public string Code { get; set; }
        }

        // Response model for both endpoints
        public class ApiResponse
        {
            public bool Sent { get; set; }
            public bool Valid { get; set; }
            public string ErrorMessage { get; set; }
            // this property is for testing only
            public string GetThisCodeonPhone { get; set; }
        }

    }
}
