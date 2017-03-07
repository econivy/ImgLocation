
using ImgLocation.Models;
using ImgLocation.Repository;
using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace ImgLocation.Services
{
    public struct FileInformation
    {
        public string FilenameWithoutExtension;
        public string FileExtension;
        public string FileFullname;
    }

    /// <summary>
    /// 负责与源文件目录内的文件进行交互操作
    /// </summary>
    public class ConvertHelper
    {
        public ConvertHelper()
        {

        }
        public ConvertHelper(string sourceLrmDirectory, string sourceResDirectory)
        {
            this.SourceLrmDirectory = sourceLrmDirectory;
            this.SourceResDirectory = sourceResDirectory;
            this.GetAllFileInformatons();
        }


        void GetAllFileInformatons()
        {
            if (Directory.Exists(SourceLrmDirectory) && Directory.Exists(SourceResDirectory))
            {
                List<FileInformation> allFileInformations = new List<FileInformation>();
                List<String> allDirectory = new List<String>();

                allDirectory.Add(SourceLrmDirectory);
                DirectoryInfo lrmDirectoryInfos = new DirectoryInfo(SourceLrmDirectory);
                //遍历任免表目录下的子目录
                foreach (FileSystemInfo lrmChildrenInfo in lrmDirectoryInfos.GetFileSystemInfos())
                {
                    if ((lrmChildrenInfo is DirectoryInfo))
                    {
                        allDirectory.Add(lrmChildrenInfo.FullName);
                    }
                }

                //如果任免表目录与考察文件目录不一致时，遍历考察材料目录子目录
                if (SourceLrmDirectory != SourceResDirectory)
                {
                    allDirectory.Add(SourceResDirectory);
                    DirectoryInfo resDirectoryInfos = new DirectoryInfo(SourceResDirectory);
                    foreach (FileSystemInfo resChildrenInfo in resDirectoryInfos.GetFileSystemInfos())
                    {
                        if ((resChildrenInfo is DirectoryInfo))
                        {
                            allDirectory.Add(resChildrenInfo.FullName);
                        }
                    }
                }
                //遍历所有目录与子目录获取信息
                foreach (String directory in allDirectory)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                    foreach (FileSystemInfo fileInfo in directoryInfo.GetFileSystemInfos())
                    {
                        if ((fileInfo is FileInfo))
                        {
                            FileInformation f = new FileInformation();
                            f.FilenameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName).Replace(" ", "").Replace("　", "");
                            f.FileExtension = fileInfo.Extension.ToLower();
                            f.FileFullname = fileInfo.FullName;
                            allFileInformations.Add(f);
                        }
                    }
                }
                this.AllFileInformatons = allFileInformations;
            }
            else
            {
                this.AllFileInformatons = new List<FileInformation>();
            }
        }

        //公开属性
        public string SourceLrmDirectory { get; set; }
        public string SourceResDirectory { get; set; }
        public List<FileInformation> AllFileInformatons { get; set; }

        //公开方法
        public void UpdateDataId()
        {
            Project p = Global.LoadDefaultProject();
            p.LASTDATAID = Guid.NewGuid().ToString();
            SystemRepository sr = new SystemRepository();
            sr.EditProject(p);
        }

        public bool RelationIsAlive(string duty)
        {
            List<string> deadlist = new List<string>();
            deadlist.Add("去世");
            deadlist.Add("已逝");
            deadlist.Add("逝世");
            deadlist.Add("病故");
            deadlist.Add("已故");
            bool isalive = true;
            foreach (string dead in deadlist)
            {
                if ((duty + "").Contains(dead))
                {
                    isalive = false;
                }
            }
            return isalive;
        }
        public void ZoomImageToFile(Image img, string ofile, double times)
        {

            Bitmap bm = (Bitmap)img;
            int nowWidth = (int)(bm.Width * times);
            int nowHeight = (int)(bm.Height * times);
            Bitmap new_Bitmap = new Bitmap(nowWidth, nowHeight);//新建一个放大后大小的图片
            
                Graphics g = Graphics.FromImage(new_Bitmap);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(bm, new Rectangle(0, 0, nowWidth, nowHeight), new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                g.Dispose();

            new_Bitmap.Save(ofile);
            bm.Dispose();
            new_Bitmap.Dispose();
        }

        public bool JudgeStringHasLrm(string XM)
        {
            return (XM.Length > 1 && this.AllFileInformatons.Where(f => f.FilenameWithoutExtension == XM && (f.FileExtension == ".lrm" || f.FileExtension == ".lrmx")).Any());
        }
        public string GetLrmFilePath(string XM)
        {
            IQueryable<FileInformation> qf = this.AllFileInformatons.Where(f => f.FilenameWithoutExtension == XM && (f.FileExtension == ".lrm" || f.FileExtension == ".lrmx")).AsQueryable();
            return qf.Any() ? qf.First().FileFullname : "";
        }
        public string GetPicFilePath(string XM)
        {
            IQueryable<FileInformation> qf = this.AllFileInformatons.Where(f => f.FilenameWithoutExtension == XM && (f.FileExtension == ".pic")).AsQueryable();
            return qf.Any() ? qf.First().FileFullname : "";
        }
        public string GetResFilePath(string XM)
        {
            IQueryable<FileInformation> qf = this.AllFileInformatons.Where(f => f.FilenameWithoutExtension.Contains(XM) && f.FilenameWithoutExtension.Contains("考察") && (f.FileExtension == ".doc" || f.FileExtension == ".docx")).AsQueryable();
            return qf.Any() ? qf.First().FileFullname : "";
        }



      
    }
}
