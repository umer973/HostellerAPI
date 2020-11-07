using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Modals;
using BusinessLogic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.IO;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.Configuration;
using System.Data;
using HostellerAPI.Common;

namespace HostellerAPI.Controllers
{
    [Authorize]
    public class HostelController : ApiController
    {
        HostelBL _hostelBL;

        public  HostelController()
        {
            _hostelBL = new HostelBL();
        }

        [AllowAnonymous]
        [Route("api/UpdateHostelProfile")]
        public async Task<IHttpActionResult> POST()
        {

            // Check if the request contains multipart/form-data.  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;
            //access files  
            IList<HttpContent> files = provider.Files;
            if (files.Count > 0)
            { }

            HttpContent file1 = files[0];
            var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');
            string filename = String.Empty;
            Stream input = await file1.ReadAsStreamAsync();
            string directoryName = String.Empty;
            string URL = String.Empty;
            string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];


            var path = HttpRuntime.AppDomainAppPath;
            directoryName = System.IO.Path.Combine(path, "Images");
            filename = System.IO.Path.Combine(directoryName, thisFileName);

            //Deletion exists file  
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string DocsPath = tempDocUrl + "/" + "Images" + "/";
            URL = DocsPath + thisFileName;



            //Directory.CreateDirectory(@directoryName);  
            try
            {
                using (Stream file = File.OpenWrite(filename))
                {
                    input.CopyTo(file);
                    //close file  
                    file.Close();
                }
            }

            catch { }

            var hostel = new Hostel
            {
                hostelName = formData["hostelName"],
                address = formData["address"],
                websiteLink = formData["websiteLink"],
                doubleBedRooms = Convert.ToInt32(formData["doubleBedRooms"]),
                femaleDormRooms = Convert.ToInt32(formData["femaleDormRooms"]),
                hostelDormRoomwithBunks = Convert.ToInt32(formData["hostelDormRoomwithBunks"]),
                hostelDormRoomwithoutBunks = Convert.ToInt32(formData["hostelDormRoomwithoutBunks"]),
                profilePic = URL,
                singleBedRooms = Convert.ToInt32(formData["singleBedRooms"]),
                userId = Convert.ToInt32(formData["userId"]),
                cityName = formData["cityName"],

            };

            return Ok(_hostelBL.UpdateHostelUser(hostel));
        }
       
        [Route("api/AddGallery")]
        public async Task<IHttpActionResult> AddGallery()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;
            Int32 hostelId = Convert.ToInt32(formData["hostelId"]);
            //access files  
            IList<HttpContent> files = provider.Files;

            DataTable dtGallery = new DataTable();
            dtGallery.Columns.Add("ImageUrl");

            for (int i = 0; i < files.Count; i++)
            {
                HttpContent file1 = files[i];
                var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');
                string filename = String.Empty;
                Stream input = await file1.ReadAsStreamAsync();
                string directoryName = String.Empty;
                string URL = String.Empty;
                string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];
                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, "Images");
                filename = System.IO.Path.Combine(directoryName, thisFileName);

                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                string DocsPath = tempDocUrl + "/" + "Images" + "/";
                URL = DocsPath + thisFileName;
                DataRow dr = dtGallery.NewRow();
                dr["ImageUrl"] = URL;
                dtGallery.Rows.Add(dr);

                using (Stream file = File.OpenWrite(filename))
                {
                    input.CopyTo(file);
                    //close file  
                    file.Close();
                }
            }

            return Ok(_hostelBL.AddGallery(dtGallery, hostelId));
        }

       
        [Route("api/GetGallery")]
        public IHttpActionResult GET(string hostelId)
        {
            return Ok(_hostelBL.GetGallery(Convert.ToInt32(hostelId)));
        }

      
        [Route("api/GetHostels")]
        public IHttpActionResult GetHostels(string key)
        {
            return Ok(_hostelBL.GetHostels(key));
        }

       
        [Route("api/GetHostelProfile")]
        public IHttpActionResult GetHostelProfile(string  hostelId)
        {
            return Ok(_hostelBL.GetHostelProfile(Convert.ToInt32(hostelId)));
        }

       
        [Route("api/GetAllTravellerCheckInDetails")]
        public IHttpActionResult GetTravellerCheckInDetails(Int64 hostelId, string mode)
        {

            return Ok(_hostelBL.GetAllTravellerCheckInDetails(hostelId, mode));
        }

    }


}

