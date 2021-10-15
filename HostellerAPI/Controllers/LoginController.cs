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
using System.Collections.Specialized;
using CommonLib.Encryption;

namespace HostellerAPI.Controllers
{

    public class LoginController : ApiController
    {
        LoginBL _loginBL;
        LoginController()
        {
            _loginBL = new LoginBL();

        }
      
        [Route("api/TestApi")]
        public IHttpActionResult GET()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Api Is Running 3 " + identity.Name);
        }

        [AllowAnonymous]
        [Route("api/LoginUser")]
        public async Task<IHttpActionResult> POST()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;

            var user = new User
            {
                username = formData["userName"],
                password = Encryption.encrypt(formData["password"]),
                email = formData["email"],
                deviceToken = "",

            };

            return Ok(_loginBL.Login(user));
        }
    }


}
