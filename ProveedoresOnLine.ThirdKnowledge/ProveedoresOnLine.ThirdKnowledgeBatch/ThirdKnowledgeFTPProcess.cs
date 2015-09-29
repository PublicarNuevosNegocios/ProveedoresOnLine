using ProveedoresOnLine.ThirdKnowledgeBatch.Models;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProveedoresOnLine.ThirdKnowledgeBatch
{
    public static class ThirdKnowledgeFTPProcess
    {
        public static void StartProcess()
        {
            try
            {
                List<TDQueryModel> oQueryResult = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetQueriesInProgress();
                if (oQueryResult  != null)
                {
                    //Buscar el archivo
                    string ftpServerIP = ThirdKnowledgeBatch.Models.InternalSettings.Instance[Constants.C_Settings_FTPServerIP].Value;
                    string uploadToFolder = ThirdKnowledgeBatch.Models.InternalSettings.Instance[Constants.C_Settings_UploadFTPFileName].Value;
                    string UserName = ThirdKnowledgeBatch.Models.InternalSettings.Instance[Constants.C_Settings_FTPUserName].Value;
                    string UserPass = ThirdKnowledgeBatch.Models.InternalSettings.Instance[Constants.C_Settings_FTPPassworUser].Value;

                    string uri = "ftp://" + ftpServerIP + "/" + uploadToFolder + "/";

                    oQueryResult.All(query => 
                    {
                        //using (FileStream fs = new FileStream("Res_ThirdKnowledgeFile_DA5C572E_20150928092624.xml", FileMode.OpenOrCreate)) //Create or open a local file
                        //{
                        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                        //    request.Credentials = new NetworkCredential(UserName, UserPass, ftpServerIP);
                        //    request.Proxy = null;
                        //    request.KeepAlive = false;//Command execution after closing the connection
                        //    request.UseBinary = true;
                        //    request.UsePassive = false;
                        //    request.EnableSsl = false;                            

                        //    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        //    {
                        //        fs.Position = fs.Length;
                        //        byte[] buffer = new byte[4096];//4K
                        //        int count = response.GetResponseStream().Read(buffer, 0, buffer.Length);
                        //        while (count > 0)
                        //        {
                        //            fs.Write(buffer, 0, count);
                        //            count = response.GetResponseStream().Read(buffer, 0, buffer.Length);
                        //        }
                        //        response.GetResponseStream().Close();
                        //    }
                        //}
                        int bytesRead = 0;
                        byte[] buffer = new byte[1024];

                        uri = uri + "Res_" + query.RelatedQueryInfoModel.FirstOrDefault().Value;

                        FtpWebRequest request = ((FtpWebRequest)WebRequest.Create(new Uri(uri)));
                        request.Proxy = null;
                        request.Credentials = new NetworkCredential(UserName, UserPass, ftpServerIP);
                        request.UsePassive = false;
                        request.UseBinary = true;
                        request.KeepAlive = false;

                        request.Method = WebRequestMethods.Ftp.DownloadFile;

                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        string xml = reader.ReadToEnd();
                        XDocument thisXmlDoc = new XDocument();
                        thisXmlDoc.Add(xml);

                        return true;
                    });                    
                }
                else
                {

                }               
                
                //Get query masive, in process, file name
                //Connect to FTP for the ftp file

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
