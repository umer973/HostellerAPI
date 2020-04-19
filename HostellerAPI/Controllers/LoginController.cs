using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLogic;
using Modals;
using System.Security.Claims;
using System.Threading.Tasks;
using HostellerAPI.Common;

namespace HostellerAPI.Controllers
{
    
    public class LoginController : ApiController
    {
        LoginBL _loginBL;
        LoginController()
        {
            _loginBL = new LoginBL();

        }
     
        [Authorize]
        public IHttpActionResult GET()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Api Is Running 3 "+identity.Name);
        }

        [AllowAnonymous]
        public IHttpActionResult POST(User _user)
        {
            return Ok(_loginBL.Login(_user));
        }


       


    }

    
}
