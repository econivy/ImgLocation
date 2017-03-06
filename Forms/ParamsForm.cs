using ImgLocation.Repository;
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
    public partial class ParamsForm : Form
    {
        public ParamsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SystemRepository sr = new SystemRepository();
            if (radioInitial.Checked)
            {
                sr.WriteSystemConfig(601, "SearchModel", "段首查找");
            }
            else
            {
                sr.WriteSystemConfig(601, "SearchModel", "全文查找");
            }

            if (radioZZB.Checked)
            {
                sr.WriteSystemConfig(602, "ModelType", "中央组织部模板");
            }
            else
            {
                sr.WriteSystemConfig(602, "ModelType", "省委组织部模板");
            }

            if (checkAddRedTitle.Checked)
            {
                sr.WriteSystemConfig(603, "RedTitle", "是");
            }
            else
            {
                sr.WriteSystemConfig(603, "RedTitle", "否");
            }

            if (checkAttachLrmRes.Checked)
            {
                sr.WriteSystemConfig(604, "AttachLrmRes", "是");
            }
            else
            {
                sr.WriteSystemConfig(604, "AttachLrmRes", "否");
            }

            if (checkShowWord.Checked)
            {
                sr.WriteSystemConfig(701, "ShowWord", "是");
            }
            else
            {
                sr.WriteSystemConfig(701, "ShowWord", "否");
            }

            if (checkChooseSameCadre.Checked)
            {
                sr.WriteSystemConfig(702, "ChooseSameCadre", "是");
            }
            else
            {
                sr.WriteSystemConfig(702, "ChooseSameCadre", "否");
            }
            sr.WriteSystemConfig(811, "",tbM1Left.Text);
            sr.WriteSystemConfig(812, "", tbM1Top.Text);
            sr.WriteSystemConfig(821, "", tbM2Left.Text);
            sr.WriteSystemConfig(822, "", tbM2Top.Text);
            sr.WriteSystemConfig(831, "", tbM3Left.Text);
            sr.WriteSystemConfig(832, "", tbM3Top.Text);

            Global.RefreshParams();
            Global.ValidateDirectory();
            if (MessageBox.Show("参数设置成功，是否关闭当前页面？") == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ParamsForm_Load(object sender, EventArgs e)
        {
            SystemRepository sr = new SystemRepository();

            //外置文件强制中央组织部模板
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "zzb.ini")))
            {
                sr.WriteSystemConfig(602, "ModelType", "中央组织部模板");
                this.radioSWZZB.Visible = false;
                this.radioZZB.Visible = false;
                this.radioZZB.Checked = true;
            }


            if (sr.ReadSystemConfig(601).Trim().Length > 0)
            {
                radioInitial.Checked = sr.ReadSystemConfig(601).Trim() == "段首查找";
                radioFull.Checked = sr.ReadSystemConfig(601).Trim() == "全文查找";
            }
            else
            {
                radioInitial.Checked = true;
            }
            if (sr.ReadSystemConfig(602).Trim().Length > 0)
            {
                radioZZB.Checked = sr.ReadSystemConfig(602).Trim() == "中央组织部模板";
                radioSWZZB.Checked = sr.ReadSystemConfig(602).Trim() == "省委组织部模板";
            }
            else
            {
                radioSWZZB.Checked = true;
            }
            checkAddRedTitle.Checked = sr.ReadSystemConfig(603).Trim() == "是";
            checkAddRedTitle.Enabled = radioZZB.Checked;
            checkAttachLrmRes.Checked = sr.ReadSystemConfig(604).Trim() == "是";
            checkAttachLrmRes.Enabled = radioZZB.Checked;
            groupRedModel.Enabled = radioZZB.Checked;

            checkShowWord.Checked = sr.ReadSystemConfig(701).Trim().Length > 0 ? sr.ReadSystemConfig(701) == "是" : true;
            checkChooseSameCadre.Checked = sr.ReadSystemConfig(702).Trim().Length > 0 ? sr.ReadSystemConfig(702) == "是" : true;
            
            tbM1Left.Text = sr.ReadSystemConfig(811).Trim();
            tbM1Top.Text = sr.ReadSystemConfig(812).Trim();
            tbM2Left.Text = sr.ReadSystemConfig(821).Trim();
            tbM2Top.Text = sr.ReadSystemConfig(822).Trim();
            tbM3Left.Text = sr.ReadSystemConfig(831).Trim();
            tbM3Top.Text = sr.ReadSystemConfig(832).Trim();
        }

        private void radioZZB_CheckedChanged(object sender, EventArgs e)
        {
            SystemRepository sr = new SystemRepository();
            if(radioZZB.Checked)
            {
                checkAddRedTitle.Enabled = true;
                checkAttachLrmRes.Enabled = true;
                groupRedModel.Enabled = true;
            }
            else
            {
                checkAddRedTitle.Checked = false;
                checkAddRedTitle.Enabled = false;
                checkAttachLrmRes.Checked = false;
                checkAttachLrmRes.Enabled = false;
                groupRedModel.Enabled = false;
                sr.WriteSystemConfig(603, "RedTitle", "否");
                sr.WriteSystemConfig(604, "AttachLrmRes", "否");
            }
        }
    }
}
