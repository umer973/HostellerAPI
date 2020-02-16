using KI.RIS.General.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;


namespace HostellerAPI
{
    public class WrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            return BuildApiResponse(request, response,true);

        }

        private static HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response, bool isLicenseValid)
        {
            object content;
            string errorMessage = null;
            HttpError error = null;
            int ValidationMessageType = 1; // enum ValidationMessageType        
            bool IsAPIException = false;

            if (isLicenseValid == false)
            {
                ValidationMessageType = (int)KI.RIS.Enumerators.Common.ValidationMessageType.Blocking; // enum ValidationMessageType    
                                                                                                       //  response = new HttpResponseMessage(HttpStatusCode.Accepted);
                response.StatusCode = HttpStatusCode.Accepted;
                content = GlobalCaching.LicenseExpiredMessage;
                IsAPIException = true;
                response.Headers.Add("ValidationMessageType", ((byte)KI.RIS.Enumerators.Common.ValidationMessageType.Blocking).ToString());
            }
            else
            {
                ValidationMessageType = 1; // enum ValidationMessageType        
                                           //  Info = 1,
                                           //Blocking = 2,
                                           //Warning = 3,
                                           //YesNoCancel = 4,
                                           //OkCancel = 5


                IEnumerable<string> values = null;


                if (response.TryGetContentValue(out content)) //  && !response.IsSuccessStatusCode
                {
                    ///it will if the Controller handle the exact HttpStatusCodes eg: Resource -  public IHttpActionResult Delete(Int64 resourceMasterId, int categoryType) 
                    if (content is APIException)
                    {
                        APIException objException = (APIException)content;
                        IsAPIException = true;
                        ValidationMessageType = (int)objException.MessageType;
                        content = objException.Message;
                    }
                }

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    content = MessageLib.Error; // "Please contact technical support team..............."; /// This message hardcode in client side
                                                //content = MessageLib.Error;
                }
                else if (!IsAPIException && response.StatusCode == HttpStatusCode.Accepted)
                {
                    content = response.ReasonPhrase;
                    HttpHeaders headers = response.Headers;
                    headers.TryGetValues("ValidationMessageType", out values);
                    ValidationMessageType = Convert.ToInt16(((string[])values)[0]);
                }
            }
            var newResponse = request.CreateResponse(response.StatusCode
                , new ApiResponse(response.StatusCode, content, ValidationMessageType, response.RequestMessage, response));

            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }



            //if (error != null)
            //{
            //    newResponse.Headers.Add("ExceptionType", error.ExceptionType);
            //    newResponse.Headers.Add("ExceptionMessage", error.ExceptionMessage);
            //}

            return newResponse;
        }
    }


    public class ApiResponse
    {

        public string Version
        {
            get
            {
                StringBuilder VersionData = new StringBuilder();
                VersionData.Append(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString());
                VersionData.Append(".");
                VersionData.Append(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString());
                return VersionData.ToString();

            }
        }

        public int StatusCode { get; set; }


        public string ErrorMessage { get; set; }


        public int ValidationMessageType { get; set; }


        public object Result { get; set; }

        public string Message { get; set; }

        public ApiResponse(HttpStatusCode statusCode, object result,
            int validationMessageType, HttpRequestMessage requestMessage, HttpResponseMessage response) //  string errorMessage = null,
        {
            StatusCode = (int)statusCode;
            ValidationMessageType = validationMessageType;
            switch (statusCode)
            {
                case HttpStatusCode.OK: // 200
                    Result = result;
                    if (requestMessage.Method.ToString().Equals("POST"))
                    {
                        this.Message = MessageLib.Save;
                    }
                    else if (requestMessage.Method.ToString().Equals("PUT"))
                    {
                        this.Message = MessageLib.Update;
                    }
                    else if (requestMessage.Method.ToString().Equals("DELETE"))
                    {
                        this.Message = MessageLib.Delete;
                    }
                    break;
                case HttpStatusCode.Accepted: //202
                    Result = "";
                    this.Message = result.ToString();
                    break;
                case HttpStatusCode.Unauthorized: //401
                    ValidationMessageType = 2;
                    response.StatusCode = HttpStatusCode.Accepted;
                    StatusCode = (int)HttpStatusCode.Accepted;
                    Result = "";
                    this.Message = "Authorization has been denied for this request.(Session validity is expired)"; //result.ToString();
                    break;
                //
                default:
                    break;
            }
        }


    }
}