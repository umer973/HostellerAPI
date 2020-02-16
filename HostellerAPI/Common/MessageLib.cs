
using HostellerAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KI.RIS.General.Common
{
    public static class MessageLib
    {
        public static string Save { get; set; } //= "CmnMsgSave";// Saved Successfully";
        public static string Update { get; set; }//= "CmnMsgUpdate";//"Updated Successfully";
        public static string Delete { get; set; } //= "CmnMsgDelete";// "Deleted Successfully";
        public static string Error { get; set; }
        private static DataTable CustomMessages { get; set; }

        static MessageLib()
        {
            //var OrderLookup = GlobalCaching.GenLookup.AsEnumerable().Where(r =>
            //  Convert.ToString(r["LookupType"]).Equals("MultilingualCustomMessage"));
            //if (OrderLookup.Any())
            //{
            //    CustomMessages = OrderLookup.CopyToDataTable();
            //}

            Save = "Saved";
            Update = "Updated";
            Delete = "Deleted";
            Error = "Error";
        }

        public static string GetMultilingualMessage(string message)
        {

            return message;
        }
    }
}
