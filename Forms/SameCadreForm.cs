using ImgLocation.Models;
using ImgLocation.Services;
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
    public partial class SameCadreForm : Form
    {
        public SameCadreForm()
        {
            InitializeComponent();
        }
        public GB Converting_GB { get; set; }
        //public string LrmDirectory { get; set; }
        //public string ResDirectory { get; set; }
        public List<PersonWithFile> GB_LrmPersons { get; set; }
        public List<DocumentWithFile> GB_DocContents { get; set; }
        private void SameCadreForm_Load(object sender, EventArgs e)
        {
            GBXM.Text = "干部姓名：" + Converting_GB.XM;

            for(int i = 0; i < 10; i++)
            {
                if (GB_LrmPersons.Count>i)
                {
                    tableLayoutPanel2.Controls[("lrm" + (i+1))].Text = GB_LrmPersons[i].IsLrmx?GB_LrmPersons[i].LrmxFilename:GB_LrmPersons[i].LrmFilename;
                    tableLayoutPanel2.Controls[("lrmc" + (i + 1))].Text = "现任职务：" + GB_LrmPersons[i].XianRenZhiWu + "\r\n" + "拟任职务：" + GB_LrmPersons[i].NiRenZhiWu + "\r\n" + "拟免职务：" + GB_LrmPersons[i].NiMianZhiWu;
                }
                else
                {
                    tableLayoutPanel2.Controls[("lrm" + (i + 1))].Enabled = false;
                    tableLayoutPanel2.Controls[("lrmc" + (i + 1))].Enabled = false;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (GB_DocContents.Count > i)
                {
                    tableLayoutPanel3.Controls[("doc" + (i + 1))].Text = GB_DocContents[i].DocxFilename;
                    tableLayoutPanel3.Controls[("docc" + (i + 1))].Text = GB_DocContents[i].Content;
                }
                else
                {
                    tableLayoutPanel3.Controls[("doc" + (i + 1))].Enabled = false;
                    tableLayoutPanel3.Controls[("docc" + (i + 1))].Enabled = false;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for(int i=0;i<10;i++)
            {
                RadioButton r= (RadioButton)tableLayoutPanel2.Controls[("lrm" + (i + 1))];
                if(r.Checked)
                {
                    this.Converting_GB.Local_SourceLrmFullpath = GB_LrmPersons[i].IsLrmx ? GB_LrmPersons[i].LrmxFullPath : GB_LrmPersons[i].LrmFullPath;
                    this.Converting_GB.Local_SourcePicFullpath = GB_LrmPersons[i].IsLrmx ? "" : GB_LrmPersons[i].PicFullPath;
                }
            }
            for(int i=0;i<5;i++)
            {
                RadioButton r = (RadioButton)tableLayoutPanel3.Controls[("doc" + (i + 1))];
                if (r.Checked)
                {
                    this.Converting_GB.Local_SourceResFullpath = GB_DocContents[i].DocxFullPath;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
