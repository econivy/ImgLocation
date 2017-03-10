using ImgLocation.Models;
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
    public partial class CadreForm : Form
    {
        public GB g {get;set;}
        public bool isLrm { get; set; }
        public bool isRes { get; set; }
        public bool isOther { get; set; }
        public CadreForm()
        {
            InitializeComponent();
            //groupKCCL.Enabled = checkSPB.Checked;
        }
        public CadreForm(GB gb)
        {
            InitializeComponent();

            this.g = gb;
            lbllrm.Text = g.Local_SourceLrmFullpath;
            lblpic.Text = g.Local_SourcePicFullpath;
            lblres.Text = g.Local_SourceResFullpath;
            if (g.Local_SourceLrmFullpath.Trim().Length > 0)
            {
                checkSPB.Checked = true;
            }
            if (g.Local_SourceResFullpath.Trim().Length > 0)
            {
                checkKCCL.Checked = true;
            }
            if((g.Local_SourceOtherFullpath + string.Empty).Trim().Length>0)
            {
                checkOther.Checked = true;
            }
            groupKCCL.Enabled = checkKCCL.Checked;
            gSPB.Enabled = checkSPB.Checked;
            groupOther.Enabled = checkOther.Checked;
            this.isRes = checkKCCL.Checked;
            this.isLrm = checkSPB.Checked;
            this.isOther = checkOther.Checked;
        }


        private void checkLrm_CheckedChanged(object sender, EventArgs e)
        {
            gSPB.Enabled = checkSPB.Checked;
            this.isLrm = checkSPB.Checked;

        }
        private void checkRes_CheckedChanged(object sender, EventArgs e)
        {
            groupKCCL.Enabled = checkKCCL.Checked;
            this.isRes = checkKCCL.Checked;
        }

        private void checkOther_CheckedChanged(object sender, EventArgs e)
        {
            groupOther.Enabled = checkOther.Checked;
            this.isOther = checkOther.Checked;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ///TODO 手工添加干部时，应当在这里就打开任免表获取干部姓名
            bool exist_lrm = !isLrm;
            bool exist_res = !isRes;
            bool exist_other = !isOther;
            if (isLrm)
            {
                g.Local_SourceLrmFullpath = lbllrm.Text.Trim();
                g.Local_SourcePicFullpath = lblpic.Text.Trim();
                exist_lrm = ((Path.GetExtension(g.Local_SourceLrmFullpath) == ".lrm" && File.Exists(g.Local_SourceLrmFullpath) && File.Exists(g.Local_SourcePicFullpath)) || (Path.GetExtension(g.Local_SourceLrmFullpath) == ".lrmx" && File.Exists(g.Local_SourceLrmFullpath)));
                LrmHelper lr = new LrmHelper();
                Person person = lr.GetPersonFromLrmFile(g.Local_SourceLrmFullpath);//此时GB对象的XM尚未赋值，因此不能使用Storge属性。
                g.XM = person.XingMing;
            }
            else
            {
                g.Local_SourceLrmFullpath = "";
                g.Local_SourcePicFullpath ="";
            }
            if (isRes)
            {
                g.Local_SourceResFullpath = lblres.Text.Trim();
                exist_res = File.Exists(lblres.Text.Trim());
            }
            else
            {
                g.Local_SourceResFullpath = "";
            }
            if(isOther)
            {
                g.Local_SourceOtherFullpath = lblOther.Text.Trim();
                exist_other = File.Exists(lblOther.Text.Trim());
            }
            else
            {
                g.Local_SourceOtherFullpath = "";
            }
            if (exist_lrm&&exist_other&&exist_other)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("指定的文档路径或者目录路径不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnlrm_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.Filter = "任免审批表文档(*.lrm;*.lrmx)|*.lrm;*.lrmx";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                lbllrm.Text = fbd.FileName;
                if(Path.GetExtension(fbd.FileName)==".lrm"&&File.Exists(fbd.FileName.Substring(0, fbd.FileName.Length - 4) + @".pic"))
                {
                      lblpic.Text = fbd.FileName.Substring(0, fbd.FileName.Length - 4) + @".pic";
                }
                else
                {
                    lblpic.Text ="";
                }
            }
        }

        private void btnpic_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.Filter = "任免审批表文档(*.pic)|*.pic";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                lblpic.Text = fbd.FileName;
            }
        }

        private void btnres_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.Filter = "Office Word 文档(*.doc;*.docx)|*.doc;*.docx";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                lblres.Text = fbd.FileName;
            }
        }

        private void btnOther_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.Filter = "XPS 文档(*.xps)|*.xps";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                lblOther.Text = fbd.FileName;
            }
        }

        private void CadreForm_Load(object sender, EventArgs e)
        {

        }

    }
}
