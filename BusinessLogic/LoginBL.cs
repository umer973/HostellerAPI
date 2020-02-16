using BusinessData;
using System;
using System.Data;
using Modals;
using System.Web;
using System.Web.SessionState;
using KI.RIS.DAL;

namespace BusinessLogic
{
    public class LoginBL
    {
        LoginDL _loginDL;
        public LoginBL()
        {
            _loginDL = new LoginDL();
        }

        IDbConnection connection = null;
        //IDbTransaction transaction = null

        public object Login(User _user)
        {
            IDbConnection con = null;
            DataSet dsData = new DataSet();
            DataTable fromdt = new DataTable();
            try
            {
                con = KI.RIS.DAL.DALHelper.GetConnection();

                //string EncryptedPassword = Encryption(dtUserData);


                DataTable loginData = _loginDL.Login(_user, con);
                if (loginData == null || loginData.Rows.Count == 0)
                {

                    return "Invalid User or Password";
                }
                else
                {
                    SessionIDManager manager = new SessionIDManager();
                    string sid = manager.CreateSessionID(HttpContext.Current);


                    return sid;
                }

                //}
                //DLSecurity objDLSecurity = new DLSecurity();

                //DataTable privileges = objDLSecurity.SelectUserPrivileges(Convert.ToInt32(loginData.Rows[0]["GenUserID"]), con);
                //string[] lookUpTypes = { "WorklistMenu", "PDFPrintHeader" };
                //var Temp = from dr in DLGlobalCaching.GetLookupData(string.Empty, con, lookUpTypes).AsEnumerable()
                //           where dr["LookupType"].ToString().Equals("WorklistMenu") && dr["Field3"].ToString().Equals(Convert.ToString(loginData.Rows[0]["GenUserID"]))
                //           select dr;
                //fromdt = Temp.Count() > 0 ? Temp.CopyToDataTable() : null;
                //if (fromdt != null)
                //{
                //    string[] selectedColumns = new[] { "Field1", "Field3", "Field2", "Field4", "GenLookupId", "LookupType", "lookupValue" };
                //    DataTable dt = new DataView(fromdt).ToTable(false, selectedColumns);
                //    dt.TableName = "FolderMenu";
                //    dsData.Tables.Add(dt.Copy());
                //}
                //else
                //{
                //    DataTable dt1 = new DataTable();
                //    dt1.TableName = "FolderMenu";
                //    dsData.Tables.Add(dt1.Copy());
                //}

                //var filter = from dr in DLGlobalCaching.GetLookupData(string.Empty, con, lookUpTypes).AsEnumerable()
                //             where dr["LookupType"].ToString().Equals("PDFPrintHeader") && dr["LookUpValue"].ToString().Equals("Header")
                //             select dr;
                //if (filter.Any())
                //{
                //    DataTable dtPDFPrintHeader = filter.CopyToDataTable();
                //    dtPDFPrintHeader.TableName = "PDFPrintHeader";
                //    dsData.Tables.Add(dtPDFPrintHeader);
                //}
                //dsData.Tables.Add(privileges.Copy());
                //DLUISettings oDLUISettings = new DLUISettings();
                //DataTable dtUiTheme = oDLUISettings.SelectUiSetting(4, "REPORTING_THEME", Convert.ToInt32(loginData.Rows[0]["GenUserID"]), con);
                //dtUiTheme.TableName = "UiTheme";
                //dsData.Tables.Add(dtUiTheme.Copy());
                return dsData;
            }
            catch
            {
                throw;
            }
            finally
            {
                KI.RIS.DAL.DALHelper.CloseDB(con);
            }
        }

        //private string Encryption(DataTable dtUserData)
        //{
        //    TripleDES objTripleDES = new TripleDES();
        //    return objTripleDES.Encrypt(dtUserData.Rows[0]["Password"].ToString(), FluenceKey.GetKey());
        //}

        //public object SelectInitialData()
        //{

        //    try
        //    {

        //        connection = DALHelper.GetConnection();
        //        Dictionary<string, object> objectResultData = new Dictionary<string, object>();

        //        //Lookups
        //        //GetLookupData(objectResultData);

        //        //Supervisor

        //        //DataTable dtSupervisor = ObjSecurity.SelectUser(1, "%", connection).Copy();
        //        //var FilterSupervisor = from dr in dtSupervisor.AsEnumerable()
        //        //                       select new
        //        //                       {
        //        //                           EmpName = dr["FullName"].ToString(),
        //        //                           Value = dr["GenUserId"].ToString()
        //        //                       };
        //        //objectResultData.Add("Employee", FilterSupervisor);

        //        ////

        //        return objectResultData;

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        DALHelper.CloseDB(connection);
        //    }
        //}

        //public DataTable SelectPortalUser(Int16 Mode, DataTable drCriteria)
        //{
        //    IDbConnection con = null;
        //    try
        //    {
        //        con = DALHelper.GetConnection();

        //        return ObjLogin.SelectportalUser(Mode, drCriteria, con);

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        DALHelper.CloseDB(con);
        //    }
        //}

        //public bool ChangeCredentials(DataTable dt, short mode)
        //{
        //    bool IsSuccess = true;
        //    IDbTransaction transaction = null;
        //    try
        //    {
        //        transaction = DALHelper.GetTransaction();
        //        return ObjLogin.UpdateCredentails(dt, transaction, mode);
        //    }
        //    catch
        //    {
        //        IsSuccess = false;
        //        throw;
        //    }
        //    finally
        //    {
        //        DALHelper.CloseDB(transaction, IsSuccess);
        //    }
        //}



        //private Int16 ExistingPortalUser(Int16 Mode, DataTable drCriteria)
        //{
        //    bool IsSuccess = true;
        //    IDbTransaction transaction = null;
        //    try
        //    {
        //        transaction = DALHelper.GetTransaction();

        //        return ObjLogin.ExistingPortalUser(Mode, drCriteria, transaction);

        //    }
        //    catch
        //    {
        //        IsSuccess = false;
        //        throw;
        //    }
        //    finally
        //    {
        //        DALHelper.CloseDB(transaction, IsSuccess);
        //    }
        //}

        public Int16 RegisterUser(Hostel _hostel)
        {
            bool IsSuccess = true;
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                return _loginDL.InsertHostelUser(_hostel, transaction);
            }
            catch
            {
                IsSuccess = false;
                throw;
            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }
        }
        public Int16 RegisterTravellerUser(Traveller _traveller)
        {
            bool IsSuccess = true;
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                return _loginDL.InsertTravellerUser(_traveller, transaction);

            }
            catch
            {
                IsSuccess = false;
                throw;
            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }
        }
    }
}


