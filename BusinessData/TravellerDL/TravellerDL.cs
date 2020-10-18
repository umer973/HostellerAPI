using KI.RIS.DAL;
using Modals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessData.TravellerDL
{
    public class TravellerDL
    {



        public Int16 RegisterTravellerUser(Traveller _traveller, IDbTransaction transaction)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "InsertTravellerProfile"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "UserId":
                            Item.Value = _traveller.UserId;
                            break;
                        case "Password":
                            Item.Value = _traveller.password;
                            break;
                        case "EmailId":
                            Item.Value = _traveller.emailId;
                            break;
                        case "Address":
                            Item.Value = "";
                            break;
                        case "UserType":
                            Item.Value = "Traveller";
                            break;
                        case "FirstName":
                            Item.Value = _traveller.firstName;
                            break;
                        case "LastName":
                            Item.Value = _traveller.lastName;
                            break;

                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "InsertTravellerProfile", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "RegisterTravellerUser");
                throw ex;
            }
        }

        public DataTable GetTravellerProfile(Int64 travellerID, IDbConnection con)
        {
            DataSet dsResult = new DataSet();
            try
            {
                IDbDataParameter[] paramData;

                paramData = DALHelperParameterCache.GetSpParameterSet(con, "GetTravellerProfile"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "TravellerId":
                            Item.Value = travellerID;

                            break;

                    }
                }
                DALHelper.FillDataset(con, CommandType.StoredProcedure, "GetTravellerProfile", dsResult, new string[] { "TravellerProfile" }, paramData);

                return dsResult.Tables.Contains("TravellerProfile") ? dsResult.Tables["TravellerProfile"] : null;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetTravellerProfile");
                throw ex;
            }
        }

        public Int16 AddTravellerCheckInDetails(TravellerCheckIn _traveller, IDbTransaction transaction)
        {

            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "InsertTravellerCheckInCheckOutDetails"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "HostelId":
                            Item.Value = _traveller.hostelId;

                            break;
                        case "QRCode":
                            Item.Value = _traveller.travellerQRCode;
                            break;

                        case "Status":
                            Item.Value = _traveller.Action;
                            break;
                        case "checkInDate":
                            Item.Value = _traveller.checkInDate;
                            break;
                        case "checkOutDate":
                            Item.Value = _traveller.checkOutDate;
                            break;
                        case "TrackingId":
                            Item.Value = _traveller.trackingId;
                            break;


                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "InsertTravellerCheckInCheckOutDetails", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "AddTravellerCheckInDetails");
                throw ex;
            }
        }

        public DataTable GetTravellerChekInInfo(TravellerCheckIn _traveller, IDbTransaction transaction, string mode)
        {
            DataSet dsResult = new DataSet();
            try
            {
                IDbDataParameter[] paramData;

                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "GetTravellerCheckinDetails"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "TravellerId":
                            Item.Value = 0;
                            break;
                        case "TravellerQrCode":
                            Item.Value = _traveller.travellerQRCode;
                            break;
                        case "HostelId":
                            Item.Value = _traveller.hostelId;
                            break;
                        case "Mode":
                            Item.Value = mode;

                            break;

                    }
                }
                DALHelper.FillDataset(transaction, CommandType.StoredProcedure, "GetTravellerCheckinDetails", dsResult, new string[] { "Traveller" }, paramData);

                return dsResult.Tables.Contains("Traveller") ? dsResult.Tables["Traveller"] : null;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetTravellerChekInInfo");
                throw ex;
            }
        }

        public DataTable GetTravellerCheckInHistory(Int64 travellerId, IDbConnection con, string mode)
        {
            DataSet dsResult = new DataSet();
            try
            {
                IDbDataParameter[] paramData;

                paramData = DALHelperParameterCache.GetSpParameterSet(con, "GetTravellerCheckinDetails"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "TravellerId":
                            Item.Value = travellerId;
                            break;
                        case "Mode":
                            Item.Value = mode;

                            break;

                    }
                }
                DALHelper.FillDataset(con, CommandType.StoredProcedure, "GetTravellerCheckinDetails", dsResult, new string[] { "Traveller" }, paramData);

                return dsResult.Tables.Contains("Traveller") ? dsResult.Tables["Traveller"] : null;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetTravellerCheckInHistory");
                throw ex;
            }
        }
    }
}
