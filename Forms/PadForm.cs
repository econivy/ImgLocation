using ImgLocation.Models;
using ImgLocation.Repository;
using ImgLocation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImgLocation.Forms
{
    public partial class PadForm : Form
    {
        public PadForm()
        {
            InitializeComponent();
        }
        public PadInfo[] PadInfos { get; set; }
        private void button2_Click(object sender, EventArgs e)
        {
            var r = ProcessHelper.Run(Global.ADBProgramPath, string.Format(" get-serialno"));
            tbSerial.Text += "********************************************\r\n";
            tbSerial.Text += r.ToString();
        }

        private void PadForm_Load(object sender, EventArgs e)
        {
            SystemRepository sr = new SystemRepository();
            PadInfos = new PadInfo[30];
            for (int i = 0; i < 30; i++)
            {
                PadInfo p = sr.GetPadInfo(i + 1);
                if (p != null)
                {
                    PadInfos[i] = p;
                }
                else
                {
                    PadInfos[i] = new PadInfo { id = i + 1 };
                }
            }

            textBox1.Text = PadInfos[0].SERIAL;
            textBox2.Text = PadInfos[1].SERIAL;
            textBox3.Text = PadInfos[2].SERIAL;
            textBox4.Text = PadInfos[3].SERIAL;
            textBox5.Text = PadInfos[4].SERIAL;
            textBox6.Text = PadInfos[5].SERIAL;
            textBox7.Text = PadInfos[6].SERIAL;
            textBox8.Text = PadInfos[7].SERIAL;
            textBox9.Text = PadInfos[8].SERIAL;
            textBox10.Text = PadInfos[9].SERIAL;
            textBox11.Text = PadInfos[10].SERIAL;
            textBox12.Text = PadInfos[11].SERIAL;
            textBox13.Text = PadInfos[12].SERIAL;
            textBox14.Text = PadInfos[13].SERIAL;
            textBox15.Text = PadInfos[14].SERIAL;
            textBox16.Text = PadInfos[15].SERIAL;
            textBox17.Text = PadInfos[16].SERIAL;
            textBox18.Text = PadInfos[17].SERIAL;
            textBox19.Text = PadInfos[18].SERIAL;
            textBox20.Text = PadInfos[19].SERIAL;
            textBox21.Text = PadInfos[20].SERIAL;
            textBox22.Text = PadInfos[21].SERIAL;
            textBox23.Text = PadInfos[22].SERIAL;
            textBox24.Text = PadInfos[23].SERIAL;
            textBox25.Text = PadInfos[24].SERIAL;
            textBox26.Text = PadInfos[25].SERIAL;
            textBox27.Text = PadInfos[26].SERIAL;
            textBox28.Text = PadInfos[27].SERIAL;
            textBox29.Text = PadInfos[28].SERIAL;
            textBox30.Text = PadInfos[29].SERIAL;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PadInfos[0].SERIAL = textBox1.Text;
            PadInfos[1].SERIAL = textBox2.Text;
            PadInfos[2].SERIAL = textBox3.Text;
            PadInfos[3].SERIAL = textBox4.Text;
            PadInfos[4].SERIAL = textBox5.Text;
            PadInfos[5].SERIAL = textBox6.Text;
            PadInfos[6].SERIAL = textBox7.Text;
            PadInfos[7].SERIAL = textBox8.Text;
            PadInfos[8].SERIAL = textBox9.Text;
            PadInfos[9].SERIAL = textBox10.Text;
            PadInfos[10].SERIAL = textBox11.Text;
            PadInfos[11].SERIAL = textBox12.Text;
            PadInfos[12].SERIAL = textBox13.Text;
            PadInfos[13].SERIAL = textBox14.Text;
            PadInfos[14].SERIAL = textBox15.Text;
            PadInfos[15].SERIAL = textBox16.Text;
            PadInfos[16].SERIAL = textBox17.Text;
            PadInfos[17].SERIAL = textBox18.Text;
            PadInfos[18].SERIAL = textBox19.Text;
            PadInfos[19].SERIAL = textBox20.Text;

            PadInfos[20].SERIAL = textBox21.Text;
            PadInfos[21].SERIAL = textBox22.Text;
            PadInfos[22].SERIAL = textBox23.Text;
            PadInfos[23].SERIAL = textBox24.Text;
            PadInfos[24].SERIAL = textBox25.Text;
            PadInfos[25].SERIAL = textBox26.Text;
            PadInfos[26].SERIAL = textBox27.Text;
            PadInfos[27].SERIAL = textBox28.Text;
            PadInfos[28].SERIAL = textBox29.Text;
            PadInfos[29].SERIAL = textBox30.Text;
            bool con = true;
            for (int i = 0; i < 30; i++)
            {
                for (int j = i + 1; j < 30; j++)
                {
                    if ((PadInfos[i].SERIAL+"").Trim().Length>0&&PadInfos[i].SERIAL == PadInfos[j].SERIAL)
                    {
                        MessageBox.Show(string.Format("平板{0}的序列号与平板{1}的序列号相同，无法保存！", PadInfos[i].id, PadInfos[j].id), 
                            "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        con = false;
                    }
                }
            }
            if (con)
            {
                for (int i = 0; i < 30; i++)
                {
                    SystemRepository sr = new SystemRepository();
                    sr.DeletePadInfo(i + 1);
                    sr.AddPadInfo(PadInfos[i]);
                }
               if(DialogResult.Yes== MessageBox.Show(string.Format("平板数据保存成功,是否关闭平板管理窗口？"), "完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
               {
                   this.DialogResult = DialogResult.Cancel;
                   this.Close();
               }
            }
        }
    }
}
