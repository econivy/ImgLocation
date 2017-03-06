using ImgLocation.Models;
using ImgLocation.Repository;
using ImgLocation.Services;
using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText = iTextSharp.text;
using iTextPdf = iTextSharp.text.pdf;
using Word = Microsoft.Office.Interop.Word;

namespace ImgLocation
{
    public partial class ConvertPDFFileForm : Form
    {
        public ConvertPDFFileForm()
        {
            InitializeComponent();
        }
        public ImgLocationForm FatherForm { get; set; }
        public int ConvertType { get; set; }
        public string WordDirectory { get; set; }
        public string LrmDirectory { get; set; }
        public string ResDirectory { get; set; }
        public bool IsAdd { get; set; }
        public DateTime CountDate { get; set; }

        int iSumDocument = 0;
        int iCountDocument = 0;
        ConvertHelper convertHelper = new ConvertHelper();

        void LogRecord(string message)
        {
            LogRecord(message, 0);
        }
        void LogRecord(string message, int type)
        {
            lblMessage.Text = message;
            tbLog.Text += string.Format("[{0}]{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);
            switch (type)
            {
                case -2:
                    tbError.Text += string.Format("[错误]{0}\r\n", message);
                    break;
                case -1:
                    tbError.Text += string.Format("[警告]{0}\r\n", message);
                    break;
                default:
                    break;
            }
        }
        public DialogResult ShowMessage(string message)
        {
            return ShowMessage(message, 2);
        }
        public DialogResult ShowMessage(string message, int type)
        {
            DialogResult dr;
            switch (type)
            {
                case -2:
                    dr = MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -1:
                    dr = MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 1:
                    dr = MessageBox.Show(message, "询问", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    break;
                default:
                    dr = MessageBox.Show(message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
            LogRecord(message);
            return dr;
        }

        public void ConvertDocumentListToPDF()
        {
            ConvertProcess.Maximum = 100;
            ConvertProcess.Value = 0;
            //转换前关闭Word等进程
            try
            {
                Process[] pcs = Process.GetProcessesByName("WINWORD");
                if (pcs.Length > 0)
                {
                    foreach (Process pc in pcs)
                    {
                        try
                        {
                            pc.Kill(); //强制关闭
                        }
                        catch (Exception ep)
                        {
                            ShowMessage(string.Format("关闭进程（{0}）错误：{1}", pc.ProcessName, ep.Message), -2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("关闭进错误：{1}", ex.Message), -2);
            }

            List<DW> ds = new List<DW>();

            LogRecord("正在读取目录信息：" + WordDirectory);
            DirectoryInfo rootDirectoryInfos = new DirectoryInfo(WordDirectory);
            iSumDocument = 0;
            foreach (FileSystemInfo rootFileInfo in rootDirectoryInfos.GetFileSystemInfos())
            {
                if ((rootFileInfo is FileInfo))
                {
                    string fileName = Path.GetFileNameWithoutExtension(rootFileInfo.FullName);
                    string fileExtension = Path.GetExtension(rootFileInfo.FullName);
                    if ((fileExtension == ".doc" || fileExtension == ".docx") && (!fileName.Contains("$")))
                    {
                        iSumDocument++;
                        LogRecord("已经发现" + iSumDocument.ToString() + "个Word文档！");
                    }
                }
            }

            iCountDocument = 0;
            foreach (FileSystemInfo rootFileInfo in rootDirectoryInfos.GetFileSystemInfos())
            {
                if ((rootFileInfo is FileInfo))
                {
                    //try
                    //{
                    string fileName = Path.GetFileNameWithoutExtension(rootFileInfo.FullName);
                    string fileExtension = Path.GetExtension(rootFileInfo.FullName);
                    if ((fileExtension == ".doc" || fileExtension == ".docx") && (!fileName.Contains("$")))
                    {
                        LogRecord(string.Format("[共{0}个文档，已转换{1}个文档，现在开始读取第{2}个文档信息：{3}]",
                            iSumDocument, iCountDocument, iCountDocument + 1, Path.GetFileName(rootFileInfo.FullName)));

                        DW d = new DW();
                        d.id = Guid.NewGuid().ToString();

                        d.Local_SourceDocumnetFullpath = rootFileInfo.FullName;

                        LogRecord(string.Format("[共{0}个文档，已转换{1}个文档，正在转换第{2}个文档信息：{3}]",
                           iSumDocument, iCountDocument, iCountDocument + 1, d.Local_SourceDocumnetFullpath));

                        //try
                        //{
                        d = ConvertDocumentToSaveAsPDFForPDF(d, Global.ProjectPDFTempDirectory, Global.ProjectPDFOutputDirectory, LrmDirectory, ResDirectory);
                        LogRecord(string.Format("[共{0}个文档，已转换{1}个文档，正在转换第{2}个文档信息：文档内共发现{3}名干部信息。]", iSumDocument, iCountDocument, iCountDocument + 1, d.GBS.Count));

                        int GBCount = 1;
                        foreach (GB g in d.GBS)
                        {
                            LogRecord(string.Format("[共{0}个文档，已转换{1}个文档，正在转换第{2}个文档信息：文档内共发现{3}名干部信息，正在转换第{4}名干部信息。]", iSumDocument, iCountDocument, iCountDocument + 1, d.GBS.Count, GBCount));

                            g.Local_SourceLrmFullpath = convertHelper.GetLrmFilePath(g.XM);
                            g.Local_SourcePicFullpath = convertHelper.GetPicFilePath(g.XM);
                            g.Local_SourceResFullpath = convertHelper.GetResFilePath(g.XM);
                            if ((Path.GetExtension(g.Local_SourceLrmFullpath) == ".lrm") && (g.Local_SourceLrmFullpath.Trim().Length > 0 && g.Local_SourcePicFullpath.Trim().Length > 0))
                            {
                                try
                                {
                                    //g.Local_StorgeLrmFullpath = Global.ProjectLrmPicDirectory + g.XM + ".lrm";
                                    //g.Local_StorgePicFullpath = Global.ProjectLrmPicDirectory + g.XM + ".pic";
                                    //g.Local_StorgeDocumentWordFullpath = Global.ProjectTempWordDirectory + g.XM + ".docx";
                                    //g.Local_StorgeLrmPdfFullpath = Global.ProjectPDFTempDirectory + d.WH + @"\" + g.id + "_1.pdf";
                                    File.Copy(g.Local_SourceLrmFullpath, g.Local_StorgeLrmFullpath, true);
                                    File.Copy(g.Local_SourcePicFullpath, g.Local_StorgePicFullpath, true);
                                    ConvertLrmToWordForPDF(Global.LrmToPDFModelPath, g, Convert.ToDouble(CountDate.ToString("yyyy.MM")), (CountDate.Date));
                                }
                                catch (Exception ex)
                                {
                                    ShowMessage(string.Format("转换干部任免审批表失败：{0}", ex.Message), -2);
                                }
                            }
                            else if ((Path.GetExtension(g.Local_SourceLrmFullpath) == ".lrmx") && (g.Local_SourceLrmFullpath.Trim().Length > 0))
                            {
                                try
                                {
                                    //g.Local_StorgeLrmFullpath = Global.ProjectLrmPicDirectory + g.XM + ".lrmx";
                                    //g.Local_StorgePicFullpath = "";
                                    //g.Local_StorgeDocumentWordFullpath = Global.ProjectTempWordDirectory + g.XM + ".docx";
                                    //g.Local_StorgeLrmPdfFullpath = Global.ProjectPDFTempDirectory + d.WH + @"\" + g.id + "_1.pdf";
                                    File.Copy(g.Local_SourceLrmFullpath, g.Local_StorgeLrmFullpath, true);
                                    ConvertLrmToWordForPDF(Global.LrmToPDFModelPath, g, Convert.ToDouble(CountDate.ToString("yyyy.MM")), (CountDate.Date));

                                }
                                catch (Exception ex)
                                {
                                    ShowMessage(string.Format("转换干部任免审批表失败：{0}", ex.Message), -2);
                                }
                            }

                            //转换考察材料到PDF
                            if (g.Local_SourceResFullpath.Trim().Length > 0)
                            {
                                try
                                {
                                    //string ex = Path.GetExtension(g.Local_SourceResFullpath);
                                    //g.Local_StorgeResFullpath = Global.ProjectResDirectory + g.XM + ex;
                                    //g.Local_StorgeResPdfFullPath = Global.ProjectPDFTempDirectory + d.WH + @"\" + g.id + "_2.pdf";
                                    File.Copy(g.Local_SourceResFullpath, g.Local_StorgeResFullpath, true);
                                    ConvertWordToPDF(g.Local_StorgeResFullpath, g.Local_StorgeResPdfFullPath);
                                }
                                catch (Exception ex)
                                {
                                    ShowMessage(string.Format("转换干部考察材料失败：{0}", ex.Message), -2);
                                }
                            }

                            try
                            {
                                DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
                                dr.EditGB(g);
                            }
                            catch (Exception ex)
                            {
                                ShowMessage(string.Format("存储GB记录失败：{0}", ex.Message), -2);
                            }

                            System.GC.Collect();

                            LogRecord(string.Format("[共{0}个文档，已转换{1}个文档，正在转换第{2}个文档信息：文档内共发现{3}名干部信息，第{4}名干部信息转换完成。]", iSumDocument, iCountDocument, iCountDocument + 1, d.GBS.Count, GBCount));
                            GBCount++;
                        }
                        List<string> pdffileList = new List<string>();

                        DirectoryInfo pdfDirectoryInfos = new DirectoryInfo((Global.ProjectPDFTempDirectory + d.WH + @"\"));
                        foreach (FileSystemInfo pdfFileInfo in pdfDirectoryInfos.GetFileSystemInfos())
                        {
                            if ((pdfFileInfo is FileInfo))
                            {
                                string pdfFileExtension = Path.GetExtension(pdfFileInfo.FullName);
                                if (pdfFileExtension == ".pdf")
                                {
                                    pdffileList.Add(pdfFileInfo.FullName);
                                }
                            }
                        }
                        iTextPdf.PdfReader blankreader;
                        blankreader = new iTextPdf.PdfReader(Global.BlankPDFModelPath);
                        iTextPdf.PdfReader reader;
                        iText.Document document = new iText.Document();
                        iTextPdf.PdfWriter writer = iTextPdf.PdfWriter.GetInstance(document, new FileStream(d.Local_StorgeDocumentFullpath, FileMode.Create));
                        document.Open();
                        iTextPdf.PdfContentByte cb = writer.DirectContent;
                        iTextPdf.PdfImportedPage newpage;
                        for (int i = 0; i < pdffileList.ToArray().Length; i++)
                        {
                            reader = new iTextPdf.PdfReader(pdffileList.ToArray()[i]);
                            int ipagenumber = reader.NumberOfPages;
                            for (int j = 1; j <= ipagenumber; j++)
                            {
                                document.NewPage();
                                newpage = writer.GetImportedPage(reader, j);
                                cb.AddTemplate(newpage, 0, 0);
                            }
                            if (ipagenumber > 0 && ipagenumber % 2 == 1)
                            {
                                document.NewPage();
                                newpage = writer.GetImportedPage(blankreader, 1);
                                cb.AddTemplate(newpage, 0, 0);
                            }
                        }
                        document.Close();
                        //}
                        //catch (Exception ex)
                        //{
                        //    ShowMessage(string.Format("转换文档发生错误[{0}]：{1}", d.SOURCE, ex.Message), -2);
                        //}
                        System.GC.Collect();
                        ConvertProcess.Value = 100 * iCountDocument / iSumDocument;
                        iCountDocument++;
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    ShowMessage(string.Format("转换文档列表发生错误[{0}]：{1}", rootFileInfo.FullName, ex.Message), -2);
                    //}
                }
            }
            System.GC.Collect();
            this.ConvertProcess.Value = this.ConvertProcess.Maximum;
            this.picLoading.Image = Properties.Resources.right;
            ShowMessage(string.Format("转换文档列表完成[{0}]。", WordDirectory));
        }
        private void ConvertPadDataForm_Load(object sender, EventArgs e)
        {
            this.picLoading.Image = Properties.Resources.loading;
            this.convertHelper = new ConvertHelper(LrmDirectory, ResDirectory);
        }


        //内部方法
        GB ConvertLrmToWordForPDF(string ModelPath, GB g, double countdate, DateTime countdatetime)
        {
            Person cadre_person;
            using (WordHelper wordHelper = new WordHelper(ModelPath, Global.IsShowWord))
            {
                LrmHelper lrmreader = new LrmHelper();
                if (Path.GetExtension(g.Local_StorgeLrmFullpath) == ".lrm")
                {
                    cadre_person = lrmreader.GetPersonFromLrm(g.Local_StorgeLrmFullpath, g.Local_StorgePicFullpath);
                }
                else if (Path.GetExtension(g.Local_StorgeLrmFullpath) == ".lrmx")
                {
                    cadre_person = lrmreader.GetPersonFromLrmx(g.Local_StorgeLrmFullpath);
                }
                else
                {
                    cadre_person = new Person();
                }

                Word.Table t1 = wordHelper.GetTable(1);
                t1.Cell(1, 2).Range.Text = cadre_person.XingMing;
                t1.Cell(1, 4).Range.Text = cadre_person.XingBie;
                string strAge;
                try
                {
                    strAge = (Math.Floor(countdate - Convert.ToDouble(cadre_person.ChuShengNianYue))).ToString();
                }
                catch (Exception)
                {
                    strAge = "";
                }
                t1.Cell(1, 6).Range.Text = cadre_person.ChuShengNianYue + "\r\n（" + strAge + "岁）";

                object anchor = t1.Cell(1, 7).Range;
                string otpic = Path.Combine(Path.GetDirectoryName(g.Local_StorgeDocumentWordFullpath), cadre_person.XingMing + ".bmp");
                convertHelper.ZoomImageToFile(cadre_person.ZhaoPian_Image, otpic, 6);
                wordHelper.InsertImage(otpic, 2f, 0f, 101f, 134f, anchor);

                t1.Cell(2, 2).Range.Text = cadre_person.MinZu;
                t1.Cell(2, 4).Range.Text = cadre_person.JiGuan;
                t1.Cell(2, 6).Range.Text = cadre_person.ChuShengDi;

                t1.Cell(3, 2).Range.Text = cadre_person.RuDangShiJian;
                t1.Cell(3, 4).Range.Text = cadre_person.CanJiaGongZuoShiJian;
                t1.Cell(3, 6).Range.Text = cadre_person.JianKangZhuangKuang;

                t1.Cell(4, 2).Range.Text = cadre_person.ZhuanYeJiShuZhiWu;
                t1.Cell(4, 4).Range.Text = cadre_person.ShuXiZhuanYeYouHeZhuanChang;

                #region 全日制学历和学位
                if (cadre_person.QuanRiZhiJiaoYu_XueLi.Trim().Length * cadre_person.QuanRiZhiJiaoYu_XueWei.Trim().Length == 0)
                {
                    t1.Cell(5, 3).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi + cadre_person.QuanRiZhiJiaoYu_XueWei;
                    int length = cadre_person.QuanRiZhiJiaoYu_XueLi.Length + cadre_person.QuanRiZhiJiaoYu_XueWei.Length;
                    if (length <= 16)
                    {
                        t1.Cell(5, 3).Range.Font.Size = 14;
                        t1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length <= 18)
                    {
                        t1.Cell(5, 3).Range.Font.Size = 12;
                        t1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if (length <= 33)
                    {
                        t1.Cell(5, 3).Range.Font.Size = 10;
                        t1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(5, 3).Range.Font.Size = 8;
                        t1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                else
                {
                    t1.Cell(5, 3).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi + "\r\n" + cadre_person.QuanRiZhiJiaoYu_XueWei;
                    int length1 = cadre_person.QuanRiZhiJiaoYu_XueLi.Length;
                    int length2 = cadre_person.QuanRiZhiJiaoYu_XueWei.Length;
                    if (length1 <= 8 && length2 <= 8)
                    {
                        t1.Cell(5, 3).Range.Font.Size = 14;
                        t1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length1 <= 9 && length2 <= 9)
                    {
                        t1.Cell(5, 3).Range.Font.Size = 12;
                        t1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if ((length1 <= 22 && length2 <= 11) || (length1 <= 11 && length2 <= 22))
                    {
                        t1.Cell(5, 3).Range.Font.Size = 10;
                        t1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(5, 3).Range.Font.Size = 8;
                        t1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                #endregion
                #region 全日制院校和专业
                if (cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length * cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim().Length == 0)
                {
                    t1.Cell(5, 5).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim() + cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim();
                    int length = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length + cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim().Length;
                    if (length <= 22)
                    {
                        t1.Cell(5, 5).Range.Font.Size = 14;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length <= 24)
                    {
                        t1.Cell(5, 5).Range.Font.Size = 12;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if (length <= 45)
                    {
                        t1.Cell(5, 5).Range.Font.Size = 10;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(5, 5).Range.Font.Size = 8;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                else if (cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim() == cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim())//院校和专业相等的情况下只取一个
                {
                    t1.Cell(5, 5).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim();
                    int length = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length;
                    if (length <= 22)
                    {
                        t1.Cell(5, 5).Range.Font.Size = 14;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length <= 24)
                    {
                        t1.Cell(5, 5).Range.Font.Size = 12;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if (length <= 45)
                    {
                        t1.Cell(5, 5).Range.Font.Size = 10;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(5, 5).Range.Font.Size = 8;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                else
                {
                    t1.Cell(5, 5).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi + "\r\n" + cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi;
                    int length1 = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Length;
                    int length2 = cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Length;
                    if (length1 <= 11 && length2 <= 11)
                    {
                        t1.Cell(5, 5).Range.Font.Size = 14;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length1 <= 12 && length2 <= 12)
                    {
                        t1.Cell(5, 5).Range.Font.Size = 12;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if ((length1 <= 30 && length2 <= 15) || (length1 <= 15 && length2 <= 30))
                    {
                        t1.Cell(5, 5).Range.Font.Size = 10;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(5, 5).Range.Font.Size = 8;
                        t1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                #endregion
                #region 在职学历和学位
                if (cadre_person.ZaiZhiJiaoYu_XueLi.Trim().Length * cadre_person.ZaiZhiJiaoYu_XueWei.Trim().Length == 0)
                {
                    t1.Cell(6, 3).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi + cadre_person.ZaiZhiJiaoYu_XueWei;
                    int length = cadre_person.ZaiZhiJiaoYu_XueLi.Length + cadre_person.ZaiZhiJiaoYu_XueWei.Length;
                    if (length <= 16)
                    {
                        t1.Cell(6, 3).Range.Font.Size = 14;
                        t1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length <= 18)
                    {
                        t1.Cell(6, 3).Range.Font.Size = 12;
                        t1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if (length <= 33)
                    {
                        t1.Cell(6, 3).Range.Font.Size = 10;
                        t1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(6, 3).Range.Font.Size = 8;
                        t1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                else
                {
                    t1.Cell(6, 3).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi + "\r\n" + cadre_person.ZaiZhiJiaoYu_XueWei;
                    int length1 = cadre_person.ZaiZhiJiaoYu_XueLi.Length;
                    int length2 = cadre_person.ZaiZhiJiaoYu_XueWei.Length;
                    if (length1 <= 8 && length2 <= 8)
                    {
                        t1.Cell(6, 3).Range.Font.Size = 14;
                        t1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length1 <= 9 && length2 <= 9)
                    {
                        t1.Cell(6, 3).Range.Font.Size = 12;
                        t1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if ((length1 <= 22 && length2 <= 11) || (length1 <= 11 && length2 <= 22))
                    {
                        t1.Cell(6, 3).Range.Font.Size = 10;
                        t1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(6, 3).Range.Font.Size = 8;
                        t1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                #endregion
                #region 在职院校和专业
                if (cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length * cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim().Length == 0)
                {
                    t1.Cell(6, 5).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi + cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi;
                    int length = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Length + cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Length;
                    if (length <= 22)
                    {
                        t1.Cell(6, 5).Range.Font.Size = 14;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length <= 24)
                    {
                        t1.Cell(6, 5).Range.Font.Size = 12;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if (length <= 45)
                    {
                        t1.Cell(6, 5).Range.Font.Size = 10;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(6, 5).Range.Font.Size = 8;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                else if (cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length == cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim().Length)
                {
                    t1.Cell(6, 5).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim();
                    int length = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length;
                    if (length <= 22)
                    {
                        t1.Cell(6, 5).Range.Font.Size = 14;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length <= 24)
                    {
                        t1.Cell(6, 5).Range.Font.Size = 12;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if (length <= 45)
                    {
                        t1.Cell(6, 5).Range.Font.Size = 10;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(6, 5).Range.Font.Size = 8;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                else
                {
                    t1.Cell(6, 5).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi + "\r\n" + cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi;
                    int length1 = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Length;
                    int length2 = cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Length;
                    if (length1 <= 11 && length2 <= 11)
                    {
                        t1.Cell(6, 5).Range.Font.Size = 14;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 17;//磅值
                    }
                    else if (length1 <= 12 && length2 <= 12)
                    {
                        t1.Cell(6, 5).Range.Font.Size = 12;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 13;//磅值
                    }
                    else if ((length1 <= 30 && length2 <= 15) || (length1 <= 15 && length2 <= 30))
                    {
                        t1.Cell(6, 5).Range.Font.Size = 10;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 11;//磅值
                    }
                    else
                    {
                        t1.Cell(6, 5).Range.Font.Size = 8;
                        t1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 8;//磅值
                    }
                }
                #endregion

                t1.Cell(7, 2).Range.Text = cadre_person.XianRenZhiWu;
                t1.Cell(8, 2).Range.Text = cadre_person.NiRenZhiWu;
                t1.Cell(9, 2).Range.Text = cadre_person.NiMianZhiWu;

                string resume = cadre_person.JianLi;
                if (ResumeFontSizeAndLineSpaceForPDF(resume, 22, 30))
                {
                    t1.Cell(10, 2).Range.Font.Size = 14;
                    t1.Cell(10, 2).Range.Paragraphs.LineSpacing = 15;
                }
                else if (ResumeFontSizeAndLineSpaceForPDF(resume, 24, 33))
                {
                    t1.Cell(10, 2).Range.Font.Size = 13;
                    t1.Cell(10, 2).Range.Paragraphs.LineSpacing = 14;
                }
                else if (ResumeFontSizeAndLineSpaceForPDF(resume, 26, 36))
                {
                    t1.Cell(10, 2).Range.Font.Size = 12;
                    t1.Cell(10, 2).Range.Paragraphs.LineSpacing = 13;
                }
                else if (ResumeFontSizeAndLineSpaceForPDF(resume, 28, 39))
                {
                    t1.Cell(10, 2).Range.Font.Size = 11;
                    t1.Cell(10, 2).Range.Paragraphs.LineSpacing = 12;
                }
                else if (ResumeFontSizeAndLineSpaceForPDF(resume, 31, 43))
                {
                    t1.Cell(10, 2).Range.Font.Size = 10;
                    t1.Cell(10, 2).Range.Paragraphs.LineSpacing = 11;
                }
                else if (ResumeFontSizeAndLineSpaceForPDF(resume, 34, 48))
                {
                    t1.Cell(10, 2).Range.Font.Size = 9;
                    t1.Cell(10, 2).Range.Paragraphs.LineSpacing = 10;
                }
                else
                {
                    t1.Cell(10, 2).Range.Font.Size = 8;
                    t1.Cell(10, 2).Range.Paragraphs.LineSpacing = 9;
                }


                t1.Cell(10, 2).Range.Text = cadre_person.JianLi; ;


                Word.Table wt2 = wordHelper.GetTable(2);
                wt2.Cell(1, 2).Range.Text = cadre_person.JiangChengQingKuang;
                wt2.Cell(2, 2).Range.Text = cadre_person.NianDuKaoHeJieGuo;
                wt2.Cell(3, 2).Range.Text = cadre_person.RenMianLiYou;

                Regex r = new Regex(@"^\d{4}\.\d{2,4}$");

                for (int i = 0; i < 6; i++)
                {
                    if (cadre_person.JiaTingChengYuan.Count > i)
                    {

                        wt2.Cell(i + 5, 2).Range.Text = cadre_person.JiaTingChengYuan[i].ChengWei;
                        wt2.Cell(i + 5, 3).Range.Text = cadre_person.JiaTingChengYuan[i].XingMing;
                        wt2.Cell(i + 5, 4).Range.Text = r.Match(cadre_person.JiaTingChengYuan[i].ChuShengRiQi.Trim()).Success
                            && convertHelper.RelationIsAlive(cadre_person.JiaTingChengYuan[i].GongZuoDanWeiJiZhiWu)
                            ? (Math.Floor(countdate - Convert.ToDouble(cadre_person.JiaTingChengYuan[i].ChuShengRiQi))).ToString()
                            : "";
                        wt2.Cell(i + 5, 5).Range.Text = cadre_person.JiaTingChengYuan[i].ZhengZhiMianMao.Replace("\r", "");
                        wt2.Cell(i + 5, 6).Range.Text = cadre_person.JiaTingChengYuan[i].GongZuoDanWeiJiZhiWu;
                    }
                }

                wt2.Cell(14, 7).Range.Text = countdatetime.ToString("yyyy年MM月dd日");
                wordHelper.SaveDocumentAs(g.Local_StorgeDocumentWordFullpath);
                wordHelper.SaveDocumentAsPDF(g.Local_StorgeLrmPdfFullpath);
                //g.XSD = 1;
            }
            return g;
        }
        DW ConvertDocumentToSaveAsPDFForPDF(DW d, string ProjectPDFTempDirectory, string ProjectPDFOutputDirectory, string LrmDir, string ResDir)
        {
            using (WordHelper wordHelper = new WordHelper(d.Local_SourceDocumnetFullpath, Global.IsShowWord))
            {
                Word._Document oDoc = wordHelper.oDoc;
                Word._Application oWord = wordHelper.oWordApplication;
                Word.Selection oSelection = wordHelper.oWordApplication.Selection;//定义光标移动的参数对象

                object oUnitParagraph = Word.WdUnits.wdParagraph;
                object oUnitLine = Word.WdUnits.wdLine;
                object oUnitCharacter = Word.WdUnits.wdCharacter;
                object oCountN = 0;
                object oCount1 = 1;
                object oCount = 1;
                object oExtend = Word.WdMovementType.wdExtend;//移动并选择
                object oExtendNone = Type.Missing;            //只移动不选择

                int iParagraphCount = wordHelper.oDoc.Paragraphs.Count;

                //提取文号
                int iPara = 1;
                while (((d.WH + "").Trim().Contains("根据文件内容自动") || (d.WH + "").Trim().Length == 0) && iParagraphCount >= iPara)
                {
                    string strPara = wordHelper.oDoc.Paragraphs[iPara].Range.Text
                           .Replace("【", "[").Replace("﹝", "[").Replace("〔", "[")
                           .Replace("】", "]").Replace("﹞", "]").Replace("〕", "]");
                    if (strPara.Contains("[") && strPara.Contains("]") && strPara.Contains("号"))
                    {
                        d.WH = strPara.Replace(" ", "").Replace("　", "")
                            .Replace("\r", "").Replace("\n", "").Replace("\v", "").Replace("\f", "").Replace("\t", "");
                    }
                    iPara++;
                }
                //提取文件名称
                string strMC = "";
                //提取文件名称(省委组织部版)
                for (int i = 1; i <= iParagraphCount; i++)
                {
                    if (wordHelper.oDoc.Paragraphs[i].Range.Font.Name.Contains("小标宋")/*&& WordHelper.oDoc.Paragraphs[i].Range.Font.Size == 20*/)   //如果无法将字体限定到20磅此处不要使用
                    {
                        strMC += wordHelper.oDoc.Paragraphs[i].Range.Text
                            .Replace(" ", "").Replace("　", "").Replace("\r", "").Replace("\n", "")
                            .Replace("\v", "").Replace("\f", "").Replace("\t", "");
                    }
                }
                d.MC = strMC;

                if (!Directory.Exists(Global.ProjectPDFTempDirectory + d.WH + @"\"))
                {
                    Directory.CreateDirectory(Global.ProjectPDFTempDirectory + d.WH + @"\");
                }
                else
                {
                    DirectoryInfo pdfDirectoryInfos = new DirectoryInfo((Global.ProjectPDFTempDirectory + d.WH + @"\"));
                    foreach (FileSystemInfo pdfFileInfo in pdfDirectoryInfos.GetFileSystemInfos())
                    {
                        if ((pdfFileInfo is FileInfo))
                        {
                            string pdfFileExtension = Path.GetExtension(pdfFileInfo.FullName);
                            if (pdfFileExtension == ".pdf")
                            {
                                File.Delete(pdfFileInfo.FullName);
                            }
                        }
                    }
                }

                //d.Local_StorgeDocumentFullpath = ProjectPDFOutputDirectory + d.WH + "——" + d.MC + @".pdf";
                //d.PDF = ProjectPDFTempDirectory + d.WH + @"\000.pdf";

                int iCadreCount = 0;
                ///TODO:添加GB
                d.GBS = new List<GB>();
                //添加干部信息
                for (int i = 1; i <= iParagraphCount; i++)
                {
                    wordHelper.oDoc.Paragraphs[i].Range.Select();
                    string content = wordHelper.oDoc.Paragraphs[i].Range.Text;
                    oSelection.MoveUp(ref oUnitParagraph, ref oCount, ref oExtendNone);//光标移至该段段首

                    if (Global.SearchModel == "全文查找")
                    {
                        #region 逐字判断是否为黑体 获取人名
                        for (int m = 0; m < content.Length; m++)
                        {
                            oSelection.MoveRight(ref oUnitCharacter, ref oCount1, ref oExtend);//光标向右移动一个字符，并选中
                            string fontName = oSelection.Range.Font.Name;

                            if (fontName == "黑体" & fontName != "、")
                            {

                                string XM = oSelection.Range.Text.Replace("、", "").Replace("，", "").Replace("。", "").Replace("；", "").Replace("！", "");
                                if (convertHelper.JudgeStringHasLrm(XM))
                                {
                                    iCadreCount++;
                                    oSelection.MoveRight(ref oUnitCharacter, ref oCount1, ref oExtendNone);
                                    GB g = new GB();
                                    g.id = iCadreCount < 10 ? string.Format("{0}000{1}", d.id, iCadreCount) : string.Format("{0}00{1}", d.id, iCadreCount);
                                    g.DWID = d.id;
                                    g.XM = XM;
                                    g.Rank = d.Rank * 10000 + iCadreCount;

                                    g.Local_SourceLrmFullpath = convertHelper.GetLrmFilePath(g.XM);
                                    g.Local_SourcePicFullpath = convertHelper.GetPicFilePath(g.XM);
                                    g.Local_SourceResFullpath = convertHelper.GetResFilePath(g.XM);

                                    d.GBS.Add(g);
                                }
                            }
                            else
                            {
                                oSelection.MoveRight(ref oUnitCharacter, ref oCount1, ref oExtendNone);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 根据黑体判断，只提取前面的人名
                        int l = (content + "").Length < 10 ? content.Length : 10;
                        for (int j = 0; j < l; j++)
                        {
                            if (j == 0 || oSelection.Range.Font.Name == "黑体")
                            {
                                oSelection.MoveRight(ref oUnitCharacter, ref oCount1, ref oExtend);//再光标向右移动一个字符，并选中
                            }
                            else if ((oSelection.Range.Text + "").Length > 0 && (oSelection.Range.Font.Name + "" == ""))//此时已经判断可能是个人名，但是可能包含多个人名
                            {
                                break;
                            }
                        }

                        oSelection.MoveLeft(ref oUnitCharacter, ref oCount1, ref oExtend);
                        string isname = (oSelection.Text + "").Replace(" ", "").Replace("　", "").Replace("、", "").Replace("，", "").Replace(",", "");
                        if (convertHelper.JudgeStringHasLrm(isname))
                        {
                            string strName = isname;
                            oSelection.MoveDown(ref oUnitLine, ref oCount, ref oExtendNone);
                            iCadreCount++;
                            GB g = new GB();

                            string rmxx = "";

                            g.id = iCadreCount < 10 ? string.Format("{0}000{1}", d.id, iCadreCount) : string.Format("{0}00{1}", d.id, iCadreCount);
                            g.DWID = d.id;
                            g.XM = strName;
                            g.Lrm_Guid = rmxx;
                            g.Rank = d.Rank * 10000 + iCadreCount;

                            g.Local_SourceLrmFullpath = convertHelper.GetLrmFilePath(g.XM);
                            g.Local_SourcePicFullpath = convertHelper.GetPicFilePath(g.XM);
                            g.Local_SourceResFullpath = convertHelper.GetResFilePath(g.XM);

                            d.GBS.Add(g);
                        }
                        #endregion
                    }

                }
                wordHelper.SaveDocumentAsPDF(d.Local_SaveDocumentPdfForCombineFullpath);
            }
            return d;
        }
        bool ResumeFontSizeAndLineSpaceForPDF(string resume, int maxlines, int maxwords)
        {

            string[] resumecontents = resume.Split('\r');
            int rlines = resumecontents.Length;
            foreach (string resumecontent in resumecontents)
            {
                rlines += (int)Math.Floor((double)((resumecontent.Length - 9) / (maxwords - 9)));
            }
            return (maxlines > rlines);
        }
        void ConvertWordToPDF(object wordPath, object pdfPath)
        {
            using (WordHelper wordHelper = new WordHelper(wordPath.ToString(), Global.IsShowWord))
            {
                wordHelper.SaveDocumentAsPDF(pdfPath.ToString());
            }
        }
    }
}
