using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLogic;
using Modals;
using System.Collections.Specialized;
using HostellerAPI.Common;
using System.Threading.Tasks;
using BusinessLogic.TravellerBL;

namespace HostellerAPI.Controllers
{
    public class TravellerController : ApiController
    {
        TravellerBL travellerBL = new TravellerBL();

        [Route("api/UpdateTravellerProfile")]
        public async Task<IHttpActionResult> POST()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;

            var traveller = new Traveller()
            {
                firstName = formData["firstName"],
                lastName = formData["lastName"],
              
                UserId =Convert.ToInt32(formData["userId"])

            };
            return Ok(travellerBL.RegisterTravellerUser(traveller));
        }
    }
}
