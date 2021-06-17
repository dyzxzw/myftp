using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace myftp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
                                                                  
        string localPath;
        string fileName;
       // bool UploadFile = false;
       bool  UploadFile = true;
        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(localPath) || string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("路径不对");
                return;
            }

            FileInfo fi = new FileInfo(localPath);
            FtpHelper ftp = new FtpHelper();
            int dateT=0;
            
            if (UploadFile)
            {
                ftp.UpLoadFile(localPath, FtpHelper.FtpHost + fileName);
                Console.WriteLine(localPath);
                Console.WriteLine(FtpHelper.FtpHost);
                Console.WriteLine(fileName);
            
              //  MessageBox.Show("success");
            }
            else
            {
                if (radioButton1.Checked)
                {
                    dateT = 0;
                }
                if (radioButton2.Checked)
                {
                    dateT = 500;
                }
                if (radioButton3.Checked)
                {
                    dateT = 1500;
                }
                ftp.UpLoadDirectory(localPath, FtpHelper.FtpHost ,fileName,dateT);
                //MessageBox.Show("fail");
            }
                
               
            localPath = "";
            fileName = "";
            UploadFile = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                UploadFile = true;
                localPath = openFileDialog1.FileName;
                fileName = localPath.Split('\\').Last();
            }   
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = "";

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                UploadFile = false;
                path = folderBrowserDialog1.SelectedPath;
            }

            string[] temp = path.Split('\\');
            fileName = temp.Last();
            localPath = path.Replace(fileName, "");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
