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
   
    public class HostelController : ApiController
    {
        LoginBL _loginBl = new LoginBL();
        HostelBL _hostelBL = new HostelBL();


        public async Task<IHttpActionResult> POST()
        {
            Hostel _hostel = new Hostel();
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


            HttpContent file1 = files[0];
            var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');



            string filename = String.Empty;
            Stream input = await file1.ReadAsStreamAsync();
            string directoryName = String.Empty;
            string URL = String.Empty;
            string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];


            var path = HttpRuntime.AppDomainAppPath;
            directoryName = System.IO.Path.Combine(path, "ClientDocument");
            filename = System.IO.Path.Combine(directoryName, thisFileName);

            //Deletion exists file  
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
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

            _hostel.hostelName = formData["hostelName"];
            _hostel.username = formData["username"];
            _hostel.password = formData["password"];
            _hostel.emailId = formData["emailId"];
            _hostel.address = formData["address"];
            _hostel.websiteLink = formData["websiteLink"];
            _hostel.doubleBedRooms = Convert.ToInt32(formData["doubleBedRooms"]);
            _hostel.femaleDormRooms = Convert.ToInt32(formData["femaleDormRooms"]);
            _hostel.hostelDormRoomwithBunks = Convert.ToInt32(formData["hostelDormRoomwithBunks"]);
            _hostel.hostelDormRoomwithoutBunks = Convert.ToInt32(formData["hostelDormRoomwithoutBunks"]);
            _hostel.profilePic = URL;
            _hostel.singleBedRooms = Convert.ToInt32(formData["singleBedRooms"]);
            _hostel.cityName = formData["cityName"];


            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("DocsUrl", URL);
            return Ok(_loginBl.RegisterUser(_hostel));
        }


        [Route("api/Hostel/UpdateProfile")]
        public async Task<IHttpActionResult> UpdateProfile()
        {
            Hostel _hostel = new Hostel();
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

            HttpContent file1 = files[0];
            var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');


            string filename = String.Empty;
            Stream input = await file1.ReadAsStreamAsync();
            string directoryName = String.Empty;
            string URL = String.Empty;
            string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];


            var path = HttpRuntime.AppDomainAppPath;
            directoryName = System.IO.Path.Combine(path, "ClientDocument");
            filename = System.IO.Path.Combine(directoryName, thisFileName);

            //Deletion exists file  
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
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

            _hostel.hostelName = formData["hostelName"];
            _hostel.address = formData["address"];
            _hostel.websiteLink = formData["websiteLink"];
            _hostel.doubleBedRooms = Convert.ToInt32(formData["doubleBedRooms"]);
            _hostel.femaleDormRooms = Convert.ToInt32(formData["femaleDormRooms"]);
            _hostel.hostelDormRoomwithBunks = Convert.ToInt32(formData["hostelDormRoomwithBunks"]);
            _hostel.hostelDormRoomwithoutBunks = Convert.ToInt32(formData["hostelDormRoomwithoutBunks"]);
            _hostel.profilePic = URL;
            _hostel.singleBedRooms = Convert.ToInt32(formData["singleBedRooms"]);
            Int32 hostelId = Convert.ToInt32(formData["hostelId"]);
            _hostel.cityName = formData["cityName"];



            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("DocsUrl", URL);
            return Ok(_hostelBL.UpdateHostelUser(_hostel, hostelId));
        }

        [Route("api/Hostel/AddGallery")]
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
                directoryName = System.IO.Path.Combine(path, "ClientDocument");
                filename = System.IO.Path.Combine(directoryName, thisFileName);

                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
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


        public IHttpActionResult GET(string hostelId)
        {
            return Ok(_hostelBL.GetGallery(Convert.ToInt32(hostelId)));
        }

        [Route("api/Hostel/GetHostels")]
        public IHttpActionResult GetHostels(string cityName)
        {
            return Ok(_hostelBL.GetHostels(cityName));
        }

    }


}

