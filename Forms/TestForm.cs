using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImgLocation.Services;
using ImgLocation.Models;
using System.Xml.Serialization;
using System.IO;
using System.Web.Script.Serialization;

namespace ImgLocation.Forms
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string command = string.Format(" -s {0} shell rm -rf {1}", "520515d7630d111f", "/mnt/sdcard/gbchd");
            var result = ProcessHelper.Run(Global.ADBProgramPath, command);
            ShowResult(result);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var result = ProcessHelper.Run(Global.ADBProgramPath, string.Format(" -s {0} push {1} {2}", "520515d7630d111f",
                @"D:\Projects\ImgLocation\bin\Debug\Output\Project_101\", "/mnt/sdcard"));
            ShowResult(result);
        }


        void ShowResult(ProcessRunResult r)
        {
            textBox1.Text += "————————————————————————————————————————————————\r\n";
            textBox1.Text += r.Success.ToString();
            textBox1.Text += "\r\n";
            textBox1.Text += r.ExitCode;
            textBox1.Text += "\r\n";
            textBox1.Text += r.OutputString;
            textBox1.Text += "\r\n";
            textBox1.Text += r.MoreOutputString;
            textBox1.Text += "\r\n";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var result = ProcessHelper.Run(Global.ADBProgramPath, string.Format(" -s {0} push {1} {2}", "520515d7630d111f",
                 @"D:\Projects\ImgLocation\bin\Debug\Output\Project_101\try.txt", "/mnt/sdcard/try.txt"));
            if (result.ExitCode != 0
                || result.OutputString.Contains("not found"))
            {
                textBox1.Text += "设备未连接";
                textBox1.Text += "\r\n";
            }
            else if (result.ExitCode == 0)
            {
                textBox1.Text += "设备已经连接";
                textBox1.Text += "\r\n";
            }
            ShowResult(result);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filepath = ofd.FileName;
                string picpath = Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath) + ".pic");
                LrmHelper lrm = new LrmHelper();
                Person p = lrm.GetPersonFromLrm(filepath, picpath);
                
                string savepath = Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath)+".xml");
                using (StreamWriter writer = new StreamWriter(savepath))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(Person));
                    xml.Serialize(writer, p);
                    MessageBox.Show("OK!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filepath = ofd.FileName;
                LrmHelper lrm = new LrmHelper();
                Person p = lrm.GetPersonFromLrmx(filepath);
                JavaScriptSerializer j= new JavaScriptSerializer();
                textBox2.Text = new JavaScriptSerializer().Serialize(p);
                pictureBox1.Image = p.ZhaoPian_Image;
            }
        }




    }
}
