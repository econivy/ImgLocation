using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgLocation.Models
{
    public class DW
    {
        public string id { get; set; }
        public string MC { get; set; }  //原MC字段
        public string WH { get; set; }  //原文号字段
        public string XH { get; set; }  //原XH字段
        public int Rank { get; set; }     //原SX字段
        public string DocumentImageFilename { get; set; }  //废弃字段
        public int DocumentImageCount { get; set; }
        public string Local_SourceDocumnetFullpath { get; set; } //原SOURCE字段
        public int PageNumberStopLookCadre { get; set; }//废弃字段 因为DW合成图像不再生成 中组部转换时仍然使用该字段作为请示的识别页面位置
        //public string Document_Guid { get; set; }

        //以下为非存储字段
        public string Local_StorgeDocumentFullpath
        {
            get
            {
                return Global.ProjectWordDirectory + MC + ".docx";
            }
        }
        public string Local_StorgeDocumentPdfFullpath
        {
            get
            {
                return Global.ProjectTempPDFDirectory + MC + ".pdf";
            }

        }     //转为非存储字段

        public string Local_SavePDFForPrintFullpath
        {
            get
            {
                return Global.ProjectPDFOutputDirectory + WH + "——" + MC + @".pdf";
            }
        }

        public string Local_SaveDocumentPdfForCombineFullpath
        {
            get
            {
                return Global.ProjectPDFTempDirectory + WH + @"\000.pdf";
            }
        }

        public List<string> DocumentImageFileFullPaths
        {
            get
            {
                List<string> ImageFilePaths = new List<string>();
                if(this.DocumentImageCount>0)
                {
                    string FilenameWithExtension = Path.GetFileNameWithoutExtension(Path.Combine(Global.ProjectOutputImgDirectory, this.DocumentImageFilename));
                    for (int i = 0; i < this.DocumentImageCount; i++)
                    {
                        ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, string.Format("{0}_{1}.{2}", FilenameWithExtension, i, Global.ImgFormat.ToString().ToLower())));
                    }
                    
                }
                return ImageFilePaths;
            }
        }
        public List<string> DocumentImageFilenames
        {
            get
            {
                List<string> ImageFilePaths = new List<string>();
                if (DocumentImageCount> 0)
                {
                    string FilenameWithExtension = Path.GetFileNameWithoutExtension(Path.Combine(Global.ProjectOutputImgDirectory, this.DocumentImageFilename));
                    for (int i = 0; i < DocumentImageCount; i++)
                    {
                        ImageFilePaths.Add(string.Format("{0}_{1}.{2}", FilenameWithExtension, i, Global.ImgFormat.ToString().ToLower()));
                    }
                }
                return ImageFilePaths;
            }
        }

        public List<GB> GBS { get; set; }

    }
}
