using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Threading;

namespace HostellerAPI
{
    public class GlobalCaching
    {
        public static DataTable GenLookup;

        public static DataTable UserPrivilege;

        public static DataTable GenApplicationSetting;

        public static DataTable Hospital;

        public static bool IsAlertServiceStoped;

        public static Dictionary<string, object> InitialDataSchedule;

        public static bool IsHL7Required = false;

        public static Int16 HL7InterfaceMode = 0;  //0->Outbound Table mode, 1-> Old mode (message building)

        public static bool IsInsuranceRequired = false;

        public static string MulitlingualCode = "EN";

        public static DataSet Hl7Data;

        public static bool IsLicenseValid = true;

        public static string LicenseExpiredMessage = "License Expired. Please Contact Support Team";

        public static CancellationToken licenseToken = new CancellationToken();

        //public static bool CheckPrivilege(string privilegeName)
        //{
        //    bool userPrivilege = false;
        //    if (UserPrivilege != null)
        //    {               
        //        if (!string.IsNullOrEmpty(privilegeName) && UserPrivilege != null && UserPrivilege.Rows.Count > 0 &&
        //            UserPrivilege.Select().Where(r => Convert.ToString(r["PrivilegeName"]).ToUpper() == privilegeName.ToUpper()).Any())
        //        {
        //            userPrivilege = true;
        //        }
        //    }
        //    return userPrivilege;
        //}
    }
}
