using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myftp
{
    class FtpEntity
    {
        //编号
        public int ID { get; set; }
        //文件名
        public string FileName { get; set; }
        //扩展名
        public string FileType { get; set; }
        //完整文件名
        public string FileFullName { get; set; }
        //上传时间 DateTime?表示类型可为空
        public DateTime? UploadTime { get; set; }
    }

}
