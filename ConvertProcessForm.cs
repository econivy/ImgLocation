using ImgLocation.Forms;
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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Xps.Packaging;
using Word = Microsoft.Office.Interop.Word;
using iText = iTextSharp.text;
using iTextPdf = iTextSharp.text.pdf;

namespace ImgLocation
{

    public partial class ConvertProcessForm : Form
    {
        public ConvertProcessForm()
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
        public List<PersonWithFile> AllLrmPersons { get; set; }
        public List<DocumentWithFile> AllDocContents { get; set; }

        public struct GBInfo
        {
            public int i;
            public string XM;
            public string Filename;
        }
        public struct DWInfo
        {
            public int i;
            public string WH;
            public string Filename;
        }

        bool HasError = false;
        ConvertHelper convertHelper;
        List<GBInfo> GBInfoes = new List<GBInfo>();
        List<DWInfo> DWInfoes = new List<DWInfo>();
        List<String> WordDocumentPaths = new List<String>();
        void LogRecord(string message)
        {
            LogRecord(message, MessageType.Log);
        }
        void LogRecord(string message, MessageType type)    //int type)
        {
            lblMessage.Text = message;
            tbLog.AppendText(string.Format("[{0}]{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
            tbLog.Focus();//获取焦点
            tbLog.Select(tbLog.TextLength, 0);//光标定位到文本最后           
            tbLog.ScrollToCaret();
            switch (type)
            {
                case MessageType.Error:
                    tbError.AppendText(string.Format("[错误]{0}\r\n", message));
                    tbError.Focus();//获取焦点
                    tbError.Select(tbError.TextLength, 0);//光标定位到文本最后           
                    tbError.ScrollToCaret();
                    break;
                case MessageType.Warnning:
                    tbError.AppendText( string.Format("[警告]{0}\r\n", message));
                    tbError.Focus();//获取焦点
                    tbError.Select(tbError.TextLength, 0);//光标定位到文本最后           
                    tbError.ScrollToCaret();
                    break;
                default:
                    break;
            }

        }

        public DialogResult ShowMessage(string message)
        {
            return ShowMessage(message, MessageType.Log);
        }
        public DialogResult ShowMessage(string message, MessageType type)
        {
            DialogResult dr = DialogResult.OK;
            if (Global.IsShowError)  //提醒或者终止转换
            {
                switch (type)
                {
                    case MessageType.Error:
                        dr = MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        HasError = true;
                        break;
                    case MessageType.Warnning:
                        dr = MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        HasError = true;
                        break;
                    case MessageType.Question:
                        dr = MessageBox.Show(message, "询问", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        break;
                    default:
                        dr = MessageBox.Show(message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            LogRecord(message, type);
            return dr;
        }
        public void ConvertDocumentList()
        {
            DateTime starttime = DateTime.Now;
            ShowMessage("起始时间：" + starttime.ToString("yyyy-MM-dd HH:mm:ss"), MessageType.Warnning);

            ConvertProcess.Maximum = 3000 + 30;//按照共有30个单子，每个单子计算100点进度，30用作前序操作计算进度
            this.picLoading.Image = Properties.Resources.loading;

            #region 转换前关闭Word等进程
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
                            ShowMessage(string.Format("关闭进程（{0}）错误：{1}，请手动关闭正在打开的Word进程，然后点击确定继续进行转换。", pc.ProcessName, ep.Message), MessageType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("关闭进错误：{1}", ex.Message), MessageType.Error);//仅作消息提醒，不终止转换。
            }
            ConvertProcess.Value = 10;
            #endregion

            #region 初始化当前会议的数据库和图像目录
            DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
            int iSX = 100;
            try
            {
                if (IsAdd)
                {
                    iSX = dr.GetMaxSX();
                    dr.ValidateDatabase();
                }
                else
                {
                    iSX = 100;
                    dr.InitialDatabase();
                    FatherForm.ClearImage();

                    string[] strFiles = System.IO.Directory.GetFiles(Global.ProjectOutputImgDirectory);
                    if (strFiles.Length > 0)
                    {
                        foreach (string strFile in strFiles)
                        {
                            File.Delete(strFile);
                        }
                    }
                }

                //添加会议名称
                Project p = Global.LoadDefaultProject();
                dr.AddZY(p.TITLE);
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("初始化数据库错误：{1}", ex.Message), MessageType.Error);
            }
            ConvertProcess.Value = 20;
            #endregion

            LogRecord("正在读取目录信息：" + WordDirectory);
            WordDocumentPaths = new List<string>();
            DirectoryInfo rootDirectoryInfos = new DirectoryInfo(WordDirectory);
            foreach (FileSystemInfo rootFileInfo in rootDirectoryInfos.GetFileSystemInfos())
            {
                if ((rootFileInfo is FileInfo))
                {
                    string fileName = Path.GetFileNameWithoutExtension(rootFileInfo.FullName);
                    string fileExtension = Path.GetExtension(rootFileInfo.FullName);
                    if ((fileExtension == ".doc" || fileExtension == ".docx") && (!fileName.Contains("$")))
                    {
                        WordDocumentPaths.Add(rootFileInfo.FullName);
                    }
                }
            }
            ConvertProcess.Maximum = WordDocumentPaths.Count * 100 + 30;//换算成实际单子数计算进度点
            LogRecord(string.Format("指定目录[{0}]共发现{1}个Word文档！", WordDirectory, WordDocumentPaths.Count));
            ConvertProcess.Value = 30;

            for (int i = 0; i < WordDocumentPaths.Count; i++)
            {
                string documentPath = WordDocumentPaths[i];
                LogRecord(string.Format("[共{0}个文档，已转换{1}个文档]，开始读取第{2}文档信息：{3}。", WordDocumentPaths.Count, i, i + 1, Path.GetFileName(documentPath)));
                DW d = new DW();
                iSX++;
                d.id = Guid.NewGuid().ToString();
                d.Rank = iSX;
                d.Local_SourceDocumnetFullpath = documentPath;

                string filename = Path.GetFileNameWithoutExtension(d.Local_SourceDocumnetFullpath);
                string[] filesplits = filename.Replace("、", ".").Replace("。", ".").Split('.');


                int ixh = 0;
                try
                {
                    ixh = Convert.ToInt32(filesplits[0]);
                }
                catch (Exception)
                {
                    LogRecord(string.Format("[共{0}个文档，已转换{1}个文档]，文档{2}文件名上不包含序号。", WordDocumentPaths.Count, i + 1, Path.GetFileName(documentPath)));
                }
                d.XH = ixh == 0 ? "" : ixh.ToString();

                ConvertDocument(d, string.Empty, true);

                ConvertProcess.Value = 30 + (i + 1) * 100;
                LogRecord(string.Format("[共{0}个文档，已转换{1}个文档]，文档{2}已转换完成。", WordDocumentPaths.Count, i + 1, Path.GetFileName(documentPath)));
            }


            dr.CoverSX("WH");

            GC.Collect();
            ConvertProcess.Value = ConvertProcess.Maximum;
            DateTime endtime = DateTime.Now;
            TimeSpan time = endtime - starttime;
            ShowMessage("结束时间：" + starttime.ToString("yyyy-MM-dd HH:mm:ss") + "，共花费时间：" + time.TotalMinutes + "分钟", MessageType.Warnning);
            this.picLoading.Image = Properties.Resources.right;
            if (DialogResult.Yes == MessageBox.Show(string.Format("批量转换文档[{0}]完成{1}。\r\n\r\n是否关闭当前转换窗口？", WordDirectory, HasError ? "。\r\n（提示）转换过程中存在错误，详细情况查看错误日志" : ""), "转换完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Close();
                this.FatherForm.Show();
            }
        }
        public DW ConvertSingleDocument(DW document, string ConvertPageRange, bool IsRelocationCadre)
        {
            ConvertProcess.Maximum = 100;
            this.picLoading.Image = Properties.Resources.loading;
            document = ConvertDocument(document, ConvertPageRange, IsRelocationCadre);
            GC.Collect();
            ConvertProcess.Value = 100;
            //this.picLoading.Image = Properties.Resources.right;
            if (DialogResult.Yes == MessageBox.Show(string.Format("批量转换文档[{0}]完成{1}。\r\n\r\n是否关闭当前转换窗口？", WordDirectory, HasError ? "。\r\n（提示）转换过程中存在错误，详细情况查看错误日志" : ""), "转换完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Close();
                this.FatherForm.Show();
            }
            return document;
        }
        public GB ConvertSingleCadre(DW d, GB g)
        {
            ConvertProcess.Maximum = 50;
            this.picLoading.Image = Properties.Resources.loading;
            g = ConvertCadre(g);
            GC.Collect();
            ConvertProcess.Value = 50;
            this.picLoading.Image = Properties.Resources.right;
            DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);

            #region 已停用 原向正文后方添加任免表和考察材料
            //if (Global.IsAttachLrmRes)
            //{
            //    //移除原有GB转换的DI
            //    foreach (GB cgb in d.GBS)
            //    {
            //        List<DI> rdis = d.DIS.Where(sdi => sdi.id == cgb.id + "_01" || sdi.id == cgb.id + "_02").ToList();
            //        foreach (DI rdi in rdis)
            //        {
            //            d.DIS.Remove(rdi);
            //        }
            //    }

            //    //移除文档原后附材料
            //    List<DI> firstAttachDIs = d.DIS.Where(fadi => fadi.P > d.H).ToList();
            //    foreach (DI firstAttachDI in firstAttachDIs)
            //    {
            //        d.DIS.Remove(firstAttachDI);
            //    }

            //    //移除更改的GB
            //    GB ssg = d.GBS.Where(sg => sg.id == g.id).SingleOrDefault();
            //    if (ssg != null)
            //    {
            //        d.GBS.Remove(ssg);
            //    }

            //    d.GBS.Add(g);


            //    //添加                
            //    int p = d.DIS.Max(dii => dii.P);
            //    for (int k = 0; k < d.GBS.Count; k++)
            //    {
            //        GB gg = d.GBS[k];
            //        DI gdi = new DI();
            //        gdi.id = gg.id + "_01";
            //        p++;
            //        gdi.P = p;
            //        gdi.SX = d.SX * 100 + p;
            //        gdi.DWID = d.id;
            //        gdi.DDI = gg.LrmImageFilename;
            //        gdi.DIMG = gg.LRMIMG;
            //        Image ddimg = Image.FromFile(gg.LRMIMG);
            //        gdi.W = ddimg.Width;
            //        gdi.H = ddimg.Height;
            //        d.DIS.Add(gdi);
            //        ddimg.Dispose();
            //        if ((gg.ResImageFilename + "").Trim().Length > 0)
            //        {
            //            DI gdi2 = new DI();
            //            gdi2.id = gg.id + "_02";
            //            p++;
            //            gdi2.P = p;
            //            gdi2.SX = d.SX * 100 + p;
            //            gdi2.DWID = d.id;
            //            gdi2.DDI = gg.ResImageFilename;
            //            gdi2.DIMG = gg.RESIMG;
            //            Image ddimg2 = Image.FromFile(gg.RESIMG);
            //            gdi2.W = ddimg2.Width;
            //            gdi2.H = ddimg2.Height;
            //            d.DIS.Add(gdi2);
            //            ddimg2.Dispose();
            //        }
            //    }
            //    foreach (DI firstAttachDI in firstAttachDIs)
            //    {
            //        p++;
            //        firstAttachDI.P = p;
            //        firstAttachDI.SX = d.SX * 100 + p;
            //        d.DIS.Add(firstAttachDI);
            //    }
            //    dr.EditDW(d);
            //}
            #endregion

            if (DialogResult.Yes == MessageBox.Show(string.Format("批量转换文档[{0}]完成{1}。\r\n\r\n是否关闭当前转换窗口？", WordDirectory, HasError ? "。\r\n（提示）转换过程中存在错误，详细情况查看错误日志" : ""), "转换完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Close();
                this.FatherForm.Show();
            }
            dr.EditGB(g);
            return g;
        }
        public DW ConvertDocument(DW d, string ConvertPageRange, bool IsLocationCadre)
        {
            //转换前，先根据ConvertPageRange生成需要替换的图像索引列表
            List<int> convertPageIndexes = new List<int>();
            if (ConvertPageRange.Trim().Length > 0)
            {
                foreach (string pagerange in ConvertPageRange.Split(';'))
                {
                    if (pagerange.Contains("-"))
                    {
                        int b = Convert.ToInt32(pagerange.Split('-')[0]);
                        int u = Convert.ToInt32(pagerange.Split('-')[1]);
                        for (int i = b - 1; i < u; i++)
                        {
                            convertPageIndexes.Add(i);
                        }
                    }
                    else
                    {
                        convertPageIndexes.Add(Convert.ToInt32(pagerange) - 1);
                    }
                }
            }

            //当convertPageIndexes为0时转换全部页面时，删除全部原有文档图像
            //否则，根据convertPageIndexes删除指定索引的页面
            if (d.DocumentImageCount > 0 && convertPageIndexes.Count == 0)
            {
                foreach (string documentimage in d.DocumentImageFileFullPaths)
                {
                    if (File.Exists(documentimage))
                    {
                        File.Delete(documentimage);
                    }
                }
            }
            else if (d.DocumentImageCount > 0 && convertPageIndexes.Count == 0)
            {
                foreach (int i in convertPageIndexes)
                {
                    if (File.Exists(d.DocumentImageFileFullPaths[i]))
                    {
                        File.Delete(d.DocumentImageFileFullPaths[i]);
                    }
                }
            }

            using (DataRepository dr = new DataRepository(Global.ProjectOutputDbPath))
            {
                LogRecord(string.Format("[正在读取文档]{0}", d.Local_SourceDocumnetFullpath));
                try
                {
                    //DW对象必须指定source属性
                    if ((d.Local_SourceDocumnetFullpath + "").Trim().Length > 0)
                {
                    LogRecord(string.Format("[正在转换文档]{0}:识别文档中的干部姓名，另存为PDF文件。", Path.GetFileName(d.Local_SourceDocumnetFullpath)));
                    //转换成PDF，需要调整函数，明确是否需要识别文档中的干部
                    //是否有必要使用内部方法？取消内部方法，直接使用代码
                    //d = ConvertDocumentToSaveAsPDF(d);
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
                                   .Replace("【", "〔").Replace("﹝", "〔").Replace("[", "〔")
                                   .Replace("】", "〕").Replace("﹞", "〕").Replace("]", "〕");
                            if (strPara.Contains("〔") && strPara.Contains("〕") && strPara.Contains("号"))
                            {
                                d.WH = strPara.Replace(" ", "").Replace("　", "")
                                    .Replace("\r", "").Replace("\n", "").Replace("\v", "").Replace("\f", "").Replace("\t", "");
                            }
                            iPara++;
                        }

                        //提取文件名称
                        string strMC = "";
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


                        //截取过长的文件标题名称作为存储路径
                        if (d.MC.Length > 20)
                        {
                            d.DocumentImageFilename = (d.MC.Substring(0, 20) + d.id.ToString() + "." + Global.ImgFormat.ToString().ToLower());
                        }
                        else
                        {
                            d.DocumentImageFilename = (d.MC + d.id.ToString() + "." + Global.ImgFormat.ToString().ToLower());
                        }

                        d.DocumentImageFilename = d.DocumentImageFilename.Replace(" ", "").Replace("　", "")
                                .Replace("\r", "").Replace("\n", "")
                                .Replace("\v", "").Replace("\f", "").Replace("\t", "");

                        //另存在存储路径
                        wordHelper.SaveDocumentAs(d.Local_StorgeDocumentFullpath, false);

                        //在文档末端 添加切白分割线
                        //极其特殊情况下 切白边分给线可能会添加到新页中，注意识别
                        //移到文章最末末端
                        oDoc.Paragraphs[wordHelper.oDoc.Paragraphs.Count].Range.Select();
                        //获取原页面总数
                        int orignPageCount = oSelection.get_Information(Word.WdInformation.wdActiveEndAdjustedPageNumber);

                        oSelection.MoveDown(ref oUnitParagraph, ref oCount, ref oExtendNone);
                        oDoc.Paragraphs.Add();
                        oSelection.MoveDown(ref oUnitParagraph, ref oCount, ref oExtendNone);
                        oDoc.Paragraphs.Add();
                        oSelection.MoveDown(ref oUnitParagraph, ref oCount, ref oExtendNone);
                        Word.Range r = oSelection.Range;
                        oDoc.Tables.Add(r, 1, 1);
                        Word.Table t = oDoc.Tables[oDoc.Tables.Count];

                        //识别分割线调到新页面上，并撤销操作
                        t.Select();
                        if (orignPageCount != oSelection.get_Information(Word.WdInformation.wdActiveEndAdjustedPageNumber))
                        {
                            //撤销添加段落
                            oDoc.Tables[oDoc.Tables.Count].Delete();
                            oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.Delete();
                            oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.Delete();
                        }
                        else
                        {
                            t.Select();
                            t.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleNone;
                            t.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleNone;
                            t.Shading.BackgroundPatternColor = Word.WdColor.wdColorGreen;
                            t.Shading.ForegroundPatternColor = Word.WdColor.wdColorGreen;
                        }

                        //保存更改
                        wordHelper.SaveDocument(false);

                        //获取文档总页数
                        wordHelper.oDoc.Paragraphs[wordHelper.oDoc.Paragraphs.Count].Range.Select();
                        oSelection.MoveDown(ref oUnitParagraph, ref oCount, ref oExtendNone);
                        d.DocumentImageCount = oSelection.get_Information(Word.WdInformation.wdActiveEndAdjustedPageNumber);

                        //根据IsLocationCadre判断是否获取干部姓名所在点位
                        if (IsLocationCadre)
                        {
                            ///识别指定页面的干部信息
                            ///为了识别指定页面的干部信息，获取文档干部点位时 不应当直接给d.GBS赋值，应当通过中转变量

                            //添加干部信息 通过中间变量 完成后 移除所有指定页的干部信息 注意iCadreCount的起始值 cadreid应当和pagenumber相关联
                            //d.GBS = new List<GB>();

                            //移除指定页面的GB信息
                            if (convertPageIndexes.Count > 0)
                            {
                                List<GB> removeGB = d.GBS.Where(gb => convertPageIndexes.Contains(gb.DocumentImagePageNumber - 1)).ToList();
                                foreach (GB rgb in removeGB)
                                {
                                    d.GBS.Remove(rgb);
                                }
                            }

                            List<GB> GBsFromDocumentPage = new List<GB>();
                            int iCadreCount = 0;//干部计数
                            for (int i = 1; i <= iParagraphCount; i++)
                            {
                                wordHelper.oDoc.Paragraphs[i].Range.Select();
                                string content = wordHelper.oDoc.Paragraphs[i].Range.Text;

                                if (oSelection.Range.Font.Name == "")    //只有存在多种字体 才可能存在姓名
                                {
                                    oSelection.MoveUp(ref oUnitParagraph, ref oCount, ref oExtendNone);//光标移至该段段首
                                    oCount1 = 1;

                                    int oPageStart = oSelection.get_Information(Word.WdInformation.wdActiveEndAdjustedPageNumber);
                                    int oPageEnd = oSelection.get_Information(Word.WdInformation.wdActiveEndAdjustedPageNumber);

                                    //查找干部姓名
                                    if (convertPageIndexes.Count == 0  //情况一：全部重新定义干部位置
                                        || (convertPageIndexes.Count > 0 && convertPageIndexes.Contains(oPageStart - 1))//情况二：识别指定页面的干部信息
                                        || (convertPageIndexes.Count > 0 && convertPageIndexes.Contains(oPageEnd - 1))//情况三：识别指定页面的干部信息
                                        )
                                    {
                                        #region 新思路全文查找干部姓名
                                        string rangeText = wordHelper.oDoc.Paragraphs[i].Range.Text;

                                        if (AllLrmPersons.Any(lp => rangeText.Contains(lp.XingMing)
                                            || (lp.XingMing.Length == 2 && rangeText.Contains(lp.XingMing.Substring(0, 1) + "  " + lp.XingMing.Substring(1, 1)))
                                            || (lp.XingMing.Length == 2 && rangeText.Contains(lp.XingMing.Substring(0, 1) + "　" + lp.XingMing.Substring(1, 1)))
                                            ))
                                        {
                                            List<int> allindexinp = new List<int>();
                                            foreach (PersonWithFile pwf in AllLrmPersons.Where(lp => rangeText.Contains(lp.XingMing)
                                                || (lp.XingMing.Length == 2 && rangeText.Contains(lp.XingMing.Substring(0, 1) + "  " + lp.XingMing.Substring(1, 1)))
                                                || (lp.XingMing.Length == 2 && rangeText.Contains(lp.XingMing.Substring(0, 1) + "　" + lp.XingMing.Substring(1, 1)))
                                                ).AsEnumerable())
                                            {
                                                wordHelper.oDoc.Paragraphs[i].Range.Select();
                                                oSelection.MoveUp(ref oUnitParagraph, ref oCount, ref oExtendNone);//光标移至该段段首

                                                string XM = pwf.XingMing;
                                                if (AllLrmPersons.Where(lp => lp.XingMing.Length == 2 && rangeText.Contains(lp.XingMing.Substring(0, 1) + "  " + lp.XingMing.Substring(1, 1))).Contains(pwf))
                                                {
                                                    XM = pwf.XingMing.Substring(0, 1) + "  " + pwf.XingMing.Substring(1, 1);
                                                }
                                                else if (AllLrmPersons.Where(lp => lp.XingMing.Length == 2 && rangeText.Contains(lp.XingMing.Substring(0, 1) + "　" + lp.XingMing.Substring(1, 1))).Contains(pwf))
                                                {
                                                    XM = pwf.XingMing.Substring(0, 1) + "　" + pwf.XingMing.Substring(1, 1);
                                                }

                                                int index_start = rangeText.IndexOf(XM);
                                                int index_end = index_start + XM.Length;

                                                oCount = index_end;
                                                oSelection.MoveRight(ref oUnitCharacter, ref oCount, ref oExtendNone);//光标移至该干部名字后方
                                                oSelection.MoveRight(ref oUnitCharacter, ref oCount1, ref oExtend);//选中一个字
                                                if (oSelection.Range.Font.Name != "黑体"
                                                    || (oSelection.Range.Font.Name == "黑体" && (oSelection.Range.Text.Contains("。") || oSelection.Range.Text.Contains("、") || oSelection.Range.Text.Contains("（") || oSelection.Range.Text.Contains("\r") || oSelection.Range.Text.Contains("\n"))))
                                                {
                                                    oSelection.MoveLeft(ref oUnitCharacter, ref oCount1, ref oExtendNone);//向左移动一个字符 不选中，到干部姓名后
                                                    
                                                    //获取当前光标位置与页面左端的宽度 单位英寸
                                                    double oParagraphHorizontalEnd = ((oSelection.get_Information(Word.WdInformation.wdHorizontalPositionRelativeToPage) / 72 * 25.4) - Global.HorizontalCutSize) / (210 - (Global.HorizontalCutSize * 2));
                                                    
                                                    //移动光标到干部姓名前 并选中
                                                    oCount = XM.Length;
                                                    oSelection.MoveLeft(ref oUnitCharacter, ref oCount, ref oExtend);//向左移动选中干部姓名

                                                    //获取当前段落的起始位置与页面顶端的高度 单位英寸
                                                    double oParagraphStart = ((oSelection.get_Information(Word.WdInformation.wdVerticalPositionRelativeToPage) / 72 * 25.4) - Global.VerticalCutSize) / (297 - (Global.VerticalCutSize * 2));
                                                    //获取当前段落的结束位置与页面顶端的高度 单位英寸
                                                    double oParagraphEnd = ((oSelection.get_Information(Word.WdInformation.wdVerticalPositionRelativeToPage) / 72 * 25.4) - Global.VerticalCutSize) / (297 - (Global.VerticalCutSize * 2));
                                                    //干部姓名不断行时 oParagraphStart和oParagraphEnd应当相等

                                                    //获取当前光标位置与页面左端的宽度 单位英寸
                                                    double oParagraphHorizontalStart = ((oSelection.get_Information(Word.WdInformation.wdHorizontalPositionRelativeToPage) / 72 * 25.4) - Global.HorizontalCutSize) / (210 - (Global.HorizontalCutSize * 2));

                                                    //为了矫正跨页段落的页码不正确，重新获取段落的起始和终止页面
                                                    oPageStart = oSelection.get_Information(Word.WdInformation.wdActiveEndAdjustedPageNumber);
                                                    oPageEnd = oSelection.get_Information(Word.WdInformation.wdActiveEndAdjustedPageNumber);

                                                    //在部分获取的模式下 只有当当前干部在页面范围内 才继续识别
                                                    //所以要进行进一步判断，当前干部名称是否所在页为替换页面 
                                                    if ((oSelection.Range.Font.Name == "黑体")
                                                        && (convertPageIndexes.Count == 0  //情况一：全部重新定义干部位置
                                                        || ((convertPageIndexes.Count > 0 && convertPageIndexes.Contains(oPageStart - 1))//情况二：识别指定页面的干部信息
                                                        && (convertPageIndexes.Count > 0 && convertPageIndexes.Contains(oPageEnd - 1)))//情况三：识别指定页面的干部信息
                                                        ))
                                                    {
                                                        if (!allindexinp.Contains(index_start))
                                                        {

                                                            allindexinp.Add(index_start);
                                                            iCadreCount++;
                                                            oSelection.Range.Font.ColorIndex = Word.WdColorIndex.wdDarkBlue;
                                                            oSelection.MoveRight(ref oUnitCharacter, ref oCount1, ref oExtendNone);

                                                            GB g = new GB();
                                                            g.id = string.Format("{0}_{1:000000}", d.id, oPageStart * 1000 + iCadreCount);
                                                            g.DWID = d.id;
                                                            g.XM = pwf.XingMing;
                                                            g.DocumentImagePageNumber = oPageStart;//干部姓名所在干呈件的页码
                                                            g.DocumentImageFilename = d.DocumentImageFileFullPaths[g.DocumentImagePageNumber - 1];
                                                            g.Rank = d.Rank * 1000000 + oPageStart * 1000 + iCadreCount;

                                                            g.TouchStartPointY = oParagraphStart;
                                                            g.TouchEndPointY = oParagraphStart + ((double)28 / (double)72 * 25.4) / (297 - (Global.VerticalCutSize * 2)) > 1 ? 1 : oParagraphStart + ((double)28 / (double)72 * 25.4) / (297 - (Global.VerticalCutSize * 2));
                                                            g.TouchStartPointX = oParagraphHorizontalStart;
                                                            g.TouchEndPointX = oParagraphHorizontalEnd;

                                                            g.Local_SourceLrmFullpath = convertHelper.GetLrmFilePath(g.XM);
                                                            g.Local_SourcePicFullpath = convertHelper.GetPicFilePath(g.XM);
                                                            g.Local_SourceResFullpath = convertHelper.GetResFilePath(g.XM);

                                                            if(g.TouchEndPointX < g.TouchStartPointX)
                                                            {
                                                                g.TouchEndPointX = 0.96;

                                                                //需要DDIP2 和 DDID2 解决名字跨页断行的问题
                                                                //名字跨页断行问题不再通过代码解决。
                                                                g.TouchStartPointY2 = g.TouchEndPointY;
                                                                g.TouchEndPointY2 = g.TouchEndPointY + ((double)28 / (double)72 * 25.4) / (297 - (Global.VerticalCutSize * 2));

                                                                g.TouchStartPointX2 = 0.035;
                                                                g.TouchEndPointX2 = oParagraphHorizontalEnd;
                                                            }
                                                            else
                                                            {
                                                                g.TouchStartPointY2 = 0;
                                                                g.TouchEndPointY2 = 0;
                                                                g.TouchStartPointX2 = 0;
                                                                g.TouchEndPointX2 = 0;
                                                            }

                                                            GBsFromDocumentPage.Add(g);

                                                            ////重新获取当前光标位置与页面左端的宽度 单位英寸
                                                            //oParagraphHorizontalStart = ((oSelection.get_Information(Word.WdInformation.wdHorizontalPositionRelativeToPage) / 72 * 25.4) - Global.HorizontalCutSize) / (210 - (Global.HorizontalCutSize * 2));
                                                            
                                                            //重新获取当前段落的起始位置与页面顶端的高度 单位英寸
                                                            oParagraphStart = ((oSelection.get_Information(Word.WdInformation.wdVerticalPositionRelativeToPage) / 72 * 25.4) - Global.VerticalCutSize) / (297 - (Global.VerticalCutSize * 2));
                                                        }
                                                    }

                                                }
                                                oCount = 1;
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            //根据转换类型 更新或者重新为d.GBS赋值
                            if (convertPageIndexes.Count == 0)
                            {
                                d.GBS = GBsFromDocumentPage;
                            }
                            else
                            {
                                d.GBS.AddRange(GBsFromDocumentPage);
                            }
                        }
                        wordHelper.SaveDocumentAsXPS(d.Local_StorgeDocumentPdfFullpath);

                        // ۩ 这是一个墓碑，纪念曾经困扰我们多年的RPC错误；
                        //去死吧RPC错误
                        //File.Copy(d.SOURCE, , true);
                    }
                    LogRecord(string.Format("[正在转换文档]{0}：识别文档中的干部姓名，另存为PDF文件（已完成）。", Path.GetFileName(d.Local_SourceDocumnetFullpath)));
                    ConvertProcess.Value = ConvertProcess.Value + 25;

                    //判断文号重复
                    ///TODO 文号重复的识别机制需要更新
                    DWInfo dwi = new DWInfo();
                    dwi.i = DWInfoes.Count();
                    dwi.Filename = Path.GetFileName(d.Local_SourceDocumnetFullpath);
                    dwi.WH = d.WH;
                    if (DWInfoes.Any(s => s.WH == dwi.WH))
                    {
                        foreach (DWInfo sdwi in DWInfoes.Where(s => s.WH == dwi.WH).AsEnumerable())
                        {
                            ShowMessage(string.Format("[发现文号重复的文档]“{0}”和“{1}”的文号都为：{2}", dwi.Filename, sdwi.Filename, dwi.WH), MessageType.Warnning);
                        }
                    }
                    DWInfoes.Add(dwi);

                    LogRecord(string.Format("[正在转换文档]{0}：将文档转换成图片并进行裁剪。", Path.GetFileName(d.Local_SourceDocumnetFullpath)));
                    List<Bitmap> DocumentPageImages = ConvertXPSToBitmapList(d.Local_StorgeDocumentPdfFullpath, convertPageIndexes);
                    for (int i = DocumentPageImages.Count > 1 ? DocumentPageImages.Count - 2 : 0; i < DocumentPageImages.Count; i++)
                    {
                        //切除文档底部白边 只判断最后两页
                        DocumentPageImages[i] = CutBottomBlankPart(DocumentPageImages[i]);
                        //using (Bitmap ImageAfterCutBottomBlank = DocumentPageImages[i])
                        //{
                        //    DocumentPageImages[i] = CutImage(ImageAfterCutBottomBlank, 22);
                        //    ImageAfterCutBottomBlank.Save(d.DocumentImageFileFullPaths[i]);
                        //    ImageAfterCutBottomBlank.Dispose();
                        //    GC.Collect();
                        //}
                    }

                    for (int i = 0; i < DocumentPageImages.Count; i++)
                    {
                        if (convertPageIndexes.Count == 0)
                        {
                            DocumentPageImages[i].Save(d.DocumentImageFileFullPaths[i]);
                        }
                        else
                        {
                            DocumentPageImages[i].Save(d.DocumentImageFileFullPaths[convertPageIndexes[i]]);
                        }
                    }

                    DocumentPageImages.Clear();
                    GC.Collect();

                    LogRecord(string.Format("[正在转换文档]{0}：将文档转换成图片并进行裁剪（已完成）。", Path.GetFileName(d.Local_SourceDocumnetFullpath)));
                    ConvertProcess.Value = ConvertProcess.Value + 20;

                    LogRecord(string.Format("[正在转换文档]{0}：文档中共识别{1}个干部姓名。", Path.GetFileName(d.Local_SourceDocumnetFullpath), d.GBS.Count));

                    if (IsLocationCadre)
                    {
                        List<GB> ListGBToConvert = convertPageIndexes.Count == 0 ? d.GBS : d.GBS.Where(gb => convertPageIndexes.Contains(gb.DocumentImagePageNumber - 1)).ToList();
                        for (int j = 0; j < ListGBToConvert.Count; j++)
                        {
                            GB g = ListGBToConvert[j];
                            LogRecord(string.Format("[正在转换文档]{0}：共需要转换{1}个干部信息，开始转换第{2}个干部{3}的信息。", Path.GetFileName(d.Local_SourceDocumnetFullpath), ListGBToConvert.Count, j + 1, g.XM));

                            //判断重名 20170224更新识别机制
                            IQueryable<PersonWithFile> lrms = this.AllLrmPersons.Where(l => l.XingMing == g.XM).AsQueryable();
                            IQueryable<DocumentWithFile> docs = this.AllDocContents.Where(r => r.DocxFilename.Contains(g.XM) && r.DocxFilename.Contains("考察材料")).AsQueryable();
                            if (lrms.Count() > 1 || docs.Count() > 1)
                            {
                                if (Global.IsShowError)
                                {
                                    SameCadreForm scf = new SameCadreForm();
                                    scf.Converting_GB = g;
                                    scf.GB_LrmPersons = lrms.ToList();
                                    scf.GB_DocContents = docs.ToList();
                                    if (DialogResult.OK == scf.ShowDialog())
                                    {
                                        g = scf.Converting_GB;
                                        LogRecord(string.Format("[文档{0}中的{1}的任免表重复已经选择]：{2}。", Path.GetFileName(d.Local_SourceDocumnetFullpath), g.XM, g.Local_SourceLrmFullpath), MessageType.Warnning);
                                        ConvertCadre(g);
                                    }
                                    else
                                    {
                                        ShowMessage(string.Format("[文档{0}中的{1}的任免表重复没有选择]：需要重新选择，并转换干部信息。", Path.GetFileName(d.Local_SourceDocumnetFullpath), g.XM), MessageType.Error);
                                    }
                                }
                                else
                                {
                                    ShowMessage(string.Format("[文档{0}中的{1}的任免表重复没有选择]：需要重新选择，并转换干部信息。", Path.GetFileName(d.Local_SourceDocumnetFullpath), g.XM), MessageType.Error);
                                }
                            }
                            else
                            {
                                ConvertCadre(g);
                            }


                            //修正因为切除多余空白部分导致的点位比例不正确 只需要修正最后一张图像，前面的图像不涉及裁剪的问题
                            Image DocumentImageCadreIn = Image.FromFile(d.DocumentImageFileFullPaths[g.DocumentImagePageNumber - 1]);
                            int standardHeight = (int)(Global.ImgWidth / (210 - Global.HorizontalCutSize * 2) * (297 - Global.VerticalCutSize * 2));
                            g.TouchStartPointY = g.TouchStartPointY * (standardHeight) / DocumentImageCadreIn.Height;
                            g.TouchEndPointY = g.TouchEndPointY * (standardHeight) / DocumentImageCadreIn.Height > 1 ? 1 : g.TouchEndPointY * (standardHeight) / DocumentImageCadreIn.Height;
                            DocumentImageCadreIn.Dispose();
                            dr.EditGB(g);

                            LogRecord(string.Format("[正在转换文档]{0}：共{1}个干部信息，第{2}个干部{3}的信息转换已完成。", Path.GetFileName(d.Local_SourceDocumnetFullpath), d.GBS.Count, j + 1, g.XM));
                            ConvertProcess.Value = ConvertProcess.Value + 50 / d.GBS.Count;
                        }
                    }

                    ////现有模式下一般不会在产生空白页，因此这段代码废弃
                    ////移除空白页
                    //Bitmap QuestionImage = (Bitmap)Image.FromFile(d.DocumentImageFileFullPaths[d.DocumentImageCount - 1]);
                    //bool question = true;
                    //for(int h=0;h<QuestionImage.Height;h++)
                    //{
                    //    Color p = QuestionImage.GetPixel(1116, h);
                    //    Color p2 = QuestionImage.GetPixel(1232, h);
                    //    Color p3 = QuestionImage.GetPixel(1348, h);
                    //    if((p.R==p2.R&&p2.R==p3.R &&p.G==p2.G&&p2.G==p3.G&&p.B==p2.B&&p2.B==p3.B)&&((p.R>200&&p.G>200&&p.B>200)||(p.R<50&&p.G>200&&p.B<50)))
                    //    {
                    //        h++;
                    //    }
                    //    else
                    //    {
                    //        question = false;
                    //        break;
                    //    }
                    //}
                    //if(question&& File.Exists(d.DocumentImageFileFullPaths[d.DocumentImageCount - 1]))
                    //{
                    //    QuestionImage.Dispose();
                    //    File.Delete(d.DocumentImageFileFullPaths[d.DocumentImageCount - 1]);
                    //    d.DocumentImageCount -= 1;
                    //}
                    //else
                    //{
                    //    QuestionImage.Dispose();
                    //}
                    GC.Collect();
                    ConvertProcess.Value = ConvertProcess.Value + 5;
                }
                else
                {
                    d.DocumentImageCount = 0;
                }
                }
                catch (Exception ex)
                {
                    ShowMessage(string.Format("[转换文档{0}发生错误]，{1}", d.DocumentImageFileFullPaths, ex.Message), MessageType.Error);
                }
                dr.EditDW(d);
            }

            FatherForm.ReloadTree();
            convertHelper.UpdateDataId();
            GC.Collect();
            return d;
        }
        public GB ConvertCadre(GB g)
        {
            //清理旧图像
            if (g.AllImageFileFullPaths.Count > 0)
            {
                foreach (string ImagePath in g.AllImageFileFullPaths)
                {
                    if (File.Exists(ImagePath))
                    {
                        File.Delete(ImagePath);
                    }
                }
            }

            //转换任免表
            string uuid = Guid.NewGuid().ToString();
            if (g.Local_SourceLrmFullpath != null && File.Exists(g.Local_SourceLrmFullpath))
            {
                try
                {
                    g.LrmImageCount = 1;
                    g.Lrm_Guid = Guid.NewGuid().ToString();
                    if (Path.GetExtension(g.Local_SourceLrmFullpath).ToLower() == ".lrm")
                    {
                        File.Copy(g.Local_SourceLrmFullpath, g.Local_StorgeLrmFullpath, true);
                        File.Copy(g.Local_SourcePicFullpath, g.Local_StorgePicFullpath, true);
                    }
                    else
                    {
                        File.Copy(g.Local_SourceLrmFullpath, g.Local_StorgeLrmFullpath, true);
                    }

                    if (Global.IsUsingLrmToImageModel && File.Exists(g.Local_StorgeLrmFullpath))
                    {
                        LrmHelper lrmReader = new LrmHelper();
                        Person person = lrmReader.GetPersonFromLrmFile(g.Local_StorgeLrmFullpath);
                        LrmToImage lti = new LrmToImage(person);
                        Bitmap lrmImage = lti.CreateImage();
                        lrmImage.Save(g.LrmImageFileFullPaths[0]);
                        lrmImage.Dispose();
                        person.Dispose();
                    }
                    else if (File.Exists(g.Local_StorgeLrmFullpath))
                    {
                        g = ConvertLrmToWordSaveAsXPS(Global.LrmToWordModelPath, g, Convert.ToDouble((CountDate).Date.ToString("yyyy.MM")));
                        List<Bitmap> bmps = ConvertLrmXPSToBitmapList(g.Local_StorgeLrmPdfFullpath);
                        bmps[0] = CutBottomBlankPart(bmps[0]);
                        bmps[0].Save(g.LrmImageFileFullPaths[0]);
                        bmps[0].Dispose();
                        bmps.Clear();
                    }
                    else
                    {
                        g.LrmImageCount = 0;
                    }
            }
                catch (Exception ex)
            {
                ShowMessage(string.Format("[转换干部任免审批表失败:{0}]，{1}", g.Local_SourceLrmFullpath, ex.Message), MessageType.Error);
            }
        }
            else
            {
                g.LrmImageCount = 0;
            }

            #region 原转换任免表方法 已经被简化，此段代码废弃。
            //if ((g.Local_SourceLrmFullpath != null && Path.GetExtension(g.Local_SourceLrmFullpath) == ".lrm") && (g.Local_SourceLrmFullpath.Trim().Length > 0 && g.Local_SourcePicFullpath.Trim().Length > 0))
            //{
            //    try
            //    {
            //        g.Lrm_Guid = Guid.NewGuid().ToString();
            //        g.LrmImageCount = 1;

            //        //g.LrmImageFilename = string.Format("{0}_01_{1}.{2}", g.XM, g.Lrm_Guid, Global.ImgFormat.ToString().ToLower())
            //        //    .Replace(" ", "").Replace("　", "")
            //        //    .Replace("\r", "").Replace("\n", "")
            //        //    .Replace("\v", "").Replace("\f", "").Replace("\t", "");

            //        //g.Local_StorgeLrmFullpath = Global.ProjectLrmPicDirectory + g.XM + ".lrm";
            //        //g.Local_StorgePicFullpath = Global.ProjectLrmPicDirectory + g.XM + ".pic";
            //        //g.Local_StorgeDocumentWordFullpath = Global.ProjectTempWordDirectory + g.XM + ".docx";
            //        //g.Local_StorgeLrmPdfFullpath = Global.ProjectTempPDFDirectory + g.XM + ".pdf";
            //        //g.LRMIMG = Global.ProjectOutputImgDirectory + g.LrmImageFilename;

            //        File.Copy(g.Local_SourceLrmFullpath, g.Local_StorgeLrmFullpath, true);
            //        File.Copy(g.Local_SourcePicFullpath, g.Local_StorgePicFullpath, true);
            //        g = ConvertLrmToWordSaveAsXPS(Global.LrmToWordModelPath, g, Convert.ToDouble((CountDate).Date.ToString("yyyy.MM")));
            //        List<Bitmap> bmps = ConvertLrmXPSToBitmapList(g.Local_StorgeLrmPdfFullpath);
            //        bmps[0] = CutBottomBlankPart(bmps[0]);
            //        bmps[0].Save(g.LrmImageFileFullPaths[0]);

            //        //bmps[0] = CutImage(bmps[0], 10);
            //        //bmps[1] = CutImage(bmps[1], 11);
            //        //bmps[2] = CutImage(bmps[2], 12);
            //        //bmps[2] = CutImage(bmps[2], 22);

            //        //Bitmap bmp = JoinBitmap(bmps);
            //        //bmp.Save(g.LrmImageFileFullPaths[0]);  //g.LrmImageCount = 1 第一个图像为存储图像
            //        //bmp.Dispose();
            //        bmps[0].Dispose();
            //        bmps.Clear();

            //    }
            //    catch (Exception ex)
            //    {
            //        ShowMessage(string.Format("[转换干部任免审批表失败:{0}]，{1}", g.Local_SourceLrmFullpath, ex.Message), MessageType.Error);
            //    }
            //}
            //else if (g.Local_SourceLrmFullpath != null && (Path.GetExtension(g.Local_SourceLrmFullpath) == ".lrmx") && (g.Local_SourceLrmFullpath.Trim().Length > 0))
            //{
            //    try
            //    {
            //        g.Lrm_Guid = Guid.NewGuid().ToString();
            //        g.LrmImageCount = 1;
            //        //g.LrmImageFilename = string.Format("{0}_01_{1}.{2}", g.XM, g.Lrm_Guid, Global.ImgFormat.ToString().ToLower())
            //        //    .Replace(" ", "").Replace("　", "")
            //        //    .Replace("\r", "").Replace("\n", "")
            //        //    .Replace("\v", "").Replace("\f", "").Replace("\t", "");

            //        //g.Local_StorgeLrmFullpath = Global.ProjectLrmPicDirectory + g.XM + ".lrmx";
            //        //g.Local_StorgePicFullpath = "";
            //        //g.Local_StorgeDocumentWordFullpath = Global.ProjectTempWordDirectory + g.XM + ".docx";
            //        //g.Local_StorgeLrmPdfFullpath = Global.ProjectTempPDFDirectory + g.XM + ".pdf";
            //        //g.LRMIMG = Global.ProjectOutputImgDirectory + g.LrmImageFilename;// 这个属性可以删除 不再予以保留了

            //        File.Copy(g.Local_SourceLrmFullpath, g.Local_StorgeLrmFullpath, true);
            //        g = ConvertLrmToWordSaveAsXPS(Global.LrmToWordModelPath, g, Convert.ToDouble((CountDate).Date.ToString("yyyy.MM")));
            //        List<Bitmap> bmps = ConvertLrmXPSToBitmapList(g.Local_StorgeLrmPdfFullpath);
            //        bmps[0] = CutBottomBlankPart(bmps[0]);
            //        bmps[0].Save(g.LrmImageFileFullPaths[0]);

            //        //bmps[0] = CutImage(bmps[0], 10);
            //        //bmps[1] = CutImage(bmps[1], 11);
            //        //bmps[2] = CutImage(bmps[2], 12);
            //        //bmps[2] = CutImage(bmps[2], 22);

            //        //Bitmap bmp = JoinBitmap(bmps);
            //        //bmp.Save(g.LrmImageFileFullPaths[0]);  //g.LrmImageCount = 1 第一个图像为存储图像
            //        //bmp.Dispose();
            //        bmps[0].Dispose();
            //        bmps.Clear();
            //    }
            //    catch (Exception ex)
            //    {
            //        ShowMessage(string.Format("[转换干部任免审批表失败:{0}]，{1}", g.Local_SourceLrmFullpath, ex.Message), MessageType.Error);
            //    }
            //}
            //else
            //{
            //    g.LrmImageCount = 0;
            //}
            #endregion

            //转换考察材料
            if (g.Local_SourceResFullpath != null && g.Local_SourceResFullpath.Trim().Length > 0)
            {
                try
                {
                    g.Res_Guid = Guid.NewGuid().ToString();
                    //g.ResImageFilename = string.Format("{0}_02_{1}.{2}", g.XM, g.Res_Guid, Global.ImgFormat.ToString().ToLower())
                    //    .Replace(" ", "").Replace("　", "")
                    //    .Replace("\r", "").Replace("\n", "")
                    //    .Replace("\v", "").Replace("\f", "").Replace("\t", "");

                    //string ex = Path.GetExtension(g.Local_SourceResFullpath);
                    //g.Local_StorgeResFullpath = Global.ProjectResDirectory + g.XM + ex;
                    //g.Local_StorgeResPdfFullPath = Global.ProjectTempPDFDirectory + g.XM + "同志的考察材料.pdf";

                    //20170216 合并图像不再存储 g.RESIMG = Global.ProjectOutputImgDirectory + g.ResImageFilename 该属性废除
                    //这个属性可以删除了 转为存储不含文件扩展名的文件名

                    File.Copy(g.Local_SourceResFullpath, g.Local_StorgeResFullpath, true);
                    FormatResDocumentSaveAsPDF(g);
                    List<Bitmap> bmps = ConvertXPSToBitmapList(g.Local_StorgeResPdfFullPath, new List<int>());

                    //20170216 合并图像不再存储、分图像的本地存储路径通过ResImageFileFullPaths属性获取
                    //g.ResImageCount = bmps.Count;
                    //Bitmap bmp = JoinBitmap(bmps);
                    //bmp = CutImage(bmp, 22);
                    //bmp.Save(g.RESIMG);
                    //bmp.Dispose();

                    g.ResImageCount = bmps.Count;
                    for (int i = 0; i < g.ResImageCount; i++)
                    {
                        //20170216 代码结构优化
                        Bitmap resBitmap = bmps[i];
                        resBitmap = CutBottomBlankPart(resBitmap);
                        resBitmap.Save(g.ResImageFileFullPaths[i]);
                        resBitmap.Dispose();
                    }
                    bmps.Clear();
                }
                catch (Exception ex)
                {
                    ShowMessage(string.Format("[转换干部考察材料失败:{0}]，{1}", g.Local_SourceResFullpath, ex.Message), MessageType.Error);
                }
            }
            else
            {
                g.ResImageCount = 0;
            }

            //转换其他文档
            if (g.Local_SourceOtherFullpath != null && (g.Local_SourceOtherFullpath + string.Empty).Trim().Length > 0 && File.Exists(g.Local_SourceOtherFullpath))
            {
                try
                {
                    g.Other_Guid = Guid.NewGuid().ToString();
                    //g.OtherImageFilename = string.Format("{0}_03_{1}.{2}", g.XM, g.Res_Guid, Global.ImgFormat.ToString().ToLower())
                    //    .Replace(" ", "").Replace("　", "")
                    //    .Replace("\r", "").Replace("\n", "")
                    //    .Replace("\v", "").Replace("\f", "").Replace("\t", "");
                    File.Copy(g.Local_SourceOtherFullpath, g.Local_StorgeOtherFullpath, true);
                    List<Bitmap> bmps = ConvertXPSToBitmapList(g.Local_StorgeOtherFullpath, new List<int>());

                    g.OtherImageCount = bmps.Count;
                    for (int i = 0; i < g.OtherImageCount; i++)
                    {
                        Bitmap otherBitmap = bmps[i];
                        otherBitmap = CutBottomBlankPart(otherBitmap);
                        otherBitmap.Save(g.OtherImageFileFullPaths[i]);
                        otherBitmap.Dispose();
                    }
                    bmps.Clear();

                }
                catch (Exception ex)
                {
                    ShowMessage(string.Format("[转换干部其他材料失败:{0}]，{1}", g.Local_StorgeOtherFullpath, ex.Message), MessageType.Error);

                }
            }
            else
            {
                g.OtherImageCount = 0;
            }

            try
            {
                DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
                dr.EditGB(g);
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("[存储GB记录失败]，{0}", ex.Message), MessageType.Error);
            }
            convertHelper.UpdateDataId();
            GC.Collect();
            return g;
        }
        public void ConvertProjectToPdfForStorge()
        {
            #region 转换前关闭其他Word进程
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
                            ShowMessage(string.Format("关闭进程（{0}）错误：{1}", pc.ProcessName, ep.Message),MessageType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("关闭进错误：{1}", ex.Message), MessageType.Error);
            }
            #endregion

            DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
            List<DW> convert_dws = dr.GetAllDWs();
            foreach(DW convert_d in convert_dws)
            {
                List<string> convertPdfFullPath = new List<string>();

                ConvertWordToPDF(convert_d.Local_SourceDocumnetFullpath, convert_d.Local_SaveDocumentPdfForCombineFullpath);
                //convertPdfFullPath.Add(convert_d.Local_SaveDocumentPdfForCombineFullpath);
                foreach(GB g in convert_d.GBS)
                {
                    if (File.Exists(g.Local_SourceLrmFullpath))
                    {
                        try
                        {
                            ConvertLrmToWordForPDF(Global.LrmToPDFModelPath, g, Convert.ToDouble(CountDate.ToString("yyyy.MM")), (CountDate.Date));
                            convertPdfFullPath.Add(g.Local_StorgeLrmPdfFullpath);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(string.Format("转换干部任免审批表失败：{0}", ex.Message), MessageType.Error);
                        }
                    }
                    
                    //转换考察材料到PDF
                    if (File.Exists(g.Local_SourceResFullpath))
                    {
                        try
                        {
                            ConvertWordToPDF(g.Local_SourceResFullpath, g.Local_StorgeResPdfFullPath);
                            convertPdfFullPath.Add(g.Local_StorgeResPdfFullPath);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(string.Format("转换干部考察材料失败：{0}", ex.Message), MessageType.Error);
                        }
                    }

                }
                iTextPdf.PdfReader blankreader;
                blankreader = new iTextPdf.PdfReader(Global.BlankPDFModelPath);
                iTextPdf.PdfReader reader;
                iText.Document document = new iText.Document();
                iTextPdf.PdfWriter writer = iTextPdf.PdfWriter.GetInstance(document, new FileStream(convert_d.Local_SaveDocumentPdfForCombineFullpath, FileMode.Create));
                document.Open();
                iTextPdf.PdfContentByte cb = writer.DirectContent;
                iTextPdf.PdfImportedPage newpage;
                for (int i = 0; i < convertPdfFullPath.ToArray().Length; i++)
                {
                    reader = new iTextPdf.PdfReader(convertPdfFullPath.ToArray()[i]);
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
                GC.Collect();
            }
            if (DialogResult.Yes == MessageBox.Show(string.Format("批量转换文档[{0}]完成{1}。\r\n\r\n是否关闭当前转换窗口？", WordDirectory, HasError ? "。\r\n（提示）转换过程中存在错误，详细情况查看错误日志" : ""), "转换完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Close();
                this.FatherForm.Show();
            }
        }
        public List<PersonWithFile> GetAllLrmPersonWithFile()
        {
            List<PersonWithFile> allLrms = new List<PersonWithFile>();
            LrmHelper lrmHelper = new LrmHelper();
            IQueryable<FileInformation> queryLrmFiles = this.convertHelper.AllFileInformatons
                .Where(f => (f.FileExtension == ".lrm")).AsQueryable();
            if (queryLrmFiles.Any())
            {
                foreach (FileInformation f in queryLrmFiles.AsEnumerable())
                {
                    PersonWithFile p = new PersonWithFile();
                    p.IsLrmx = false;
                    p.LrmFullPath = f.FileFullname;
                    p.LrmFilename = Path.GetFileName(f.FileFullname);
                    p.PicFullPath = Path.Combine(Path.GetDirectoryName(f.FileFullname), Path.GetFileNameWithoutExtension(f.FileFullname) + ".pic");
                    p.LrmxFullPath = "";
                    p.LrmxFilename = "";

                    Person pe = lrmHelper.GetPersonFromLrmFile(p.LrmFullPath);
                    var ParentType = typeof(Person);
                    var Properties = ParentType.GetProperties();
                    foreach (var Propertie in Properties)
                    {
                        if (Propertie.CanRead && Propertie.CanWrite)
                        {
                            Propertie.SetValue(p, Propertie.GetValue(pe, null), null);
                        }
                    }
                    allLrms.Add(p);
                }
            }
            IQueryable<FileInformation> queryLrmxFiles = this.convertHelper.AllFileInformatons.Where(f => (f.FileExtension == ".lrmx")).AsQueryable();
            if (queryLrmxFiles.Any())
            {
                foreach (FileInformation f in queryLrmxFiles.AsEnumerable())
                {
                    PersonWithFile p = new PersonWithFile();
                    p.IsLrmx = true;
                    p.LrmFullPath = "";
                    p.LrmFilename = "";
                    p.PicFullPath = "";
                    p.LrmxFullPath = f.FileFullname;
                    p.LrmxFilename = Path.GetFileName(f.FileFullname);

                    Person pe = lrmHelper.GetPersonFromLrmFile(p.LrmxFullPath);
                    var ParentType = typeof(Person);
                    var Properties = ParentType.GetProperties();
                    foreach (var Propertie in Properties)
                    {
                        if (Propertie.CanRead && Propertie.CanWrite)
                        {
                            Propertie.SetValue(p, Propertie.GetValue(pe, null), null);
                        }
                    }
                    allLrms.Add(p);
                }
            }
            return allLrms;
        }
        public List<DocumentWithFile> GetAllDocumentWithFile()
        {
            List<DocumentWithFile> allDocuments = new List<DocumentWithFile>();
            IQueryable<FileInformation> qf = this.convertHelper.AllFileInformatons.Where(f => (f.FileExtension == ".doc" || f.FileExtension == ".docx") && !f.FileFullname.Contains("$")).AsQueryable();
            if (qf.Any())
            {
                foreach (FileInformation f in qf.AsEnumerable())
                {
                    DocumentWithFile doc = new DocumentWithFile(f.FileFullname);
                    //20170223 鬼知道我为什么要提前读取文档内容
                    //获取内容太慢了，暂时不需要读取Content属性
                    //if (File.Exists(f.FileFullname))
                    //{
                    //    using (WordHelper wordHelper = new WordHelper(f.FileFullname, false))
                    //    {
                    //        Word._Document oDoc = wordHelper.oDoc;
                    //        Word._Application oWord = wordHelper.oWordApplication;
                    //        Word.Selection oSelection = wordHelper.oWordApplication.Selection;//定义光标移动的参数对象
                    //        doc.Content = "";
                    //        int iParagraphCount = wordHelper.oDoc.Paragraphs.Count;
                    //        for (int i = 1; i < iParagraphCount+1; i++)
                    //        {
                    //            doc.Content += (wordHelper.oDoc.Paragraphs[i].Range.Text + "\r");
                    //        }
                    //    }
                    //}
                    allDocuments.Add(doc);
                }
            }
            return allDocuments;
        }
        private void ConvertPadDataForm_Load(object sender, EventArgs e)
        {
            this.picLoading.Image = Properties.Resources.loading;
            this.convertHelper = new ConvertHelper(this.LrmDirectory, this.ResDirectory);
            this.AllLrmPersons = GetAllLrmPersonWithFile();
            this.AllDocContents = GetAllDocumentWithFile();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.FatherForm.Show();
        }

        //内部方法
        private GB ConvertLrmToWordSaveAsXPS(string ModelPath, GB g, double countdate)
        {
            using (WordHelper wordHelper = new WordHelper(ModelPath, Global.IsShowWord))
            {
                LrmHelper lrmReader = new LrmHelper();
                Person cadre_person = lrmReader.GetPersonFromLrmFile(g.Local_StorgeLrmFullpath);
                if ((g.XM + "").Trim().Length == 0)
                {
                    g.XM = cadre_person.XingMing;
                }

                Word.Table WordTable_1 = wordHelper.GetTable(1);

                if (cadre_person.XingMing.Length <= 10)
                {
                    WordTable_1.Cell(1, 2).Range.Text = cadre_person.XingMing;
                }
                else if (cadre_person.XingMing.Length <= 15)
                {
                    WordTable_1.Cell(1, 2).Range.Text = cadre_person.XingMing;
                    WordTable_1.Cell(1, 2).Range.Font.Size = 5;
                    WordTable_1.Cell(1, 2).Range.ParagraphFormat.LineSpacing = 5.5F;
                }
                else
                {
                    WordTable_1.Cell(1, 2).Range.Text = cadre_person.XingMing;
                    WordTable_1.Cell(1, 2).Range.Font.Size = (float)3.3;
                    WordTable_1.Cell(1, 2).Range.ParagraphFormat.LineSpacing = 3.5F;
                }

                WordTable_1.Cell(1, 4).Range.Text = cadre_person.XingBie;
                string strAge;
                try
                {
                    strAge = (Math.Floor(countdate - Convert.ToDouble(cadre_person.ChuShengNianYue))).ToString();
                }
                catch (Exception)
                {
                    strAge = "";
                }
                WordTable_1.Cell(1, 6).Range.Text = cadre_person.ChuShengNianYue.Trim().Length == 0 ? WordTable_1.Cell(1, 6).Range.Text : cadre_person.ChuShengNianYue + "\r\n（" + strAge + "岁）";

                object anchor = WordTable_1.Cell(1, 7).Range;
                string otpic = Path.Combine(Path.GetDirectoryName(g.Local_StorgeDocumentWordFullpath), cadre_person.XingMing + ".bmp");
                convertHelper.ZoomImageToFile(cadre_person.ZhaoPian_Image, otpic, 1);
                //((Bitmap)(cadre_person.ZhaoPian_Image)).Save(otpic);
                wordHelper.InsertImage(otpic, 0f, 0f, 56f, 69f, anchor);

                WordTable_1.Cell(2, 2).Range.Text = cadre_person.MinZu;


                if (cadre_person.XingMing.Length <= 10)
                {
                    WordTable_1.Cell(2, 4).Range.Text = cadre_person.JiGuan;
                }
                else if (cadre_person.XingMing.Length <= 15)
                {
                    WordTable_1.Cell(2, 4).Range.Text = cadre_person.JiGuan;
                    WordTable_1.Cell(2, 4).Range.Font.Size = 5;
                    WordTable_1.Cell(2, 4).Range.ParagraphFormat.LineSpacing = 5.5F;
                }
                else
                {
                    WordTable_1.Cell(2, 4).Range.Text = cadre_person.JiGuan;
                    WordTable_1.Cell(2, 4).Range.Font.Size = (float)3.3;
                    WordTable_1.Cell(2, 4).Range.ParagraphFormat.LineSpacing = 3.5F;
                }



                if (cadre_person.XingMing.Length <= 10)
                {
                    WordTable_1.Cell(2, 6).Range.Text = cadre_person.ChuShengDi;
                }
                else if (cadre_person.XingMing.Length <= 15)
                {
                    WordTable_1.Cell(2, 6).Range.Text = cadre_person.ChuShengDi;
                    WordTable_1.Cell(2, 6).Range.Font.Size = 5;
                    WordTable_1.Cell(2, 6).Range.ParagraphFormat.LineSpacing = 5.5F;
                }
                else
                {
                    WordTable_1.Cell(2, 6).Range.Text = cadre_person.ChuShengDi;
                    WordTable_1.Cell(2, 6).Range.Font.Size = (float)3.3;
                    WordTable_1.Cell(2, 6).Range.ParagraphFormat.LineSpacing = 3.5F;
                }


                WordTable_1.Cell(3, 2).Range.Text = cadre_person.RuDangShiJian;
                WordTable_1.Cell(3, 4).Range.Text = cadre_person.CanJiaGongZuoShiJian;
                WordTable_1.Cell(3, 6).Range.Text = cadre_person.JianKangZhuangKuang;

                WordTable_1.Cell(4, 2).Range.Text = cadre_person.ZhuanYeJiShuZhiWu;
                WordTable_1.Cell(4, 4).Range.Text = cadre_person.ShuXiZhuanYeYouHeZhuanChang;

                #region 全日制学历和学位
                if (cadre_person.QuanRiZhiJiaoYu_XueLi.Trim().Length * cadre_person.QuanRiZhiJiaoYu_XueWei.Trim().Length == 0)
                {
                    WordTable_1.Cell(5, 3).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi + cadre_person.QuanRiZhiJiaoYu_XueWei;
                    int length = cadre_person.QuanRiZhiJiaoYu_XueLi.Length + cadre_person.QuanRiZhiJiaoYu_XueWei.Length;
                    if (length <= 16)
                    {
                        WordTable_1.Cell(5, 3).Range.Font.Size = 7;
                        WordTable_1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                    }
                    else if (length <= 18)
                    {
                        WordTable_1.Cell(5, 3).Range.Font.Size = 6;
                        WordTable_1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if (length <= 33)
                    {
                        WordTable_1.Cell(5, 3).Range.Font.Size = 5;
                        WordTable_1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(5, 3).Range.Font.Size = 4;
                        WordTable_1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 4;//磅值
                    }
                }
                else
                {
                    WordTable_1.Cell(5, 3).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi + "\r\n" + cadre_person.QuanRiZhiJiaoYu_XueWei;
                    int length1 = cadre_person.QuanRiZhiJiaoYu_XueLi.Length;
                    int length2 = cadre_person.QuanRiZhiJiaoYu_XueWei.Length;
                    if (length1 <= 8 && length2 <= 8)
                    {
                        WordTable_1.Cell(5, 3).Range.Font.Size = 7;
                        WordTable_1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                    }
                    else if (length1 <= 9 && length2 <= 9)
                    {
                        WordTable_1.Cell(5, 3).Range.Font.Size = 6;
                        WordTable_1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if ((length1 <= 22 && length2 <= 11) || (length1 <= 11 && length2 <= 22))
                    {
                        WordTable_1.Cell(5, 3).Range.Font.Size = 5;
                        WordTable_1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(5, 3).Range.Font.Size = 4;
                        WordTable_1.Cell(5, 3).Range.Paragraphs.Format.LineSpacing = 4;//磅值
                    }
                }
                #endregion
                #region 全日制院校和专业
                if (cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length * cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim().Length == 0)
                {
                    WordTable_1.Cell(5, 5).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim() + cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim();
                    int length = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length + cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim().Length;
                    if (length <= 22)
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 7;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                    }
                    else if (length <= 24)
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 6;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if (length <= 45)
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 5;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 4;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 4;//磅值
                    }
                }
                else if (cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim() == cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim())//院校和专业相等的情况下只取一个
                {
                    WordTable_1.Cell(5, 5).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim();
                    int length = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length;
                    if (length <= 22)
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 7;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                    }
                    else if (length <= 24)
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 6;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if (length <= 45)
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 5;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 4;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 4;//磅值
                    }
                }
                else
                {
                    WordTable_1.Cell(5, 5).Range.Text = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi + "\r\n" + cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi;
                    int length1 = cadre_person.QuanRiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Length;
                    int length2 = cadre_person.QuanRiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Length;
                    if (length1 <= 11 && length2 <= 11)
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 7;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                    }
                    else if (length1 <= 12 && length2 <= 12)
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 6;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if ((length1 <= 30 && length2 <= 15) || (length1 <= 15 && length2 <= 30))
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 5;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(5, 5).Range.Font.Size = 4;
                        WordTable_1.Cell(5, 5).Range.Paragraphs.Format.LineSpacing = 4;//磅值
                    }
                }
                #endregion
                #region 在职学历和学位
                if (cadre_person.ZaiZhiJiaoYu_XueLi.Trim().Length * cadre_person.ZaiZhiJiaoYu_XueWei.Trim().Length == 0)
                {
                    WordTable_1.Cell(6, 3).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi + cadre_person.ZaiZhiJiaoYu_XueWei;
                    int length = cadre_person.ZaiZhiJiaoYu_XueLi.Length + cadre_person.ZaiZhiJiaoYu_XueWei.Length;
                    if (length <= 16)
                    {
                        WordTable_1.Cell(6, 3).Range.Font.Size = 7;
                        WordTable_1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                    }
                    else if (length <= 18)
                    {
                        WordTable_1.Cell(6, 3).Range.Font.Size = 6;
                        WordTable_1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if (length <= 33)
                    {
                        WordTable_1.Cell(6, 3).Range.Font.Size = 5;
                        WordTable_1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(6, 3).Range.Font.Size = 4;
                        WordTable_1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 84;//磅值
                    }
                }
                else
                {
                    WordTable_1.Cell(6, 3).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi + "\r\n" + cadre_person.ZaiZhiJiaoYu_XueWei;
                    int length1 = cadre_person.ZaiZhiJiaoYu_XueLi.Length;
                    int length2 = cadre_person.ZaiZhiJiaoYu_XueWei.Length;
                    if (length1 <= 8 && length2 <= 8)
                    {
                        WordTable_1.Cell(6, 3).Range.Font.Size = 7;
                        WordTable_1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                    }
                    else if (length1 <= 9 && length2 <= 9)
                    {
                        WordTable_1.Cell(6, 3).Range.Font.Size = 6;
                        WordTable_1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if ((length1 <= 22 && length2 <= 11) || (length1 <= 11 && length2 <= 22))
                    {
                        WordTable_1.Cell(6, 3).Range.Font.Size = 5;
                        WordTable_1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(6, 3).Range.Font.Size = 4;
                        WordTable_1.Cell(6, 3).Range.Paragraphs.Format.LineSpacing = 4;//磅值
                    }
                }
                #endregion
                #region 在职院校和专业
                if (cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length * cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim().Length == 0)
                {
                    WordTable_1.Cell(6, 5).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi + cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi;
                    int length = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Length + cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Length;
                    if (length <= 22)
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 7;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                    }
                    else if (length <= 24)
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 6;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if (length <= 45)
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 5;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 4;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 4;//磅值
                    }
                }
                else if (cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length == cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Trim().Length)
                {
                    WordTable_1.Cell(6, 5).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim();
                    int length = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Trim().Length;
                    if (length <= 22)
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 7;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 8.6F;//磅值
                    }
                    else if (length <= 24)
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 6;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if (length <= 45)
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 5;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 4;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 4;//磅值
                    }
                }
                else
                {
                    WordTable_1.Cell(6, 5).Range.Text = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi + "\r\n" + cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi;
                    int length1 = cadre_person.ZaiZhiJiaoYu_XueLi_BiYeYuanXiaoXi.Length;
                    int length2 = cadre_person.ZaiZhiJiaoYu_XueWei_BiYeYuanXiaoXi.Length;
                    if (length1 <= 11 && length2 <= 11)
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 7;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                    }
                    else if (length1 <= 12 && length2 <= 12)
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 6;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                    }
                    else if ((length1 <= 30 && length2 <= 15) || (length1 <= 15 && length2 <= 30))
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 5;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 5.5F;//磅值
                    }
                    else
                    {
                        WordTable_1.Cell(6, 5).Range.Font.Size = 4;
                        WordTable_1.Cell(6, 5).Range.Paragraphs.Format.LineSpacing = 4;//磅值
                    }
                }
                #endregion

                WordTable_1.Cell(7, 2).Range.Text = cadre_person.XianRenZhiWu;
                WordTable_1.Cell(8, 2).Range.Text = cadre_person.NiRenZhiWu;
                WordTable_1.Cell(9, 2).Range.Text = cadre_person.NiMianZhiWu;

                //Word.Table WordTabel_2 = wordHelper.GetTable(2);

                string[] jianli_array = cadre_person.JianLi.Split('\n');
                double line_count_20 = 0;
                double line_count_25 = 0;
                double line_count_31 = 0;
                foreach (string j in jianli_array)
                {
                    if (j.Length < 18)
                    {
                        line_count_20 += 1;
                        line_count_25 += 1;
                        line_count_31 += 1;
                    }
                    else
                    {
                        line_count_20 += Math.Ceiling((j.Length - 18) / 20.0);
                        line_count_25 += Math.Ceiling((j.Length - 18) / 25.0);
                        line_count_31 += Math.Ceiling((j.Length - 18) / 31.0);
                    }
                }

                if (line_count_20 < 30)
                {
                    WordTable_1.Cell(10, 2).Range.Font.Size = 7;
                    WordTable_1.Cell(10, 2).Range.Paragraphs.Format.LineSpacing = 8.5F;//磅值
                }
                else if (line_count_25 < 45)
                {
                    WordTable_1.Cell(10, 2).Range.Font.Size = 6;
                    WordTable_1.Cell(10, 2).Range.Paragraphs.Format.LineSpacing = 7.5F;//磅值
                }
                else if (line_count_31 < 52)
                {
                    WordTable_1.Cell(10, 2).Range.Font.Size = 5;
                    WordTable_1.Cell(10, 2).Range.Paragraphs.Format.LineSpacing = 6.5F;//磅值
                }
                else
                {
                    WordTable_1.Cell(10, 2).Range.Font.Size = 4;
                    WordTable_1.Cell(10, 2).Range.Paragraphs.Format.LineSpacing = 5F;//磅值
                }
                WordTable_1.Cell(10, 2).Range.Text = cadre_person.JianLi;

                //Word.Table WordTable_1 = wordHelper.GetTable(3);
                WordTable_1.Cell(11, 2).Range.Text = cadre_person.JiangChengQingKuang;
                WordTable_1.Cell(12, 2).Range.Text = cadre_person.NianDuKaoHeJieGuo;
                WordTable_1.Cell(13, 2).Range.Text = cadre_person.RenMianLiYou;

                Regex r = new Regex(@"^\d{4}\.\d{2,4}$");

                for (int i = 0; i < 6; i++)
                {
                    if (cadre_person.JiaTingChengYuan.Count > i)
                    {

                        WordTable_1.Cell(i + 15, 2).Range.Text = cadre_person.JiaTingChengYuan[i].ChengWei;
                        WordTable_1.Cell(i + 15, 3).Range.Text = cadre_person.JiaTingChengYuan[i].XingMing;
                        WordTable_1.Cell(i + 15, 4).Range.Text = r.Match(cadre_person.JiaTingChengYuan[i].ChuShengRiQi.Trim()).Success
                            && convertHelper.RelationIsAlive(cadre_person.JiaTingChengYuan[i].GongZuoDanWeiJiZhiWu)
                            ? (Math.Floor(countdate - Convert.ToDouble(cadre_person.JiaTingChengYuan[i].ChuShengRiQi))).ToString()
                            : "";
                        WordTable_1.Cell(i + 15, 5).Range.Text = cadre_person.JiaTingChengYuan[i].ZhengZhiMianMao.Replace("\r", "");
                        WordTable_1.Cell(i + 15, 6).Range.Text = cadre_person.JiaTingChengYuan[i].GongZuoDanWeiJiZhiWu;
                    }
                }


                wordHelper.SaveDocumentAs(g.Local_StorgeDocumentWordFullpath);
                wordHelper.SaveDocumentAsXPS(g.Local_StorgeLrmPdfFullpath);
                //g.XSD = 1;
                //我这是疯了么，这么多Person对象，就这样仍在内存里！！！！
                if (cadre_person != null)
                {
                    cadre_person.Dispose();
                }
            }
            return g;
        }
        private void FormatResDocumentSaveAsPDF(GB g)
        {
            using (WordHelper wordHelper = new WordHelper(g.Local_StorgeResFullpath, Global.IsShowWord))
            {
                Word.Selection oSelection = wordHelper.oWordApplication.Selection;
                Word._Document oDoc = wordHelper.oDoc;
                Word._Application oWord = wordHelper.oWordApplication;
                //定义光标移动的参数对象
                object oUnitParagraph = Word.WdUnits.wdParagraph;
                object oCount = 1;
                object oExtend = Word.WdMovementType.wdExtend;//移动并选择
                object oExtendNone = Type.Missing;                     //只移动不选择

                //添加切白分割线
                oDoc.Paragraphs[wordHelper.oDoc.Paragraphs.Count].Range.Select();   //移到文章最末末端
                int LastPageNumber = oSelection.get_Information(Word.WdInformation.wdActiveEndAdjustedPageNumber);

                oSelection.MoveDown(ref oUnitParagraph, ref oCount, ref oExtendNone);
                oDoc.Paragraphs.Add();
                oSelection.MoveDown(ref oUnitParagraph, ref oCount, ref oExtendNone);
                oDoc.Paragraphs.Add();
                oSelection.MoveDown(ref oUnitParagraph, ref oCount, ref oExtendNone);
                Word.Range r = oSelection.Range;
                oDoc.Tables.Add(r, 1, 1);
                if (LastPageNumber == oSelection.get_Information(Word.WdInformation.wdActiveEndAdjustedPageNumber))
                {
                    //添加切除白边分割线可能导致页数增多，此处需要重新获取页码总数
                    //只有当添加页面分割线后不导致页数增多时，才添加绿色
                    Word.Table t = oDoc.Tables[oDoc.Tables.Count];
                    t.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleNone;
                    t.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleNone;
                    t.Shading.BackgroundPatternColor = Word.WdColor.wdColorGreen;
                    t.Shading.ForegroundPatternColor = Word.WdColor.wdColorGreen;
                }
                else
                {
                    //撤销添加段落
                    oDoc.Tables[oDoc.Tables.Count].Delete();
                    oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.Delete();
                    oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.Delete();
                }

                wordHelper.SaveDocumentAs(g.Local_StorgeResFullpath);
                wordHelper.SaveDocumentAsXPS(g.Local_StorgeResPdfFullPath);
            }

        }

