using BusinessData;
using System;
using System.Data;
using Modals;
using System.Web;
using System.Web.SessionState;
using KI.RIS.DAL;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class LoginBL
    {
        LoginDL _loginDL;
        HostelDL _hostelDL;
        public LoginBL()
        {
            _loginDL = new LoginDL();
            _hostelDL = new HostelDL();
        }

        // IDbConnection connection = null;
        //IDbTransaction transaction = null

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
                    dsResult.Add("User",objUser);
                    if (loginData.Rows[0]["UserType"].ToString() == "Hostel")
                    {
                        var gallery = _hostelDL.GetGallery(Convert.ToInt32(loginData.Rows[0]["UserId"]), con);
                        objGallery = gallery;
                       dsResult.Add("Gallery",objGallery);
                    }
                    return dsResult;
                }
                else
                {
                    loginData = null;

                    return "Invalid Credentials";
                }

            }
            catch(Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "Login");

                throw;
            }
            finally
            {
                DALHelper.CloseDB(con);
            }

        }



        public string RegisterUser(Hostel _hostel)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _loginDL.InsertHostelUser(_hostel, transaction);
                if (resultID == 3)
                {
                    message = "Username registered sucessfully";
                }
                else if (resultID == 1)
                {
                    message = "Username already registered";
                }
            }
            catch(Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "RegisterUser");

                IsSuccess = false;
                throw;
            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }
            return message;
        }

        public string RegisterTravellerUser(Traveller _traveller)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _loginDL.InsertTravellerUser(_traveller, transaction);

                if (resultID == 3)
                {
                    message = "Username registered sucessfully";
                }
                else if (resultID == 2)
                {
                    message = "Username already registered";
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
                    ErrorLogDL.InsertErrorLog(ex.Message, "RegisterTravellerUser");
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


