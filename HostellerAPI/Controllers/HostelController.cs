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

            _hostel.address = formData["address"];
            _hostel.websiteLink = formData["websiteLink"];
            _hostel.doubleBedRooms = Convert.ToInt32(formData["doubleBedRooms"]);
            _hostel.femaleDormRooms = Convert.ToInt32(formData["femaleDormRooms"]);
            _hostel.hostelDormRoomwithBunks = Convert.ToInt32(formData["hostelDormRoomwithBunks"]);
            _hostel.hostelDormRoomwithoutBunks = Convert.ToInt32(formData["hostelDormRoomwithoutBunks"]);
            _hostel.profilePic = URL;
            _hostel.singleBedRooms = Convert.ToInt32(formData["singleBedRooms"]);
            Int32 hostelId = Convert.ToInt32(formData["hostelId"]);


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

    }




    public class InMemoryMultipartFormDataStreamProvider : MultipartStreamProvider
    {
        private NameValueCollection _formData = new NameValueCollection();
        private List<HttpContent> _fileContents = new List<HttpContent>();

        // Set of indexes of which HttpContents we designate as form data
        private Collection<bool> _isFormData = new Collection<bool>();

        /// <summary>
        /// Gets a <see cref="NameValueCollection"/> of form data passed as part of the multipart form data.
        /// </summary>
        public NameValueCollection FormData
        {
            get { return _formData; }
        }

        /// <summary>
        /// Gets list of <see cref="HttpContent"/>s which contain uploaded files as in-memory representation.
        /// </summary>
        public List<HttpContent> Files
        {
            get { return _fileContents; }
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            // For form data, Content-Disposition header is a requirement
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {
                // We will post process this as form data
                _isFormData.Add(String.IsNullOrEmpty(contentDisposition.FileName));

                return new MemoryStream();
            }

            // If no Content-Disposition header was present.
            throw new InvalidOperationException(string.Format("Did not find required '{0}' header field in MIME multipart body part..", "Content-Disposition"));
        }

        /// <summary>
        /// Read the non-file contents as form data.
        /// </summary>
        /// <returns></returns>
        public override async Task ExecutePostProcessingAsync()
        {
            // Find instances of non-file HttpContents and read them asynchronously
            // to get the string content and then add that as form data
            for (int index = 0; index < Contents.Count; index++)
            {
                if (_isFormData[index])
                {
                    HttpContent formContent = Contents[index];
                    // Extract name from Content-Disposition header. We know from earlier that the header is present.
                    ContentDispositionHeaderValue contentDisposition = formContent.Headers.ContentDisposition;
                    string formFieldName = UnquoteToken(contentDisposition.Name) ?? String.Empty;

                    // Read the contents as string data and add to form data
                    string formFieldValue = await formContent.ReadAsStringAsync();
                    FormData.Add(formFieldName, formFieldValue);
                }
                else
                {
                    _fileContents.Add(Contents[index]);
                }
            }
        }

        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }
    }
}

