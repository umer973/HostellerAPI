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
    public class LoginController : ApiController
    {
        LoginBL _loginBL;
        LoginController()
        {
            _loginBL = new LoginBL();

        }
        public IHttpActionResult GET()
        {

            return Ok("Api Is Running 1");
        }
        public IHttpActionResult POST(User _user)
        {
            return Ok(_loginBL.Login(_user));
        }
    }

    
}