        private List<Bitmap> ConvertPdfToBitmapList(string pdfPath)
        {
            return ConvertPdfToBitmapList(pdfPath, new List<int>());
        }
        private List<Bitmap> ConvertPdfToBitmapList(string pdfPath, List<int> ConvertPageIndexes)
        {
            List<Bitmap> DocumentPdfPageImages = new List<Bitmap>();
            Acrobat.CAcroPDDoc pdfDoc = null;
            Acrobat.CAcroPDPage pdfPage = null;
            Acrobat.CAcroRect pdfRect = null;
            Acrobat.CAcroPoint pdfPoint = null;
            try
            {
                pdfDoc = (Acrobat.CAcroPDDoc)Microsoft.VisualBasic.Interaction.CreateObject("AcroExch.PDDoc", "");
                if (!pdfDoc.Open(pdfPath))
                {
                    throw new ArgumentException(string.Format("PDF文件不存在:{0}", pdfPath));
                }

                if (Global.ImgFormat == null)
                {
                    Global.ImgFormat = ImageFormat.Png;
                }
                if (Global.ImgWidth <= 0)
                {
                    Global.ImgWidth = 1600;
                }
                if (Global.HorizontalCutSize < 0)
                {
                    Global.HorizontalCutSize = 0;
                }

                if (ConvertPageIndexes.Count == 0)
                {
                    int iBeginPageNumber = 1;
                    int iEndPageNumber = pdfDoc.GetNumPages();

                    //转换
                    for (int i = iBeginPageNumber; i <= iEndPageNumber; i++)
                    {
                        //2)
                        //取出当前页
                        pdfPage = (Acrobat.CAcroPDPage)pdfDoc.AcquirePage(i - 1);

                        //3)
                        //得到当前页的大小
                        pdfPoint = (Acrobat.CAcroPoint)pdfPage.GetSize();
                        //生成一个页的裁剪区矩形对象
                        pdfRect = (Acrobat.CAcroRect)Microsoft.VisualBasic.Interaction.CreateObject("AcroExch.Rect", "");

                        //计算当前页经缩放后的实际宽度和高度,zoom==1时，保持原比例大小
                        //将裁剪大小换算为像素大小
                        double dobleZoom = (double)Global.ImgWidth * 210 / (210 - Global.HorizontalCutSize * 2) / 595;
                        int imgWidth = (int)((double)pdfPoint.x * dobleZoom);
                        int imgHeight = (int)((double)pdfPoint.y * dobleZoom);

                        //设置裁剪矩形的大小为当前页的大小
                        pdfRect.Left = 0;
                        pdfRect.right = (short)imgWidth;
                        pdfRect.Top = 0;
                        pdfRect.bottom = (short)imgHeight;

                        //4)
                        //将当前页的裁剪区的内容编成图片后复制到剪贴板中
                        pdfPage.CopyToClipboard(pdfRect, 0, 0, (short)(100 * dobleZoom));

                        //5)
                        IDataObject clipboardData = Clipboard.GetDataObject();

                        //检查剪贴板中的对象是否是图片，如果是图片则将其保存为指定格式的图片文件
                        if (clipboardData.GetDataPresent(DataFormats.Bitmap))
                        {
                            Bitmap pdfBitmap = (Bitmap)clipboardData.GetData(DataFormats.Bitmap);

                            //按比例切除四周白边
                            int VerticalCutPixel = Convert.ToInt32(pdfBitmap.Height * Global.VerticalCutSize / 297);
                            int HorizontalCutPixel = Convert.ToInt32(pdfBitmap.Width * Global.HorizontalCutSize / 210);
                            Bitmap pdfCutBitmap = CutRoundBitmap(pdfBitmap, VerticalCutPixel, HorizontalCutPixel);
                            DocumentPdfPageImages.Add(pdfCutBitmap);
                            pdfBitmap.Dispose();
                            Clipboard.Clear();
                            //pdfCutBitmap.Dispose();
                        }
                    }
                }
                else
                {
                    foreach (int pageIndex in ConvertPageIndexes)
                    {
                        //2)
                        //取出当前页
                        pdfPage = (Acrobat.CAcroPDPage)pdfDoc.AcquirePage(pageIndex);

                        //3)
                        //得到当前页的大小
                        pdfPoint = (Acrobat.CAcroPoint)pdfPage.GetSize();
                        //生成一个页的裁剪区矩形对象
                        pdfRect = (Acrobat.CAcroRect)Microsoft.VisualBasic.Interaction.CreateObject("AcroExch.Rect", "");

                        //计算当前页经缩放后的实际宽度和高度,zoom==1时，保持原比例大小
                        //将裁剪大小换算为像素大小
                        double dobleZoom = (double)Global.ImgWidth * 210 / (210 - Global.HorizontalCutSize * 2) / 595;
                        int imgWidth = (int)((double)pdfPoint.x * dobleZoom);
                        int imgHeight = (int)((double)pdfPoint.y * dobleZoom);

                        //设置裁剪矩形的大小为当前页的大小
                        pdfRect.Left = 0;
                        pdfRect.right = (short)imgWidth;
                        pdfRect.Top = 0;
                        pdfRect.bottom = (short)imgHeight;

                        //4)
                        //将当前页的裁剪区的内容编成图片后复制到剪贴板中
                        pdfPage.CopyToClipboard(pdfRect, 0, 0, (short)(100 * dobleZoom));

                        //5)
                        IDataObject clipboardData = Clipboard.GetDataObject();

                        //检查剪贴板中的对象是否是图片，如果是图片则将其保存为指定格式的图片文件
                        if (clipboardData.GetDataPresent(DataFormats.Bitmap))
                        {
                            Bitmap pdfBitmap = (Bitmap)clipboardData.GetData(DataFormats.Bitmap);

                            //按比例切除四周白边
                            int VerticalCutPixel = Convert.ToInt32(pdfBitmap.Height * Global.VerticalCutSize / 297);
                            int HorizontalCutPixel = Convert.ToInt32(pdfBitmap.Width * Global.HorizontalCutSize / 210);
                            Bitmap pdfCutBitmap = CutRoundBitmap(pdfBitmap, VerticalCutPixel, HorizontalCutPixel);
                            DocumentPdfPageImages.Add(pdfCutBitmap);
                            pdfBitmap.Dispose();
                            Clipboard.Clear();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //关闭和释放相关COM对象
                pdfDoc.Close();
                if (pdfRect != null)
                    Marshal.ReleaseComObject(pdfRect);
                if (pdfPoint != null)
                    Marshal.ReleaseComObject(pdfPoint);
                if (pdfPage != null)
                    Marshal.ReleaseComObject(pdfPage);
                if (pdfDoc != null)
                    Marshal.ReleaseComObject(pdfDoc);
                Clipboard.Clear();
                GC.Collect();
            }
            return DocumentPdfPageImages;
        }
        private List<Bitmap> ConvertLrmPdfToBitmapList(string pdfPath)
        {
            List<Bitmap> DocumentPdfPageImages = new List<Bitmap>();
            Acrobat.CAcroPDDoc pdfDoc = null;
            Acrobat.CAcroPDPage pdfPage = null;
            Acrobat.CAcroRect pdfRect = null;
            Acrobat.CAcroPoint pdfPoint = null;
            try
            {
                pdfDoc = (Acrobat.CAcroPDDoc)Microsoft.VisualBasic.Interaction.CreateObject("AcroExch.PDDoc", "");
                if (!pdfDoc.Open(pdfPath))
                {
                    throw new ArgumentException(string.Format("PDF文件不存在:{0}", pdfPath));
                }

                if (Global.ImgFormat == null)
                {
                    Global.ImgFormat = ImageFormat.Png;
                }
                if (Global.ImgWidth <= 0)
                {
                    Global.ImgWidth = 1600;
                }
                if (Global.HorizontalCutSize < 0)
                {
                    Global.HorizontalCutSize = 0;
                }


                int iBeginPageNumber = 1;
                int iEndPageNumber = pdfDoc.GetNumPages();

                //转换
                for (int i = iBeginPageNumber; i <= iEndPageNumber; i++)
                {
                    //2)
                    //取出当前页
                    pdfPage = (Acrobat.CAcroPDPage)pdfDoc.AcquirePage(i - 1);

                    //3)
                    //得到当前页的大小
                    pdfPoint = (Acrobat.CAcroPoint)pdfPage.GetSize();
                    //生成一个页的裁剪区矩形对象
                    pdfRect = (Acrobat.CAcroRect)Microsoft.VisualBasic.Interaction.CreateObject("AcroExch.Rect", "");

                    //计算当前页经缩放后的实际宽度和高度,zoom==1时，保持原比例大小
                    //将裁剪大小换算为像素大小
                    double dobleZoom = (double)Global.ImgWidth * 150 / (150 - Global.HorizontalCutSize * 2) / 425;
                    int imgWidth = (int)((double)pdfPoint.x * dobleZoom);
                    int imgHeight = (int)((double)pdfPoint.y * dobleZoom);

                    //设置裁剪矩形的大小为当前页的大小
                    pdfRect.Left = 0;
                    pdfRect.right = (short)imgWidth;
                    pdfRect.Top = 0;
                    pdfRect.bottom = (short)imgHeight;

                    //4)
                    //将当前页的裁剪区的内容编成图片后复制到剪贴板中
                    pdfPage.CopyToClipboard(pdfRect, 0, 0, (short)(100 * dobleZoom));

                    //5)
                    IDataObject clipboardData = Clipboard.GetDataObject();

                    //检查剪贴板中的对象是否是图片，如果是图片则将其保存为指定格式的图片文件
                    if (clipboardData.GetDataPresent(DataFormats.Bitmap))
                    {
                        Bitmap pdfBitmap = (Bitmap)clipboardData.GetData(DataFormats.Bitmap);

                        //按比例切除四周白边
                        int VerticalCutPixel = Convert.ToInt32(pdfBitmap.Height * Global.VerticalCutSize / 500);
                        int HorizontalCutPixel = Convert.ToInt32(pdfBitmap.Width * Global.HorizontalCutSize / 150);
                        Bitmap pdfCutBitmap = CutRoundBitmap(pdfBitmap, VerticalCutPixel, HorizontalCutPixel);
                        DocumentPdfPageImages.Add(pdfCutBitmap);
                        pdfBitmap.Dispose();
                        Clipboard.Clear();
                        //pdfCutBitmap.Dispose();
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //关闭和释放相关COM对象
                pdfDoc.Close();
                if (pdfRect != null)
                    Marshal.ReleaseComObject(pdfRect);
                if (pdfPoint != null)
                    Marshal.ReleaseComObject(pdfPoint);
                if (pdfPage != null)
                    Marshal.ReleaseComObject(pdfPage);
                if (pdfDoc != null)
                    Marshal.ReleaseComObject(pdfDoc);
                Clipboard.Clear();
                GC.Collect();
            }
            return DocumentPdfPageImages;
        }
        private List<Bitmap> ConvertXPSToBitmapList(string xpsPath, List<int> ConvertPageIndexes)
        {
            //页面初始图像集
            List<Bitmap> bmps = new List<Bitmap>();

            //XpsDocument是需要引用ReachFramework的
            using (XpsDocument xpsDoc = new XpsDocument(xpsPath, FileAccess.Read))
            {
                var fs = xpsDoc.GetFixedDocumentSequence();


                if (ConvertPageIndexes.Count == 0)
                {
                    //xps文件转换为图片
                    int pageall = fs.DocumentPaginator.PageCount;
                    for (int i = 0; i < pageall; i++)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        System.Windows.Media.Imaging.BitmapEncoder bitmapEncoder
                            = new System.Windows.Media.Imaging.PngBitmapEncoder();
                        DocumentPage documentPage = fs.DocumentPaginator.GetPage(i);
                        double dobleZoom = (double)Global.ImgWidth * 210 / (210 - Global.HorizontalCutSize * 2) / documentPage.Size.Width;
                        int imgWidth = (int)((double)documentPage.Size.Width * dobleZoom);
                        int imgHeight = (int)((double)documentPage.Size.Height * dobleZoom);
                        double imgDPI = (double)96.0 * dobleZoom;
                        System.Windows.Media.Imaging.RenderTargetBitmap targetBitmap
                            = new System.Windows.Media.Imaging.RenderTargetBitmap(imgWidth, imgHeight, imgDPI, imgDPI, System.Windows.Media.PixelFormats.Default);

                        targetBitmap.Render(documentPage.Visual);
                        bitmapEncoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(targetBitmap));
                        bitmapEncoder.Save(memoryStream);
                        Bitmap xpsPageBitmap = new Bitmap(memoryStream);

                        //裁切固定白边
                        int VerticalCutPixel = Convert.ToInt32(xpsPageBitmap.Height * Global.VerticalCutSize / 297);
                        int HorizontalCutPixel = Convert.ToInt32(xpsPageBitmap.Width * Global.HorizontalCutSize / 210);
                        Bitmap pdfCutBitmap = CutRoundBitmap(xpsPageBitmap, VerticalCutPixel, HorizontalCutPixel);

                        bmps.Add(pdfCutBitmap);
                        memoryStream.Dispose();
                        xpsPageBitmap.Dispose();
                    }
                }
                else
                {
                    foreach (int index in ConvertPageIndexes)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        System.Windows.Media.Imaging.BitmapEncoder bitmapEncoder
                            = new System.Windows.Media.Imaging.PngBitmapEncoder();
                        DocumentPage documentPage = fs.DocumentPaginator.GetPage(index);
                        double dobleZoom = (double)Global.ImgWidth * 210 / (210 - Global.HorizontalCutSize * 2) / documentPage.Size.Width;
                        int imgWidth = (int)((double)documentPage.Size.Width * dobleZoom);
                        int imgHeight = (int)((double)documentPage.Size.Height * dobleZoom);
                        double imgDPI = (double)96.0 * dobleZoom;
                        System.Windows.Media.Imaging.RenderTargetBitmap targetBitmap
                            = new System.Windows.Media.Imaging.RenderTargetBitmap(imgWidth, imgHeight, imgDPI, imgDPI, System.Windows.Media.PixelFormats.Default);

                        //System.Windows.Media.Imaging.RenderTargetBitmap targetBitmap
                        //    = new System.Windows.Media.Imaging.RenderTargetBitmap((int)documentPage.Size.Width, (int)documentPage.Size.Height, 96.0 , 96.0 , System.Windows.Media.PixelFormats.Pbgra32);

                        targetBitmap.Render(documentPage.Visual);
                        bitmapEncoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(targetBitmap));
                        bitmapEncoder.Save(memoryStream);
                        Bitmap xpsPageBitmap = new Bitmap(memoryStream);

                        //裁切固定白边
                        int VerticalCutPixel = Convert.ToInt32(xpsPageBitmap.Height * Global.VerticalCutSize / 297);
                        int HorizontalCutPixel = Convert.ToInt32(xpsPageBitmap.Width * Global.HorizontalCutSize / 210);
                        Bitmap pdfCutBitmap = CutRoundBitmap(xpsPageBitmap, VerticalCutPixel, HorizontalCutPixel);

                        bmps.Add(pdfCutBitmap);
                        memoryStream.Dispose();
                        xpsPageBitmap.Dispose();
                    }
                }
            }
            return bmps;
        }
        private List<Bitmap> ConvertLrmXPSToBitmapList(string xpsPath)
        {
            //页面初始图像集
            List<Bitmap> bmps = new List<Bitmap>();

            //XpsDocument是需要引用ReachFramework的
            using (XpsDocument xpsDoc = new XpsDocument(xpsPath, FileAccess.Read))
            {
                var fs = xpsDoc.GetFixedDocumentSequence();

                //xps文件转换为图片
                int pageall = fs.DocumentPaginator.PageCount;
                for (int i = 0; i < pageall; i++)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    System.Windows.Media.Imaging.BitmapEncoder bitmapEncoder
                        = new System.Windows.Media.Imaging.PngBitmapEncoder();
                    DocumentPage documentPage = fs.DocumentPaginator.GetPage(i);
                    double dobleZoom = (double)Global.ImgWidth * 140 / (140 - (Global.HorizontalCutSize) * 2) / documentPage.Size.Width;
                    int imgWidth = (int)((double)documentPage.Size.Width * dobleZoom);
                    int imgHeight = (int)((double)documentPage.Size.Height * dobleZoom);
                    double imgDPI = (double)96.0 * dobleZoom;
                    System.Windows.Media.Imaging.RenderTargetBitmap targetBitmap
                        = new System.Windows.Media.Imaging.RenderTargetBitmap(imgWidth, imgHeight, imgDPI, imgDPI, System.Windows.Media.PixelFormats.Default);

                    targetBitmap.Render(documentPage.Visual);
                    bitmapEncoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(targetBitmap));
                    bitmapEncoder.Save(memoryStream);
                    Bitmap xpsPageBitmap = new Bitmap(memoryStream);

                    //裁切固定白边
                    int VerticalCutPixel = Convert.ToInt32(xpsPageBitmap.Height * Global.VerticalCutSize / 500);
                    int HorizontalCutPixel = Convert.ToInt32(xpsPageBitmap.Width * (Global.HorizontalCutSize) / 140);
                    Bitmap pdfCutBitmap = CutRoundBitmap(xpsPageBitmap, VerticalCutPixel, HorizontalCutPixel);

                    bmps.Add(pdfCutBitmap);
                    memoryStream.Dispose();
                    xpsPageBitmap.Dispose();
                }

            }
            return bmps;
        }
        private Bitmap CutBottomBlankPart(Bitmap sbmp)
        {
            Color[] p = new Color[4];
            int w = sbmp.Width;
            int h = sbmp.Height;
            Bitmap bmpDest = sbmp;
            while (h > 4)
            {
                for (int x = 0; x < 4; x++)
                {
                    p[x] = sbmp.GetPixel(w / 2, h - x - 1);
                }
                if (p[0].R < 50 && p[0].G > 120 && p[0].B < 50 && p[3].R > 200 && p[3].G > 200 && p[3].B > 200)
                {
                    bmpDest = new Bitmap(w, h - 4, PixelFormat.Format32bppRgb);
                    Rectangle rectDest = new Rectangle(0, 0, w, h - 4);
                    Graphics graphic = Graphics.FromImage(bmpDest);
                    graphic.DrawImage(sbmp, rectDest, rectDest, GraphicsUnit.Pixel);
                    graphic.Dispose();
                    GC.Collect();
                    break;
                }
                h--;
            }
            return bmpDest;
        }

