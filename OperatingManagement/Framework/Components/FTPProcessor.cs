using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework.Components
{
    /// <summary>
    /// FTP服务核心类
    /// </summary>
    public class FTPProcessor
    {
        #region Property

        private string _ftpUri;
        private string _userName;
        private string _password;

        /// <summary>
        /// 服务器连接地址，如ftp://192.168.1.10/FolderName/
        /// 指定到要操作的FTP目录
        /// </summary>
        public string FTPUri
        {
            get { return _ftpUri; }
            set
            {
                _ftpUri = value.EndsWith("/") ? value : (value + "/");
            }
        }

        /// <summary>
        /// FTP服务器帐户
        /// </summary>
        public string UserName
        {
            get { return _userName; }
        }

        /// <summary>
        /// FTP服务器密码
        /// </summary>
        public string Password
        {
            get { return _password; }
        }

        #endregion

        #region Constructor

        public FTPProcessor(string ftpUri, string userName, string password)
        {
            if (string.IsNullOrEmpty(ftpUri))
                throw new Exception("ftpUri不能为空。");

            if (string.IsNullOrEmpty(userName))
                throw new Exception("userName不能为空。");

            if (string.IsNullOrEmpty(password))
                throw new Exception("password不能为空。");

            _ftpUri = ftpUri.EndsWith("/") ? ftpUri : (ftpUri + "/");
            _userName = userName;
            _password = password;
        }

        public FTPProcessor(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new Exception("userName不能为空。");

            if (string.IsNullOrEmpty(password))
                throw new Exception("password不能为空。");

            _userName = userName;
            _password = password;
        }

        #endregion

        #region Private Method

        private FtpWebRequest GetConnection()
        {
            string uri = FTPUri;
            if (string.IsNullOrEmpty(uri))
                throw new Exception("FTPUri不能为空，请赋值，如ftp://192.168.1.10/FolderName/");

            FtpWebRequest ftpWebRequest = GetConnection(uri);
            return ftpWebRequest;
        }

        private FtpWebRequest GetConnection(string uri)
        {
            FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(uri)) as FtpWebRequest;
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.Credentials = new NetworkCredential(UserName, Password);

            return ftpWebRequest;
        } 

        #endregion

        #region Public Method
        /// <summary>
        /// 获得FTP服务器上指定文件夹文件列表
        /// </summary>
        /// <returns>指定文件夹文件列表</returns>
        public List<string> GetDirectoryList()
        {
            List<string> fileList = new List<string>();
            StringBuilder strBuilder = new StringBuilder();

            try
            {
                FtpWebRequest ftpWebRequest = GetConnection();
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse webResponse = ftpWebRequest.GetResponse();
                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.Default);
                string fileName = reader.ReadLine();
                while (fileName != null)
                {
                    fileList.Add(fileName);
                    fileName = reader.ReadLine();
                }

                reader.Close();
                webResponse.Close();

                return fileList;
            }
            catch(Exception ex)
            {
                throw new Exception("发生未知错误。", ex);
            }
        }


        /// <summary>
        /// 从本地计算机向FTP服务器上传文件
        /// </summary>
        /// <param name="localDirectory">本地目录，如：C:\Folder1\</param>
        /// <param name="localFileName">本地文件名称，如：File1.txt</param>
        /// <param name="ftpFileName">保存到FTP服务器文件名称，如：File1.txt</param>
        /// <returns></returns>
        public bool UploadFile(string localDirectory, string localFileName, string ftpFileName)
        {
            if (string.IsNullOrEmpty(localDirectory))
            {
                throw new Exception("localDirectory参数不能为空。");
            }
            if (string.IsNullOrEmpty(localFileName))
            {
                throw new Exception("localFileName参数不能为空。");
            }
            if (string.IsNullOrEmpty(ftpFileName))
            {
                throw new Exception("ftpFileName参数不能为空。");
            }
            if (!localDirectory.EndsWith(@"\"))
            {
                localDirectory = localDirectory + @"\";
            }
            if (!Directory.Exists(localDirectory))
            {
                throw new Exception(string.Format("指定的本地文件路径{0}不存在。", localDirectory));
            }
            if (!File.Exists(localDirectory + localFileName))
            {
                throw new Exception(string.Format("指定的本地文件路径{0}下的文件{1}不存在。", localDirectory, localFileName));
            }
            try
            {
                FileInfo fileInfo = new FileInfo(localDirectory + localFileName);
                string uri = FTPUri + ftpFileName;
                FtpWebRequest ftpWebRequest = GetConnection(uri);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpWebRequest.ContentLength = fileInfo.Length;

                int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                int contentLength;

                FileStream fileStream = fileInfo.OpenRead();
                Stream stream = ftpWebRequest.GetRequestStream();
                contentLength = fileStream.Read(buffer, 0, bufferLength);

                while (contentLength != 0)
                {
                    stream.Write(buffer, 0, contentLength);
                    contentLength = fileStream.Read(buffer, 0, bufferLength);
                }
                fileStream.Close();
                stream.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("发生未知错误。", ex);
            }
        }

        /// <summary>
        /// 续传文件
        /// </summary>
        /// <param name="localFilePath">本地文件路径，如：C:\Folder1\File1.txt</param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool AppendUploadFile(string localDirectory, string localFileName, string ftpFileName, long offset)
        {
            if (string.IsNullOrEmpty(localDirectory))
            {
                throw new Exception("localDirectory参数不能为空。");
            }
            if (string.IsNullOrEmpty(localFileName))
            {
                throw new Exception("localFileName参数不能为空。");
            }
            if (string.IsNullOrEmpty(ftpFileName))
            {
                throw new Exception("ftpFileName参数不能为空。");
            }
            if (!localDirectory.EndsWith(@"\"))
            {
                localDirectory = localDirectory + @"\";
            }
            if (!Directory.Exists(localDirectory))
            {
                throw new Exception(string.Format("指定的本地文件路径{0}不存在。", localDirectory));
            }
            if (!File.Exists(localDirectory + localFileName))
            {
                throw new Exception(string.Format("指定的本地文件路径{0}下的文件{1}不存在。", localDirectory, localFileName));
            }
            try
            {
                FileInfo fileInfo = new FileInfo(localDirectory + localFileName);
                string uri = FTPUri + ftpFileName;
                FtpWebRequest ftpWebRequest = GetConnection(uri);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.AppendFile;
                ftpWebRequest.ContentLength = fileInfo.Length - offset;

                int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                int contentLength;

                FileStream fileStream = fileInfo.OpenRead();
                fileStream.Seek(offset, SeekOrigin.Begin);
                Stream stream = ftpWebRequest.GetRequestStream();
                contentLength = fileStream.Read(buffer, 0, bufferLength);

                while (contentLength != 0)
                {
                    stream.Write(buffer, 0, contentLength);
                    contentLength = fileStream.Read(buffer, 0, bufferLength);
                }
                fileStream.Close();
                stream.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("发生未知错误。", ex);
            }
        }

        /// <summary>
        /// 从FTP服务器下载文件到本地指定目录
        /// </summary>
        /// <param name="localDirectory">本地目录，如：C:\Folder1\</param>
        /// <param name="localFileName">保存到本地文件名称，如：File1.txt</param>
        /// <param name="ftpFileName">服务器文件名称，如：File1.txt</param>
        /// <returns></returns>
        public bool DownloadFile(string localDirectory, string localFileName, string ftpFileName)
        {
            if (string.IsNullOrEmpty(localDirectory))
            {
                throw new Exception("localDirectory参数不能为空。");
            }
            if (string.IsNullOrEmpty(localFileName))
            {
                throw new Exception("localFileName参数不能为空。");
            }
            if (string.IsNullOrEmpty(ftpFileName))
            {
                throw new Exception("ftpFileName参数不能为空。");
            }
            if (!localDirectory.EndsWith(@"\"))
            {
                localDirectory = localDirectory + @"\";
            }
            if (!Directory.Exists(localDirectory))
            {
                throw new Exception(string.Format("指定的本地存储路径{0}不存在。", localDirectory));
            }
            if (File.Exists(localDirectory + localFileName))
            {
                throw new Exception(string.Format("指定的本地存储路径{0}下的文件{1}已存在。", localDirectory, localFileName));
            }

            try
            {
                string uri = FTPUri + ftpFileName;
                FtpWebRequest ftpWebRequest = GetConnection(uri);
                ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = ftpWebRequest.GetResponse() as FtpWebResponse;
                Stream stream = response.GetResponseStream();
                long contentLength = response.ContentLength;

                int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                int readCount = stream.Read(buffer, 0, bufferLength);
                FileStream fileStream = new FileStream(localDirectory + localFileName, FileMode.Create);
                while (readCount > 0)
                {
                    fileStream.Write(buffer, 0, readCount);
                    readCount = stream.Read(buffer, 0, bufferLength);
                }

                stream.Close();
                fileStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("发生未知错误。", ex);
            }
        }

        /// <summary>
        /// 删除FTP服务器上的文件
        /// </summary>
        /// <param name="ftpFileName">FTP上文件名称，如：file1.txt</param>
        /// <returns></returns>
        public bool DeleteFile(string ftpFileName)
        {
            try
            {
                string uri = FTPUri + ftpFileName;
                FtpWebRequest ftpWebRequest = GetConnection(uri);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                FtpWebResponse response = ftpWebRequest.GetResponse() as FtpWebResponse;
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("发生未知错误。", ex);
            }
        }

        /// <summary>
        /// 在FTP服务器上创建目录
        /// </summary>
        /// <param name="directoryName">目录名称</param>
        /// <returns></returns>
        public bool MakeDirectory(string directoryName)
        {
            try
            {
                string uri = FTPUri + directoryName;
                FtpWebRequest ftpWebRequest = GetConnection(uri);
                ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = ftpWebRequest.GetResponse() as FtpWebResponse;
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("发生未知错误。", ex);
            }
        }

        /// <summary>
        /// 删除FTP上的目录
        /// </summary>
        /// <param name="directoryName">目录名称</param>
        /// <returns></returns>
        public bool RemoveDirectory(string directoryName)
        {
            try
            {
                string uri = FTPUri + directoryName;
                FtpWebRequest ftpWebRequest = GetConnection(uri);
                ftpWebRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;

                FtpWebResponse response = ftpWebRequest.GetResponse() as FtpWebResponse;
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("发生未知错误。", ex);
            }
        }

        /// <summary>
        /// 获得FTP服务器文件大小
        /// </summary>
        /// <param name="ftpFileName"></param>
        /// <returns></returns>
        public long GetFileSize(string ftpFileName)
        {
            long fileSize = 0;
            try
            {
                string uri = FTPUri + ftpFileName;
                FtpWebRequest ftpWebRequest = GetConnection(uri);
                ftpWebRequest.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebResponse response = ftpWebRequest.GetResponse() as FtpWebResponse;
                fileSize = response.ContentLength;
                response.Close();
                return fileSize;
            }
            catch (Exception ex)
            {
                throw new Exception("发生未知错误。", ex);
            }
        }

        /// <summary>
        /// 更改FTP上文件名称
        /// </summary>
        /// <param name="oldFTPDirectory">原FTP文件名</param>
        /// <param name="newFTPDirectory">新FTP文件名</param>
        /// <returns></returns>
        public bool Rename(string oldFTPDirectory, string newFTPDirectory)
        {
            try
            {
                string uri = FTPUri + oldFTPDirectory;
                FtpWebRequest ftpWebRequest = GetConnection(uri);
                ftpWebRequest.Method = WebRequestMethods.Ftp.Rename;
                ftpWebRequest.RenameTo = newFTPDirectory;

                FtpWebResponse response = ftpWebRequest.GetResponse() as FtpWebResponse;
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("发生未知错误。", ex);
            }
        }

        #endregion
    }
}
