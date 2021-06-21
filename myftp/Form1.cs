using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Threading;
namespace myftp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //定义本地文件路径                                         
        static string localPath;
        //定义文件名称
        static string fileName;
       //初始化标记为true
        bool  flag = true;
       //选中的文件路径
        static string path;


        #region 确认上传按钮
        private void button1_Click(object sender, EventArgs e)
        {
           
            //如果文件路径或者文件名称为空，则提示不对
            if (string.IsNullOrEmpty(localPath) || string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("路径不对");
                return;
            }
            FileInfo fi = new FileInfo(localPath);
            FtpHelper ftp = new FtpHelper();
            //初始化单个文件的上传时间间隔为0
            int dateT = 0;
            //如果flag=true，则为上传单个文件的功能
            if (flag)
            {
                //n为文件上传的次数上限
                int n = int.Parse(textBox5.Text);
                while (n>0)
                {
                    string tmpRand = FtpHelper.GenerateRandomCode(4);
                    Thread.Sleep(int.Parse(textBox4.Text));
                    //上传单个文件
                    ftp.UpLoadFile(localPath, FtpHelper.FtpHost + tmpRand + fileName);

                    /*输出信息*/
                    Console.WriteLine(localPath);
                    Console.WriteLine(FtpHelper.FtpHost);
                    Console.WriteLine(tmpRand + fileName);
                    n--;
                }
                
            }
            //若flag=false,则为上传文件夹的功能
            else
            {
                //n为文件夹轮询的次数，即周期数
                int n = int.Parse(textBox2.Text);
                while (n>0)
                {
                    ////如果选择按钮1，则时间间隔为0ms
                    //if (radioButton1.Checked)
                    //{
                    //    dateT = 0;
                    //}
                    ////如果选择按钮2，则时间间隔为500ms
                    //else if (radioButton2.Checked)
                    //{
                    //    dateT = 500;
                    //}
                    ////如果选择按钮3，则时间间隔为1500ms
                    //else if (radioButton3.Checked)
                    //{
                    //    dateT = 1500;
                    //}
                    ////如果不选择任何按钮，则时间间隔为自定义的输入
                    //else
                    //{
                    //    dateT = int.Parse(textBox1.Text);
                    //}

                    //dateT为文件夹上传的时间间隔，采用线程休眠实现
                    dateT= int.Parse(textBox1.Text);
                    Thread.Sleep(int.Parse(textBox3.Text));
                    //上传文件夹
                    ftp.UpLoadDirectory(localPath, FtpHelper.FtpHost, fileName, dateT);
                    //获取文件夹的文件内容
                    List<List<string>> infos = FtpHelper.GetDirDetails(localPath + fileName + @"\");
                   //遍历内容，添加到控件Listbox中
                    for (int i = 0; i < infos[0].Count; i++)
                    {
                        Console.WriteLine(infos[0][i]);
                        listBox1.Items.Add(localPath + fileName+ ":"+infos[0][i]);
                    }


                    Console.WriteLine(localPath);
                    Console.WriteLine(FtpHelper.FtpHost);
                    Console.WriteLine(fileName);
                    n--;
                }
            }
               
            //初始化三个变量
            localPath = "";
            fileName = "";
            flag = false;
        }
        #endregion

        #region 上传单个文件功能按钮
        private void button2_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                flag = true;
                localPath = openFileDialog1.FileName;
                 fileName = localPath.Split('\\').Last();
              
            }
        }
        #endregion

        #region 上传文件夹功能按钮
        private void button4_Click(object sender, EventArgs e)
        {
           // string path = "";

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                flag = false;
                path = folderBrowserDialog1.SelectedPath;
            }

            string[] temp = path.Split('\\');
            fileName = temp.Last();
            localPath = path.Replace(fileName, "");
        }
        #endregion








        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