        //private Bitmap CutImage(Bitmap sbmp, int type)
        //{
        //    Color pixel;
        //    Color pixel2;
        //    int sw = sbmp.Width;
        //    int sh = sbmp.Height;
        //    int h = 0;
        //    int hs = 0;
        //    int he = 0;
        //    int w = sw;
        //    bool IsReachGreenCutLine = true;

        //    if (type == 10)
        //    {
        //        while (h < sh && IsReachGreenCutLine)
        //        {
        //            pixel = sbmp.GetPixel(w / 2, h);
        //            if (pixel.R < 50 && pixel.G > 120 && pixel.B < 50)
        //            {
        //                IsReachGreenCutLine = false;
        //                he = h;
        //            }
        //            else
        //            {
        //                h++;
        //            }
        //        }
        //    }
        //    else if (type == 12)
        //    {
        //        while (h < sh - 1 && IsReachGreenCutLine)
        //        {
        //            pixel = sbmp.GetPixel(w / 2, h);
        //            pixel2 = sbmp.GetPixel(w / 2, h + 1);
        //            if (pixel.R < 50 && pixel.G > 120 && pixel.B < 50 && pixel2.R == 0 && pixel2.G == 0 && pixel2.B == 0)
        //            {
        //                IsReachGreenCutLine = false;
        //                hs = h + 1;
        //                he = sh;
        //            }
        //            else
        //            {
        //                h++;
        //            }
        //        }
        //    }
        //    else if (type == 11)
        //    {
        //        while (h < sh - 1 && IsReachGreenCutLine)
        //        {
        //            pixel = sbmp.GetPixel(w / 2, h);
        //            pixel2 = sbmp.GetPixel(w / 2, h + 1);
        //            if (pixel.R < 50 && pixel.G > 120 && pixel.B < 50 && pixel2.R == 255 && pixel2.G == 255 && pixel2.B == 255)
        //            {
        //                hs = h + 1;
        //                while (h < sh - 1 && IsReachGreenCutLine)
        //                {
        //                    pixel = sbmp.GetPixel(w / 2, h + 1);
        //                    pixel2 = sbmp.GetPixel(w / 2, h);
        //                    if (pixel.R < 50 && pixel.G > 120 && pixel.B < 50 && pixel2.R == 255 && pixel2.G == 255 && pixel2.B == 255)
        //                    {
        //                        he = h - 1;
        //                        IsReachGreenCutLine = false;
        //                    }
        //                    else
        //                    {
        //                        h++;
        //                    }
        //                }
        //                IsReachGreenCutLine = false;
        //            }
        //            else
        //            {
        //                h++;
        //            }
        //        }
        //    }
        //    else if (type == 22)//切除底部白边
        //    {
        //        while (h < sh - 1 && IsReachGreenCutLine)
        //        {
        //            pixel = sbmp.GetPixel(w / 2, h + 1);
        //            pixel2 = sbmp.GetPixel(w / 2, h);
        //            if (pixel.R < 50 && pixel.G > 120 && pixel.B < 50 && pixel2.R == 255 && pixel2.G == 255 && pixel2.B == 255)
        //            {
        //                he = h - 1;
        //                IsReachGreenCutLine = false;
        //            }
        //            else
        //            {
        //                h++;
        //            }
        //        }
        //    }

