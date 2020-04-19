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
    public class TravellerCheckInController : ApiController
    {
        TravellerCheckInHistoryBL _travellerBL = new TravellerCheckInHistoryBL();

        public async Task<IHttpActionResult> POST()
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
                _traveller.Action = "CheckIn";
            }
            else
            {
                _traveller.checkInDate= System.DateTime.Now;
            }
            if (formData["checkOutDate"] != "")
            {
                _traveller.checkOutDate = Convert.ToDateTime(formData["checkOutDate"]);
                _traveller.Action = "CheckOut";
            }
            else
            {
                _traveller.checkOutDate = System.DateTime.Now;
            }

            return Ok(_travellerBL.AddTravellerCheckInDetails(_traveller));
        }
    }
}
