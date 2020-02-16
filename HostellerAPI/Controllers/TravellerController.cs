using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLogic;
using Modals;

namespace HostellerAPI.Controllers
{
    public class TravellerController : ApiController
    {
        LoginBL _loginBL = new LoginBL();

        public IHttpActionResult POST(Traveller _traveller)
        {
            return Ok(_loginBL.RegisterTravellerUser(_traveller));
        }
    }
}
