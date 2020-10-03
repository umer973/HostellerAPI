using KI.RIS.DAL;
using Modals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessData
{
    public class HostelDL
    {
        public Int16 UpdateHostelUser(Hostel _hostel, IDbTransaction transaction)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "InsertHostelProfile"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "UserID":
                            Item.Value = _hostel.userId;

                            break;
                        case "Password":
                            Item.Value = _hostel.password;
                            break;
                        case "EmailId":
                            Item.Value = _hostel.emailId;
                            break;
                        case "Address":
                            Item.Value = _hostel.address;
                            break;

                        case "PersonalWebSiteLink":
                            Item.Value = _hostel.websiteLink;
                            break;
                        case "ProfilePic":
                            Item.Value = _hostel.profilePic;
                            break;
                        case "HostelDormRoomWithBunks":
                            Item.Value = _hostel.hostelDormRoomwithBunks;
                            break;
                        case "HostelDormRoomWithoutBunks":
                            Item.Value = _hostel.hostelDormRoomwithoutBunks;
                            break;
                        case "FemaleBedRooms":
                            Item.Value = _hostel.femaleDormRooms;
                            break;
                        case "SingleBedRooms":
                            Item.Value = _hostel.singleBedRooms;
                            break;
                        case "DoubleBedRooms":
                            Item.Value = _hostel.doubleBedRooms;
                            break;
                        case "HostelName":
                            Item.Value = _hostel.hostelName;
                            break;
                        case "City":
                            Item.Value = _hostel.cityName;
                            break;

                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "InsertHostelProfile", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "HostelDL::UpdateHostelUser");
                throw ex;
            }
        }

        public Int16 AddGallery(DataRow dr, IDbTransaction transaction, Int32 hostelId)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "InsertGallery"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "HostelId":
                            Item.Value = hostelId;

                            break;
                        case "ImageUrl":
                            Item.Value = dr["ImageUrl"];
                            break;
                        

                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "InsertGallery", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "AddGallery");
                throw ex;
            }
        }

        public DataTable GetGallery(Int32 hostelId, IDbConnection con)
        {
            DataSet dsResult = new DataSet();
            try
            {
                IDbDataParameter[] paramData;
               
                paramData = DALHelperParameterCache.GetSpParameterSet(con, "GetHostellerGallery"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "HostelId":
                            Item.Value = hostelId;

                            break;
                     
                    }
                }
                DALHelper.FillDataset(con, CommandType.StoredProcedure, "GetHostellerGallery", dsResult, new string[] { "HostelGallery" }, paramData);

                return dsResult.Tables.Contains("HostelGallery") ? dsResult.Tables["HostelGallery"] : null;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetGallery");
                throw ex;
            }
        }

        public DataTable GetHostelsByKey(string key, IDbConnection con)
        {
            DataSet dsResult = new DataSet();
            try
            {
                IDbDataParameter[] paramData;

                paramData = DALHelperParameterCache.GetSpParameterSet(con, "GetHostels"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "Key":
                            Item.Value = key;

                            break;

                    }
                }
                DALHelper.FillDataset(con, CommandType.StoredProcedure, "GetHostels", dsResult, new string[] { "HostelProfile" }, paramData);

                return dsResult.Tables.Contains("HostelProfile") ? dsResult.Tables["HostelProfile"] : null;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetHostelsByKey");
                throw ex;
            }
        }

        public DataTable GetHostelProfile(Int32 hostelId, IDbConnection con)
        {
            DataSet dsResult = new DataSet();
            try
            {
                IDbDataParameter[] paramData;

                paramData = DALHelperParameterCache.GetSpParameterSet(con, "GetHostellerProfile"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "HostelId":
                            Item.Value = hostelId;

                            break;

                    }
                }
                DALHelper.FillDataset(con, CommandType.StoredProcedure, "GetHostellerProfile", dsResult, new string[] { "HostelProfile" }, paramData);

                return dsResult.Tables.Contains("HostelProfile") ? dsResult.Tables["HostelProfile"] : null;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetHostelProfile");
                throw ex;
            }
        }

    }
}
