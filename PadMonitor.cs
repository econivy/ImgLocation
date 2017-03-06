using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImgLocation.Repository;
using ImgLocation.Models;
using ImgLocation.Services;
using ImgLocation.Forms;

namespace ImgLocation
{
    public partial class PadMonitor : UserControl
    {
        public PadMonitor()
        {
            InitializeComponent();
        }
        public int No { get; set; }
        public string Serial { get; set; }
        public PadInfo _PadInfo { get; set; }
        public bool ExistInfo { get; set; }
        public bool PadIsOnline { get; set; }
        public bool PadIsSendedData { get; set; }
        public bool IsMonitor { get; set; }
        public bool PushAllFile { get; set; }
        
        private void PadMonitor_Load(object sender, EventArgs e)
        {
            imgPlugined.Image = Imgs.Images[0];
            imgPushedData.Image = Imgs.Images[3];
            lblNo.Text = No.ToString();
            IsMonitor = false;

        }
        public void LoadPadInfo()
        {
            SystemRepository sr = new SystemRepository();
            this.ExistInfo = sr.ExistPadInfo(No);
            if (this.ExistInfo)
            {
                _PadInfo = sr.GetPadInfo(No);
                if ((_PadInfo.SERIAL + "").Trim().Length == 0)
                {
                    this.ExistInfo = false;
                }
            }
        }
    
        public void BeginMonitor()
        {
            if(!IsMonitor)
            {
                bkpm.RunWorkerAsync();
                this.IsMonitor = true;
            }
        }
        public void EndMonitor()
        {
            bkpm.CancelAsync();
        }

        public string PushData(bool IsPushAllFile)
        {
            this.PushAllFile = IsPushAllFile;
            string r = "";
            SystemRepository sr = new SystemRepository();
            TestOnline();
            
            if (this.ExistInfo && this.PadIsOnline)
            {
                try
                {
                    PushDataForm pdf = new PushDataForm(this._PadInfo);
                    pdf.IsPushAllFile = this.PushAllFile;
                    if (DialogResult.OK == pdf.ShowDialog())
                    {
                        Project p =Global.LoadDefaultProject();
                        this._PadInfo.DATAID = p.LASTDATAID;
                        sr.EditPadInfo(this._PadInfo);
                        LoadPadInfo();
                        r = string.Format("平板{0}推送{1}已完成。", this.No,this.PushAllFile?"全部数据":"差异数据");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("平板{0}推送数据发生错误：{1}", this.No.ToString(), ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    r = string.Format("平板{0}推送数据发生错误：{1}", this.No.ToString(), ex.Message);
                }
            }
            else if(this.PadIsSendedData)
            {
                 r= string.Format("平板{0}未连接，但数据已经推送完成。",this.No);
            }
            else
            {
                r= string.Format("平板{0}配置信息不存在或者未连接。", this.No);
            }
            bkpm.RunWorkerAsync();
            return r;
        }

        public bool PushDataAysc()
        {
            //TODO:异步推送进程。
            return false;
        }

        private void lblNo_Click(object sender, EventArgs e)
        {

        }



        private void bkpm_DoWork(object sender, DoWorkEventArgs e)
        {
            bkpm.ReportProgress(99);
            LoadPadInfo();
            SystemRepository sr = new SystemRepository();
            Project p = Global.LoadDefaultProject();

            if (this.ExistInfo)
            {
                if (this._PadInfo.DATAID == p.LASTDATAID && (this._PadInfo.DATAID + "").Trim().Length > 0 && (p.LASTDATAID + "").Trim().Length > 0)
                {
                    bkpm.ReportProgress(4);
                }
                else
                {
                    bkpm.ReportProgress(3);
                }
            }
            if (this.ExistInfo)
            {
                var result = ProcessHelper.Run(Global.ADBProgramPath, string.Format(" -s {0} push {1} {2}", _PadInfo.SERIAL, Global.ADBTestFile, "/mnt/sdcard/online.oln"));
                if (!result.Success
                    || result.ExitCode != 0
                    || (result.OutputString != null && result.OutputString.Contains("failed"))
                    || (result.OutputString != null && result.OutputString.Contains("not found")))
                {
                    bkpm.ReportProgress(1);
                }
                else
                {
                    bkpm.ReportProgress(2);
                }
            }
            else
            {
                bkpm.ReportProgress(0);
            }
            bkpm.ReportProgress(100);
        }

        private void bkpm_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 0:
                    this.PadIsOnline = false;
                    imgPlugined.Image = Imgs.Images[0];
                    break;
                case 1:
                    this.PadIsOnline = false;
                    imgPlugined.Image = Imgs.Images[1];
                    break;
                case 2:
                    this.PadIsOnline = true;
                    imgPlugined.Image = Imgs.Images[2];
                    break;
                case 3:
                    this.PadIsSendedData = false;
                    this.imgPushedData.Image = Imgs.Images[3];
                    break;
                case 4:
                    this.PadIsSendedData = true;
                    this.imgPushedData.Image = Imgs.Images[4];
                    break;
                case 99:
                    this.Enabled = false;
                    break;
            }
        }

        private void bkpm_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            this.IsMonitor = false;
        }

        public void TestOnline()
        {
            LoadPadInfo();
            if (this.ExistInfo)
            {
                var result = ProcessHelper.Run(Global.ADBProgramPath, string.Format(" -s {0} push {1} {2}", _PadInfo.SERIAL, Global.ADBTestFile, "/mnt/sdcard/online.oln"));
                if (!result.Success
                    || result.ExitCode != 0
                    || (result.OutputString != null && result.OutputString.Contains("failed"))
                    || (result.OutputString != null && result.OutputString.Contains("not found")))
                {
                    imgPlugined.Image = Imgs.Images[1];
                    this.PadIsOnline = false;
                }
                else
                {
                    imgPlugined.Image = Imgs.Images[2];
                    this.PadIsOnline = true;
                }
            }
            else
            {
                imgPlugined.Image = Imgs.Images[0];
                this.PadIsOnline = false;
            }
        }

        private void imgPushedData_Click(object sender, EventArgs e)
        {
            TestOnline();
            if (!this.ExistInfo)
            {
                MessageBox.Show(string.Format("平板{0}的信息不存在，无法找到指定平板，请到“平板管理”界面进行修改！", this.No.ToString()), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!this.PadIsOnline)
            {
                MessageBox.Show(string.Format("平板{0}未连接，请将平板用数据线连接到此计算机！", this.No.ToString()), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(PushData(true), "数据推送结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
