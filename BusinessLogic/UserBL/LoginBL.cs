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

        public DataTable Login(User _user)
        {
            IDbConnection con = null;
            DataSet dsData = new DataSet();
            DataTable fromdt = new DataTable();
            Dictionary<string, object> dsResult = new Dictionary<string, object>();
            try
            {
                con = DALHelper.GetConnection();
                return _loginDL.Login(_user, con);


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

        public object ValidateEmail(string email)
        {
            object objResult = null;
            Int64 Result = 0;
            IDbConnection con = null;
            try
            {


                con = DALHelper.GetConnection();
                Result = _loginDL.ValidateEmail(email, con);
                if (Result > 0)
                {
                    objResult = Result;
                }
                else
                {
                    objResult = false;
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "ValidateEmail");
            }


            return objResult;

        }

        public string ResetPassword(User _user)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _loginDL.ResetPassword(_user, transaction);

                if (resultID > 0)
                {
                    message = "Password reset sucessfully";
                }
                else
                {
                    message = "unable to reset password please contact support provider";
                }

            }
            catch (Exception ex)
            {


                IsSuccess = false;
                ErrorLogDL.InsertErrorLog(ex.Message, "ResetPassword");
                throw;

            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }

            return message;
        }

        public Boolean InsertHelpUs(Int64 userId, string title, string message)
        {
            bool IsSuccess = true;
            bool result = false;
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _loginDL.InsertHelpUs(userId, title, message, transaction);

                if (resultID > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception ex)
            {


                IsSuccess = false;
                ErrorLogDL.InsertErrorLog(ex.Message, "InsertHelpUs");
                throw;

            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }

            return result;
        }
    }
}


