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
            List<string> LrmxPaths = new List<string>();
            DirectoryInfo rootDirectoryInfos = new DirectoryInfo(@"D:\Projects\20170114\特殊任免表\");
            foreach (FileSystemInfo rootFileInfo in rootDirectoryInfos.GetFileSystemInfos())
            {
                if ((rootFileInfo is FileInfo))
                {
                    string fileName = Path.GetFileNameWithoutExtension(rootFileInfo.FullName);
                    string fileExtension = Path.GetExtension(rootFileInfo.FullName);
                    if (fileExtension == ".lrmx" )
                    {
                        LrmxPaths.Add(rootFileInfo.FullName);
                    }
                }
            }
            List<string> paths = new List<string>();
            int count = 10;
            Random r = new Random();
            while(count>0)
            {
                paths.Add(LrmxPaths[r.Next(LrmxPaths.Count)]);
                count--;
            }
            List<Image> images = new List<Image>();
            foreach( string path in paths)
            {
                LrmHelper l = new LrmHelper();
                LrmToImage lti = new LrmToImage();
                lti.person = l.GetPersonFromLrmFile(path);
                Bitmap p = lti.CreateImage();
                images.Add(p);
            }
            int h = images.Sum(i => i.Height);
            int w = images[0].Width;

            Bitmap image = new Bitmap(w,h);
            Graphics g = Graphics.FromImage(image);
            int sh = 0;
            for(int k=0;k<images.Count;k++)
            {
                g.DrawImage(images[k], 0, sh);
                sh +=images[k].Height;
            }
            pictureBox1.Image = image;
        }
    }
}
