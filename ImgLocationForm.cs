using ImgLocation.Forms;
using ImgLocation.Models;
using ImgLocation.Repository;
using ImgLocation.Services;
using ImgLocation.UnitTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText = iTextSharp.text;
using iTextPdf = iTextSharp.text.pdf;

namespace ImgLocation
{
    public partial class ImgLocationForm : Form
    {
        public ImgLocationForm()
        {
            InitializeComponent();

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
                default:
                    dr = MessageBox.Show(message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
            return dr;
        }

        ConvertHelper convertHelper = new ConvertHelper();

        void QuickButton()
        {
            TreeNode t = TreeDW.SelectedNode;
            iDocAdd.Enabled = (t != null && t.Level == 0) || t == null;
            iNodeUp.Enabled = t != null && t.Level == 0;
            iNodeDown.Enabled = t != null && t.Level == 0;
            iRemoveNode.Enabled = t != null && t.Level == 0;
            iDocEditInfo.Enabled = t != null && t.Level == 0;
            iCadreEditInfo.Enabled = t != null && t.Level == 1;
        }

        struct GB_Rectangle
        {
            public GB _GB;
            public Rectangle _Tangle;
            public Point CenterPoint;
            public int RangeY;
            public int RangeX;
        }
        List<int> ShowImageHeights;
        List<int> ShowImageWeights;
        List<int> ShowImageCuts;
        List<GB_Rectangle> ShowCadreRectangles;
        int PanelWidth = 1;
        int SourceImageAverageWidth = 1;
        int PanelHeight = 1;
        int CellHeight = (int)(Global.ImgWidth / (210 - Global.HorizontalCutSize * 2) * (297 - Global.VerticalCutSize * 2)) / 25;
        int CellWidth = Global.ImgWidth / 25;
        void LoadImage()
        {
            ShowImageHeights = new List<int>();
            ShowImageWeights = new List<int>();
            ShowImageCuts = new List<int>();
            ShowCadreRectangles = new List<GB_Rectangle>();
            GC.Collect();
            PanelPicture.VerticalScroll.Value = 0;
            TreeNode t = TreeDW.SelectedNode;

            PanelWidth = PanelPicture.Width > 700 ? 700 - 30 : PanelPicture.Width - 30;
            PanelHeight = PanelPicture.Height;
            CellHeight = (int)(Global.ImgWidth / (210 - Global.HorizontalCutSize * 2) * (297 - Global.VerticalCutSize * 2)) / 25;
            CellWidth = (int)(Global.ImgWidth) / 25;

            if (t.Level == 0)
            {
                DW d = (DW)t.Tag;
                if (d.DocumentImageCount > 0)
                {

                    List<Image> SourceImages = new List<Image>();
                    foreach (string imagepath in d.DocumentImageFileFullPaths)
                    {
                        Image SourceImage = Image.FromFile(imagepath);

                        //添加触发点标注功能
                        //if(checkTouch.Checked)
                        //{
                        //    using(Graphics g_img = Graphics.FromImage(img))
                        //    {
                        //        foreach(GB sgb in d.GBS.Where(gb=>gb.DIP==i.P).ToArray())
                        //        {
                        //            int left= (int)(img.Width * sgb.DIHS);
                        //            int right = (int)(img.Width * sgb.DIHE);
                        //            int top = (int)(img.Height * sgb.DIS);
                        //            int bottom = (int)(img.Height * sgb.DIE);
                        //            Pen pen = new Pen(Color.Red,2);
                        //            Rectangle tangle = new Rectangle(new Point(left, top), new Size(right - left, bottom - top));
                        //            Region region = new Region(tangle);
                        //            g_img.DrawRectangle(pen, tangle);
                        //            ShowCadreRectangles.Add(tangle);
                        //            if(sgb.DIS2>0)
                        //            {
                        //                left = (int)(img.Width * sgb.DIHS2);
                        //                right = (int)(img.Width * sgb.DIHE2);
                        //                top = (int)(img.Height * sgb.DIS2);
                        //                bottom = (int)(img.Height * sgb.DIE2);
                        //                Rectangle tangle2 = new Rectangle(new Point(left, top), new Size(right - left, bottom - top));
                        //                Region region2 = new Region(tangle2);
                        //                g_img.DrawRectangle(pen, tangle2);
                        //                ShowCadreRectangles.Add(tangle2);
                        //            }
                        //        }
                        //    }
                        //}

                        SourceImages.Add(SourceImage);
                    }

                    SourceImageAverageWidth = (int)SourceImages.Average(i => i.Width);
                    int SourceImageSumHeight = SourceImages.Sum(i => i.Height);
                    int ZoomImageWidth = PanelWidth;
                    int ZoomImageHeight = SourceImageSumHeight * PanelWidth / SourceImageAverageWidth;

                    Bitmap ZoomImage = new Bitmap(ZoomImageWidth, ZoomImageHeight);
                    Graphics g = Graphics.FromImage(ZoomImage);
                    Pen PenForSplitLine = new Pen(Color.FromArgb(221, 221, 221), 1);

                    for (int i = 0; i < SourceImages.Count; i++)
                    {
                        int ZoomSingleImageTop = SourceImages.Take(i).Sum(img => img.Height) * PanelWidth / SourceImageAverageWidth;
                        //int itop = 0;
                        //int inow = 0;
                        //while (inow < i)
                        //{
                        //    itop += SourceImages[inow].Height * PanelWidth / SourceImages[inow].Width;
                        //    inow++;
                        //}

                        //绘制单个图像
                        g.DrawImage(SourceImages[i], new Rectangle(0, ZoomSingleImageTop, PanelWidth, SourceImages[i].Height * PanelWidth / SourceImages[i].Width), new Rectangle(0, 0, SourceImages[i].Width, SourceImages[i].Height), GraphicsUnit.Pixel);

                        if (checkTouch.Checked)
                        {
                            foreach (GB SingleGB in d.GBS.Where(gb => gb.DocumentImagePageNumber == i + 1).AsEnumerable())
                            {
                                int StartPointX = (int)(SourceImages[i].Width * PanelWidth / SourceImageAverageWidth * SingleGB.TouchStartPointX);
                                int EndPointX = (int)(SourceImages[i].Width * PanelWidth / SourceImageAverageWidth * SingleGB.TouchEndPointX);
                                int StartPointY = ZoomSingleImageTop + (int)(SourceImages[i].Height * PanelWidth / SourceImageAverageWidth * SingleGB.TouchStartPointY);
                                int EndPointY = ZoomSingleImageTop + (int)(SourceImages[i].Height * PanelWidth / SourceImageAverageWidth * SingleGB.TouchEndPointY);
                                Pen pen = new Pen(Color.Red, 2);
                                Rectangle tangle = new Rectangle(new Point(StartPointX, StartPointY), new Size(EndPointX - StartPointX, EndPointY - StartPointY));
                                Region region = new Region(tangle);
                                g.DrawRectangle(pen, tangle);

                                GB_Rectangle gb_tangle = new GB_Rectangle();
                                gb_tangle._GB = SingleGB;
                                gb_tangle._Tangle = tangle;
                                gb_tangle.CenterPoint = new Point((StartPointX + EndPointX) / 2, (StartPointY + EndPointY) / 2);

                                gb_tangle.RangeX = gb_tangle.CenterPoint.X / (CellWidth * PanelWidth / SourceImageAverageWidth);
                                gb_tangle.RangeY = gb_tangle.CenterPoint.Y / (CellHeight * PanelWidth / SourceImageAverageWidth);

                                ShowCadreRectangles.Add(gb_tangle);
                                if (SingleGB.TouchStartPointY2 > 0)
                                {
                                    StartPointX = (int)(SourceImages[i].Width * PanelWidth / SourceImageAverageWidth * SingleGB.TouchStartPointX2);
                                    EndPointX = (int)(SourceImages[i].Width * PanelWidth / SourceImageAverageWidth * SingleGB.TouchEndPointX2);
                                    StartPointY = ZoomSingleImageTop + (int)(SourceImages[i].Height * PanelWidth / SourceImageAverageWidth * SingleGB.TouchStartPointY2);
                                    EndPointY = ZoomSingleImageTop + (int)(SourceImages[i].Height * PanelWidth / SourceImageAverageWidth * SingleGB.TouchEndPointY2);
                                    Rectangle tangle2 = new Rectangle(new Point(StartPointX, StartPointY), new Size(EndPointX - StartPointX, EndPointY - StartPointY));

                                    GB_Rectangle gb_tangle2 = new GB_Rectangle();
                                    //gb_tangle._GB = SingleGB;
                                    //gb_tangle._Tangle = tangle2;
                                    //gb_tangle.CenterPoint = new Point((StartPointX + EndPointX) / 2, (StartPointY + EndPointY) / 2);

                                    //gb_tangle.RangeX = gb_tangle.CenterPoint.X / (CellWidth * PanelWidth / SourceImageAverageWidth);
                                    //gb_tangle.RangeY = gb_tangle.CenterPoint.Y / (CellHeight * PanelWidth / SourceImageAverageWidth);

                                    g.DrawRectangle(pen, tangle2);

                                    ShowCadreRectangles.Add(gb_tangle2);
                                }
                            }
                        }

                        //绘制分割线
                        g.DrawLine(PenForSplitLine, 0, ZoomSingleImageTop, SourceImages[i].Height * PanelWidth / SourceImages[i].Width, ZoomSingleImageTop);

                        //记录每个图像缩放后的像素
                        ShowImageWeights.Add(PanelWidth);
                        ShowImageHeights.Add(SourceImages[i].Height * PanelWidth / SourceImages[i].Width);
                        ShowImageCuts.Add(ZoomSingleImageTop);
                    }
                    g.Dispose();

                    for (int i = 0; i < SourceImages.Count; i++)
                    {
                        SourceImages[i].Dispose();
                    }
                    SourceImages.Clear();
                    FilePicture.Image = ZoomImage;
                }
                else
                {
                    FilePicture.Image = null;
                }

            }
            if (t.Level == 1)
            {
                GB gb = (GB)t.Tag;
                List<Image> SourceImages = new List<Image>();

                //if (gb.LrmImageCount > 0 && gb.ResImageCount == 0 && File.Exists(gb.LRMIMG))
                //{
                //    Image LrmImage = Image.FromFile(gb.LRMIMG);
                //    SourceImages.Add(LrmImage);
                //}
                //if (gb.ResImageCount > 0)
                //{
                //    string ImgDirectory = Path.GetDirectoryName(gb.RESIMG);
                //    string Filename = Path.GetFileNameWithoutExtension(gb.RESIMG);
                //    string FileExtension = Path.GetExtension(gb.RESIMG);

                //    for (int i=0;i<gb.ResImageCount;i++)
                //    {
                //        string ImagePath =Path.Combine(ImgDirectory, string.Format("{0}_{1}{2}", Filename, i, FileExtension));
                //        if(File.Exists(ImagePath))
                //        {
                //            Image ResImage = Image.FromFile(ImagePath);
                //            SourceImages.Add(ResImage);
                //        }
                //    }
                //}
                if (gb.AllImageFileFullPaths.Count > 0)
                {
                    foreach (string ImagePath in gb.AllImageFileFullPaths)
                    {
                        if (File.Exists(ImagePath))
                        {
                            SourceImages.Add(Image.FromFile(ImagePath));
                        }
                    }

                }

                if (SourceImages.Count > 0)
                {
                    SourceImageAverageWidth = (int)SourceImages.Average(i => i.Width);
                    int SourceImageSumHeight = SourceImages.Sum(i => i.Height);
                    int ZoomImageWidth = PanelWidth;
                    int ZoomImageHeight = SourceImageSumHeight * PanelWidth / SourceImageAverageWidth;

                    Bitmap ZoomImage = new Bitmap(ZoomImageWidth, ZoomImageHeight);
                    Graphics g = Graphics.FromImage(ZoomImage);
                    Pen PenForSplitLine = new Pen(Color.FromArgb(221, 221, 221), 1);

                    for (int i = 0; i < SourceImages.Count; i++)
                    {
                        int ZoomSingleImageTop = SourceImages.Take(i).Sum(img => img.Height) * PanelWidth / SourceImageAverageWidth;
                        //逐个绘制图像
                        g.DrawImage(SourceImages[i], new Rectangle(0, ZoomSingleImageTop, PanelWidth, SourceImages[i].Height * PanelWidth / SourceImages[i].Width), new Rectangle(0, 0, SourceImages[i].Width, SourceImages[i].Height), GraphicsUnit.Pixel);
                        //绘制分割线
                        g.DrawLine(PenForSplitLine, 0, ZoomSingleImageTop, SourceImages[i].Height * PanelWidth / SourceImages[i].Width, ZoomSingleImageTop);
                    }
                    g.Dispose();
                    //销毁对象
                    for (int i = 0; i < SourceImages.Count; i++)
                    {
                        SourceImages[i].Dispose();
                    }
                    SourceImages.Clear();
                    //显示总图像
                    FilePicture.Image = ZoomImage;
                }
                else
                {
                    FilePicture.Image = null;
                }



                //if (gb.SPBSL > 0 && gb.CLSL == 0 && File.Exists(gb.LRMIMG))
                //{
                //    Image img = Image.FromFile(gb.LRMIMG);
                //    int iw = img.Width;
                //    int ih = img.Height;
                //    int zoomw = PanelWidth;
                //    int zoomh = (int)(ih * PanelWidth / iw);
                //    Bitmap zoomimg = new Bitmap(zoomw, zoomh);
                //    Graphics g = Graphics.FromImage(zoomimg);

                //    g.DrawImage(img, new Rectangle(0, 0, zoomw, zoomh), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                //    g.Dispose();
                //    img.Dispose();
                //    FilePicture.Image = zoomimg;
                //}
                //else if (/*gb.SPBSL > 0 && */gb.CLSL > 0 /*&& File.Exists(gb.LRMIMG)*/ && File.Exists(gb.RESIMG))
                //{
                //    Image img = Image.FromFile(gb.LRMIMG);
                //    Image img2 = Image.FromFile(gb.RESIMG);

                //    int iw = img.Width;
                //    int ih1 = img.Height;
                //    int ih2 = img2.Height;
                //    int zoomw = PanelWidth;
                //    int zoomh1 = (int)(ih1 * PanelWidth / iw);
                //    int zoomh2 = (int)(ih2 * PanelWidth / iw);
                //    Bitmap zoomimg = new Bitmap(zoomw, zoomh1 + zoomh2);
                //    Graphics g = Graphics.FromImage(zoomimg);

                //    g.DrawImage(img, new Rectangle(0, 0, zoomw, zoomh1), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                //    g.DrawImage(img2, new Rectangle(0, zoomh1, zoomw, zoomh2), new Rectangle(0, 0, img2.Width, img2.Height), GraphicsUnit.Pixel);
                //    g.Dispose();
                //    img.Dispose();
                //    img2.Dispose();
                //    FilePicture.Image = zoomimg;
                //}
                //else
                //{
                //    FilePicture.Image = null;
                //}
            }
        }
        public void ClearImage()
        {
            FilePicture.Image = null;
            FilePicture.Refresh();
        }
        public void ReloadTree()
        {
            try
            {
                ///
                ///TODO:此处需要优化，不要全部清空节点，应当相互比较进行添加删除操作。
                /// 
                TreeDW.Nodes.Clear();
                DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
                dr.OrderSX();
                List<DW> ds = dr.GetAllDWs();
                foreach (DW d in ds.OrderBy(dd => dd.Rank))
                {
                    TreeNode td = CreateDocumentTreenode(d);
                  
                    TreeDW.Nodes.Add(td);
                }
                TreeDW.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载目录错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            System.GC.Collect();
        }
        void FreshPad()
        {
            padMonitor0.BeginMonitor();
            padMonitor1.BeginMonitor();
            padMonitor2.BeginMonitor();
            padMonitor3.BeginMonitor();
            padMonitor4.BeginMonitor();
            padMonitor5.BeginMonitor();
            padMonitor6.BeginMonitor();
            padMonitor7.BeginMonitor();
            padMonitor8.BeginMonitor();
            padMonitor9.BeginMonitor();
            padMonitor10.BeginMonitor();
            padMonitor11.BeginMonitor();
            padMonitor12.BeginMonitor();
            padMonitor13.BeginMonitor();
            padMonitor14.BeginMonitor();
            padMonitor15.BeginMonitor();
            padMonitor16.BeginMonitor();
            padMonitor17.BeginMonitor();
            padMonitor18.BeginMonitor();
            padMonitor19.BeginMonitor();
            padMonitor20.BeginMonitor();
            padMonitor21.BeginMonitor();
            padMonitor22.BeginMonitor();
            padMonitor23.BeginMonitor();
            padMonitor24.BeginMonitor();
            padMonitor25.BeginMonitor();
            padMonitor26.BeginMonitor();
            padMonitor27.BeginMonitor();
            padMonitor28.BeginMonitor();
            padMonitor29.BeginMonitor();
        }



        private void ImgLocationForm_Load(object sender, EventArgs e)
        {
            SystemRepository sr = new SystemRepository();

            Project p = Global.LoadDefaultProject();
            lblProject.Text = p.TITLE;
            comboMeeting.Text = "会议议题1";
            Global.RefreshParams();
            Global.RefreshDirectory(p, comboMeeting.Text.Replace("会议议题", "Meeting"));
            iConvertPDF.Visible = true;
            btnOpenPDFDiatory.Visible = true;
            ReloadTree();
            QuickButton();
            FreshPad();

            checkDrawPoint.Enabled = checkTouch.Checked;

            checkShowWord.Checked = sr.ReadSystemConfig(701).Trim().Length > 0 ? sr.ReadSystemConfig(701) == "是" : true;
            checkShowError.Checked = sr.ReadSystemConfig(702).Trim().Length > 0 ? sr.ReadSystemConfig(702) == "是" : true;
            checkUseLrmImageModel.Checked = sr.ReadSystemConfig(801).Trim().Length > 0 ? sr.ReadSystemConfig(801) == "是" : true;
        }
        private void iConvertPadData_Click(object sender, EventArgs e)
        {
            ClearImage();
            FolderForm fm = new FolderForm();
            if (fm.ShowDialog() == DialogResult.OK)
            {
                ConvertProcessForm c = new ConvertProcessForm();
                c.FatherForm = this;
                c.WordDirectory = fm.sworddir;
                c.LrmDirectory = fm.slrmdir;
                c.ResDirectory = fm.sresdir;
                c.IsAdd = fm.isadd;
                c.CountDate = this.iCountDate.Value;
                c.Show();
                c.ConvertDocumentList();
                this.ReloadTree();
            }
        }
        private void iConvertPDF_Click(object sender, EventArgs e)
        {
            //Project p = Global.LoadDefaultProject();
            DataRepository sr = new DataRepository(Global.ProjectOutputDbPath);
            List<DW> ldw = sr.GetAllDWs();
            if (DialogResult.OK == MessageBox.Show(string.Format("是否转换当前项目：{0}，共存在{1}个文档。", Global.ProjectName, ldw.Count), "是否开始转换项目为存档PDF", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                ConvertProcessForm cpf = new ConvertProcessForm();
                cpf.FatherForm = this;
                cpf.CountDate = this.iCountDate.Value;
                cpf.Show();
                cpf.ConvertProjectToPdfForStorge();
            }
        }
        struct OrderDW
        {
            public int R;
            public DW OriDW;
        }
        private void iReOrder_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("确认要按照文号进行排序？\r执行此操作前，请确认已经对所有文件设置文号，无文号的节点将被移到最顶端。", "确认操作", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                try
                {
                    DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
                    List<OrderDW> DWsForOrder = new List<OrderDW>();
                    foreach (DW d in dr.GetAllDWs())
                    {
                        OrderDW odw = new OrderDW();
                        odw.OriDW = d;
                        try
                        {
                            string stri = d.WH.Split('〕')[1].Split('号')[0];
                            int ir = Convert.ToInt32(stri);
                            odw.R = ir;
                        }
                        catch
                        {
                            odw.R = 999;
                        }
                        DWsForOrder.Add(odw);
                    }
                    DWsForOrder = DWsForOrder.OrderBy(dfo => dfo.R).ToList();
                    for (int i = 0; i < DWsForOrder.Count; i++)
                    {
                        DW d = DWsForOrder[i].OriDW;
                        d.Rank = i + 101;
                        dr.EditDW(d);
                    }
                    //dr.CoverSX("WH");
                    ReloadTree();
                    TreeDW.CollapseAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("排序错误：" + ex.Message, "排序错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                convertHelper.UpdateDataId();
            }
        }
        private void iNodeUp_Click(object sender, EventArgs e)
        {
            TreeNode t = TreeDW.SelectedNode;
            TreeNode tp = t.PrevNode;
            if (t != null && tp != null && t.Level == 0)
            {
                DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
                DW d = (DW)t.Tag;
                DW dp = (DW)tp.Tag;

                string dxh = d.XH;
                if (dr.MoveUp(d))
                {
                    TreeNode tn = (TreeNode)t.Clone();
                    d.Rank--;
                    d.XH = dp.XH;
                    tn.Tag = d;

                    tn.Text = d.XH.Trim().Length > 0 ? d.XH + "." + d.MC : d.MC;
                    if (d.WH.Length > 0)
                    {
                        tn.Text += string.Format("（{0}）", d.WH);
                    }

                    TreeDW.Nodes.Insert(tp.Index, tn);

                    dp.Rank++;
                    dp.XH = dxh;
                    tp.Tag = dp;

                    tp.Text = dp.XH.Trim().Length > 0 ? dp.XH + "." + dp.MC : dp.MC;
                    if (dp.WH.Length > 0)
                    {
                        tp.Text += string.Format("（{0}）", dp.WH);
                    }

                    t.Remove();
                    TreeDW.SelectedNode = tn;
                    tn.Expand();
                }
                convertHelper.UpdateDataId();
            }
            GC.Collect();
        }
        private void iNodeDown_Click(object sender, EventArgs e)
        {
            TreeNode t = TreeDW.SelectedNode;
            TreeNode tn = t.NextNode;
            if (t != null && tn != null && t.Level == 0)
            {
                DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
                DW d = (DW)t.Tag;

                if (dr.MoveDown(d))
                {
                    TreeNode tnn = (TreeNode)tn.Clone();
                    DW dn = (DW)tn.Tag;
                    string dnxh = dn.XH;
                    dn.Rank--;
                    dn.XH = d.XH;
                    tnn.Tag = dn;

                    tnn.Text = dn.XH.Trim().Length > 0 ? dn.XH + "." + dn.MC : dn.MC;
                    if (dn.WH.Length > 0)
                    {
                        tn.Text += string.Format("（{0}）", dn.WH);
                    }

                    TreeDW.Nodes.Insert(t.Index, tnn);

                    d.Rank++;
                    d.XH = dnxh;
                    t.Tag = d;

                    t.Text = d.XH.Trim().Length > 0 ? d.XH + "." + d.MC : d.MC;
                    if (d.WH.Length > 0)
                    {
                        t.Text += string.Format("（{0}）", d.WH);
                    }

                    tn.Remove();
                    tnn.Expand();
                    TreeDW.SelectedNode = t;
                }
                convertHelper.UpdateDataId();
            }
            GC.Collect();
        }
        private void iRemoveNode_Click(object sender, EventArgs e)
        {
            ClearImage();
            TreeNode t = TreeDW.SelectedNode;
            if (t.Level == 0)
            {
                try
                {
                    DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
                    DW d = (DW)t.Tag;
                    dr.RemoveDW(d);
                    dr.OrderSX();
                    //TreeDW.Nodes.Remove(t);
                    ReloadTree();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除文档节点错误：" + ex.Message, "删除错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                convertHelper.UpdateDataId();
            }
        }
        private void iDocAdd_Click(object sender, EventArgs e)
        {
            ClearImage();

            DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
            TreeNode t = TreeDW.SelectedNode;
            DW d;
            bool start = true;
            if (t != null && t.Level == 0)
            {
                d = new DW();
                DW de = (DW)t.Tag;
                d.id = Guid.NewGuid().ToString();
                d.Rank = de.Rank;
            }
            else
            {
                d = new DW();
                d.id = Guid.NewGuid().ToString();
                d.Rank = dr.GetMaxSX() + 1;
            }

            if (start)
            {
                DocumentForm df = new DocumentForm(d);
                if (df.ShowDialog() == DialogResult.OK)
                {
                    if (df.IsConvertFileImage)
                    {
                        ConvertProcessForm c = new ConvertProcessForm();
                        c.FatherForm = this;
                        c.LrmDirectory = df.LrmDirectory;
                        c.ResDirectory = df.ResDirectory;
                        c.CountDate = this.iCountDate.Value;
                        c.Show();
                        //添加单子时忽视是否替换文档页码范围和查找干部信息
                        d = c.ConvertSingleDocument(df.document, string.Empty, true);
                        dr.EditDW(df.document);
                    }
                    else
                    {
                        dr.EditDW(df.document);
                    }
                    convertHelper.UpdateDataId();
                }
                ReloadTree();
                TreeDW.SelectedNode = t;
            }
        }
        private void iDocEditInfo_Click(object sender, EventArgs e)
        {
            ClearImage();

            TreeNode t = TreeDW.SelectedNode;
            if (t.Level == 0)
            {
                DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);

                if (t != null && t.Level == 0)
                {
                    DW d = (DW)t.Tag;
                    DocumentForm df = new DocumentForm(d);
                    if (df.ShowDialog() == DialogResult.OK)
                    {
                        if (df.IsConvertFileImage)
                        {
                            //dr.RemoveDW(df.document);
                            ConvertProcessForm c = new ConvertProcessForm();
                            c.FatherForm = this;
                            c.LrmDirectory = df.LrmDirectory;
                            c.ResDirectory = df.ResDirectory;
                            c.CountDate = this.iCountDate.Value;
                            c.Show();
                            d = c.ConvertSingleDocument(df.document, df.ConvertPageRange, df.IsRelocationCadre);
                            dr.EditDW(df.document);
                        }
                        else
                        {
                            dr.EditDW(df.document);
                        }
                        convertHelper.UpdateDataId();

                    }

                    ReloadTree();
                    TreeDW.SelectedNode = t;
                }
                else
                {
                    MessageBox.Show("请选择所需要编辑的节点！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (t.Level == 1)
            {
                GB gb = (GB)t.Tag;
                CadreForm cf = new CadreForm(gb);
                if (DialogResult.OK == cf.ShowDialog())
                {
                    ConvertProcessForm c = new ConvertProcessForm();
                    c.FatherForm = this;
                    c.CountDate = this.iCountDate.Value;
                    c.Show();
                    GB g = c.ConvertCadre(cf.g);
                    convertHelper.UpdateDataId();

                    t.Tag = g;
                    TreeDW.SelectedNode = t;
                    LoadImage();

                }
            }
        }
        private void TreeDW_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FilePicture.MouseClick += new MouseEventHandler(FilePicture_MouseClick);
            FilePicture.MouseWheel += new MouseEventHandler(FilePicture_MouseWheel);
            QuickButton();
            LoadImage();
        }
        private void FilePicture_MouseClick(object sender, MouseEventArgs e)
        {
            FilePicture.Focus();
        }
        private void FilePicture_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.X > PanelPicture.Left && e.X < PanelPicture.Left + PanelPicture.Width && e.Y > PanelPicture.Top && e.Y < PanelPicture.Top + PanelPicture.Height)
            {
                FilePicture.Focus();
                PanelPicture.AutoScrollPosition = new Point(0, PanelPicture.VerticalScroll.Value - (int)(e.Delta / 10));
            }
        }
        private void iCadreEditInfo_Click(object sender, EventArgs e)
        {
            ClearImage();

            TreeNode t = TreeDW.SelectedNode;
            GB gb = (GB)t.Tag;

            TreeNode tf = t.Parent;
            DW d = (DW)tf.Tag;
            int index = d.GBS.IndexOf(gb);

            CadreForm cf = new CadreForm(gb);
            if (DialogResult.OK == cf.ShowDialog())
            {
                ConvertProcessForm c = new ConvertProcessForm();
                c.FatherForm = this;
                c.CountDate = this.iCountDate.Value;
                c.Show();
                GB g = c.ConvertSingleCadre(d, cf.g);
                convertHelper.UpdateDataId();

                //更新选中的干部节点
                t.Tag = g;
                t.ForeColor = g.LrmImageCount > 0 ? Color.Black : Color.Red;
                TreeDW.SelectedNode = t;

                //更新选中的干部节点对应的父节点
                d.GBS[index] = g;
                tf.ForeColor = d.GBS.Any(dg => dg.LrmImageCount == 0) ? Color.Red : Color.Black;

                LoadImage();
            }
        }
        private void btnPAD_Click(object sender, EventArgs e)
        {
            Form tf = new PadForm();
            if (tf.ShowDialog() == DialogResult.Cancel)
            {
                FreshPad();
            }
        }
        private void btnFreshPad_Click(object sender, EventArgs e)
        {
            FreshPad();
        }
        private void btnPushAll_Click(object sender, EventArgs e)
        {
            string message = "";
            message += padMonitor0.PushData(true) + "\r\n";
            message += padMonitor1.PushData(true) + "\r\n";
            message += padMonitor2.PushData(true) + "\r\n";
            message += padMonitor3.PushData(true) + "\r\n";
            message += padMonitor4.PushData(true) + "\r\n";
            message += padMonitor5.PushData(true) + "\r\n";
            message += padMonitor6.PushData(true) + "\r\n";
            message += padMonitor7.PushData(true) + "\r\n";
            message += padMonitor8.PushData(true) + "\r\n";
            message += padMonitor9.PushData(true) + "\r\n";
            message += padMonitor10.PushData(true) + "\r\n";
            message += padMonitor11.PushData(true) + "\r\n";
            message += padMonitor12.PushData(true) + "\r\n";
            message += padMonitor13.PushData(true) + "\r\n";
            message += padMonitor14.PushData(true) + "\r\n";
            message += padMonitor15.PushData(true) + "\r\n";
            message += padMonitor16.PushData(true) + "\r\n";
            message += padMonitor17.PushData(true) + "\r\n";
            message += padMonitor18.PushData(true) + "\r\n";
            message += padMonitor19.PushData(true) + "\r\n";
            message += padMonitor20.PushData(true) + "\r\n";
            message += padMonitor21.PushData(true) + "\r\n";
            message += padMonitor22.PushData(true) + "\r\n";
            message += padMonitor23.PushData(true) + "\r\n";
            message += padMonitor24.PushData(true) + "\r\n";
            message += padMonitor25.PushData(true) + "\r\n";
            message += padMonitor26.PushData(true) + "\r\n";
            message += padMonitor27.PushData(true) + "\r\n";
            message += padMonitor28.PushData(true) + "\r\n";
            message += padMonitor29.PushData(true) + "\r\n";
            MessageBox.Show(string.Format("向所有平板推送全部数据：\r\n{0}", message), "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnPushDifferent_Click(object sender, EventArgs e)
        {
            string message = "";
            message += padMonitor0.PushData(false) + "\r\n";
            message += padMonitor1.PushData(false) + "\r\n";
            message += padMonitor2.PushData(false) + "\r\n";
            message += padMonitor3.PushData(false) + "\r\n";
            message += padMonitor4.PushData(false) + "\r\n";
            message += padMonitor5.PushData(false) + "\r\n";
            message += padMonitor6.PushData(false) + "\r\n";
            message += padMonitor7.PushData(false) + "\r\n";
            message += padMonitor8.PushData(false) + "\r\n";
            message += padMonitor9.PushData(false) + "\r\n";
            message += padMonitor10.PushData(false) + "\r\n";
            message += padMonitor11.PushData(false) + "\r\n";
            message += padMonitor12.PushData(false) + "\r\n";
            message += padMonitor13.PushData(false) + "\r\n";
            message += padMonitor14.PushData(false) + "\r\n";
            message += padMonitor15.PushData(false) + "\r\n";
            message += padMonitor16.PushData(false) + "\r\n";
            message += padMonitor17.PushData(false) + "\r\n";
            message += padMonitor18.PushData(false) + "\r\n";
            message += padMonitor19.PushData(false) + "\r\n";
            message += padMonitor20.PushData(false) + "\r\n";
            message += padMonitor21.PushData(false) + "\r\n";
            message += padMonitor22.PushData(false) + "\r\n";
            message += padMonitor23.PushData(false) + "\r\n";
            message += padMonitor24.PushData(false) + "\r\n";
            message += padMonitor25.PushData(false) + "\r\n";
            message += padMonitor26.PushData(false) + "\r\n";
            message += padMonitor27.PushData(false) + "\r\n";
            message += padMonitor28.PushData(false) + "\r\n";
            message += padMonitor29.PushData(false) + "\r\n";
            MessageBox.Show(string.Format("向所有平板推送差异数据：\r\n{0}", message), "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void comboMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            Project p = Global.LoadDefaultProject();
            lblProject.Text = p.TITLE;
            Global.RefreshParams();
            Global.RefreshDirectory(p, comboMeeting.Text.Replace("会议议题", "Meeting"));
            ReloadTree();
            FilePicture.Image = null;

            QuickButton();
        }
        private void iProjectSetting_Click(object sender, EventArgs e)
        {
            ProjectForm pf = new ProjectForm();
            if (DialogResult.OK == pf.ShowDialog())
            {
                SystemRepository sr = new SystemRepository();
                Project p = sr.GetProject(Convert.ToInt32(sr.ReadSystemConfig(201)));
                lblProject.Text = p.TITLE;
                ReloadTree();
            }
        }
        private void btnOpenPDFDiatory_Click(object sender, EventArgs e)
        {
            Process.Start(Global.ProjectPDFOutputDirectory);
        }
        private void checkTouch_CheckedChanged(object sender, EventArgs e)
        {
            if (TreeDW.SelectedNode != null)
            {
                LoadImage();
            }
            checkDrawPoint.Enabled = checkTouch.Checked;
            if (!checkTouch.Checked)
            {
                checkDrawPoint.Checked = false;
            }
        }
        private void iAbout_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        private void linkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        private void FilePicture_Click(object sender, EventArgs e)
        {

        }

        Bitmap drawDocumentImage;
        bool StartDraw = false;
        Point StartPoint = Point.Empty;
        Bitmap StoreDocumentImage;
        private void FilePicture_MouseMove(object sender, MouseEventArgs e)
        {
            label2.Text = string.Format("X:{0}  Y:{1}", e.X, e.Y);
            if (checkDrawPoint.Checked && StartDraw)
            {
                Bitmap destBitmap = (Bitmap)drawDocumentImage.Clone();
                Point newPoint = new Point(StartPoint.X, StartPoint.Y);
                Graphics g = Graphics.FromImage(destBitmap);
                Pen pen = new Pen(Color.Blue, 1);
                int width = Math.Abs(e.X - newPoint.X);
                int height = Math.Abs(e.Y - newPoint.Y);
                newPoint.X = e.X < newPoint.X ? e.X : newPoint.X;
                newPoint.Y = e.Y < newPoint.Y ? e.Y : newPoint.Y;

                Rectangle tangle = new Rectangle(newPoint, new Size(width, height));
                g.DrawRectangle(pen, tangle);
                g.Dispose();
                pen.Dispose();

                //利用双缓冲技术绘制图像
                Graphics gl = FilePicture.CreateGraphics();
                StoreDocumentImage = (Bitmap)destBitmap.Clone();
                gl.DrawImage(destBitmap, new Point(0, 0));
                gl.Dispose();
                destBitmap.Dispose();
                GC.Collect();
            }
        }

        private void FilePicture_MouseDown(object sender, MouseEventArgs e)
        {

            if (checkDrawPoint.Checked && e.Button == MouseButtons.Left)
            {
                StartDraw = true;
                drawDocumentImage = (Bitmap)FilePicture.Image;
                StartPoint = new Point(e.X, e.Y);

            }
        }

        private void FilePicture_MouseUp(object sender, MouseEventArgs e)
        {
            if (StartDraw)
            {
                Point newPoint = new Point(StartPoint.X, StartPoint.Y);
                int width = Math.Abs(e.X - newPoint.X);
                int height = Math.Abs(e.Y - newPoint.Y);
                newPoint.X = e.X < newPoint.X ? e.X : newPoint.X;
                newPoint.Y = e.Y < newPoint.Y ? e.Y : newPoint.Y;
                Rectangle tangle = new Rectangle(newPoint, new Size(width, height));
                //foreach (Rectangle t in ShowCadreRectangles)
                //{
                //    bool b = tangle.IntersectsWith(t);
                //}

                //判断画框是否跨页
                if (ShowImageCuts.Any(s => (StartPoint.Y - s) * (e.Y - s) < 0))
                {
                    MessageBox.Show("触发点位置不允许跨页，请重新绘制触发点！");
                }
                //判断画框点位是否和现有画框重合
                else if (ShowCadreRectangles.Any(r => r._Tangle.IntersectsWith(tangle)))
                {
                    if (DialogResult.Yes == MessageBox.Show("触发点位置与现有干部触发点重合，是否替换现有干部触发位置？", "是否替换触发位置", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        GB gb = ShowCadreRectangles.First(r => r._Tangle.IntersectsWith(tangle))._GB;

                        //获取当前Rectangle的位置
                        for (int i = 0; i < ShowImageHeights.Count; i++)
                        {
                            if ((e.Y - ShowImageHeights.Take(i).Sum()) * (e.Y - ShowImageHeights.Take(i + 1).Sum()) < 0)
                            {
                                gb.TouchStartPointY = (StartPoint.Y - ShowImageHeights.Take(i).Sum()) / (double)(ShowImageHeights[i]);
                                gb.TouchEndPointY = (e.Y - ShowImageHeights.Take(i).Sum()) / (double)(ShowImageHeights[i]);
                                gb.TouchStartPointX = StartPoint.X / (double)(StoreDocumentImage.Width);
                                gb.TouchEndPointX = e.X / (double)(StoreDocumentImage.Width);

                                gb.TouchStartPointY2 = 0;
                                gb.TouchEndPointY2 = 0;
                                gb.TouchStartPointX2 = 0;
                                gb.TouchEndPointX2 = 0;

                                DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
                                dr.EditGB(gb);
                                LoadImage();
                                break;
                            }
                        }
                    }
                }
                else
                {

                    Bitmap drawDocumentImage = (Bitmap)FilePicture.Image;
                    FilePicture.Image = StoreDocumentImage;

                    TreeNode t = TreeDW.SelectedNode;

                    DW d = (DW)TreeDW.SelectedNode.Tag;

                    //获取点位所在页
                    int PageNumber = 0;
                    for (int i = 0; i < ShowImageCuts.Count; i++)
                    {
                        if (newPoint.Y + tangle.Height / 2 - ShowImageCuts[ShowImageCuts.Count - 1] > 0)
                        {
                            PageNumber = ShowImageCuts.Count;
                            break;
                        }
                        else if (newPoint.Y + tangle.Height / 2 - ShowImageCuts[i] > 0 && newPoint.Y + tangle.Height / 2 - ShowImageCuts[i + 1] < 0)
                        {
                            PageNumber = i + 1;
                            break;
                        }
                    }

                    //获取当前Rectangle的位置
                    for (int i = 0; i < ShowImageHeights.Count; i++)
                    {
                        if ((e.Y - ShowImageHeights.Take(i).Sum()) * (e.Y - ShowImageHeights.Take(i + 1).Sum()) < 0)
                        {
                            GB gb = new GB();

                            gb.DocumentImagePageNumber = PageNumber;

                            gb.TouchStartPointY = (StartPoint.Y - ShowImageHeights.Take(i).Sum()) / (double)(ShowImageHeights[i]);
                            gb.TouchEndPointY = (e.Y - ShowImageHeights.Take(i).Sum()) / (double)(ShowImageHeights[i]);
                            gb.TouchStartPointX = StartPoint.X / (double)(StoreDocumentImage.Width);
                            gb.TouchEndPointX = e.X / (double)(StoreDocumentImage.Width);

                            gb.TouchStartPointY2 = 0;
                            gb.TouchEndPointY2 = 0;
                            gb.TouchStartPointX2 = 0;
                            gb.TouchEndPointX2 = 0;

                            gb.id = string.Format("{0}_{1:000000}", d.id, d.GBS.Count);
                            gb.DWID = d.id;

                            gb.Local_SourceLrmFullpath = "";
                            gb.Local_SourcePicFullpath = "";
                            gb.Local_SourceResFullpath = "";

                            gb.DocumentImageFilename = d.DocumentImageFilenames[gb.DocumentImagePageNumber - 1];
                            //gb.DIID = string.Format("{0}_{1:0000}", d.id, PageNumber);
                            gb.Rank = d.Rank * 10000 + d.GBS.Count;//需要栅格化后更新
                            gb.XM = "手工添加触发点" + gb.Rank;

                            CadreForm cf = new CadreForm(gb);
                            if (DialogResult.OK == cf.ShowDialog())
                            {
                                ConvertProcessForm c = new ConvertProcessForm();
                                c.FatherForm = this;
                                c.CountDate = this.iCountDate.Value;
                                c.Show();

                                GB_Rectangle gbr = new GB_Rectangle();
                                gbr._GB = gb;
                                gbr._Tangle = tangle;
                                gbr.CenterPoint = new Point(newPoint.X + tangle.Width / 2, newPoint.Y + tangle.Height / 2);
                                gbr.RangeX = gbr.CenterPoint.X / (CellWidth * PanelWidth / SourceImageAverageWidth);
                                gbr.RangeY = gbr.CenterPoint.Y / (CellHeight * PanelWidth / SourceImageAverageWidth);
                                ShowCadreRectangles.Add(gbr);
                                ShowCadreRectangles.OrderBy(scr => scr.RangeY * 100 + scr.RangeX);

                                gb = c.ConvertSingleCadre(d, cf.g);
                                DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
                                //dr.EditGB(gb);
                                d.GBS.Add(gb);
                                dr.EditDW(d);
                                convertHelper.UpdateDataId();

                                TreeNode tnew = CreateDocumentTreenode(d);
                                TreeDW.Nodes.Insert(t.Index, tnew);
                                TreeDW.Nodes.Remove(t);
                                TreeDW.SelectedNode = tnew;
                                tnew.Expand();
                                LoadImage();
                            }
                            break;
                        }
                    }
                }
                StartPoint = Point.Empty;
                StartDraw = false;
                checkDrawPoint.Checked = false;
            }
        }
        private TreeNode CreateDocumentTreenode(DW d)
        {
            TreeNode td = new TreeNode();
            td.Tag = d;

            td.Text = d.XH.Trim().Length > 0 ? d.XH + "." + d.MC : d.MC;
            if (d.WH.Trim().Length > 0)
            {
                td.Text += string.Format("（{0}）", d.WH);
            }
            if (d.GBS.Any(dg => dg.LrmImageCount == 0 && dg.OtherImageCount == 0))
            {
                td.Text += string.Format("***文档中{0}个任免审图像不存在***", d.GBS.Count(dg => dg.LrmImageCount == 0 && dg.OtherImageCount == 0).ToString());
                td.ForeColor = Color.Red;
            }

            foreach (GB g in d.GBS.OrderBy(gg => gg.Rank))
            {
                TreeNode tg = new TreeNode();
                tg.ImageIndex = 1;
                tg.Text = g.XM;
                tg.Tag = g;
                tg.ForeColor = g.LrmImageCount > 0 ? Color.Black : Color.Red;
                //tg.ToolTipText = string.Format("{0},任免表信息：[{1}]；文件任免信息：[{2}]；相似度：[{3}]。", g.XM, g.RESUUID + "", g.SPBUUID + "", g.XSD + "");
                td.Nodes.Add(tg);
            }
            return td;
        }

        private void checkDrawPoint_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDrawPoint.Checked)
            {
                TreeNode t = TreeDW.SelectedNode;
                if (t != null && t.Level != 0)
                {
                    MessageBox.Show("只有选择文档页面时，才能手动绘制触发点");
                    checkDrawPoint.Checked = false;
                }
            }
            label2.Visible = checkDrawPoint.Checked;
        }

        private void checkShowWord_CheckedChanged(object sender, EventArgs e)
        {
            SystemRepository sr = new SystemRepository();
            if (checkShowWord.Checked)
            {
                sr.WriteSystemConfig(701, "ShowWord", "是");
            }
            else
            {
                sr.WriteSystemConfig(701, "ShowWord", "否");
            }

            Global.RefreshParams();
            Global.ValidateDirectory();
        }

        private void checkShowError_CheckedChanged(object sender, EventArgs e)
        {
            SystemRepository sr = new SystemRepository();
            if (checkShowError.Checked)
            {
                sr.WriteSystemConfig(702, "ShowError", "是");
            }
            else
            {
                sr.WriteSystemConfig(702, "ShowError", "否");
            }

            Global.RefreshParams();
            Global.ValidateDirectory();
        }

        private void checkUseLrmImageModel_CheckedChanged(object sender, EventArgs e)
        {
            SystemRepository sr = new SystemRepository();
            if (checkUseLrmImageModel.Checked)
            {
                sr.WriteSystemConfig(801, "UseLrmImageModel", "是");
            }
            else
            {
                sr.WriteSystemConfig(801, "UseLrmImageModel", "否");
            }

            Global.RefreshParams();
            Global.ValidateDirectory();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestLrmToImage t = new TestLrmToImage();
            t.ShowDialog();
        }

        private void splitContainer1_Panel1_DoubleClick(object sender, EventArgs e)
        {
            this.button1.Visible = !this.button1.Visible;
        }
    }
}
