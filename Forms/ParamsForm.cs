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


            if (sr.ReadSystemConfig(601).Trim().Length > 0)
            {
                radioInitial.Checked = sr.ReadSystemConfig(601).Trim() == "段首查找";
                radioFull.Checked = sr.ReadSystemConfig(601).Trim() == "全文查找";
            }
            else
            {
                radioInitial.Checked = true;
            }

            checkShowWord.Checked = sr.ReadSystemConfig(701).Trim().Length > 0 ? sr.ReadSystemConfig(701) == "是" : true;
            checkChooseSameCadre.Checked = sr.ReadSystemConfig(702).Trim().Length > 0 ? sr.ReadSystemConfig(702) == "是" : true;

        }

        private void checkShowWord_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
