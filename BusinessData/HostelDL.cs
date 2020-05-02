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
        public Int16 UpdateHostelUser(Hostel _hostel, IDbTransaction transaction,Int32 hostelId)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "UpdateHostelProfile"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "HostelId":
                            Item.Value = hostelId;

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
                        case "HostelDomRoomsWithBunks":
                            Item.Value = _hostel.hostelDormRoomwithBunks;
                            break;
                        case "HostelDomRoomsWithOutBunks":
                            Item.Value = _hostel.hostelDormRoomwithoutBunks;
                            break;
                        case "FemaleDomRooms":
                            Item.Value = _hostel.femaleDormRooms;
                            break;
                        case "SingleBedRooms":
                            Item.Value = _hostel.singleBedRooms;
                            break;
                        case "DoubleBedRooms":
                            Item.Value = _hostel.doubleBedRooms;
                            break;

                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "UpdateHostelProfile", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "UpdateHostelUser");
                throw ex;
            }
        }

        public Int16 AddGallery(DataRow dr, IDbTransaction transaction, Int32 hostelId)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "InsertHostelGallery"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "HostelId":
                            Item.Value = hostelId;

                            break;
                        case "Gallery":
                            Item.Value = dr["ImageUrl"];
                            break;
                        

                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "InsertHostelGallery", paramData));
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
               
                paramData = DALHelperParameterCache.GetSpParameterSet(con, "SelectHostelGallery"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "HostelId":
                            Item.Value = hostelId;

                            break;
                     
                    }
                }
                DALHelper.FillDataset(con, CommandType.StoredProcedure, "SelectHostelGallery", dsResult, new string[] { "HostelGallery" }, paramData);

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

                paramData = DALHelperParameterCache.GetSpParameterSet(con, "SelectHostelsByKey"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "Key":
                            Item.Value = key;

                            break;

                    }
                }
                DALHelper.FillDataset(con, CommandType.StoredProcedure, "SelectHostelsByKey", dsResult, new string[] { "Users" }, paramData);

                return dsResult.Tables.Contains("Users") ? dsResult.Tables["Users"] : null;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetHostelsByKey");
                throw ex;
            }
        }

    }
}
