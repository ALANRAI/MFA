using static MFA_assignment.Models.Confirmation;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Http.Results;
using System;

namespace MFA_assignment.Controllers
{
    public interface IConfirmationService
    {
        IHttpActionResult SendConfirmationCode(SendConfirmationCodeRequest request);
        IHttpActionResult CheckConfirmationCode(CheckConfirmationCodeRequest request);
    }

}