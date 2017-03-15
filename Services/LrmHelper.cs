using ImgLocation.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ImgLocation.Services
{
    public class LrmHelper
    {
        public LrmHelper() { }

        private string ContentText { get; set; }

        public Person GetPersonFromLrmFile(string LrmPath)
        {
            if (!File.Exists(LrmPath))
            {
                throw new FileNotFoundException(string.Format("指定路径的任免审批表文件{0}不存在。",LrmPath));
            }
            else if (Path.GetExtension(LrmPath) == ".lrm")
            {
                string PicPath = Path.Combine(Path.GetDirectoryName(LrmPath), Path.GetFileNameWithoutExtension(LrmPath)+".pic");
                if(!File.Exists(PicPath))
                {
                    throw new FileNotFoundException(string.Format("指定路径的任免审批表2.0文件{0}的头像文件{1}不存在。",LrmPath,PicPath));
                }
                else
                {
                    return this.GetPersonFromLrm(LrmPath,PicPath);
                }
            }
            else if (Path.GetExtension(LrmPath) == ".lrmx")
            {
                return GetPersonFromLrmx(LrmPath);
            }
            else
            {
                throw new FileFormatException(string.Format("指定路径的文件{0}不是合法的任免审批表2.0或者3.0格式文件。", LrmPath));
            }
        }


        Person GetPersonFromLrmx(string lrmxPath)
        {
            if (File.Exists(lrmxPath))
            {
                using (StreamReader reader = new StreamReader(lrmxPath))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Person));
                    Person p = (Person)xs.Deserialize(reader);
                    p.ChuShengNianYue = this.AddDot(p.ChuShengNianYue);
                    p.RuDangShiJian = this.AddDot(p.RuDangShiJian);
                    p.CanJiaGongZuoShiJian = this.AddDot(p.CanJiaGongZuoShiJian);
                    p.JiSuanNianLingShiJian = this.AddDot(p.JiSuanNianLingShiJian);

                    foreach (Item i in p.JiaTingChengYuan)
                    {
                        i.ChuShengRiQi = this.AddDot(i.ChuShengRiQi);
                    }

                    p.JianLi = p.JianLi.Replace("","");

                    if ((p.ZhaoPian + "").Trim().Length > 0)
                    {
                        byte[] b = Convert.FromBase64String(p.ZhaoPian);//转化为byte[]  
                        using (MemoryStream sm = new MemoryStream())
                        {
                            sm.Write(b, 0, b.Length);//把照片写到流中  
                            p.ZhaoPian_Image = Image.FromStream(sm);
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException(p.XingMing + "'s ZhaoPian is not Exists");
                    }
                    return p;
                }
            }
            else
            {
                throw new ArgumentNullException(lrmxPath + " is not Exists");
            }
        }

        /// <summary>
        /// 从中组部文件读出文本流为依据创建干部信息
        /// </summary>
        /// <returns></returns>
        Person GetPersonFromLrm(string lrmPath, string picPath)
        {
            this.ContentText = this.ReadFile(lrmPath);

            Person cadre_person = new Person();
            Regex r = new Regex(@"^\d{6}|\d{8}$");

            string[] spliterArrayInfo = this.SpliterArrayInfo();
            string[,] spliterArrayRelations = this.SpliterArrayRelations();

            cadre_person.XingMing = spliterArrayInfo[0];
            cadre_person.XingBie = spliterArrayInfo[1];

            if (!r.Match(spliterArrayInfo[2].Trim()).Success)
            {
                //出生日期格式不匹配，无法计算年龄
                throw new Exception(cadre_person.XingMing + "的任免审批表中出生年月格式不正确！");
            }
            cadre_person.ChuShengNianYue = this.AddDot(spliterArrayInfo[2]);

            cadre_person.MinZu = spliterArrayInfo[3];
            cadre_person.ChuShengDi = spliterArrayInfo[7];
            cadre_person.JiGuan = spliterArrayInfo[4];

            //入党时间，无党派和民主党派通过ADDDot直接显示
            cadre_person.RuDangShiJian = this.AddDot(spliterArrayInfo[5]);

            cadre_person.JianKangZhuangKuang = spliterArrayInfo[6];

            //工作时间，无党派和民主党派通过ADDDot直接显示
            cadre_person.CanJiaGongZuoShiJian = this.AddDot(spliterArrayInfo[8]);

            cadre_person.QuanRiZhiJiaoYu_XueLi = spliterArrayInfo[9];
            cadre_person.QuanRiZhiJiaoYu_XueWei = spliterArrayInfo[10];
            cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi = spliterArrayInfo[13];
            cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi = spliterArrayInfo[14];

            cadre_person.ZaiZhiJiaoYu_XueLi = spliterArrayInfo[11];
            cadre_person.ZaiZhiJiaoYu_XueWei = spliterArrayInfo[12];
            cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi = spliterArrayInfo[15];
            cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi = spliterArrayInfo[16];

            cadre_person.ZhuanYeJiShuZhiWu = spliterArrayInfo[17];//技术职称
            cadre_person.ShuXiZhuanYeYouHeZhuanChang = spliterArrayInfo[18];//熟悉专业有何专长
            cadre_person.JianLi = spliterArrayInfo[19];//工作简历
            cadre_person.JiangChengQingKuang = spliterArrayInfo[20];//奖惩情况            
            cadre_person.NianDuKaoHeJieGuo = spliterArrayInfo[21];//年度考核信息
            cadre_person.XianRenZhiWu = spliterArrayInfo[22].Replace("\r", "").Replace("\n", "");//现任职务
            cadre_person.NiRenZhiWu = spliterArrayInfo[23].Replace("\r", "").Replace("\n", "");//拟任职务
            cadre_person.NiMianZhiWu = spliterArrayInfo[24].Replace("\r", "").Replace("\n", "");//拟免职务            
            cadre_person.RenMianLiYou = spliterArrayInfo[25];//任免理由
            cadre_person.ChengBaoDanWei = spliterArrayInfo[26];//呈报单位

            if (!r.Match(spliterArrayInfo[27].Trim()).Success)
            {
                spliterArrayInfo[27] = DateTime.Now.ToString("yyyyMM");
                //throw new Exception(spliterArrayInfo[0].ToString() + "的任免审批表中年龄计算时间格式不正确！");
            }
            cadre_person.JiSuanNianLingShiJian = this.AddDot(spliterArrayInfo[27]);//计算年龄时间

            cadre_person.TianBiaoRen = spliterArrayInfo[28];//录入人
            cadre_person.TianBiaoShiJian = spliterArrayInfo[29].Replace("\"", "");//录入时间

            cadre_person.JiaTingChengYuan = new List<Item>();

            for (int i = 0; i < 6; i++)
            {
                if ((spliterArrayRelations[i, 0] + "").Trim().Length > 0)
                {
                    Item itm = new Item();
                    itm.ChengWei = spliterArrayRelations[i, 0];
                    itm.XingMing = spliterArrayRelations[i, 1];
                    itm.ChuShengRiQi = this.AddDot(spliterArrayRelations[i, 2]);
                    itm.ZhengZhiMianMao = spliterArrayRelations[i, 3];
                    itm.GongZuoDanWeiJiZhiWu = spliterArrayRelations[i, 4];
                    cadre_person.JiaTingChengYuan.Add(itm);
                }
            }
            cadre_person.ZhaoPian_Image = Image.FromFile(picPath);
            return cadre_person;
        }


        #region 内部方法集合
        private string AddDot(string strDate)
        {
            Regex r = new Regex(@"^\d{6}|\d{8}$");
            if (r.Match(strDate).Success)
            {
                string strYear = strDate.Substring(0, 4);
                string strMonth = strDate.Substring(4, 2);
                return strYear + "." + strMonth;
            }
            else
            {
                return strDate;
            }
        }

        private string ReadFile(string filePath)
        {
            StreamReader sReader = new StreamReader(filePath, Encoding.GetEncoding("GB2312"));
            //注意此处文件编码，否则读出的为乱码
            string txtreader = "";
            string txtreaderline = "";
            try
            {
                while (txtreaderline != null)
                {
                    txtreaderline = sReader.ReadLine();
                    if (txtreaderline != null)
                    {
                        txtreader += txtreaderline;
                        txtreader += '\n';
                    }
                }
            }
            catch
            {
                txtreader = "";
            }
            finally
            {
                sReader.Close();
            }
            return txtreader;
        }

        /// <summary>
        /// 根据读取中组部格式文件的文本流字符串返回干部信息数组
        /// </summary>
        /// <returns></returns>
        private string[] SpliterArrayInfo()
        {
            string[] spliterContextText = ContentText.Replace("\",\"", "▲").Split('▲');
            spliterContextText[0] = spliterContextText[0].Substring(1, spliterContextText[0].Length - 1);
            spliterContextText[spliterContextText.Count() - 1] = spliterContextText[spliterContextText.Count() - 1].Substring(0, spliterContextText[spliterContextText.Count() - 1].Length - 1);


            //将前9项传入最终数组
            string[] spliterArrayInfo = new string[50];

            for (int j = 0; j < 9; j++)
            {
                spliterArrayInfo[j] = spliterContextText[j];//0 姓名; 1 性别; 2 出生年月; 3 民族; 4 籍贯; 5 入党时间 6 健康状况 7 出生地 8 工作时间
            }

            //9 所有学历与学位
            string[] _spliterEducation = spliterContextText[9].Split('@');
            string[] _spliterFulltimeEducation = _spliterEducation[0].Split('#');
            string[] _spliterPartimeEducation = _spliterEducation[1].Split('#');

            //将学历与学位信息传入最终数组
            spliterArrayInfo[9] = _spliterFulltimeEducation[0];//9 全日制学历
            spliterArrayInfo[10] = _spliterFulltimeEducation[1];//10 全日制学位
            spliterArrayInfo[11] = _spliterPartimeEducation[0];//11 在职学历
            spliterArrayInfo[12] = _spliterPartimeEducation[1];//12 在职学位

            //10 处理院校
            string[] _spliterCollage = spliterContextText[10].Split('@');
            string[] _spliterFulltimeCollage = _spliterCollage[0].Split('#');
            string[] _spliterPartimeCollage = _spliterCollage[1].Split('#');

            //将学历与学位信息传入最终数组
            spliterArrayInfo[13] = _spliterFulltimeCollage[0];//全日制院校
            spliterArrayInfo[14] = _spliterFulltimeCollage[1];//全日制学历
            spliterArrayInfo[15] = _spliterPartimeCollage[0];//在职院校
            spliterArrayInfo[16] = _spliterPartimeCollage[1];//在职学历

            spliterArrayInfo[17] = spliterContextText[11];//技术职称
            spliterArrayInfo[18] = spliterContextText[16];//熟悉专业有何专长
            spliterArrayInfo[19] = spliterContextText[17];//简历
            spliterArrayInfo[20] = spliterContextText[18];//奖惩情况
            spliterArrayInfo[21] = spliterContextText[19];//年度考核情况
            spliterArrayInfo[22] = spliterContextText[25];//现任职务
            spliterArrayInfo[23] = spliterContextText[26];//拟任职务
            spliterArrayInfo[24] = spliterContextText[27];//拟免职务
            spliterArrayInfo[25] = spliterContextText[28];//任免理由
            spliterArrayInfo[26] = spliterContextText[29];//呈报单位
            spliterArrayInfo[27] = spliterContextText[30];//计算年龄时间
            spliterArrayInfo[28] = spliterContextText[31];//录入人
            spliterArrayInfo[29] = spliterContextText[32];//录入时间

            return spliterArrayInfo;
        }


        /// <summary>
        /// 根据读取中组部格式文件的文本流字符串返回干部亲属关系数组
        /// </summary>
        /// <returns></returns>
        private string[,] SpliterArrayRelations()
        {

            string[] spliterContextText = ContentText.Replace("\",\"", "▲").Split('▲');
            spliterContextText[0] = spliterContextText[0].Substring(1, spliterContextText[0].Length - 1);
            spliterContextText[spliterContextText.Count() - 1] = spliterContextText[spliterContextText.Count() - 1].Substring(0, spliterContextText[spliterContextText.Count() - 1].Length - 1);


            string[,] spliterArrayRelations = new string[13, 5];

            for (int j = 0; j < 5; j++)
            {
                int p = j + 20;
                string[] spliterArraySingleRelation = spliterContextText[p].Split('@');
                for (int i = 0; i < spliterArraySingleRelation.Count(); i++)
                {
                    spliterArrayRelations[i, j] = spliterArraySingleRelation[i];
                }
            }
            return spliterArrayRelations;
        }

        #endregion
    }

}
