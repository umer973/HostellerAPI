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

        public object AddTravellerCheckInDetails(TravellerCheckIn _traveller)
        {
            bool IsSuccess = true;
            IDbTransaction transaction = null;
            object objResult = null;
            DataTable dtResult = null;
            IDbConnection con = null;

            try
            {
                transaction = DALHelper.GetTransaction();
                con = DALHelper.GetConnection();

                Int64 resultID = _travellerDL.AddTravellerCheckInDetails(_traveller, transaction);

                if (resultID > 0)
                {
                    if (_traveller.Action == "1") // Check In
                    {
                        dtResult = _travellerDL.GetTravellerChekInInfo(_traveller, transaction, "4");
                        objResult = dtResult;

                    }
                    else if (_traveller.Action == "2") // Check Out
                    {
                        dtResult = _travellerDL.GetTravellerChekInInfo(_traveller, transaction, "5");
                        objResult = dtResult;
                    }

                }
                else if (resultID == -1)
                {
                    objResult = "Traveller already Checked In";
                }
                else if (resultID == -3)
                {
                    objResult = "Traveller is not Checked In";
                }
                else if (resultID == -4)
                {
                    objResult = "QRCode not found";
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

        public object GetTravellerProfile(Int64 travellerID)
        {
            object objResult = null;
            DataTable dtResult = null;
            IDbConnection con = null;
            try
            {


                con = DALHelper.GetConnection();
                dtResult = _travellerDL.GetTravellerProfile(travellerID, con);
                if (dtResult.Rows.Count > 0)
                {
                    objResult = dtResult;
                }
                else
                {
                    objResult = null;
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetTravellerProfile");
            }


            return objResult;

        }

        public object GetTravellerCheckInHistory(Int64 travellerID,string mode)
        {
            object objResult = null;
            DataTable dtResult = null;
            IDbConnection con = null;
            try
            {


                con = DALHelper.GetConnection();
                dtResult = _travellerDL.GetTravellerCheckInHistory(travellerID, con, mode);
                if (dtResult.Rows.Count > 0)
                {
                    objResult = dtResult;
                }
                else
                {
                    objResult = "No data found";
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetTravellerCheckInHistory");
            }


            return objResult;

        }
    }
}
