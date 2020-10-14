using System;
using System.Data;
using Modals;
using System.Web;
using System.Web.SessionState;
using KI.RIS.DAL;
using System.Data.SqlClient;
using System.Collections.Generic;
using CommonLib.CommonHelper;
using BusinessData;

namespace BusinessLogic
{
    public class LoginBL
    {
        LoginDL _loginDL;
        HostelDL _hostelDL;
        User _userDL;
        public LoginBL()
        {
            _loginDL = new LoginDL();
            _hostelDL = new HostelDL();
            _userDL = new User();
        }

        public object Login(User _user)
        {
            IDbConnection con = null;
            DataSet dsData = new DataSet();
            DataTable fromdt = new DataTable();
            Dictionary<string, object> dsResult = new Dictionary<string, object>();
            try
            {
                con = DALHelper.GetConnection();
                DataTable loginData = _loginDL.Login(_user, con);
                if (loginData != null && loginData.Rows.Count > 0)
                {
                    object objUser = loginData;
                    object objGallery = null;
                    dsResult.Add("User", objUser);
                    if (loginData.Rows[0]["UserType"].ToString() == "Hostel")
                    {
                        // var gallery = _hostelDL.GetGallery(Convert.ToInt32(loginData.Rows[0]["UserId"]), con);
                        // objGallery = gallery;
                        // dsResult.Add("Gallery", objGallery);
                    }
                    return dsResult;
                }
                else
                {
                    loginData = null;

                    return "Invalid Credentials";
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "LoginBL:Login");

                throw;
            }
            finally
            {
                DALHelper.CloseDB(con);
            }

        }

        public string RegisterUser(User _user)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 Result = _loginDL.RegisterUser(_user, transaction);

                if (Result > 0)
                {
                    message = Result.ToString();
                    Email.SendMail("Dear " + _user.username + " your account has been created successfully", _user.email, "Account Registration");
                }
                else
                {
                    message = "unable to create user please contact provider support";
                }
            }
            catch (SqlException ex)
            {
                IsSuccess = false;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    message = "User already registered";
                }
                else
                {
                    IsSuccess = false;
                    ErrorLogDL.InsertErrorLog(ex.Message, "LoginBL : RegisterUser");
                    throw;
                }
            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }
            return message;
        }


        public string ChangeUserAuthentication(User _user, string newPassword)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _loginDL.ChangePassword(_user, transaction, newPassword);

                if (resultID > 0)
                {
                    message = "Password changed sucessfully";
                }
                else if (resultID == -1)
                {
                    message = "OldPasword is incorrect.";
                }

            }
            catch (Exception ex)
            {


                IsSuccess = false;
                ErrorLogDL.InsertErrorLog(ex.Message, "RegisterTravellerUser");
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


