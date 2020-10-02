using KI.RIS.
    DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Net.WebRequestMethods;

namespace BusinessData
{
    public static class ErrorLogDL
    {
        public static void InsertErrorLog(string errorMessge, string pageName)
        {
            IDbTransaction transaction = null;
            try
            {

                transaction = DALHelper.GetTransaction();
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "InsertErrorLog"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "ErrorMessage":
                            Item.Value = errorMessge;

                            break;
                        case "PageName":
                            Item.Value = pageName;
                            break;
                        case "Host":
                            Item.Value = HttpContext.Current.Request.UserHostAddress;
                            break;


                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "InsertErrorLog", paramData));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DALHelper.CloseDB(transaction, true);
            }
        }
    }
}
