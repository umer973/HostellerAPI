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
                DateTime dt = System.DateTime.Now;
                var day = dt.DayOfWeek;
                var time = dt.TimeOfDay;
                if (resultID > 0)
                {
                    if (_traveller.Action == "CheckIn")
                    {
                        message = "Traveller CheckedIn Sucessfully on " + day + " at " + time;
                    }
                    else if (_traveller.Action == "CheckOut")
                    {
                        message = "Traveller Checked Out Sucessfully on " + day + " at " + time;
                    }

                }
                else if (resultID == -1)
                {
                    message = "Traveller already Checked In ";
                }

            }
            catch (Exception ex)
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
