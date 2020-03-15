using KI.RIS.DAL;
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
            catch
            {
                IsSuccess = false;
                throw;
            }
            finally
            {
                DALHelper.CloseDB(transaction, IsSuccess);
            }
            return message;
        }

        public string AddGallery(DataTable dt, Int32 hostelId)
        {
            bool IsSuccess = true;
            string message = "";
            IDbTransaction transaction = null;
            try
            {
                transaction = DALHelper.GetTransaction();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Int64 resultID = _hostelDL.AddGallery(dt.Rows[i], transaction, hostelId);

                    if (resultID > 0)
                    {
                        message = "Gallery Added";
                    }
                }

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
            return message;
        }
    }
}
