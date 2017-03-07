using ImgLocation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ImgLocation.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class LrmToImage
    {
        /// <summary>
        /// 通用字体名称
        /// </summary>
        private readonly FontFamily fontName = new FontFamily("仿宋");

        /// <summary>
        /// 通用字体大小
        /// </summary>
        private readonly float fontSize = 32.0f;

        /// <summary>
        /// 通用线条画笔
        /// </summary>
        private readonly Pen pen = new Pen(new SolidBrush(Color.Black), 1.0f);

        /// <summary>
        /// 表格起点 X 坐标
        /// </summary>
        private readonly int xStart = 50;
        /// <summary>
        /// 表格起点 Y 坐标
        /// </summary>
        private readonly int yStart = 150;
        /// <summary>
        /// 最小行高
        /// </summary>
        private readonly int minHeight = 100;
        /// <summary>
        /// 最小列宽
        /// </summary>
        private readonly int minWidth = 200;
        /// <summary>
        /// 家庭成员最小数量
        /// </summary>
        private readonly int maxFamilyCount = 7;
        /// <summary>
        /// 图片高度
        /// </summary>
        private int imageHeight = 0;
        /// <summary>
        /// 图片宽度
        /// </summary>
        private int imageWidth = 1600;


        public Person person = null;

        public LrmToImage()
        {
           
        }

        public LrmToImage(Person _person)
        {
            person = _person;
        }

        public LrmToImage(Person _person,int _imageWidth)
        {
            person = _person;
            this.imageWidth = _imageWidth;
        }

        /// <summary>
        /// 创建图片
        /// </summary>
        public Bitmap CreateImage()
        {
            Bitmap b = new Bitmap(imageWidth, 3200, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Graphics g = Graphics.FromImage(b);

            try
            {
                // 插值算法的质量
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; //高质量
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality; //高像素偏移质量
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;//增强字体
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //画笔
                SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
                //字体
                Font font = new System.Drawing.Font(this.fontName, this.fontSize, FontStyle.Regular);
                Font fontTitle = new System.Drawing.Font("方正小标宋简体", 50, FontStyle.Bold);

                int x1 = xStart;
                int x2 = x1 + minWidth;
                int x3 = x2 + minWidth;
                int x4 = x3 + minWidth;
                int x5 = x4 + minWidth;
                int x6 = x5 + minWidth;
                int x7 = x6 + minWidth;
                int x8 = x7 + 300;

                int x11 = x1;
                int x12 = x11 + 100;
                int x13 = x12 + 150;
                int x14 = x13 + 200;
                int x15 = x14 + 100;
                int x16 = x15 + 200;
                int x17 = x8;

                int y0 = yStart;
                int y1 = y0 + fontTitle.Height + 50;
                int y2 = y1 + minHeight;
                int y3 = y2 + minHeight;
                int y4 = y3 + minHeight;
                int y5 = y4 + minHeight;
                int y6 = y5 + minHeight;
                int y7 = y6 + minHeight;

                int y8, y9, y10, y11, y12, y13, y14, y15;



                //计算图片大小
                int tempHeight = 0;
                //现 任 职 务
                tempHeight = GetHeight(g, person.XianRenZhiWu, font, brush, x8 - x3);
                y8 = y7 + tempHeight;
                //拟 任 职 务
                tempHeight = GetHeight(g, person.NiRenZhiWu, font, brush, x8 - x3);
                y9 = y8 + tempHeight;
                //拟 免 职 务
                tempHeight = GetHeight(g, person.NiMianZhiWu, font, brush, x8 - x3);
                y10 = y9 + tempHeight;
                //简历
                tempHeight = GetResumeHeight(g, person.JianLi, font, brush, x12, y10, x17 - x12);
                y11 = y10 + tempHeight;
                //奖惩情况
                tempHeight = GetHeight(g, person.JiangChengQingKuang, font, brush, x17 - x12);
                if (tempHeight < font.Height * 4)
                {
                    tempHeight = font.Height * 4;
                }
                y12 = y11 + tempHeight;
                //年核\r\n度结\r\n考果
                tempHeight = GetHeight(g, person.NianDuKaoHeJieGuo, font, brush, x17 - x12);
                if (tempHeight < font.Height * 4)
                {
                    tempHeight = font.Height * 4;
                }
                y13 = y12 + tempHeight;
                //任免理由
                tempHeight = GetHeight(g, person.RenMianLiYou, font, brush, x17 - x12);
                if (tempHeight < font.Height * 4)
                {
                    tempHeight = font.Height * 4;
                }
                y14 = y13 + tempHeight;

                y15 = y14 + GetFamilyHeight(g, person.JiaTingChengYuan, font, brush, x17 - x16);

                imageHeight = y15 + 150;

                //计算图片大小
                Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);

                //重新创建画布和画笔
                b = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                g = Graphics.FromImage(b);
                g.FillRectangle(Brushes.White, rect);


                //列
                //第1列
                g.DrawLine(pen, x1, y1, x1, y7);
                //第2列
                g.DrawLine(pen, x2, y1, x2, y7);
                //第3列
                g.DrawLine(pen, x3, y1, x3, y4);
                g.DrawLine(pen, x3, y5, x3, y7);
                //第4列
                g.DrawLine(pen, x4, y1, x4, y5);
                //第5列
                g.DrawLine(pen, x5, y1, x5, y7);
                //第6列
                g.DrawLine(pen, x6, y1, x6, y4);
                g.DrawLine(pen, x6, y5, x6, y7);
                //第7列
                g.DrawLine(pen, x7, y1, x7, y5);
                //第8列
                g.DrawLine(pen, x8, y1, x8, y7);

                //行
                //第1行
                g.DrawLine(pen, x1, y1, x8, y1);
                //第2行
                g.DrawLine(pen, x1, y2, x7, y2);
                //第3行
                g.DrawLine(pen, x1, y3, x7, y3);
                //第4行
                g.DrawLine(pen, x1, y4, x7, y4);
                //第5行
                g.DrawLine(pen, x1, y5, x7, y5);
                //第6行
                g.DrawLine(pen, x1, y6, x8, y6);
                //第7行
                g.DrawLine(pen, x1, y7, x8, y7);


                //绘制标题文字
                DrawInCenter(g, "干 部 任 免 审 批 表", fontTitle, brush, x1, y0, x8, y1);
                //照片
                g.DrawImage(person.ZhaoPian_Image, x7 + 1, y1 + 1, x8 - x7 - 2, y5 - y1 - 2);

                DrawInCenter(g, "姓  名", font, brush, x1, y1, x2, y2);
                DrawInCenterByFont(g, person.XingMing, font, brush, x2, y1, x3, y2);
                DrawInCenter(g, "性  别", font, brush, x3, y1, x4, y2);

                DrawInCenter(g, "男", font, brush, x4, y1, x5, y2);
                DrawInCenterByLine(g, "出生年月\r\n（ 岁）", font, brush, x5, y1, x6, y2);

                int age = CalAge(person.ChuShengNianYue);

                DrawInCenterByLine(g, person.ChuShengNianYue + "\r\n（" + age + "岁）", font, brush, x6, y1, x7, y2);

                DrawInCenter(g, "民  族", font, brush, x1, y2, x2, y3);
                DrawInCenterByFont(g, person.MinZu, font, brush, x2, y2, x3, y3);
                DrawInCenter(g, "籍  贯", font, brush, x3, y2, x4, y3);
                DrawInCenterByFont(g, person.JiGuan, font, brush, x4, y2, x5, y3);
                DrawInCenter(g, "出 生 地", font, brush, x5, y2, x6, y3);
                DrawInCenterByFont(g, person.ChuShengDi, font, brush, x6, y2, x7, y3);

                DrawInCenterByLine(g, "入  党\r\n时  间", font, brush, x1, y3, x2, y4);
                DrawInCenter(g, person.RuDangShiJian, font, brush, x2, y3, x3, y4);
                DrawInCenterByLine(g, "参加工\r\n作时间", font, brush, x3, y3, x4, y4);
                DrawInCenter(g, person.CanJiaGongZuoShiJian, font, brush, x4, y3, x5, y4);
                DrawInCenter(g, "健康状况", font, brush, x5, y3, x6, y4);
                DrawInCenter(g, person.JianKangZhuangKuang, font, brush, x6, y3, x7, y4);

                DrawInCenterByLine(g, "专业技术\r\n职    务", font, brush, x1, y4, x2, y5);
                DrawInCenter(g, person.ZhuanYeJiShuZhiWu, font, brush, x2, y4, x4, y5);
                DrawInCenterByLine(g, "熟悉专业\r\n有何专长", font, brush, x4, y4, x5, y5);
                DrawInCenter(g, person.ShuXiZhuanYeYouHeZhuanChang, font, brush, x5, y4, x7, y5);


                y7 = y6 + minHeight;

                DrawInCenter(g, "学  历", font, brush, x1, y5, x2, y6);
                DrawInCenter(g, "学  位", font, brush, x1, y6, x2, y7);
                DrawInCenterByLine(g, "全日制\r\n教  育", font, brush, x2, y5, x3, y6);
                DrawInCenterByLine(g, person.QuanRiZhiJiaoYu_XueLi + "\r\n" + person.QuanRiZhiJiaoYu_XueWei, font, brush, x3, y5, x5, y6);
                DrawInCenterByLine(g, "毕业院校\r\n系及专业", font, brush, x5, y5, x6, y6);
                DrawSchool(g, person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi, person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi, font, brush, x6, y5, x8, y6);

                DrawInCenterByLine(g, "在  职\r\n教  育", font, brush, x2, y6, x3, y7);
                DrawInCenter(g, person.ZaiZhiJiaoYu_XueLi + "\r\n" + person.ZaiZhiJiaoYu_XueWei, font, brush, x3, y6, x5, y7);
                DrawInCenterByLine(g, "毕业院校\r\n系及专业", font, brush, x5, y6, x6, y7);
                DrawSchool(g, person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi, person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi, font, brush, x6, y6, x8, y7);


                g.DrawLine(pen, x1, y7, x1, y8);
                g.DrawLine(pen, x3, y7, x3, y8);
                g.DrawLine(pen, x8, y7, x8, y8);
                g.DrawLine(pen, x1, y8, x8, y8);//截至行

                DrawInCenter(g, "现 任 职 务", font, brush, x1, y7, x3, y8);
                DrawInLeftAutoRow(g, person.XianRenZhiWu, font, brush, x3, y7, x8, y8);


                g.DrawLine(pen, x1, y8, x1, y9);
                g.DrawLine(pen, x3, y8, x3, y9);
                g.DrawLine(pen, x8, y8, x8, y9);
                g.DrawLine(pen, x1, y9, x8, y9);//截至行

                DrawInCenter(g, "拟 任 职 务", font, brush, x1, y8, x3, y9);
                DrawInLeftAutoRow(g, person.NiRenZhiWu, font, brush, x3, y8, x8, y9);




                g.DrawLine(pen, x1, y9, x1, y10);
                g.DrawLine(pen, x3, y9, x3, y10);
                g.DrawLine(pen, x8, y9, x8, y10);
                g.DrawLine(pen, x1, y10, x8, y10);//截至行

                DrawInCenter(g, "拟 免 职 务", font, brush, x1, y9, x3, y10);
                DrawInLeftAutoRow(g, person.NiMianZhiWu, font, brush, x3, y9, x8, y10);


                g.DrawLine(pen, x1, y10, x1, y11);
                g.DrawLine(pen, x12, y10, x12, y11);
                g.DrawLine(pen, x8, y10, x8, y11);
                g.DrawLine(pen, x1, y11, x8, y11);//截至行


                DrawInCenterH(g, "简          历", font, brush, x1, y10, x12, y11);
                DrawResume(g, person.JianLi, font, brush, x12, y10, x8 - x12);


                g.DrawLine(pen, x1, y11, x1, y12);
                g.DrawLine(pen, x12, y11, x12, y12);
                g.DrawLine(pen, x8, y11, x8, y12);
                g.DrawLine(pen, x1, y12, x8, y12);//截至行

                DrawInCenterH(g, "奖惩情况", font, brush, x1, y11, x12, y12);
                DrawInLeftAutoRow(g, person.JiangChengQingKuang, font, brush, x12, y11, x8, y12);


                g.DrawLine(pen, x1, y12, x1, y13);
                g.DrawLine(pen, x12, y12, x12, y13);
                g.DrawLine(pen, x8, y12, x8, y13);
                g.DrawLine(pen, x1, y13, x8, y13);//截至行

                DrawKaoHeTitle(g, font, brush, x11, y12, x12, y13);
                DrawInLeftAutoRow(g, person.NianDuKaoHeJieGuo, font, brush, x12, y12, x8, y13);



                g.DrawLine(pen, x1, y13, x1, y14);
                g.DrawLine(pen, x12, y13, x12, y14);
                g.DrawLine(pen, x8, y13, x8, y14);
                g.DrawLine(pen, x1, y14, x8, y14);//截至行

                DrawInCenterH(g, "任免理由", font, brush, x1, y13, x12, y14);
                DrawInLeftAutoRow(g, person.RenMianLiYou, font, brush, x12, y13, x8, y14);

                //家庭成员
                int yFamily = y14;
                List<Model.Item> Items = person.JiaTingChengYuan;

                //标题
                g.DrawLine(pen, x11, yFamily, x11, yFamily + this.minHeight);
                g.DrawLine(pen, x12, yFamily, x12, yFamily + this.minHeight);
                g.DrawLine(pen, x13, yFamily, x13, yFamily + this.minHeight);
                g.DrawLine(pen, x14, yFamily, x14, yFamily + this.minHeight);
                g.DrawLine(pen, x15, yFamily, x15, yFamily + this.minHeight);
                g.DrawLine(pen, x16, yFamily, x16, yFamily + this.minHeight);
                g.DrawLine(pen, x17, yFamily, x17, yFamily + this.minHeight);
                g.DrawLine(pen, x12, yFamily + this.minHeight, x17, yFamily + this.minHeight);

                DrawInCenter(g, "称谓", font, brush, x12, yFamily, x13, yFamily + minHeight);
                DrawInCenter(g, "姓  名", font, brush, x13, yFamily, x14, yFamily + minHeight);
                DrawInCenterH(g, "年龄", font, brush, x14, yFamily, x15, yFamily + minHeight);
                DrawInCenterByLine(g, "政  治\r\n面  貌", font, brush, x15, yFamily, x16, yFamily + minHeight);
                DrawInCenter(g, "工 作 单 位 及 职 务", font, brush, x16, yFamily, x17, yFamily + minHeight);
                yFamily = yFamily + this.minHeight;

                int max = Items.Count;
                if (max < maxFamilyCount)
                {
                    max = maxFamilyCount;
                }

                for (int n = 0; n < max; n++)
                {
                    int familyHeight = this.minHeight;
                    if (n < Items.Count)
                    {
                        Model.Item item = Items[n];
                        familyHeight = GetHeight(g, item.GongZuoDanWeiJiZhiWu, font, brush, x17 - x16);

                        //绘制内容
                        DrawInCenter(g, item.ChengWei, font, brush, x12, yFamily, x13, yFamily + familyHeight);
                        DrawInCenter(g, item.XingMing, font, brush, x13, yFamily, x14, yFamily + familyHeight);
                        string strAge = "";
                        if (!string.IsNullOrWhiteSpace(item.ChuShengRiQi))
                        {
                            strAge = CalAge(item.ChuShengRiQi).ToString();
                        }
                        DrawInCenter(g, strAge, font, brush, x14, yFamily, x15, yFamily + familyHeight);
                        DrawInCenter(g, item.ZhengZhiMianMao, font, brush, x15, yFamily, x16, yFamily + familyHeight);
                        DrawInLeftAutoRow(g, item.GongZuoDanWeiJiZhiWu, font, brush, x16, yFamily, x17, yFamily + familyHeight);
                    }


                    //绘制表格
                    g.DrawLine(pen, x11, yFamily, x11, yFamily + familyHeight);
                    g.DrawLine(pen, x12, yFamily, x12, yFamily + familyHeight);
                    g.DrawLine(pen, x13, yFamily, x13, yFamily + familyHeight);
                    g.DrawLine(pen, x14, yFamily, x14, yFamily + familyHeight);
                    g.DrawLine(pen, x15, yFamily, x15, yFamily + familyHeight);
                    g.DrawLine(pen, x16, yFamily, x16, yFamily + familyHeight);
                    g.DrawLine(pen, x17, yFamily, x17, yFamily + familyHeight);
                    g.DrawLine(pen, x12, yFamily + familyHeight, x17, yFamily + familyHeight);

                    yFamily = yFamily + familyHeight;
                }

                //补充划线
                g.DrawLine(pen, x11, yFamily, x12, yFamily);
                DrawInCenterH(g, "家 庭 成 员 及 主 要 社 会 关 系", font, brush, x11, y14, x12, yFamily);


                g.DrawImage(b, 0, 0, imageWidth, imageHeight);
                //b.Save(strPath);
            }
            catch(Exception Ex)
            {
                b = null;
            }
            return b;
        }

        private int GetFamilyHeight(Graphics g, List<Model.Item> Items, Font font, SolidBrush b, int width)
        {
            int back = this.minHeight;

            int max = Items.Count;
            if (max < this.maxFamilyCount)
            {
                max = this.maxFamilyCount;
            }

            //字体排版格式
            StringFormat sf = new StringFormat();
            //居左显示
            sf.Alignment = StringAlignment.Near;
            //垂直居中
            sf.LineAlignment = StringAlignment.Center;
            //自动换行
            sf.FormatFlags = StringFormatFlags.LineLimit;
            //如果注销，不允许计算空格
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            for (int n = 0; n < max; n++)
            {
                int myHeight = this.minHeight;
                if (n < Items.Count)
                {
                    Model.Item item = Items[n];

                    myHeight = GetHeight(g, item.GongZuoDanWeiJiZhiWu, font, b, width);
                }
                back = back + myHeight;
            }
            return back;
        }

        private int CalAge(string dateStr)
        {
            double d1 = DateTime.Now.Year + (double)DateTime.Now.Month / 100;
            double d2 = Convert.ToDouble(dateStr);
            int age = (int)Math.Floor(d1 - d2);
            return age;
        }

        /// <summary>
        /// 绘制【简历】
        /// </summary>
        private void DrawResume(Graphics g, string text, Font font, SolidBrush sbrush, int x, int y, int width)
        {
            //字体排版格式
            StringFormat sf = new StringFormat();
            //居左显示
            sf.Alignment = StringAlignment.Near;
            //垂直居中
            sf.LineAlignment = StringAlignment.Center;
            //自动换行
            sf.FormatFlags = StringFormatFlags.LineLimit;
            //如果注销，不允许计算空格
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            List<string> Lines = new List<string>();

            string[] temps = text.Split('\n');


            foreach (string s in temps)
            {
                string temp = s.Trim();
                string str = "";
                for (int k = 0; k < temp.Length; k++)
                {
                    SizeF sizef = g.MeasureString(str + temp[k].ToString(), font, 10000, sf);
                    if (sizef.Width >= width)
                    {
                        Lines.Add(str);
                        str = "";
                        str = "          " + "        ";
                        str = str + temp[k].ToString();
                    }
                    else
                    {
                        str = str + temp[k].ToString();
                    }

                    if (k == temp.Length - 1)
                    {
                        Lines.Add(str);
                    }
                }

            }
            int x0 = x + 2;
            int y0 = y + font.Height;
            foreach (string line in Lines)
            {
                g.DrawString(line, font, sbrush, x0, y0, sf);
                y0 = y0 + font.Height;
            }

            //绘制边框
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 1.0f), x, y, width, y0 - y);

        }

        private int GetResumeHeight(Graphics g, string text, Font font, SolidBrush sbrush, int x, int y, int width)
        {
            //字体排版格式
            StringFormat sf = new StringFormat();
            //居左显示
            sf.Alignment = StringAlignment.Near;
            //垂直居中
            sf.LineAlignment = StringAlignment.Center;
            //自动换行
            sf.FormatFlags = StringFormatFlags.LineLimit;
            //如果注销，不允许计算空格
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            List<string> Lines = new List<string>();

            string[] temps = text.Split('\n');


            foreach (string s in temps)
            {
                string temp = s.Trim();
                string str = "";
                for (int k = 0; k < temp.Length; k++)
                {
                    SizeF sizef = g.MeasureString(str + temp[k].ToString(), font, 10000, sf);
                    if (sizef.Width >= width)
                    {
                        Lines.Add(str);
                        str = "";
                        str = "          " + "        ";
                        str = str + temp[k].ToString();
                    }
                    else
                    {
                        str = str + temp[k].ToString();
                    }

                    if (k == temp.Length - 1)
                    {
                        Lines.Add(str);
                    }
                }

            }
            int x0 = x + 2;
            int y0 = y + font.Height;
            foreach (string line in Lines)
            {
                y0 = y0 + font.Height;
            }
            return y0 - y;
        }


        private void DrawInCenter(Graphics g, string text, Font font, SolidBrush sbrush, int x, int y, int width, int height, StringFormat format)
        {
            //字体排版格式
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            //计算空格并且不支持换行
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.LineLimit;

            int fieldWidth = 0;//定义内容宽度

            sf.FormatFlags |= StringFormatFlags.NoWrap;//禁用文本换行功能


            SizeF sizeF = g.MeasureString(text, font, width, sf);

            PointF h = new PointF(x, y);
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();

            gp.AddString(text, font.FontFamily, (int)font.Style, font.Size, Point.Round(h), sf);

            RectangleF gc = gp.GetBounds();

            // TODO: need fix: get incorrect size if CJK fonts
            //sizeF.Height += 2;

            float x0 = x + width / 2 - sizeF.Width / 2 + 1;
            float y0 = y + height / 2 - sizeF.Height / 2 + 1;

            float x00 = x + width / 2 - gc.Width / 2 + 1;
            float y00 = y + height / 2 - gc.Height / 2 + 1;

            //3.
            RectangleF rectangleF1 = new RectangleF(x00, y00, sizeF.Width, sizeF.Height);
            RectangleF rectangleF2 = new RectangleF(x00, y00, gc.Width, gc.Height);

            //g.DrawString(text, font, sbrush, rectangleF1, sf);
            g.DrawString(text, font, sbrush, rectangleF2, sf);
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 1.0f), rectangleF1.X, rectangleF1.Y, rectangleF1.Width, rectangleF1.Height);
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Blue), 1.0f), rectangleF2.X, rectangleF2.Y, rectangleF2.Width, rectangleF2.Height);
        }


        private int GetHeight(Graphics g, string text, Font font, SolidBrush b, int width)
        {
            int back = this.minHeight;
            //字体排版格式
            StringFormat sf = new StringFormat();
            //居左显示
            sf.Alignment = StringAlignment.Near;
            //垂直居中
            sf.LineAlignment = StringAlignment.Center;
            //自动换行
            sf.FormatFlags = StringFormatFlags.LineLimit;
            //如果注销，不允许计算空格
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            SizeF sizeF = g.MeasureString(text, font, width, sf);


            back = (int)sizeF.Height + font.Height;
            if (back <= this.minHeight)
            {
                back = this.minHeight;
            }
            return back;
        }

        private void DrawSchool(Graphics g, string xueli, string xuexiao, Font font, SolidBrush b, int x1, int y1, int x2, int y2)
        {
            //字体排版格式
            StringFormat sf = new StringFormat();
            //居左显示
            sf.Alignment = StringAlignment.Near;
            //垂直居中
            sf.LineAlignment = StringAlignment.Center;
            //自动换行
            sf.FormatFlags = StringFormatFlags.LineLimit;
            //如果注销，不允许计算空格
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            int width = x2 - x1 - 1;
            int height = y2 - y1 - 1;

            xueli = xueli.Trim();
            xuexiao = xuexiao.Trim();

            int mode = 1;
            string text = "";
            if (xueli.Length == 0 && xuexiao.Length == 0)
            {
                mode = 0;
            }
            else if (xueli.Length == 0 && xuexiao.Length != 0)
            {
                mode = 1;
                text = xuexiao;
            }
            else if (xueli.Length != 0 && xuexiao.Length == 0)
            {
                mode = 1;
                text = xueli;
            }
            else if (xueli == xuexiao)
            {
                mode = 1;
                text = xueli;
            }
            else
            {
                mode = 2;
                text = xueli + "\r\n" + xuexiao;
            }


            if (mode == 1)
            {
                SizeF sizeF = g.MeasureString(text, font, width, sf);

                float x0 = x1;
                float y0 = y1 + height / 2 - sizeF.Height / 2;

                float xStart = x0;
                float yStart = y0 + 2;

                Font newFont = font;//依次减少字体
                while (sizeF.Width >= width || sizeF.Height >= height)
                {
                    newFont = new Font(font.FontFamily, newFont.Size - 1);
                    sizeF = g.MeasureString(text, newFont, width, sf);
                }

                y0 = y1 + height / 2 - sizeF.Height / 2;
                yStart = y0 + 2;
                RectangleF rectangleF = new RectangleF(x0, yStart, sizeF.Width, sizeF.Height);
                g.DrawString(text, newFont, b, rectangleF, sf);
                //g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 1.0f), x0, y0, sizeF.Width, sizeF.Height);
            }
            else if (mode == 2)
            {
                SizeF sizeF1 = g.MeasureString(xueli, font, width, sf);
                float myY1 = y1 + height * 1 / 4 - sizeF1.Height / 2 + 2;

                Font newFont1 = font;//依次减少字体
                while (sizeF1.Width >= width || sizeF1.Height >= height / 2)
                {
                    newFont1 = new Font(font.FontFamily, newFont1.Size - 1);
                    sizeF1 = g.MeasureString(xueli, newFont1, width, sf);
                }

                SizeF sizeF2 = g.MeasureString(xuexiao, font, width, sf);
                float myY2 = y1 + height * 3 / 4 - sizeF2.Height / 2 + 2;

                Font newFont2 = font;//依次减少字体
                while (sizeF2.Width >= width || sizeF2.Height >= height / 2)
                {
                    newFont2 = new Font(font.FontFamily, newFont2.Size - 1);
                    sizeF2 = g.MeasureString(xuexiao, newFont2, width, sf);
                }

                if (newFont2.Size >= newFont1.Size)
                {
                    newFont2 = new Font(font.FontFamily, newFont1.Size);
                }
                else
                {
                    newFont1 = new Font(font.FontFamily, newFont2.Size);
                }

                myY1 = y1 + height * 1 / 4 - sizeF1.Height / 2 + 2;
                myY2 = y1 + height * 3 / 4 - sizeF2.Height / 2 + 2;


                RectangleF rectangleF1 = new RectangleF(x1, myY1, sizeF1.Width, sizeF1.Height);
                g.DrawString(xueli, newFont1, b, rectangleF1, sf);

                RectangleF rectangleF2 = new RectangleF(x1, myY2, sizeF2.Width, sizeF2.Height);
                g.DrawString(xuexiao, newFont2, b, rectangleF2, sf);
            }
            else
            {

            }
        }

        /// <summary>
        /// 居左，自动换行：（现任职务  拟任职务 拟免职务）
        /// </summary>
        private void DrawInLeftAutoRow(Graphics g, string text, Font font, SolidBrush b, int x1, int y1, int x2, int y2)
        {
            //字体排版格式
            StringFormat sf = new StringFormat();
            //居左显示
            sf.Alignment = StringAlignment.Near;
            //垂直居中
            sf.LineAlignment = StringAlignment.Center;
            //自动换行
            sf.FormatFlags = StringFormatFlags.LineLimit;
            //如果注销，不允许计算空格
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            int width = x2 - x1 - 1;
            int height = y2 - y1 - 1;

            SizeF sizeF = g.MeasureString(text, font, width, sf);

            //备注：高度自动加 2 像素
            //sizeF.Height += 2;

            float x0 = x1;
            float y0 = y1 + height / 2 - sizeF.Height / 2;

            float xStart = x0;
            float yStart = y0 + 5;

            RectangleF rectangleF = new RectangleF(x0, yStart, sizeF.Width, sizeF.Height);

            //g.DrawString(text, font, b, x0, y0, sf);
            g.DrawString(text, font, b, rectangleF, sf);
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 1.0f), x0, y0, sizeF.Width, sizeF.Height);
        }

        private void DrawInCenterByLine(Graphics g, string text, Font font, SolidBrush b, int x1, int y1, int x2, int y2)
        {
            //字体排版格式
            StringFormat sf = new StringFormat();
            //水平居中
            sf.Alignment = StringAlignment.Center;
            //垂直居中
            sf.LineAlignment = StringAlignment.Center;
            //自动换行
            sf.FormatFlags = StringFormatFlags.NoWrap;
            //注销，不允许计算空格
            //sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            //自动换行
            //sf.FormatFlags |= StringFormatFlags.LineLimit;

            int width = x2 - x1 - 1;
            int height = y2 - y1 - 1;

            SizeF sizeF = g.MeasureString(text, font, width, sf);

            //备注：高度自动加 2 像素
            sizeF.Height += 2;

            float x0 = x1 + width / 2 - sizeF.Width / 2;
            float y0 = y1 + height / 2 - sizeF.Height / 2;

            float xStart = x0;
            float yStart = y0 + 5;

            RectangleF rectangleF = new RectangleF(x0, yStart, sizeF.Width, sizeF.Height);

            //g.DrawString(text, font, b, x0, y0, sf);
            g.DrawString(text, font, b, rectangleF, sf);
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 1.0f), x0, y0, sizeF.Width, sizeF.Height);
        }


        private void DrawInCenter(Graphics g, string text, Font font, SolidBrush b, int x1, int y1, int x2, int y2)
        {
            //字体排版格式
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            //计算空格并且不支持换行
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.LineLimit;

            sf.FormatFlags |= StringFormatFlags.NoWrap;//禁用文本换行功能

            int width = x2 - x1 - 1;
            int height = y2 - y1 - 1;

            SizeF sizeF = g.MeasureString(text, font, width, sf);

            Font newFont = font;//依次减少字体
            while (sizeF.Width >= (width - 2))
            {
                newFont = new Font(font.FontFamily, newFont.Size - 1);
                sizeF = g.MeasureString(text, newFont, width, sf);
            }

            // TODO: need fix: get incorrect size if CJK fonts
            sizeF.Height += 2;

            float x0 = x1 + width / 2 - sizeF.Width / 2 + 1;
            float y0 = y1 + height / 2 - sizeF.Height / 2 + 1;

            RectangleF rectangleF = new RectangleF(x0, y0, sizeF.Width, sizeF.Height);

            float xStart = x0;
            float yStart = y0 + 5;
            g.DrawString(text, newFont, b, xStart, yStart, sf);
            //g.DrawString(text, font, sbrush, rectangleF, sf);
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 1.0f), x0, y0, sizeF.Width, sizeF.Height);
        }

        /// <summary>
        /// 绘制【年度考核结果】
        /// </summary>
        private void DrawKaoHeTitle(Graphics g, Font font, SolidBrush b, int x1, int y1, int x2, int y2)
        {
            //字体排版格式
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            //计算空格并且不支持换行
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.LineLimit;

            sf.FormatFlags |= StringFormatFlags.NoWrap;//禁用文本换行功能

            int width = x2 - x1 - 1;
            int height = y2 - y1 - 1;

            int textHeight = font.Height * 3;

            SizeF sizeF = g.MeasureString("年度", font, width, sf);
            float x0 = x1 + width / 2 - sizeF.Width / 2 + 1;
            float y0 = y1 + height / 2 - textHeight / 2 + 1;

            g.DrawString("年核", font, b, x0, y0 + font.Height * 0, sf);
            g.DrawString("度结", font, b, x0, y0 + font.Height * 1, sf);
            g.DrawString("考果", font, b, x0, y0 + font.Height * 2, sf);
        }

        private void DrawInCenterH(Graphics g, string text, Font font, SolidBrush b, int x1, int y1, int x2, int y2)
        {
            //字体排版格式
            StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical);
            //计算空格并且不支持换行
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.LineLimit;

            sf.FormatFlags |= StringFormatFlags.NoWrap;//禁用文本换行功能

            int width = x2 - x1 - 1;
            int height = y2 - y1 - 1;

            SizeF sizeF = g.MeasureString(text, font, width, sf);

            Font newFont = font;//依次减少字体

            // TODO: need fix: get incorrect size if CJK fonts
            sizeF.Height += 2;

            float x0 = x1 + width / 2 - sizeF.Width / 2 + 1;
            float y0 = y1 + height / 2 - sizeF.Height / 2 + 1;

            RectangleF rectangleF = new RectangleF(x0, y0, sizeF.Width, sizeF.Height);

            float xStart = x0;
            float yStart = y0 + 5;
            g.DrawString(text, newFont, b, xStart, yStart, sf);
            //g.DrawString(text, font, sbrush, rectangleF, sf);
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 1.0f), x0, y0, sizeF.Width, sizeF.Height);
        }

        /// <summary>
        /// 根据字体大小居中显示
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="b"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void DrawInCenterByFont(Graphics g, string text, Font font, SolidBrush b, int x1, int y1, int x2, int y2)
        {
            //字体排版格式
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            //计算空格并且不支持换行
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.LineLimit;

            sf.FormatFlags |= StringFormatFlags.NoWrap;//禁用文本换行功能

            int width = x2 - x1 - 1;
            int height = y2 - y1 - 1;

            SizeF sizeF = g.MeasureString(text, font, width, sf);

            Font newFont = font;//依次减少字体
            while (sizeF.Width >= (width - 2))
            {
                newFont = new Font(font.FontFamily, newFont.Size - 1);
                sizeF = g.MeasureString(text, newFont, width, sf);
            }

            // TODO: need fix: get incorrect size if CJK fonts
            sizeF.Height += 2;

            float x0 = x1 + width / 2 - sizeF.Width / 2 + 1;
            float y0 = y1 + height / 2 - sizeF.Height / 2 + 1;

            RectangleF rectangleF = new RectangleF(x0, y0, sizeF.Width, sizeF.Height);

            float xStart = x0;
            float yStart = y0 + 5;
            g.DrawString(text, newFont, b, xStart, yStart, sf);
            //g.DrawString(text, font, sbrush, rectangleF, sf);
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 1.0f), x0, y0, sizeF.Width, sizeF.Height);
        }

        private void DrawInLeft(Graphics g, string text, Font font, SolidBrush b, int x1, int y1, int x2, int y2)
        {
            //字体排版格式
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            //计算空格并且不支持换行
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.LineLimit;

            sf.FormatFlags |= StringFormatFlags.NoWrap;//禁用文本换行功能

            int width = x2 - x1 - 1;
            int height = y2 - y1 - 1;

            SizeF sizeF = g.MeasureString(text, font, width, sf);

            Font newFont = font;//依次减少字体
            while (sizeF.Width >= (width - 2))
            {
                newFont = new Font(font.FontFamily, newFont.Size - 1);
                sizeF = g.MeasureString(text, newFont, width, sf);
            }

            // TODO: need fix: get incorrect size if CJK fonts
            sizeF.Height += 2;

            float x0 = x1;
            float y0 = y1 + height / 2 - sizeF.Height / 2 + 1;

            RectangleF rectangleF = new RectangleF(x0, y0, sizeF.Width, sizeF.Height);

            float xStart = x0;
            float yStart = y0 + 5;
            g.DrawString(text, newFont, b, xStart, yStart, sf);
            //g.DrawString(text, font, sbrush, rectangleF, sf);
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 1.0f), x0, y0, sizeF.Width, sizeF.Height);
        }
    }
}
