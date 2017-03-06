using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace ImgLocation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ClearRuntimeAndStartApplication();
            Application.Run(new ImgLocationForm());
        }
        static void ClearRuntimeAndStartApplication()
        {
            try
            {
                Process[] pws = Process.GetProcessesByName("WINWORD");
                Process[] pas = Process.GetProcessesByName("Acrobat");
                Process[] paas = Process.GetProcessesByName("Adobe Acrobat");
                if (pws.Length > 0 || pas.Length > 0 ||paas.Length>0)
                {
                    DialogResult dre = MessageBox.Show("部分未关闭的Office Word进程和Adobe Acrobat进程可能会导致数据转换失败，请在使用前关闭所有Word和Acrobat窗口。建议使用自动清理功能清理相关程序进程。\r\r是否自动清理影响程序运行的相关进程？\r\r点击“是”进行自动清理，清理完成后进入程序\r点击“否”不进行清理，直接进入程序\r点击“取消”退出",
                        "使用准备", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dre == DialogResult.Yes)
                    {
                        try
                        {
                            pws = Process.GetProcessesByName("WINWORD");
                            pas = Process.GetProcessesByName("Acrobat");
                            paas = Process.GetProcessesByName("Adobe Acrobat");
                            if (pws.Length > 0)
                            {
                                foreach (Process pc in pws)
                                {
                                    try
                                    {
                                        pc.Kill(); //强制关闭
                                    }
                                    catch (Exception ep)
                                    {
                                        MessageBox.Show("关闭进程[" + pc.ProcessName + "]错误：" + ep.Message);
                                    }
                                }
                                foreach (Process pc in pas)
                                {
                                    try
                                    {
                                        pc.Kill(); //强制关闭
                                    }
                                    catch (Exception ep)
                                    {
                                        MessageBox.Show("关闭进程[" + pc.ProcessName + "]错误：" + ep.Message);
                                    }
                                }
                                foreach (Process pc in paas)
                                {
                                    try
                                    {
                                        pc.Kill(); //强制关闭
                                    }
                                    catch (Exception ep)
                                    {
                                        MessageBox.Show("关闭进程[" + pc.ProcessName + "]错误：" + ep.Message);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("进程关闭错误：" + ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    else if (dre == DialogResult.Cancel)
                    {
                        Application.Exit();
                    }
                }
            }
            catch (Exception ea)
            {
                MessageBox.Show("启动程序失败：" + ea.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}