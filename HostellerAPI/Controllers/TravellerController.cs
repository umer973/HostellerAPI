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
using BusinessData;

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
        public IHttpActionResult GetTravellerProfile(int travelerId)
        {
            return Ok(travellerBL.GetTravellerProfile(travelerId));
        }

        [Route("api/CheckInCheckOut")]
        public async Task<IHttpActionResult> CheckInCheckOut()
        {
            TravellerCheckIn _traveller = new TravellerCheckIn();

            ErrorLogDL.InsertErrorLog("Log","Logging");

            // Check if the request contains multipart/form-data.  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;
            try
            {
                _traveller.hostelId = Convert.ToInt32(formData["hostelId"]);
                _traveller.travellerQRCode = formData["travellerQRCode"];
                _traveller.trackingId = string.IsNullOrEmpty(formData["trackingId"]) ? 0 : Convert.ToInt64(formData["trackingId"]);

                //   _traveller.checkInDate = string.IsNullOrEmpty(formData["checkInDate"]) ? Convert.ToDateTime(formData["checkInDate"]) : System.DateTime.Now;
                //  _traveller.checkOutDate = string.IsNullOrEmpty(formData["checkOutDate"]) ? Convert.ToDateTime(formData["checkOutDate"]) : System.DateTime.Now;

                _traveller.Action = formData["status"];


                if (!string.IsNullOrEmpty(formData["checkInDate"]))
                {
                    _traveller.checkInDate = Convert.ToDateTime(formData["checkInDate"].ToString());
                }
                else
                {
                    _traveller.checkInDate = System.DateTime.Now;
                }
                if (!string.IsNullOrEmpty(formData["checkOutDate"]))
                {
                    _traveller.checkOutDate = Convert.ToDateTime(formData["checkOutDate"].ToString());
                }
                else
                {
                    _traveller.checkOutDate = System.DateTime.Now;
                }


            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "Error Is Logged without conversion " + formData["checkInDate"].ToString());

                ErrorLogDL.InsertErrorLog(ex.Message, "Error Is Logged with conversion" + Convert.ToDateTime(formData["checkInDate"].ToString()));

                ErrorLogDL.InsertErrorLog(ex.Message, formData["hostelId"]);

                ErrorLogDL.InsertErrorLog(ex.Message, formData["trackingId"]);

                ErrorLogDL.InsertErrorLog(ex.Message, formData["status"]);

            }
            return Ok(travellerBL.AddTravellerCheckInDetails(_traveller));
        }

        [Route("api/GetTravellerCheckInHistory")]
        public IHttpActionResult GetTravellerCheckInHistory(Int64 travellerID, string mode)
        {

            return Ok(travellerBL.GetTravellerCheckInHistory(travellerID, mode));
        }


    }
}
