﻿using KI.RIS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessData;
using Modals;

namespace BusinessLogic
{
    public class HostelBL
    {
        HostelDL _hostelDL = new HostelDL();

        public string UpdateHostelUser(Hostel _hostel, Int32 hostelId)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();

                Int64 resultID = _hostelDL.UpdateHostelUser(_hostel, transaction, hostelId);
                if (resultID > 0)
                {
                    message = "Hostel updated successfully";
                }

            }
            catch (Exception ex)
            {
                ErrorLogDL.InsertErrorLog(ex.Message, "UpdateHostelUser");
                IsSuccess = false;
                throw;
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
                if(dtResult.Rows.Count>0)
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
                ErrorLogDL.InsertErrorLog(ex.Message, "GetGallery");
            }

            return objResult;

        }


    }
}