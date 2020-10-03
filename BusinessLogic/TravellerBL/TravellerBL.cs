using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessData.TravellerDL;
using System.Data;
using BusinessData;
using KI.RIS.DAL;
using Modals;
using System.Data.SqlClient;

namespace BusinessLogic.TravellerBL
{
    public class TravellerBL
    {
        TravellerDL _travellerDL = new TravellerDL();

        public string AddTravellerCheckInDetails(TravellerCheckIn _traveller)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _travellerDL.AddTravellerCheckInDetails(_traveller, transaction);
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

        public object GetTravellerCheckIninfo(Int64 travellerID)
        {
            object objResult = null;
            DataTable dtResult = null;
            IDbConnection con = null;
            try
            {
                DateTime dt = System.DateTime.Now;
                var day = dt.DayOfWeek;
                var time = dt.TimeOfDay;

                con = DALHelper.GetConnection();
                dtResult = _travellerDL.GetTravellerChekInInfo(travellerID, con);
                if (dtResult.Rows.Count > 0)
                {
                    objResult = "CheckedIn on " + day + " at " + time;
                }
                else
                {
                    objResult = null;
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetTravellerCheckIninfo");
            }

            return objResult;

        }

        public string RegisterTravellerUser(Traveller _traveller)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _travellerDL.RegisterTravellerUser(_traveller, transaction);

                if (resultID > 0)
                {
                    message = "Profile Updated Successfully";
                }
                else
                {
                    message = "error";
                }

            }
            catch (SqlException ex)
            {
                IsSuccess = false;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    message = "Email already taken";
                }
                else
                {
                    IsSuccess = false;
                    ErrorLogDL.InsertErrorLog(ex.Message, "TravellerBL:RegisterTravellerUser");
                    throw;
                }
            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }

            return message;
        }
    }
}
