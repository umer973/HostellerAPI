
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace HostellerAPI
{
    public class APIExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            if (actionExecutedContext.Exception is APIException)
            {
                APIException exceptionData = (APIException)actionExecutedContext.Exception;

                //The Response Message Set by the Action During Ececution
                var res = exceptionData.Message;

                //Define the Response Message

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    Content = new StringContent(exceptionData.ToString()),
                    StatusCode = HttpStatusCode.Accepted,
                    ReasonPhrase = res
                };

                response.Headers.Add("ValidationMessageType", ((byte)exceptionData.MessageType).ToString());

                //Create the Error Response

                actionExecutedContext.Response = response;
            }
            else
            {

                ///exception loging
                //BLGenExceptionLog objException = new BLGenExceptionLog();
                //objException.SaveException(actionExecutedContext.Exception, "", "", 1, "");

                var res = actionExecutedContext.Exception.Message;
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(res),
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = res
                };

                //Create the Error Response
                actionExecutedContext.Response = response;


            }
        }
    }

    //[DataContract]
    //public class ApiResponse
    //{
    //    [DataMember]
    //    public string Version
    //    {
    //        get
    //        {
    //            StringBuilder VersionData = new StringBuilder();
    //            VersionData.Append(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString());
    //            VersionData.Append(".");
    //            VersionData.Append(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString());
    //            return VersionData.ToString();

    //        }
    //    }

    //    [DataMember]
    //    public int StatusCode { get; set; }

    //    [DataMember(EmitDefaultValue = false)]
    //    public string ErrorMessage { get; set; }

    //    [DataMember(EmitDefaultValue = false)]
    //    public object Result { get; set; }

    //    public ApiResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
    //    {
    //        StatusCode = (int)statusCode;
    //        Result = result;
    //        ErrorMessage = errorMessage;
    //    }
    //}
}