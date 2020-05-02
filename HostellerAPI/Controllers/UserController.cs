using HostellerAPI.Common;

using Modals;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BusinessLogic;

namespace HostellerAPI.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        LoginBL _loginBL;
        public UserController()
        {
            _loginBL = new LoginBL();
        }

        public async Task<IHttpActionResult> POST()
        {
            User _user = new User();

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;

            _user.username = formData["username"];
            _user.password = formData["oldPassword"];
            var newPassword = formData["newPassword"];

            return Ok(_loginBL.ChangeUserAuthentication(_user, newPassword));

        }
    }
}
