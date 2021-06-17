using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
namespace myftp
{
    class FtpHelper
    {
        #region FTP地址，账号，密码
        //FSU的FTP地址
        public static string FtpHost = "ftp://192.168.126.1/";
        //FSU的FTP账号
        public static string FtpAccount = "cntower";
        //FSU的FTP密码
        public static string FtpPassword = "12345";
        //实例化Timer类
        System.Timers.Timer aTimer = new System.Timers.Timer();
        #endregion


        /// <summary>
        /// 通过FTP上传单个文件
        /// </summary>
        /// <param name="localFile">要上传到FTP服务器的文件</param>
        /// <param name="ftpPath">FTP地址</param>
        #region 上传文件
        public  void UpLoadFile(string localFile, string ftpPath)
        {
            
            //判断文件是否存在
            if (!File.Exists(localFile))
            {
                MessageBox.Show("文件：“" + localFile + "” 不存在！");
                return;
            }
           
            //实现FTP传输协议客户端
            FtpWebRequest ftpWebRequest = null;

            
            //创建文件流对象
            FileStream localFileStream = null;

            
            //字节对象都被存储为连续的字节序列
            //字节按照一定的顺序进行排序组成了字节序列
            //创建流对象
            Stream requestStream = null;

            try
            {
                
                //根据uri创建FtpWebRequest对象
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(ftpPath);

                //ftp用户名和密码
                ftpWebRequest.Credentials = new NetworkCredential(FtpAccount, FtpPassword);

                // 默认为true，连接不会被关闭 在一个命令之后被执行
                ftpWebRequest.KeepAlive = false;

                // 指定数据传输类型,true表示服务器要传输的二进制数据
                ftpWebRequest.UseBinary = true;

                //表示可与FTP请求一起使用的FTP协议方法的类型。
                //Upload​File表示将文件上载到FTP服务器的 FTP STOR 协议方法。
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;

                //上传文件时通知服务器文件的大小 
                ftpWebRequest.ContentLength = localFile.Length;


               // ftpWebRequest.Proxy = new WebProxy();


                //缓冲大小设置为4kb  
                int buffLength = 4096;
                byte[] buff = new byte[buffLength];

                //提供用于创建、复制、删除、移动和打开文件的属性和实例方法
                ///并且帮助创建 FileStream 对象
                FileInfo fileInf = new FileInfo(localFile);

                // 打开一个文件流 (System.IO.FileStream) 去读上传的文件  
                localFileStream = fileInf.OpenRead();

                //把上传的文件写入流中
                //GetRequestStream()方法用于得到向FTP服务器上传的数据流
                requestStream = ftpWebRequest.GetRequestStream();

                //初始化局部变量,字节总数
                int contentLen;
                //每次读文件流的4KB
                contentLen = localFileStream.Read(buff, 0, buffLength);

                //如果文件流内容没有结束
                while (contentLen != 0)
                {
                    //把内容从filestream写入upload stream  
                    requestStream.Write(buff, 0, contentLen);
                    contentLen = localFileStream.Read(buff, 0, buffLength);
                }
            }

            catch (Exception ex)
            {
               
                MessageBox.Show(ex.Message, "FileUpLoad0001");
            }
            //关闭两个流
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
                if (localFileStream != null)
                {
                    localFileStream.Close();
                }
               
            }


        }
        #endregion



        /// <summary>
        /// 判断FTP服务器中该目录是否存在
        /// </summary>
        /// <param name="ftpPath">FTP路径目录</param>
        /// <param name="dirName">要检查的目录的文件夹名称</param>
        /// <returns></returns>
        #region 检查目录是否存在
        public static bool CheckDirectoryExist(string ftpPath,string dirName)
        {
            bool flag = false;
            try
            {
                //实例化FTP连接
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpPath+dirName);
                //ftp用户名和密码,获取通信
                request.Credentials = new NetworkCredential(FtpAccount, FtpPassword);
                //指定FTP操作类型为获取目录
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                //获取FTP服务器的响应
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //关闭连接
                response.Close();
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        #endregion


        /// <summary>
        /// 新建一个目录
        /// </summary>
        /// <param name="ftpPath">ftp的路径</param>
        /// <param name="dirName">文件夹的名称</param>
        #region 新建目录
        public static void MakeDir(string ftpPath, string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                string ui = (ftpPath + dirName).Trim();
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(ui);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FtpAccount, FtpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
                MessageBox.Show("文件夹【" + dirName + "】创建成功！");
            }
            catch(Exception ex)
            {
                MessageBox.Show("新建文件夹【" + dirName + "】时，发生错误：" + ex.Message);
            }
        }

        #endregion

        /// <summary>  
        /// 获取目录下的详细信息  
        /// </summary>  
        /// <param name="localDir">本机目录</param>  
        /// <returns></returns>  
        /// 
        #region 获取目录下详细信息
        private static List<List<string>> GetDirDetails(string localDir)
        {
            List<List<string>> infos = new List<List<string>>();
            try
            {
                infos.Add(Directory.GetFiles(localDir).ToList()); //获取当前目录的文件  

                infos.Add(Directory.GetDirectories(localDir).ToList()); //获取当前目录的目录  

                for (int i = 0; i < infos[0].Count; i++)
                {
                    int index = infos[0][i].LastIndexOf(@"\");
                    infos[0][i] = infos[0][i].Substring(index + 1);
                }
                for (int i = 0; i < infos[1].Count; i++)
                {
                    int index = infos[1][i].LastIndexOf(@"\");
                    infos[1][i] = infos[1][i].Substring(index + 1);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return infos;
        }

        #endregion



        #region 上传文件夹
        /// <summary>
        /// 通过FTP上传整个文件夹内的文件
        /// </summary>
        /// <param name="localDir"></param>
        /// <param name="ftpPath"></param>
        /// <param name="dirName"></param>
        /// 
        public  void UpLoadDirectory(string localDir, string ftpPath, string dirName,int dateT)
        {
            string dir = localDir + dirName + @"\"; //获取当前目录（父目录在目录名）  
                                                    //检测本地目录是否存在  
            if (!Directory.Exists(dir))
            {
                MessageBox.Show("本地目录：“" + dir + "” 不存在！<br/>");
                return;
            }
            //检测FTP的目录路径是否存在  
            if (!CheckDirectoryExist(ftpPath, dirName))
            {
                MakeDir(ftpPath, dirName);//不存在，则创建此文件夹  
            }
            
            List<List<string>> infos = GetDirDetails(dir); //获取当前目录下的所有文件和文件夹  

            //先上传文件  
            //Response.Write(dir + "下的文件数：" + infos[0].Count.ToString() + "<br/>");  
            for (int i = 0; i < infos[0].Count; i++)
            {
                Console.WriteLine(infos[0][i]);
                UpLoadFile(dir + infos[0][i], ftpPath + dirName + @"/" + infos[0][i]);
            }
            
            Thread.Sleep(dateT);
            
            //再处理文件夹  
            //Response.Write(dir + "下的目录数：" + infos[1].Count.ToString() + "<br/>");  
            for (int i = 0; i < infos[1].Count; i++)
            {
                UpLoadDirectory(dir, ftpPath + dirName + @"/", infos[1][i],0);
                //Response.Write("文件夹【" + dirName + "】上传成功！<br/>");  
            }
        }
        #endregion



     

    }





}
