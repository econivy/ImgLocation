
using ImgLocation.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgLocation.Forms
{
    public partial class FolderForm : Form
    {
        SystemRepository sr = new SystemRepository();
        public string sworddir { get; set; }
        public string slrmdir { get; set; }
        public string sresdir { get; set; }
        public bool isadd { get; set; }

        public FolderForm()
        {
            InitializeComponent();
            tbWord.Text = sr.ReadSystemConfig(301);
            tbLrm.Text = sr.ReadSystemConfig(302);
            tbRes.Text = sr.ReadSystemConfig(303);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnConvert_Click(object sender, EventArgs e)
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
            Global.RefreshParams();

            if (tbLrm.Text.Trim().Length * tbRes.Text.Trim().Length * tbWord.Text.Trim().Length > 0)
            {
                this.sworddir = tbWord.Text.Trim();
                this.slrmdir = tbLrm.Text.Trim();
                this.sresdir = tbRes.Text.Trim();
                this.isadd = RadioAdd.Checked;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("目录指定不完整！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnWord_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tbWord.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbWord.Text = fbd.SelectedPath.Substring(fbd.SelectedPath.Length - 1, 1) == @"\"? fbd.SelectedPath: fbd.SelectedPath + @"\";
                sr.WriteSystemConfig(301, "", tbWord.Text);
            }
        }

        private void btnLrm_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tbWord.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbLrm.Text = fbd.SelectedPath.Substring(fbd.SelectedPath.Length - 1, 1) == @"\" ? fbd.SelectedPath : fbd.SelectedPath + @"\";
                sr.WriteSystemConfig(302, "", tbLrm.Text);
            }
        }

        private void btnRes_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tbWord.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbRes.Text = fbd.SelectedPath.Substring(fbd.SelectedPath.Length - 1, 1) == @"\" ? fbd.SelectedPath : fbd.SelectedPath + @"\";
                sr.WriteSystemConfig(303, "", tbRes.Text);
            }
        }

        private void FolderForm_Load(object sender, EventArgs e)
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
        }
    }
}
