
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
    using CommonLib.Encryption;

    [Authorize]
    public class UserController : ApiController
    {
        LoginBL _loginBL;

        public UserController()
        {
            _loginBL = new LoginBL();
        }

        [AllowAnonymous]
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
            _user.password = Encryption.encrypt(formData["password"]);
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
                password = Encryption.encrypt(formData["oldPassword"]),
            };
            var newPassword = Encryption.encrypt(formData["newPassword"]);

            return Ok(_loginBL.ChangeUserAuthentication(user, newPassword));

        }

        [Route("api/PostFeedback")]
        public IHttpActionResult PostFeedBack()
        {
            return Ok();

        }

        [Route("api/SaveHelpUsText")]
        public async Task<IHttpActionResult> SaveHelpUsText()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;


            Int64 userId = Convert.ToInt32(formData["userId"]);
            string title = formData["title"];
            string message = formData["message"];

            return Ok(_loginBL.InsertHelpUs(userId, title, message));
        }

        [HttpGet]
        [Route("api/ValidateEmail")]
        public IHttpActionResult ValidateEmail(string email)
        {
            return Ok(_loginBL.ValidateEmail(email));
        }

        [Route("api/ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword()
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
                password = Encryption.encrypt(formData["password"])
            };
            return Ok(_loginBL.ResetPassword(user));
        }


    }
}