        //    else if (type == 20)
        //    {
        //        hs = 0;
        //        he = sh - 72;
        //    }
        //    else if (type == 21)
        //    {
        //        hs = 112;
        //        he = sh;
        //    }
        //    if (he > hs)
        //    {
        //        Bitmap bmpDest = new Bitmap(w, he - hs, PixelFormat.Format32bppRgb);
        //        Rectangle rectSource = new Rectangle(0, hs, w, he - hs);
        //        Rectangle rectDest = new Rectangle(0, 0, w, he - hs);
        //        Graphics graphic = Graphics.FromImage(bmpDest);
        //        graphic.DrawImage(sbmp, rectDest, rectSource, GraphicsUnit.Pixel);
        //        return bmpDest;
        //    }
        //    else
        //    {
        //        return sbmp;
        //    }
        //}


        //private Bitmap JoinBitmap(List<Bitmap> bmps)
        //{
        //    if (bmps.Count <= 0) return null;
        //    int width = bmps.Max(b => b.Width);
        //    int height = bmps.Sum(b => b.Height);
        //    Bitmap tableChartImage = new Bitmap(width, height);
        //    Graphics graph = Graphics.FromImage(tableChartImage);
        //    graph.DrawImage(tableChartImage, width, height);
        //    int currentHeight = 0;
        //    foreach (Bitmap i in bmps)
        //    {
        //        graph.DrawImage(i, 0, currentHeight);
        //        currentHeight += i.Height;
        //    }
        //    return tableChartImage;
        //}

