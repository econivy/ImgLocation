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

namespace ImgLocation.UnitTest
{
    public partial class TestLrmToImage : Form
    {
        public TestLrmToImage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LrmHelper l = new LrmHelper();
            LrmToImage lti = new LrmToImage();
            lti.person= l.GetPersonFromLrmFile(@"D:\Projects\20170114\测试任免表绘制图像\毕宪顺.lrmx");
            Image p = lti.CreateImage();
            pictureBox1.Image = p;
        }
    }
}
