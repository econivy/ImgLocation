using ImgLocation.Models;
using ImgLocation.Repository;
using ImgLocation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgLocation.Forms
{
    public partial class PushDataForm : Form
    {
        public PushDataForm(PadInfo p)
        {
            InitializeComponent();
            this._PadInfo = p;
        }
        public PadInfo _PadInfo { get; set; }
        public bool IsPushAllFile { get; set; }

        int TimerDelayLoadCount = 0;
        int FileSum = 0;
        int FileCount = 0;

        private void PushDataForm_Load(object sender, EventArgs e)
        {
            TimerDelayLoad.Start();
            this.Text = string.Format("正在向平板{0}推送数据...", this._PadInfo.id);
            this.lblPad.Text = string.Format("正在向平板{0}推送数据...", this._PadInfo.id);
        }

        private void TimerDelayLoad_Tick(object sender, EventArgs e)
        {
            TimerDelayLoadCount++;
            if (TimerDelayLoadCount > 1)
            {
                this.bk.RunWorkerAsync();
                this.TimerDelayLoad.Stop();
            }
        }

        string ExcetiveADBCommand(string command)
        {
            try
            {
                var resultd = ProcessHelper.Run(Global.ADBProgramPath, command);
                if (!resultd.Success
                    || resultd.ExitCode != 0
                    || (resultd.OutputString != null && resultd.OutputString.Contains("failed"))
                    || (resultd.OutputString != null && resultd.OutputString.Contains("not found")))
                {
                    if (DialogResult.OK == MessageBox.Show(string.Format("推送数据错误：[{0}]\r\n{1}\r\n{2}", resultd.ExitCode, resultd.OutputString, resultd.MoreOutputString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error))
                    {
                        this.bk.CancelAsync();
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                }
                return resultd.ToString();
            }
            catch (Exception ex)
            {
                if (DialogResult.OK == MessageBox.Show(string.Format("错误：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    this.bk.CancelAsync();
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                return ex.Message;
            }
        }

        private void BackgroundWorkPushData_DoWork(object sender, DoWorkEventArgs e)
        {

            if (bk.CancellationPending) //这里判断一下是否用户要求取消后台进行，并可以尽早退出。
            {
                bk.ReportProgress((int)(FileCount * 100 / FileSum), "用户中断操作");
            }
            else
            {
                if (IsPushAllFile)
                {
                    #region 文件全部推送
                    ExcetiveADBCommand(string.Format(" -s {0} shell rm -rf {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/"));
                    ExcetiveADBCommand(string.Format(" -s {0} shell mkdir {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/"));
                    for (int i = 1; i <= 8; i++)
                    {
                        string dbpath = Global.ProjectOutputDirectory + @"gbchd\Meeting" + i.ToString() + @"\db\imgLocation.db";
                        string picdic = Global.ProjectOutputDirectory + @"gbchd\Meeting" + i.ToString() + @"\pic\";
                        if (File.Exists(dbpath))
                        {
                            FileSum++;
                        }
                        if (Directory.Exists(picdic))
                        {
                            DirectoryInfo directoryInfos = new DirectoryInfo(picdic);
                            foreach (FileSystemInfo fileInfo in directoryInfos.GetFileSystemInfos())
                            {
                                if ((fileInfo is FileInfo))
                                {
                                    string fileExtension = Path.GetExtension(fileInfo.FullName);
                                    if (fileExtension == ".png")
                                    {
                                        FileSum++;
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 1; i <= 8; i++)
                    {
                        string dbpath = Global.ProjectOutputDirectory + @"gbchd\Meeting" + i.ToString() + @"\db\imgLocation.db";
                        string picdic = Global.ProjectOutputDirectory + @"gbchd\Meeting" + i.ToString() + @"\pic\";
                        if (File.Exists(dbpath))
                        {
                            string command = string.Format(" -s {0} shell mkdir {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/Meeting" + i.ToString() + "/");
                            ExcetiveADBCommand(command);

                            command = string.Format(" -s {0} shell mkdir {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/Meeting" + i.ToString() + "/db/");
                            ExcetiveADBCommand(command);

                            command = string.Format(" -s {0} push {1} {2}", this._PadInfo.SERIAL.Trim(), dbpath, "/mnt/sdcard/gbchd/Meeting" + i.ToString() + @"/db/imgLocation.db");
                            ExcetiveADBCommand(command);

                            FileCount++;
                        }
                        if (Directory.Exists(picdic))
                        {
                            string command = string.Format(" -s {0} shell mkdir {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/Meeting" + i.ToString() + "/pic/");
                            ExcetiveADBCommand(command);

                            DirectoryInfo directoryInfos = new DirectoryInfo(picdic);
                            foreach (FileSystemInfo fileInfo in directoryInfos.GetFileSystemInfos())
                            {
                                if ((fileInfo is FileInfo))
                                {
                                    string fileExtension = Path.GetExtension(fileInfo.FullName);
                                    string filename = Path.GetFileName(fileInfo.FullName);
                                    if (fileExtension == ".png")
                                    {
                                        command = string.Format(" -s {0} push {1} {2}", this._PadInfo.SERIAL.Trim(), fileInfo.FullName, " /mnt/sdcard/gbchd/Meeting" + i.ToString() + @"/pic/");
                                        if (bk.CancellationPending) //这里判断一下是否用户要求取消后台进行，并可以尽早退出。
                                        {
                                            bk.ReportProgress((int)(FileCount * 100 / FileSum), "用户中断操作");
                                        }
                                        else
                                        {
                                            bk.ReportProgress((int)(FileCount * 100 / FileSum), "推送会议议题" + i.ToString() + @"的文件：" + filename);
                                            ExcetiveADBCommand(command);
                                        }
                                        FileCount++;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 差异推送文件
                    for (int i = 1; i <= 8; i++)
                    {
                        List<Guid> imgid = new List<Guid>();
                        string dbpath = Global.ProjectOutputDirectory + @"gbchd\Meeting" + i.ToString() + @"\db\imgLocation.db";
                        string picdic = Global.ProjectOutputDirectory + @"gbchd\Meeting" + i.ToString() + @"\pic\";
                        if (File.Exists(dbpath))
                        {
                            FileSum++;
                            DataRepository dr = new DataRepository(dbpath);
                            List<DW> dws = dr.GetAllDWs();
                            foreach (DW dw in dws)
                            {
                                FileSum += dw.DocumentImageCount;
                                //foreach (DI di in dw.DIS)
                                //{
                                //    if ((di.DIUUID+"").Trim().Length>0&&File.Exists(di.Local_StorgeDocumentImageFullpath))
                                //    {
                                //        FileSum++;
                                //    }
                                //}
                                foreach (GB g in dw.GBS)
                                {
                                    //if (File.Exists(g.LRMIMG))
                                    //{
                                    //    FileSum++;
                                    //}
                                    //if (File.Exists(g.RESIMG))
                                    //{
                                    //    FileSum++;
                                    //}

                                    //20170216 每个干部的图像总数
                                    FileSum += g.LrmImageCount + g.ResImageCount + g.OtherImageCount;
                                }
                            }
                        }
                    }

                    string command = string.Format(" -s {0} shell ls {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/");
                    string dirmessage = ExcetiveADBCommand(command);
                    if (!dirmessage.Contains("Meeting"))
                    {
                        ExcetiveADBCommand(string.Format(" -s {0} shell mkdir {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/"));
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        if (dirmessage.Contains("Meeting" + i.ToString()))
                        {
                            ExcetiveADBCommand(string.Format(" -s {0} shell rm -rf {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/Meeting" + i.ToString() + "/db/"));
                            ExcetiveADBCommand(string.Format(" -s {0} shell mkdir {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/Meeting" + i.ToString() + "/db/"));
                        }
                        else
                        {
                            ExcetiveADBCommand(string.Format(" -s {0} shell mkdir {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/Meeting" + i.ToString() + "/"));
                            ExcetiveADBCommand(string.Format(" -s {0} shell mkdir {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/Meeting" + i.ToString() + "/db/"));
                            ExcetiveADBCommand(string.Format(" -s {0} shell mkdir {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/Meeting" + i.ToString() + "/pic/"));
                        }
                        string dbpath = Global.ProjectOutputDirectory + @"gbchd\Meeting" + i.ToString() + @"\db\imgLocation.db";
                        FileCount++;
                        //更新数据库
                        if (File.Exists(dbpath))
                        {
                            command = string.Format(" -s {0} push {1} {2}", this._PadInfo.SERIAL.Trim(), dbpath, "/mnt/sdcard/gbchd/Meeting" + i.ToString() + @"/db/imgLocation.db");
                            ExcetiveADBCommand(command);
                            FileCount++;

                            command = string.Format(" -s {0} shell ls {1}", this._PadInfo.SERIAL.Trim(), "/mnt/sdcard/gbchd/Meeting" + i.ToString() + "/pic/");
                            string lsmessage = ExcetiveADBCommand(command);
                            bk.ReportProgress((int)(FileCount * 100 / FileSum), "正在读取平板目录 /mnt/sdcard/gbchd/Meeting" + i.ToString() + "/pic/ 内已经存在的文件：\r\n" + lsmessage);
                            DataRepository dr = new DataRepository(dbpath);
                            List<DW> dws = dr.GetAllDWs();
                            foreach (DW dw in dws)
                            {
                                foreach (string imagepath in dw.DocumentImageFileFullPaths)
                                {
                                    if (File.Exists(imagepath))
                                    {
                                        if (lsmessage.Contains(dw.id))
                                        {
                                            bk.ReportProgress((int)(FileCount * 100 / FileSum), "会议议题" + i.ToString() + @"的文件：" + imagepath + "，文件已经存在，不需要推送。");
                                        }
                                        else
                                        {
                                            command = string.Format(" -s {0} push {1} {2}", this._PadInfo.SERIAL.Trim(), imagepath, " /mnt/sdcard/gbchd/Meeting" + i.ToString() + @"/pic/");

                                            if (bk.CancellationPending) //这里判断一下是否用户要求取消后台进行，并可以尽早退出。
                                            {
                                                bk.ReportProgress((int)(FileCount * 100 / FileSum), "用户中断操作");
                                            }
                                            else
                                            {
                                                bk.ReportProgress((int)(FileCount * 100 / FileSum), "推送会议议题" + i.ToString() + @"的文件：" + imagepath);
                                                ExcetiveADBCommand(command);
                                            }
                                        }
                                        FileCount++;
                                    }
                                }
                                foreach (GB g in dw.GBS)
                                {
                                    #region 20170216废弃代码
                                    //if (File.Exists(g.LRMIMG))
                                    //{
                                    //    if (lsmessage.Contains(g.Lrm_Guid))
                                    //    {
                                    //        bk.ReportProgress((int)(FileCount * 100 / FileSum), "会议议题" + i.ToString() + @"的文件：" + g.LRMIMG + "，文件已经存在，不需要推送。");
                                    //    }
                                    //    else
                                    //    {
                                    //        command = string.Format(" -s {0} push {1} {2}", this._PadInfo.SERIAL.Trim(), g.LRMIMG, " /mnt/sdcard/gbchd/Meeting" + i.ToString() + @"/pic/");
                                    //        if (bk.CancellationPending) //这里判断一下是否用户要求取消后台进行，并可以尽早退出。
                                    //        {
                                    //            bk.ReportProgress((int)(FileCount * 100 / FileSum), "用户中断操作");
                                    //        }
                                    //        else
                                    //        {
                                    //            bk.ReportProgress((int)(FileCount * 100 / FileSum), "推送会议议题" + i.ToString() + @"的文件：" + g.LRMIMG);
                                    //            ExcetiveADBCommand(command);
                                    //        }
                                    //    }
                                    //    FileCount++;
                                    //}
                                    //if (File.Exists(g.RESIMG))
                                    //{
                                    //    if (lsmessage.Contains(g.Res_Guid))
                                    //    {
                                    //        bk.ReportProgress((int)(FileCount * 100 / FileSum), "会议议题" + i.ToString() + @"的文件：" + g.RESIMG + "，文件已经存在，不需要推送。");
                                    //    }
                                    //    else
                                    //    {
                                    //        command = string.Format(" -s {0} push {1} {2}", this._PadInfo.SERIAL.Trim(), g.RESIMG, " /mnt/sdcard/gbchd/Meeting" + i.ToString() + @"/pic/");
                                    //        if (bk.CancellationPending) //这里判断一下是否用户要求取消后台进行，并可以尽早退出。
                                    //        {
                                    //            bk.ReportProgress((int)(FileCount * 100 / FileSum), "用户中断操作");
                                    //        }
                                    //        else
                                    //        {
                                    //            bk.ReportProgress((int)(FileCount * 100 / FileSum), "推送会议议题" + i.ToString() + @"的文件：" + g.RESIMG);
                                    //            ExcetiveADBCommand(command);
                                    //        }
                                    //    }
                                    //    FileCount++;
                                    //}
                                    #endregion

                                    //20170216更新图像获取方式同事更新文件推送方式
                                    foreach (string ImagePath in g.AllImageFileFullPaths)
                                    {
                                        if (File.Exists(ImagePath))
                                        {
                                            if (lsmessage.Contains(g.Res_Guid))
                                            {
                                                bk.ReportProgress((int)(FileCount * 100 / FileSum), "会议议题" + i.ToString() + @"的文件：" + ImagePath + "，文件已经存在，不需要推送。");
                                            }
                                            else
                                            {
                                                command = string.Format(" -s {0} push {1} {2}", this._PadInfo.SERIAL.Trim(), ImagePath, " /mnt/sdcard/gbchd/Meeting" + i.ToString() + @"/pic/");
                                                if (bk.CancellationPending) //这里判断一下是否用户要求取消后台进行，并可以尽早退出。
                                                {
                                                    bk.ReportProgress((int)(FileCount * 100 / FileSum), "用户中断操作");
                                                }
                                                else
                                                {
                                                    bk.ReportProgress((int)(FileCount * 100 / FileSum), "推送会议议题" + i.ToString() + @"的文件：" + ImagePath);
                                                    ExcetiveADBCommand(command);
                                                }
                                            }
                                            FileCount++;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                }
            }
        }

        private void BackgroundWorkPushData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressPushData.Value = e.ProgressPercentage * 100;
            this.lblPad.Text = string.Format("正在向平板{0}推送数据：{1}\r\n", this._PadInfo.id, e.UserState.ToString());
            DateTime d = DateTime.Now;
            this.tbOut.AppendText(string.Format("[{0}]正在向平板{1}推送数据：{2}\r\n", d.ToString("yyyy-MM-dd HH:mm:ss"), this._PadInfo.id, e.UserState.ToString()));
            tbOut.Focus();
            tbOut.Select(tbOut.TextLength, 0);
            tbOut.ScrollToCaret();
        }

        private void BackgroundWorkPushData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