        private Bitmap CutRoundBitmap(Bitmap sourceBitmap, int VerticalCutPixel, int HorizontalCutPixel)
        {
            int iSourceWidth = sourceBitmap.Width;
            int iSourceHeight = sourceBitmap.Height;

            int cutWidth = iSourceWidth - HorizontalCutPixel * 2;
            int cutHeight = iSourceHeight - VerticalCutPixel * 2;
            int cutLeft = HorizontalCutPixel;
            int cutTop = VerticalCutPixel;

            //先初始化一个位图对象，来存储截取后的图像
            Bitmap bmpDest = new Bitmap(cutWidth, cutHeight, PixelFormat.Format32bppRgb);

            //这个矩形定义了，你将要在被截取的图像上要截取的图像区域的左顶点位置和截取的大小
            Rectangle rectSource = new Rectangle(cutLeft, cutTop, cutWidth, cutHeight);

            //这个矩形定义了，你将要把 截取的图像区域 绘制到初始化的位图的位置和大小
            //我的定义，说明，我将把截取的区域，从位图左顶点开始绘制，绘制截取的区域原来大小
            Rectangle rectDest = new Rectangle(0, 0, cutWidth, cutHeight);
            Graphics graphic = Graphics.FromImage(bmpDest);
            //第一个参数就是加载你要截取的图像对象，第二个和第三个参数及如上所说定义截取和绘制图像过程中的相关属性，第四个属性定义了属性值所使用的度量单位
            graphic.DrawImage(sourceBitmap, rectDest, rectSource, GraphicsUnit.Pixel);
            return bmpDest;
        }


        GB ConvertLrmToWordForPDF(string ModelPath, GB g, double countdate, DateTime countdatetime)
        {
            using (WordHelper wordHelper = new WordHelper(ModelPath, Global.IsShowWord))
            {
                LrmHelper lrmreader = new LrmHelper();
                Person cadre_person = lrmreader.GetPersonFromLrmFile(g.Local_StorgeLrmFullpath);

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
                wordHelper.SaveDocumentAsPDF(g.Local_SaveLrmPdfForCombineFullpath);
            }
            return g;
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
