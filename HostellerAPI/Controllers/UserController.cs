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
   
    public class UserController : ApiController
    {
        LoginBL _loginBL;
        public UserController()
        {
            _loginBL = new LoginBL();
        }

        [Route("api/RegisterUser")]
        public async Task<IHttpActionResult> RegisterUser()
        {
            User _user = new User();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;

            _user.username = formData["userName"];
            _user.password = formData["password"];
            _user.email = formData["email"];
            _user.userType = formData["userType"];

            return Ok(_loginBL.RegisterUser(_user));

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
        [Route("api/PostFeedback")]
       
        public IHttpActionResult POST()
        {

        }
    }
}
