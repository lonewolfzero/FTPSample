using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Cache;
using Renci.SshNet;
using System.Text.RegularExpressions;

namespace FTPTestingConsole
{
    public class FTPTestingHelper
    {
        static string host = "ftp://192.168.0.1";
        static string hostIP = "192.168.0.1";
        static string UserId = "username";
        static string Password = "password";


        //SAMPLE CALL CreateFolder("FoldeName")
        public bool CreateFolder(string FoldeName)
        {
            //string path = "/IndexTEST";
            bool IsCreated = true;
            try
            {
                WebRequest request = WebRequest.Create(host + FoldeName);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(UserId, Password);
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine(resp.StatusCode);
                }
            }
            catch (Exception ex)
            {
                IsCreated = false;
            }
            return IsCreated;
        }


        //SAMPLE CALL DoesFtpDirectoryExist("ftp://172.18.95.70/directorytest")
        public static bool DoesFtpDirectoryExist(string dirPath)
        {
            bool isexist = false;

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
                request.Credentials = new NetworkCredential(UserId, Password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    isexist = true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }
                }
            }
            return isexist;
        }


        // SAMPLE CALL  GetAllFtpFiles("ftp://172.18.95.70/directorytest")
        public static List<string> GetAllFtpFiles(string ParentFolderpath)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ParentFolderpath);
                ftpRequest.Credentials = new NetworkCredential(UserId, Password);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var lineArr = line.Split('/');
                    line = lineArr[lineArr.Count() - 1];
                    directories.Add(line);
                    line = streamReader.ReadLine();
                }

                streamReader.Close();

                return directories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // SAMPLE CALL  DownloadFileFTP("ftp://172.18.95.70/directorytest/TestFile0.txt")
        public static void DownloadFileFTP(String ParentFolderFilepath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ParentFolderFilepath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(UserId, Password);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());

                Console.WriteLine($"Download Complete, status {response.StatusDescription}");

                reader.Close();
                response.Close();

            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message.ToString());
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }


        public static void DownloadFile(string ParentFolderFilepath)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[2048];
            string localDestinationFilePath = @"D://BindingSnap//FTPTestingConsole";

            //string localDestinationFilePath = @"D://Internal//PaymentGatewayService//PaymentGatewayService//FileFTV";
            // string localDestinationFilePath = @"\\eztestweb02\apiroot\ezpaygateway\certificate_key";
            
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ParentFolderFilepath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(UserId, Password);
            Stream reader = request.GetResponse().GetResponseStream();

            //using (Stream fileStream = File.Create(localDestinationFilePath))
            //{
              //  reader.CopyTo(fileStream);
            //}
            
            using (var fileStream = File.Create("D:\\BindingSnap\\FTPTestingConsole\\Result.CSV"))
            {
                //reader.Seek(0, SeekOrigin.Begin);
                reader.CopyTo(fileStream);
            }

        }


        public static void DownloadFileFTPReadCSV(String ParentFolderFilepath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ParentFolderFilepath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(UserId, Password);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                string line;
                
                while ((line = reader.ReadLine()) != null)
                {
                    //Define pattern
                    Console.WriteLine(line);

                    //Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    //Separating columns to array
                    //string[] dataX = CSVParser.Split(line);
                    //Console.WriteLine(dataX);
                    /* Do something with X */
                }

                //Console.WriteLine(reader.ReadToEnd());
                

                Console.WriteLine($"Download Complete, status {response.StatusDescription}");

                reader.Close();
                response.Close();

            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message.ToString());
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }


        // SAMPLE CALL  UploadDatatoFTP("ftp://172.18.95.70/directorytest/TestFile0.txt","E:\yourlocation\SampleFile.txt")
        public static void UploadDatatoFTP(String ParentFolderFilepath, String SourceFile)
        {
            try
            {
                bool checkpath = DoesFtpDirectoryExist(ParentFolderFilepath);

                if (checkpath == true)
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ParentFolderFilepath);
                    request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(UserId, Password);

                    // Copy the contents of the file to the request stream.
                    //StreamReader sourceStream = new StreamReader(@"E:\yourlocation\SampleFile.txt");

                    StreamReader sourceStream = new StreamReader(SourceFile);
                    byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                    sourceStream.Close();
                    request.ContentLength = fileContents.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Close();
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

                    response.Close();
                }

            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message.ToString());
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        // SAMPLE CALL  UploadDatatoFTP2("ftp://172.18.95.70/directorytest/TestFile0.txt","E:\yourlocation\SampleFile.txt")
        public static void UploadDatatoFTP2(String ParentFolderFilepath, String SourceFile)
        {
            try
            {
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(UserId, Password);
                client.UploadFile(ParentFolderFilepath, SourceFile);

            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message.ToString());
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }



        public static int SendUploadFileFTP(string Directory,string fileName,string NewFileName)
        {
            var connectionInfo = new ConnectionInfo(hostIP, "ezeelink", new PasswordAuthenticationMethod(UserId, Password));

            // Upload File
            using (var sftp = new SftpClient(connectionInfo))
            {

                sftp.Connect();
                sftp.ChangeDirectory("./"+Directory);
                using (var uplfileStream = System.IO.File.OpenRead(fileName))
                {
                    sftp.UploadFile(uplfileStream, NewFileName, true);
                }
                sftp.Disconnect();
            }
            return 0;
        }


        //SAMPLE CALL DeleteFTPFile("ftp://172.18.95.70/directorytest/filetest.txt");
        public static void DeleteFTPFile(string Filepath)
        {

            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Filepath);
            ftpRequest.Credentials = new NetworkCredential(UserId, Password); ;
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            //ftpRequest.GetResponse();
            ftpRequest.GetResponse().Close();
        }


        //SAMPLE CALL DeleteFTPFolder("ftp://172.18.95.70/directorytest");
        public static void DeleteFTPFolder(string Folderpath)
        {

            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Folderpath);
            ftpRequest.Credentials = new NetworkCredential(UserId, Password); ;
            //ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;

            //ftpRequest.GetResponse();
            ftpRequest.GetResponse().Close();
        }

       

    }
}
