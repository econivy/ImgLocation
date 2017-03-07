using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgLocation.Models
{
    public class GB
    {
        public string id { get; set; }
        public string DWID { get; set; }
        public string XM { get; set; }
        public string Lrm_Guid { get; set; }  
        public string Res_Guid { get; set; }  
        public string Other_Guid { get; set; }  

        public int Rank { get; set; }

        public string LrmImageFilename
        {
            get
            {
                return string.Format("{0}_01_{1}.{2}", XM, Lrm_Guid, Global.ImgFormat.ToString().ToLower());
            }
        }
        public int LrmImageCount { get; set; }

        public string ResImageFilename
        {
            get
            {
                return string.Format("{0}_02_{1}.{2}", XM, Res_Guid, Global.ImgFormat.ToString().ToLower());
            }
        }
        public int ResImageCount { get; set; }

        public string OtherImageFilename
        {
            get
            {
                return string.Format("{0}_03_{1}.{2}", XM, Res_Guid, Global.ImgFormat.ToString().ToLower());
            }
        }
        public int OtherImageCount { get; set; }

        public int DocumentImagePageNumber { get; set; }    
        public string DocumentImageFilename { get; set; } 

        public double TouchStartPointY { get; set; }
        public double TouchEndPointY { get; set; }
        public double TouchStartPointX { get; set; } 
        public double TouchEndPointX { get; set; } 

        public double TouchStartPointY2 { get; set; }
        public double TouchEndPointY2 { get; set; }
        public double TouchStartPointX2 { get; set; }
        public double TouchEndPointX2 { get; set; }

        public string Local_SourceLrmFullpath { get; set; } 
        public string Local_SourcePicFullpath { get; set; }
        public string Local_SourceResFullpath { get; set; } 
        public string Local_SourceOtherFullpath { get; set; }

        public string Local_StorgeLrmFullpath
        {
            get
            {
                return Global.ProjectLrmPicDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + Path.GetExtension(Local_SourceLrmFullpath);
            }
        }
        public string Local_StorgePicFullpath
        {
            get
            {
                return Global.ProjectLrmPicDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + Path.GetExtension(Local_SourcePicFullpath);
            }
        }
        public string Local_StorgeResFullpath
        {
            get
            {
                return Global.ProjectResDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + "的考察材料" + Path.GetExtension(Local_SourceResFullpath);
            }
        }
        public string Local_StorgeOtherFullpath
        {
            get
            {
                return Global.ProjectResDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + "的其他文件" + Path.GetExtension(Local_SourceOtherFullpath);
            }
        }

        public string Local_StorgeDocumentWordFullpath
        {
            get { return Global.ProjectTempWordDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + ".docx"; }
        } 

        public string Local_StorgeLrmPdfFullpath
        {
            get
            {
                return Global.ProjectTempPDFDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + ".pdf";
            }
        }
        public string Local_StorgeResPdfFullPath
        {
            get
            {
                return Global.ProjectTempPDFDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + "的考察材料.pdf";
            }
        }
        public string Local_StorgeOtherPdfFullpath
        {
            get
            {
                return Global.ProjectTempPDFDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + "的其他文件.pdf";
            }
        }

        /// <summary>
        /// 非存储字段
        /// 所有的Lrm图像的存放位置
        /// </summary>
        public List<string> LrmImageFileFullPaths
        {
            get
            {
                List<string> ImageFilePaths = new List<string>();
                if (this.LrmImageCount > 0)
                {
                    string FilenameWithExtension = Path.GetFileNameWithoutExtension(Path.Combine(Global.ProjectOutputImgDirectory, this.LrmImageFilename));
                    for (int i = 0; i < LrmImageCount; i++)
                    {
                        ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, string.Format("{0}_{1}.{2}", FilenameWithExtension, i, Global.ImgFormat.ToString().ToLower())));
                    }
                }
                return ImageFilePaths;
            }
        }

        /// <summary>
        /// 非存储字段
        /// 所有的Res图像的存放位置
        /// </summary>
        public List<string> ResImageFileFullPaths
        {
            get
            {
                List<string> ImageFilePaths = new List<string>();
                if (this.ResImageCount >0)
                {
                    string FilenameWithExtension = Path.GetFileNameWithoutExtension(Path.Combine(Global.ProjectOutputImgDirectory, this.ResImageFilename));
                    for (int i = 0; i < ResImageCount; i++)
                    {
                        ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, string.Format("{0}_{1}.{2}", FilenameWithExtension, i, Global.ImgFormat.ToString().ToLower())));
                    }
                }
                return ImageFilePaths;
            }
        }

        /// <summary>
        /// 非存储字段
        /// 所有的Other图像的存放位置
        /// </summary>
        public List<string> OtherImageFileFullPaths
        {
            get
            {
                List<string> ImageFilePaths = new List<string>();
                if (this.OtherImageCount > 0)
                {
                    string FilenameWithExtension = Path.GetFileNameWithoutExtension(Path.Combine(Global.ProjectOutputImgDirectory, this.OtherImageFilename));
                    for (int i = 0; i < OtherImageCount; i++)
                    {
                        ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, string.Format("{0}_{1}.{2}", FilenameWithExtension, i, Global.ImgFormat.ToString().ToLower())));
                    }
                }
                return ImageFilePaths;
            }
        }

        public List<string> AllImageFileFullPaths
        {
            get
            {
                List<string> ImageFilePaths = new List<string>();
                if(this.LrmImageCount>0)
                {
                    ImageFilePaths = ImageFilePaths.Union(this.LrmImageFileFullPaths).ToList();
                }
                if(this.ResImageCount>0)
                {
                    ImageFilePaths = ImageFilePaths.Union(this.ResImageFileFullPaths).ToList();
                }
                if (this.OtherImageCount > 0)
                {
                    ImageFilePaths = ImageFilePaths.Union(this.OtherImageFileFullPaths).ToList();
                }
                return ImageFilePaths;
            }
        }

    }

}
