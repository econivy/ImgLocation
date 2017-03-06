using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgLocation.Repository;
using ImgLocation.Models;

namespace ImgLocation.Forms
{
    public partial class DocumentForm : Form
    {
        SystemRepository sr = new SystemRepository();
        string root = Directory.GetCurrentDirectory();
        public DW document { get; set; }
        public string DocumentsDirectory { get; set; }
        public string LrmDirectory { get; set; }
        public string ResDirectory { get; set; }
        public bool IsConvertFileImage { get; set; }
        public string ConvertPageRange { get; set; }
        public bool IsRelocationCadre { get; set; }

        //public bool IsReloadLrmAndResFiles { get; set; }

        public DocumentForm()
        {
            InitializeComponent();
        }

        public DocumentForm(DW d)
        {
            InitializeComponent();
            document = d;
            tbLrm.Text = sr.ReadSystemConfig(302);
            tbRes.Text = sr.ReadSystemConfig(303);
            tbid.Text = document.id;
            tbXH.Text = document.XH;
            tbMC.Text = document.MC;
            tbWH.Text = document.WH;
            tbWord.Text = document.Local_SourceDocumnetFullpath;
        }

        private void btnLrm_Click(object sender, EventArgs e)
        {
           FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = btnLrm.Text.Trim().Length==0?root: btnLrm.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbLrm.Text = fbd.SelectedPath.Substring(fbd.SelectedPath.Length - 1, 1) == @"\" ? fbd.SelectedPath : fbd.SelectedPath + @"\";
                sr.WriteSystemConfig(302, "", tbLrm.Text);
            }
        }

        private void btnRes_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = btnRes.Text.Trim().Length == 0 ? root : btnRes.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbRes.Text = fbd.SelectedPath.Substring(fbd.SelectedPath.Length - 1, 1) == @"\" ? fbd.SelectedPath : fbd.SelectedPath + @"\";
                sr.WriteSystemConfig(303, "", tbRes.Text);
            }
        }

        private void btnWord_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.FileName = btnWord.Text.Trim().Length == 0 ? root : btnWord.Text;
            fbd.Filter = "Office Word 文档(*.doc;*.docx)|*.doc;*.docx";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                
                tbWord.Text = fbd.FileName;
                string fileName = Path.GetFileNameWithoutExtension(tbWord.Text);
                tbMC.Text = fileName;
            }
         
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnCovert_Click(object sender, EventArgs e)
        {
            if (checkCover.Checked && tbLrm.Text.Trim().Length * tbRes.Text.Trim().Length * tbWord.Text.Trim().Length == 0)
            {
                MessageBox.Show("目录指定不完整！","错误",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else if (!File.Exists(tbWord.Text.Trim()) || !Directory.Exists(tbLrm.Text.Trim())||!Directory.Exists(tbRes.Text))
            {
                MessageBox.Show("指定的文档路径或者目录路径不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                tbPageRange.Text = tbPageRange.Text.Replace("；", ";").Replace("——", "-").Replace("—", "-");

                this.DocumentsDirectory = tbWord.Text.Trim();
                this.LrmDirectory = tbLrm.Text.Trim();
                this.ResDirectory = tbRes.Text.Trim();
                this.document.Local_SourceDocumnetFullpath = DocumentsDirectory;
                this.document.XH = tbXH.Text;
                this.document.MC = tbMC.Text;
                this.document.WH = tbWH.Text;
                this.IsConvertFileImage = checkCover.Checked;
                this.IsRelocationCadre = checkRelocationCadre.Checked;
                this.ConvertPageRange = tbPageRange.Text.Trim();

                //需要判断输入的页码范围是否正确
                if(tbPageRange.Text.Trim().Length>0)
                {
                    bool MatchString = true;
                    string[] RangeArray = 
                        tbPageRange.Text.Contains(";") 
                        ? tbPageRange.Text.Split(';') 
                        : new string[] { tbPageRange.Text };
                    foreach(string RangeString in RangeArray)
                    {
                        if(RangeString.Contains("-"))
                        {
                            string[] RangeBorder = RangeString.Split('-');
                            if(RangeBorder.Length != 2
                                ||!Regex.IsMatch(RangeBorder[0], "^[0-9]+$") 
                                || !Regex.IsMatch(RangeBorder[1], "^[0-9]+$")
                                || Convert.ToInt32(RangeBorder[0]) > Convert.ToInt32(RangeBorder[1])
                                || Convert.ToInt32(RangeBorder[1]) > document.DocumentImageCount)
                            {
                                MatchString = false;
                                break;
                            }
                        }
                        else if(!Regex.IsMatch(RangeString, "^[0-9]+$") || Convert.ToInt32(RangeString) > document.DocumentImageCount)
                        {
                                MatchString = false;
                                break;
                        }
                    }
                    if(MatchString)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("指定的文档页码不存在或者不符合要求，请重新输入！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbPageRange.Text = string.Empty;
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
        private void DocumentForm_Load(object sender, EventArgs e)
        {
            checkCover.Enabled = true;
            checkCover.Checked = false;
            groupCover.Enabled = checkCover.Checked;

            checkRelocationCadre.Checked = false;
            checkRelocationCadre.Enabled = checkCover.Checked;
            groupRelocation.Enabled = checkRelocationCadre.Checked;
        }


        private void checkCover_CheckedChanged(object sender, EventArgs e)
        {
            if(checkCover.Checked)
            {
                checkRelocationCadre.Enabled = true;
            }
            else
            {
                checkRelocationCadre.Checked = false;
                checkRelocationCadre.Enabled = false;
            }
            groupCover.Enabled = checkCover.Checked;
        }

        private void checkRelocationCadre_CheckedChanged(object sender, EventArgs e)
        {
            groupRelocation.Enabled = checkRelocationCadre.Checked;
            if (checkCover.Checked && checkRelocationCadre.Checked)
            {
                tbWH.Enabled = false;
                tbWH.Text = "文号将根据文件内容自动提取！";
            }
            else
            {
                tbWH.Text = this.document.WH;
            }
        }


    }
}
