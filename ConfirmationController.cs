using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using MFA_assignment.Models;
using static MFA_assignment.Models.Confirmation;
using AllowAnonymousAttribute = System.Web.Mvc.AllowAnonymousAttribute;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using RouteAttribute = System.Web.Mvc.RouteAttribute;

namespace MFA_assignment.Controllers
{
    public class ConfirmationController : ApiController
    {
        private readonly IConfirmationService _confirmationService;

        public ConfirmationController(IConfirmationService confirmationService)
        {
            _confirmationService = confirmationService;
        }

        [HttpPost]
        [Route("send-code")]
        [AllowAnonymous]
        public IHttpActionResult SendConfirmationCode([FromBody] SendConfirmationCodeRequest request)
        {
            return _confirmationService.SendConfirmationCode(request);
        }

        [HttpPost]
        [Route("check-code")]
        [Authorize]
        public IHttpActionResult CheckConfirmationCode([FromBody] CheckConfirmationCodeRequest request)
        {
            return _confirmationService.CheckConfirmationCode(request);
        }
    }

}
