
namespace HostellerAPI.Controllers
{
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

        [Route("api/ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword()
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
                userId = Convert.ToInt32(formData["userId"]),
                password = formData["oldPassword"]
            };
            var newPassword = formData["newPassword"];

            return Ok(_loginBL.ChangeUserAuthentication(user, newPassword));

        }

        [Route("api/PostFeedback")]
        public IHttpActionResult PostFeedBack()
        {
            return Ok();

        }

        [Route("api/GetHelpUsText")]
        public IHttpActionResult GetHelpUsText()
        {
            return Ok("Test help text");
        }


    }
}
