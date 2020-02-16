//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Configuration;
//using EO.Pdf;


///*-----------------------------------------------------------------------
//<Copyright file="Methods.cs" Company="Kameda Infologics">
//    Copyright@Kameda Infologics Pvt Ltd. All rights reserved.
//</Copyright>

// Description     :Common methods 
// Created  By     :Biju S J 
// Created  Date   :25-Jan-2018 
// Modified By     :ModifiedBy  
// Modified Date   :ModifiedDate 
// Modified Purpose:ModifiedPur 
// -----------------------------------------------------------------------*/


//namespace KI.RIS.General.Common
//{

//    public class MethodLib
//    {
//        /// <summary>
//        /// This Function will check whether that column is already exist if so Gets the value of that column.
//        /// </summary>
//        /// <param name="drRowItem">The data row item.</param>
//        /// <param name="strColumnName">Name of the column.</param>
//        /// <returns></returns>
//        public object GetColumnValue(DataRow drRowItem, string strColumnName)
//        {
//            try
//            {
//                object objReturnValue = DBNull.Value;
//                if (drRowItem != null && drRowItem.Table != null && drRowItem.Table.Columns != null &&
//                    drRowItem.Table.Columns.Contains(strColumnName))
//                {
//                    if (drRowItem.RowState != DataRowState.Deleted)
//                    {
//                        objReturnValue = drRowItem[strColumnName];
//                    }
//                    else
//                    {
//                        objReturnValue = drRowItem[strColumnName, DataRowVersion.Original];
//                    }
//                }
//                return objReturnValue;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        public string ValidateHtmlData(string htmlInputData)
//        {
//            string returnHtmlData = string.Empty;
//            try
//            {
//                if (htmlInputData != "")
//                {
//                    returnHtmlData = htmlInputData.Replace("<", "&lt;");
//                    returnHtmlData = returnHtmlData.Replace(">", "&gt;");
//                }
//                return returnHtmlData;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        /// <summary>
//        /// Function to validate password policy
//        /// </summary>
//        /// <param name="password"></param>
//        /// <param name="isPatient"></param>
//        /// <param name="dtPasswordPolicy"></param>
//        /// <returns></returns>
//        public bool ValidatePasswordPolicy(string password, DataTable dtPasswordPolicy)
//        {
//            try
//            {
//                bool status = false;

//                status = true;  // implement the code here

//                return status;
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }

//        public static string SaveImage(string ImgStr, string ImgName,string OptionPath=null)
//        {
//            String path=null;
//            if (OptionPath==null)
//            {
//                path = WebConfigurationManager.AppSettings["ScanPath"].ToString();
//            }
//            else if(OptionPath != null)
//            {
//                path = OptionPath;
//            }

//            //using (KI.RIS.General.Impersonation.Impersonation objImpersonation = new Impersonation.Impersonation())
//            //{
//            //    objImpersonation.GetImpersionation();
//            //    {
//            //        //Check if directory exist
//            //        if (!System.IO.Directory.Exists(path))
//            //        {
//            //            System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
//            //        }

//            //        string imageName = ImgName;

//            //        //set the image path
//            //        string imgPath = Path.Combine(path, imageName);

//            //        byte[] imageBytes = Convert.FromBase64String(ImgStr);

//            //        File.WriteAllBytes(imgPath, imageBytes);
//            //    }
//            //}

//            //Check if directory exist
//            if (!System.IO.Directory.Exists(path))
//            {
//                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
//            }

//            string imageName = ImgName;

//            //set the image path
//            string imgPath = Path.Combine(path, imageName);

//            byte[] imageBytes = Convert.FromBase64String(ImgStr);

//            File.WriteAllBytes(imgPath, imageBytes);

//            return imgPath;
//        }

//        public static string GetByteArray(String path)
//        {
//            //using (KI.RIS.General.Impersonation.Impersonation objImpersonation = new Impersonation.Impersonation())
//            //{
//            //    objImpersonation.GetImpersionation();
//            //    {
//                    byte[] imageArray = File.ReadAllBytes(path);
//                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);
//                    return base64ImageRepresentation;
//            //    }
//            //}
//        }

//        public static string GetHeaderFooterByteArray(String path, String htmltag)
//        {
//            String byteArraypath = string.Empty;
//            htmltag = MessageLib.GetMultilingualMessage(htmltag);
//            if (path != null && path.Trim().Length > 0)
//            {
//                String imagebytearray = GetByteArray(path);
//                htmltag = htmltag.Replace("‡Path‡", imagebytearray);
//                byteArraypath = htmltag;
//            }
//            else if (htmltag != null && htmltag.Trim().Length > 0)
//            {
//                byteArraypath = htmltag;
//            }
//            else
//            {
//                byteArraypath = "<img src=\"\"/>";
//            }

//            return byteArraypath;
//        }

//        public static byte[] GetPDF(Int16 mode,string HTMLtext=null,string url=null )
//        {
//            EO.Pdf.Runtime.AddLicense(
//  "ua9qsKaxHvSbvPwBFPGe6sUF6KFtprXJ2rFpqbSzy/We6ff6Gu12mbXKzZ9o" +
//  "tZGby59Zl8AEFOan2PgGHeR3ufP5DNKr58ngFbChudfn2tGxqubB7Lx2s7ME" +
//  "FOan2PgGHeR3hI7N2uui2un/HuR3hI514+30EO2s3MKetZ9Zl6TNF+ic3PIE" +
//  "EMidtbbB3Lhpq7bD269qrbbH3bJmqMDAF+ic3PIEEMidtZGby59Zl8D9FOKe" +
//  "5ff2EL11pvD6DuSn6un26YxDl6Sxy7ua4/AAIr1GgaSxy59Zl6Sx5+Cd26QF" +
//  "JO+etKbW+q2J2+qzy/We6ff6Gu12mbXK2a9bl7PPuIlZl6Sx566a4/AAIr1G" +
//  "gaSxy5915vb1EPGC5eoAy+Oa6+nOzbNoqLzA3Q==");
//            PdfDocument doc = new PdfDocument();
//            if (mode == 1) // convert from HTML string
//            {
//                string style = @"<style>.fr-view table {
//    border: 0;
//    border-collapse: collapse;
//    empty-cells: show;
//    max-width: 100%;
//} .fr-view table td, .fr-view table th {
//          border: 1px solid #ddd;
//      }</style>";
//                HtmlToPdf.ConvertHtml(style+HTMLtext, doc);
//            }
//            MemoryStream stream = new MemoryStream();
//            doc.Save(stream);
//            byte[] data = stream.ToArray();
//            return data;
//        }

//    }
//}
