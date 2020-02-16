using KI.RIS.Enumerators.Common;
using KI.RIS.General.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HostellerAPI
{
    public class APIException : Exception
    {
        public APIException()
        {
            MessageType = ValidationMessageType.Info;
        }
        public ValidationMessageType MessageType { get; set; }

        public APIException(string message) : base(MessageLib.GetMultilingualMessage(message)) { MessageType = ValidationMessageType.Info; }
        public APIException(ValidationMessageType messageType, string message) : base(MessageLib.GetMultilingualMessage(message))
        {
            MessageType = messageType;
        }
        //public ProcessException(string message, Exception innerException):base(message,innerException)
        //{

        //}
    }
}