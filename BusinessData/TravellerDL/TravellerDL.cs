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

        public Int16 AddTravellerCheckInDetails(TravellerCheckIn _traveller, IDbTransaction transaction)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "InsertTravellerCheckInDetails"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "HostelId":
                            Item.Value = _traveller.hostelId;

                            break;
                        case "TravellerQRCode":
                            Item.Value = _traveller.travellerQRCode;
                            break;
                        case "CheckInDate":
                            Item.Value = _traveller.checkInDate;

                            break;
                        case "CheckOutDate":
                            Item.Value = _traveller.checkOutDate;
                            break;
                        case "Action":
                            Item.Value = _traveller.Action;
                            break;


                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "InsertTravellerCheckInDetails", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "TravellerCheckIn");
                throw ex;
            }
        }

        public DataTable GetTravellerChekInInfo(Int64 travellerID, IDbConnection con)
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
                            Item.Value = travellerID;

                            break;

                    }
                }
                DALHelper.FillDataset(con, CommandType.StoredProcedure, "GetTravellerCheckinDetails", dsResult, new string[] { "Traveller" }, paramData);

                return dsResult.Tables.Contains("Traveller") ? dsResult.Tables["Traveller"] : null;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetTravellerChekInInfo");
                throw ex;
            }
        }
    }
}
