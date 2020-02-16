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
        //public DataTable SelectportalUser(Int16 Mode, DataTable drCriteria, IDbConnection con)
        //{
        //    return SelectSecurityProfile(Mode, drCriteria.Rows[0], con);
        //}

        //public bool UpdateCredentails(DataTable dtUser, IDbTransaction transaction, Int16 mode = 0, Int64 genUserId = 0)
        //{
        //    return ChangeCredentials(dtUser.Rows[0], transaction, mode, genUserId);
        //}

        public Int16 Insertsecurityprofile(Int16 Mode, DataTable drCriteria, IDbTransaction transaction)
        {
            return RegisterportalUser(Mode, drCriteria.Rows[0], transaction);
        }

        //public Int16 ExistingPortalUser(Int16 Mode, DataTable drCriteria, IDbTransaction transaction)
        //{
        //    return ExistingUserData(Mode, drCriteria.Rows[0], transaction);
        //}
        public DataTable Login(User _user, IDbConnection con)
        {

            return SelectSecurityProfile(_user, con);
        }
        #endregion

        #region Private Methods
        //private bool ChangeCredentials(DataRow dr, IDbTransaction trans, Int16 mode, Int64 genUserID)
        //{

        //    try
        //    {
        //        IDbDataParameter[] paramData;

        //        MethodLib objCommonMethodLib = new MethodLib();

        //        bool IsSuccess = false;
        //        paramData = DALHelperParameterCache.GetSpParameterSet(trans, "UpdateSecurityProfile"); foreach (IDbDataParameter Item in paramData)
        //        {
        //            switch (Item.ParameterName)
        //            {
        //                case "P_mode":
        //                    Item.Value = mode;
        //                    break;
        //                case "P_GenUserId":
        //                    Item.Value = genUserID;
        //                    break;
        //                case "P_UserID":
        //                    Item.Value = dr["UserID"];
        //                    break;
        //                case "P_ProfileType":
        //                    Item.Value = dr["ProfileType"];
        //                    break;
        //                case "P_Password":
        //                    if (dr.Table.Columns.Contains("NewPassword"))
        //                    {
        //                        Item.Value = dr["NewPassword"];
        //                    }
        //                    else if (dr.Table.Columns.Contains("Password"))
        //                    {
        //                        Item.Value = dr["Password"];
        //                    }
        //                    break;
        //                case "P_PasswordExpDate":
        //                    Item.Value = dr["PasswordExpDate"];
        //                    break;
        //            }
        //        }
        //        IsSuccess = DALHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "UpdateSecurityProfile", paramData) > 0 ? true : false;
        //        return IsSuccess;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

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
                throw ex;
            }
        }

        private Int16 RegisterportalUser(Int16 Mode, DataRow dr, IDbTransaction transaction)
        {
            try
            {
                IDbDataParameter[] paramData;
                Int16 Result = 0;
                paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "Insertsecurityprofile"); foreach (IDbDataParameter Item in paramData)
                {
                    switch (Item.ParameterName)
                    {
                        case "P_GenUserID":
                            Item.Value = dr["GenUserID"];
                            break;
                        case "P_PatDemographicsID":
                            Item.Value = dr["PatDemographicsID"];
                            break;
                        case "P_Mrno":
                            Item.Value = dr["Mrno"];
                            break;
                        case "P_EmpID":
                            Item.Value = dr["EmpID"];
                            break;
                        case "P_ProfileType":
                            Item.Value = dr["ProfileType"];
                            break;
                        case "P_Password":
                            Item.Value = dr["Password"];
                            break;
                        case "P_PasswordExpDate":
                            Item.Value = dr["PasswordExpDate"];
                            break;
                        case "P_IsValid":
                            Item.Value = dr["IsValid"];
                            break;
                        case "P_EntryDate":
                            Item.Value = dr["EntryDate"];///.ToString();
                            break;
                    }
                }
                Result = Convert.ToInt16(DALHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "Insertsecurityprofile", paramData));
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private Int16 ExistingUserData(Int16 Mode, DataRow drCriteria, IDbTransaction transaction)
        //{
        //    try
        //    {
        //        MethodLib objCommonMethodLib = new MethodLib();
        //        IDbDataParameter[] paramData;
        //        Int16 Result = 0;
        //        paramData = DALHelperParameterCache.GetSpParameterSet(transaction, "[SelectPortalUser]"); foreach (IDbDataParameter Item in paramData)
        //        {
        //            switch (Item.ParameterName)
        //            {
        //                case "P_Mode":
        //                    Item.Value = Mode;
        //                    break;
        //                case "P_UserID":
        //                    if (drCriteria.Table.Columns.Contains("ProfileType") && drCriteria["ProfileType"] != DBNull.Value)
        //                    {
        //                        if (Convert.ToInt16(drCriteria["ProfileType"]) == 0) //patient
        //                        {
        //                            Item.Value = objCommonMethodLib.GetColumnValue(drCriteria, "Mrno");
        //                        }
        //                        else  //employee
        //                        {
        //                            Item.Value = objCommonMethodLib.GetColumnValue(drCriteria, "EmpID");
        //                        }
        //                    }
        //                    else if (Mode == 0)
        //                        Item.Value = objCommonMethodLib.GetColumnValue(drCriteria, "Mrno");
        //                    else if (Mode == 1)
        //                        Item.Value = objCommonMethodLib.GetColumnValue(drCriteria, "EmpID");
        //                    break;
        //                case "P_MobileNo":
        //                    Item.Value = objCommonMethodLib.GetColumnValue(drCriteria, "Phone");
        //                    break;
        //                case "P_Email":
        //                    Item.Value = objCommonMethodLib.GetColumnValue(drCriteria, "Email");
        //                    break;
        //            }
        //        }
        //        DataSet dsResult = new DataSet();
        //        DALHelper.FillDataset(transaction, CommandType.StoredProcedure, "[SelectPortalUser]", dsResult, new string[] { "SecurityProfile" }, paramData);

        //        DataTable dt = dsResult.Tables.Contains("SecurityProfile") ? dsResult.Tables["SecurityProfile"] : null;
        //        if (dt.Rows.Count > 0 & dt != null)
        //            Result = Convert.ToInt16(dt.Rows[0][0]);
        //        return Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion
    }
}
