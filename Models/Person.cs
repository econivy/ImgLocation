using ImgLocation.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace ImgLocation.Models
{
    [XmlType(TypeName = "Person")]
    public class Person
    {
        public string XingMing { get; set; }
        public string XingBie { get; set; }
        public string ChuShengNianYue { get; set; }
        public string MinZu { get; set; }
        public string JiGuan { get; set; }
        public string ChuShengDi { get; set; }
        public string RuDangShiJian { get; set; }
        public string CanJiaGongZuoShiJian { get; set; }
        public string JianKangZhuangKuang { get; set; }
        public string ZhuanYeJiShuZhiWu { get; set; }
        public string ShuXiZhuanYeYouHeZhuanChang { get; set; }

        public string QuanRiZhiJiaoYu_XueLi { get; set; }
        public string QuanRiZhiJiaoYu_XueWei { get; set; }
        public string QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi { get; set; }
        public string QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi { get; set; }

        public string ZaiZhiJiaoYu_XueLi { get; set; }
        public string ZaiZhiJiaoYu_XueWei { get; set; }
        public string ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi { get; set; }
        public string ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi { get; set; }
        public string XianRenZhiWu { get; set; }
        public string NiRenZhiWu { get; set; }
        public string NiMianZhiWu { get; set; }
        public string JianLi { get; set; }

        /////以上是第一页

        /////以下是第二页

        public string JiangChengQingKuang { get; set; }

        public string NianDuKaoHeJieGuo { get; set; }

        public string RenMianLiYou { get; set; }

        [XmlArray("JiaTingChengYuan")]
        public List<Item> JiaTingChengYuan { get; set; }

        public string ChengBaoDanWei { get; set; }
        public string JiSuanNianLingShiJian { get; set; }
        public string TianBiaoShiJian { get; set; }
        public string TianBiaoRen { get; set; }

        public Image ZhaoPian_Image { get; set; }

        public string ZhaoPian { get; set; }
        public string Version { get; set; }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管资源
                if(this.ZhaoPian_Image!=null)
                {
                    this.ZhaoPian_Image.Dispose();
                }
            }
            //清理非托管资源
        }
        public void Dispose()
        {
            //调用带参数的Dispose方法，释放托管和非托管资源
            Dispose(true);
            //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
            //不需要再调用本对象的Finalize方法
            GC.SuppressFinalize(this);
        }
        ~Person()
        {
            Dispose();
        }


        //public void ReadyForSave()
        //{
        //    if (!this.ZhaoPian_Image.Equals(null))
        //    {
        //        byte[] bt = null;
        //        using (MemoryStream mostream = new MemoryStream())
        //        {
        //            Bitmap bmp = new Bitmap(this.ZhaoPian_Image);
        //            bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Bmp);//将图像以指定的格式存入缓存内存流
        //            bt = new byte[mostream.Length];
        //            mostream.Position = 0;//设置留的初始位置
        //            mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
        //            this.ZhaoPian = System.Text.Encoding.Default.GetString(bt);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("ZhaoPian_Image is Null");
        //    }
        //}

        //public void ReadForRead()
        //{
        //    if(!this.ZhaoPian.Equals(null))
        //    {
        //        byte[] bt = System.Text.Encoding.Default.GetBytes(this.ZhaoPian);
        //        Image photo = null;
        //        using (MemoryStream ms = new MemoryStream(bt))
        //        {
        //            ms.Write(bt, 0, bt.Length);
        //            photo = Image.FromStream(ms, true);
        //            this.ZhaoPian_Image = photo;
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("ZhaoPian is Null");
        //    }
        //}

    }

    public class Item
    {
        public string ChengWei { get; set; }
        public string XingMing { get; set; }
        public string ChuShengRiQi { get; set; }
        public string ZhengZhiMianMao { get; set; }
        public string GongZuoDanWeiJiZhiWu { get; set; }
    }

    public class PersonWithFile:Person
    {
        public bool IsLrmx { get; set; }

        public string LrmFullPath { get; set; }
        public string PicFullPath { get; set; }
        public string LrmFilename { get; set; }

        public string LrmxFullPath { get; set; }
        public string LrmxFilename { get; set; }

    }

    public class DocumentWithFile
    {
        public string DocxFullPath { get; set; }
        public string DocxFilename { get; set; }
        public string Content { get; set; }
        public DocumentWithFile()
        {

        }
        public DocumentWithFile(string fullpath)
        {
            if(File.Exists(fullpath))
            {
                this.DocxFullPath = fullpath;
                this.DocxFilename = Path.GetFileName(fullpath);
            }
        }
    }
}
