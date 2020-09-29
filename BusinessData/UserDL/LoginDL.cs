using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using System.IO;
using KI.RIS.DAL;
using Modals;

namespace BusinessData
{
    public class LoginDL
    {
        #region Pubic Methods


        public Int16 InsertTravellerUser(Traveller _travller, IDbTransaction transaction)
        {
            return RegisterTravellerUser(_travller, transaction);
        }
        public Int16 InsertHostelUser(Hostel _hostel, IDbTransaction transaction)
        {
            return RegisterHostelUser(_hostel, transaction);
        }
        public Int16 ChangePassword(User _user, IDbTransaction transaction, string newPassword)
        {
            return ChangeUserPassword(_user, transaction, newPassword);
        }

        public DataTable Login(User _user, IDbConnection con)
        {

            return SelectSecurityProfile(_user, con);
        }
        #endregion

        #region Private Methods

        private DataTable SelectSecurityProfile(User _user, IDbConnection con)
        {
            DataSet dsResult = new DataSet();
            try
            {


                IDbDataParameter[] paramData;
                paramData = DALHelperParameterCache.GetSpParameterSet(con, "sp_Login"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "EmailId":
                            Item.Value = _user.email;
                            break;
                        case "UserName":
                            Item.Value = _user.username;
                            break;
                        case "Password":

                            Item.Value = _user.password;

                            break;
                        case "Latitude":
                            Item.Value = "";
                            break;
                        case "Longitude":
                            Item.Value = "";
                            break;
                        case "DeviceToken":
                            Item.Value = _user.deviceToken;
                            break;
                    }
                }

                DALHelper.FillDataset(con, CommandType.StoredProcedure, "sp_Login", dsResult, new string[] { "User" }, paramData);

                return dsResult.Tables.Contains("User") ? dsResult.Tables["User"] : null;
            }
            catch (FileNotFoundException)
            {
                return dsResult.Tables.Contains("SecurityProfile") ? dsResult.Tables["SecurityProfile"] : null;
                //throw new RisException("¥CmnFilenotfound¥");
                //throw new Exception("File not found");
            }
            catch (UnauthorizedAccessException)
            {
                return dsResult.Tables.Contains("SecurityProfile") ? dsResult.Tables["SecurityProfile"] : null;
                //throw new RisException("¥CmnFilenotfound¥");
                //throw new Exception("File not found");
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "SelectSecurityProfile");
                throw ex;
            }
        }

        private Int16 RegisterTravellerUser(Traveller _traveller, IDbTransaction transaction)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "sp_CreateUser"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "UserName":
                            Item.Value = _traveller.username;
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
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "sp_CreateUser", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "RegisterTravellerUser");
                throw ex;
            }
        }

        private Int16 RegisterHostelUser(Hostel _hostel, IDbTransaction transaction)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "sp_CreateUser"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "UserName":
                            Item.Value = _hostel.username;

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
                        case "UserType":
                            Item.Value = "Hostel";
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
                        case "CityName":
                            Item.Value = _hostel.cityName;
                            break;

                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "sp_CreateUser", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "RegisterHostelUser");
                throw ex;
            }
        }

        private Int16 ChangeUserPassword(User _user, IDbTransaction transaction, string newPassword)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "Usp_ChangeUserPassWord"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "UserName":
                            Item.Value = _user.username;
                            break;
                        case "NewPassWord":
                            Item.Value = newPassword;
                            break;
                        case "OldPassword":
                            Item.Value = _user.password;
                            break;

                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "Usp_ChangeUserPassWord", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "ChangeUserPassword");
                throw ex;
            }
        }

        public Int16 RegisterUser(User _user, IDbTransaction transaction)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "RegisterUsers"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "UserName":
                            Item.Value = _user.username;
                            break;
                        case "Password":
                            Item.Value = _user.password;
                            break;
                        case "EmailId":
                            Item.Value = _user.email;
                            break;
                        case "Address":
                            Item.Value = "";
                            break;
                        case "UserType":
                            Item.Value = _user.userType;
                            break;



                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "RegisterUsers", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                // ErrorLogDL.InsertErrorLog(ex.Message, "RegisterTravellerUser");
                throw ex;
            }
        }



        #endregion
    }
}
