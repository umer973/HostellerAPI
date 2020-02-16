using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Modals;
using BusinessLogic;

namespace HostellerAPI.Controllers
{
    public class HostelController : ApiController
    {
        LoginBL _loginBl = new LoginBL();
        public IHttpActionResult POST(Hostel _hostel)
        {
            return Ok(_loginBl.RegisterUser(_hostel));
        }
    }
}
