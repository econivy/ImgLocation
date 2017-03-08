using ImgLocation.Models;
using ImgLocation.Repository;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgLocation
{
    public enum MessageType
    {
        Error,
        Warnning,
        Question,
        Infomation,
        Log
    }
    public static  class Global
    {
        public static int ImgWidth;//转换完成后的图像的宽度 像素数
        public static double VerticalCutSize;//裁边毫米数
        public static double HorizontalCutSize;//裁边毫米数
        public static ImageFormat ImgFormat;
        //public static string SearchModel;
        public static bool IsShowWord;
        public static bool IsShowError;
        public static bool IsUsingLrmToImageModel;

        public static void RefreshParams()
        {
            SystemRepository sr = new SystemRepository();
            ImgWidth = 1600;
            ImgFormat = ImageFormat.Png;
            VerticalCutSize = 26;
            HorizontalCutSize = 20;
            VerticalCutSize = 26;
            IsShowWord = sr.ReadSystemConfig(701).Trim().Length > 0 ? sr.ReadSystemConfig(701) == "是" : true;
            IsShowError = sr.ReadSystemConfig(702).Trim().Length > 0 ? sr.ReadSystemConfig(702) == "是" : true;
            IsUsingLrmToImageModel = sr.ReadSystemConfig(801).Trim().Length > 0 ? sr.ReadSystemConfig(801) == "是" : true;
        }

        public static Project LoadDefaultProject()
        {
            Project p = new Project();
            string approot = Directory.GetCurrentDirectory();
            approot = approot.Substring(approot.Length - 1, 1) == @"\" ? approot : approot + @"\";
            SystemRepository sr = new SystemRepository();
            Project[] ps = sr.GetAllProjects().ToArray();
            if (ps.Count() > 0)
            {
                if (sr.ReadSystemConfig(201).Trim().Length > 0 && sr.ExistProject(Convert.ToInt32(sr.ReadSystemConfig(201).Trim())))
                {
                    p = sr.GetProject(Convert.ToInt32(sr.ReadSystemConfig(201).Trim()));
                }
                else
                {
                    p = ps[ps.Count() - 1];
                }
            }
            else
            {
                p = new Project
                {
                    id = 101,
                    TITLE = "默认项目",
                    PATH = approot + @"Project\Project_101\",
                    ADDDATE = DateTime.Now,
                    LASTDATAID = Guid.NewGuid().ToString(),
                };
                sr.AddProject(p);
                sr.WriteSystemConfig(201, "", "101");
                sr.WriteSystemConfig(101, "OutputPath", approot + @"Output\");
            }
            return p;
        }

        public static string ProjectName;
        public static string ProjectDirectory;
        public static string ProjectWordDirectory;//存储整理后的呈批件word文档
        public static string ProjectLrmPicDirectory;//存储整理后的lrm和pic文件
        public static string ProjectResDirectory;

        public static string ProjectTemp;//生成过程中的临时目录
        public static string ProjectTempPDFDirectory;//生成过程中的pdf文件目录
        public static string ProjectTempWordDirectory;//生成过程中的，lrm转制成的word文档目录

        public static string OutputDirectory;//总的输出目录，存储在SystemConfig的101属性中
        public static string ProjectOutputDirectory;//每个项目生成图片和数据存储的具体目录
        public static string ProjectOutputDbDirectory;
        public static string ProjectOutputImgDirectory;

        public static string ProjectPDFTempDirectory;
        public static string ProjectPDFOutputDirectory;

        public static string ProjectOutputDbPath;
        public static string LrmToWordModelPath;//lrm转成图片的word模板路径
        public static string LrmToPDFModelPath;//lrm转成图片的word模板路径
        public static string BlankPDFModelPath;

        public static string ADBProgramPath;
        public static string ADBFile1Path;
        public static string ADBFile2Path;
        public static string ADBlibgccPath;

        public static string ADBTestFile;


        public static void RefreshDirectory(Project p, string meeting)
        {
            SystemRepository sr = new SystemRepository();
            string approot = Directory.GetCurrentDirectory();
            approot = approot.Substring(approot.Length - 1, 1) == @"\" ? approot : approot + @"\";

            if (sr.ReadSystemConfig(101).Trim().Length == 0)
            {
                sr.WriteSystemConfig(101, "OutputPath", approot + @"Output\");
            }
            OutputDirectory = sr.ReadSystemConfig(101);

            ProjectName = p.TITLE;
            ProjectDirectory = p.PATH;
            ProjectWordDirectory = p.PATH + @"word\";
            ProjectLrmPicDirectory = p.PATH + @"lrm\";
            ProjectResDirectory = p.PATH + @"res\";
            ProjectTemp = p.PATH + @"temp\";
            ProjectTempPDFDirectory = ProjectTemp + @"pdf\";
            ProjectTempWordDirectory = ProjectTemp + @"word\";

            ProjectOutputDirectory = OutputDirectory + @"Project_" + p.id + @"\";

            ProjectOutputDbDirectory = ProjectOutputDirectory + @"gbchd\" + meeting.Trim() + @"\db\";
            ProjectOutputDbPath = ProjectOutputDbDirectory + @"imgLocation.db";
            ProjectOutputImgDirectory = ProjectOutputDirectory + @"gbchd\" + meeting.Trim() + @"\pic\";

            ProjectPDFTempDirectory = ProjectTemp + @"wordtopdf\";
            ProjectPDFOutputDirectory = OutputDirectory + @"Project_" + p.id + @"_PDF\";

            LrmToWordModelPath = approot + @"model.docx";
            LrmToPDFModelPath = approot + @"model2.docx";
            BlankPDFModelPath = approot + @"blank.pdf";

            ADBProgramPath = approot + @"adb.exe";
            ADBlibgccPath = approot + @"libgcc_s_dw2-1.dll";
            ADBFile1Path = approot + @"AdbWinApi.dll";
            ADBFile2Path = approot + @"AdbWinUsbApi.dll";
            ADBTestFile = approot + @"online.oln";
            ValidateDirectory();
        }


        public static void ValidateDirectory()
        {
            if (!Directory.Exists(ProjectDirectory))
            {
                Directory.CreateDirectory(ProjectDirectory);
            }
            if (!Directory.Exists(ProjectWordDirectory))
            {
                Directory.CreateDirectory(ProjectWordDirectory);
            }
            if (!Directory.Exists(ProjectLrmPicDirectory))
            {
                Directory.CreateDirectory(ProjectLrmPicDirectory);
            }
            if (!Directory.Exists(ProjectResDirectory))
            {
                Directory.CreateDirectory(ProjectResDirectory);
            }
            if (!Directory.Exists(ProjectTempPDFDirectory))
            {
                Directory.CreateDirectory(ProjectTempPDFDirectory);
            }
            if (!Directory.Exists(ProjectTempWordDirectory))
            {
                Directory.CreateDirectory(ProjectTempWordDirectory);
            }

            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }
            if (!Directory.Exists(ProjectOutputDbDirectory))
            {
                Directory.CreateDirectory(ProjectOutputDbDirectory);
            }
            if (!Directory.Exists(ProjectOutputImgDirectory))
            {
                Directory.CreateDirectory(ProjectOutputImgDirectory);
            }
            if (!Directory.Exists(ProjectPDFTempDirectory))
            {
                Directory.CreateDirectory(ProjectPDFTempDirectory);
            }
            if (!Directory.Exists(ProjectPDFOutputDirectory))
            {
                Directory.CreateDirectory(ProjectPDFOutputDirectory);
            }

            byte[] bsBlank = Properties.Resources.blank;
            File.WriteAllBytes(BlankPDFModelPath, bsBlank);

            byte[] bsModel = Properties.Resources.model;
            File.WriteAllBytes(LrmToWordModelPath, bsModel);

            byte[] bsModel2 = Properties.Resources.model2;
            File.WriteAllBytes(LrmToPDFModelPath, bsModel2);

            if (!File.Exists(ADBProgramPath))
            {
                byte[] bsadb = Properties.Resources.adb;
                File.WriteAllBytes(ADBProgramPath, bsadb);
            }

            if (!File.Exists(ADBlibgccPath))
            {
                byte[] bslib = Properties.Resources.libgcc_s_dw2_1;
                File.WriteAllBytes(ADBlibgccPath, bslib);
            }
            if (!File.Exists(ADBFile1Path))
            {
                byte[] bsAdbWinApi = Properties.Resources.AdbWinApi;
                File.WriteAllBytes(ADBFile1Path, bsAdbWinApi);
            }
            if (!File.Exists(ADBFile2Path))
            {
                byte[] bsAdbWinUsbApi = Properties.Resources.AdbWinUsbApi;
                File.WriteAllBytes(ADBFile2Path, bsAdbWinUsbApi);
            }

            byte[] bsOnline = Properties.Resources.online;
            File.WriteAllBytes(ADBTestFile, bsOnline);

            DataRepository dr = new DataRepository(ProjectOutputDbPath);
            dr.ValidateDatabase();
        }


    }
}
