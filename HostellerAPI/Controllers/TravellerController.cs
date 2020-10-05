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

                UserId = Convert.ToInt32(formData["userId"])

            };
            return Ok(travellerBL.RegisterTravellerUser(traveller));
        }

        [Route("api/GetTravellerProfile")]
        public  IHttpActionResult GetTravellerProfile(int travelerId)
        {
            return Ok(travellerBL.GetTravellerProfile(travelerId));
        }

        [Route("api/CheckInCheckOut")]
        public async Task<IHttpActionResult> CheckInCheckOut()
        {
            TravellerCheckIn _traveller = new TravellerCheckIn();
            // Check if the request contains multipart/form-data.  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;

            _traveller.hostelId = Convert.ToInt32(formData["hostelId"]);
            _traveller.travellerQRCode = formData["travellerQRCode"];
            if (formData["checkInDate"] != "")
            {
                _traveller.checkInDate = Convert.ToDateTime(formData["checkInDate"]);
                _traveller.Action = "1";
            }
            else
            {
                _traveller.checkInDate = System.DateTime.Now;
            }
            if (formData["checkOutDate"] != "")
            {
                _traveller.checkOutDate = Convert.ToDateTime(formData["checkOutDate"]);
                _traveller.Action = "2";
            }
            else
            {
                _traveller.checkOutDate = System.DateTime.Now;
            }

            return Ok(travellerBL.AddTravellerCheckInDetails(_traveller));
        }
    }
}
