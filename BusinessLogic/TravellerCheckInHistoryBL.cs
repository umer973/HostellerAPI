using KI.RIS.DAL;
using Modals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessData;

namespace BusinessLogic
{
    public class TravellerCheckInHistoryBL
    {
        TravellerCheckInHistoryDL _travllerDL = new TravellerCheckInHistoryDL();
        public string AddTravellerCheckInDetails(TravellerCheckIn _traveller)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _travllerDL.AddTravellerCheckInDetails(_traveller, transaction);
                if (resultID > 0)
                {
                    message = "Data saved successfully";
                }

            }
            catch(Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "AddTravellerCheckInDetails");

                IsSuccess = false;
                throw;
            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }
            return message;
        }
    }
}
