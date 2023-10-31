using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPTestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("List Data");
                List<string> allFileData = FTPTestingHelper.GetAllFtpFiles("ftp://172.18.95.70/TESTFTP");

                Console.WriteLine("List Process ");

                foreach (string item in allFileData)
                {
                    Console.WriteLine(" ");
                    Console.Write(item);
                    Console.WriteLine(" ");
                }

                Console.WriteLine(" ");

                Console.WriteLine("Download Prcocess");
                //FTPTestingHelper.DownloadFileFTP("ftp://172.18.95.70/TESTFTP/Tempdata31.txt");
                FTPTestingHelper.DownloadFileFTPReadCSV("ftp://172.18.95.70/TESTFTP/testing1.csv");
                FTPTestingHelper.DownloadFile("ftp://172.18.95.70/TESTFTP/testing1.csv");
                Console.WriteLine("Complete Download");

                Console.WriteLine("Upload Prcocess");
                FTPTestingHelper.SendUploadFileFTP("TESTFTP", @"D:\BindingSnap\FTPTestingConsole\testing2.csv", "testing2.csv");
                Console.WriteLine("Complete Upload");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}
