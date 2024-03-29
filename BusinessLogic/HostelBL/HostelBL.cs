﻿using KI.RIS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessData;
using Modals;
using System.Data.SqlClient;

namespace BusinessLogic
{
    public class HostelBL
    {
        HostelDL _hostelDL = new HostelDL();

        public string UpdateHostelUser(Hostel _hostel)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _hostelDL.UpdateHostelUser(_hostel, transaction);
                if (resultID > 0)
                {
                    message = "Hostel updated successfully";
                }

            }
            catch (SqlException ex)
            {
                IsSuccess = false;
                if (ex.Message.Contains("UNIQUE KEY"))
                {
                    message = "cannot insert hostel name with same name";
                }
                else
                {
                    IsSuccess = false;
                    ErrorLogDL.InsertErrorLog(ex.Message, "HostelBL : UpdateHostelUser");
                    throw;
                }
            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }
            return message;
        }

        public object AddGallery(DataTable dt, Int32 hostelId)
        {
            InsertGalery(dt, hostelId);
            return GetGallery(hostelId);
        }

        public object GetGallery(int hostelId)
        {
            object objResult = null;
            DataTable dtResult = null;
            IDbConnection con = null;
            try
            {

                con = DALHelper.GetConnection();
                dtResult = _hostelDL.GetGallery(hostelId, con);
                if (dtResult.Rows.Count > 0)
                {
                    objResult = dtResult;
                }
                else
                {
                    objResult = "No gallery found";
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetGallery");
            }

            return objResult;

        }

        private void InsertGalery(DataTable dt, Int32 hostelId)
        {
            bool IsSuccess = true;
            IDbTransaction transaction = null;

            try
            {
                transaction = DALHelper.GetTransaction();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Int64 resultID = _hostelDL.AddGallery(dt.Rows[i], transaction, hostelId);
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "AddGallery");
                IsSuccess = false;
                throw;
            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }

        }

        public object GetHostels(string key)
        {
            object objResult = null;
            DataTable dtResult = null;
            IDbConnection con = null;
            try
            {

                con = DALHelper.GetConnection();
                dtResult = _hostelDL.GetHostelsByKey(key, con);
                if (dtResult.Rows.Count > 0)
                {
                    objResult = dtResult;
                }
                else
                {
                    objResult = null;
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetHostels");
            }

            return objResult;

        }

        public object GetHostelProfile(int hostelId)
        {
            object objResult = null;
            DataTable dtResult = null;
            Dictionary<string, object> dsResult = new Dictionary<string, object>();
            IDbConnection con = null;
            try
            {

                con = DALHelper.GetConnection();
                dtResult = _hostelDL.GetHostelProfile(hostelId, con);
                if (dtResult.Rows.Count > 0)
                {
                    objResult = dtResult;
                    dsResult.Add("Profile", dtResult);
                    dtResult = _hostelDL.GetGallery(hostelId, con);
                    dsResult.Add("Gallery", dtResult);
                }
                else
                {
                    objResult = "No data found";
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetHostelProfile");
            }

            return dsResult;

        }

        public object GetAllTravellerCheckInDetails(Int64 hostelId, string mode)
        {
            object objResult = null;
            DataTable dtResult = null;
            IDbConnection con = null;
            try
            {
                con = DALHelper.GetConnection();
                dtResult = _hostelDL.GetAllTravellerCheckInDetails(hostelId, con, mode);
                if (dtResult.Rows.Count > 0)
                {
                    objResult = dtResult;
                }
                else
                {
                    objResult = "No ChecK In found";
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "GetAllTravellerCheckInDetails");
            }


            return objResult;

        }
    }
}
