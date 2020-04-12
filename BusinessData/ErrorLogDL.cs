using KI.RIS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessData
{
    public static class ErrorLogDL
    {
        public static void InsertErrorLog(string errorMessge, string MethodName)
        {
            try
            {
                IDbTransaction transaction = null;
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
                        case "MethodName":
                            Item.Value = errorMessge;
                            break;


                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "InsertErrorLog", paramData));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
