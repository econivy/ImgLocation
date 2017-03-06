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
        //public string DIID { get; set; } //原DIID
        public string XM { get; set; }
        public string Lrm_Guid { get; set; }   //原LRMUUID字段，用来识别文件推送
        public string Res_Guid { get; set; }   //原RESUUID考察材料的推送识别id
        public string Other_Guid { get; set; }   //新增字段 其他文件的推送识别id

        public int Rank { get; set; }   //原SX

        public string LrmImageFilename { get; set; }   //原SPB  任免表相对路径文件名 SPBSL=1时可直接获取到文件 >1时需要转换
        public int LrmImageCount { get; set; }    //元SPBSL 任免审批表数量

        public string ResImageFilename { get; set; }  //原KCCL 考察材料相对路径文件名 SPBSL=1时可直接获取到文件 >1时需要转换
        public int ResImageCount { get; set; }   //原KCCLSL 考察材料数量

        public string OtherImageFilename { get; set; } //新增字段
        public int OtherImageCount { get; set; } //新增字段

        //public double S { get; set; }  //废弃字段
        //public double E { get; set; }  //废弃字段


        public int DocumentImagePageNumber { get; set; }     //原DIP
        public string DocumentImageFilename { get; set; }    //原DIIP

        public double TouchStartPointY { get; set; } //原DIS
        public double TouchEndPointY { get; set; } //原DIE
        public double TouchStartPointX { get; set; } //原DIHS
        public double TouchEndPointX { get; set; } //原DIHE

        public double TouchStartPointY2 { get; set; } //原DIS2
        public double TouchEndPointY2 { get; set; } //原DIE2
        public double TouchStartPointX2 { get; set; } //原DIHS2
        public double TouchEndPointX2 { get; set; } //原DIHE2

        public string Local_SourceLrmFullpath { get; set; } //原SOURCELRM
        public string Local_SourcePicFullpath { get; set; } //原SOURCEPIC
        public string Local_SourceResFullpath { get; set; } //原SOURCERES
        public string Local_SourceOtherFullpath { get; set; } //新增字段



        //后面均为非存储字段
        //20170216 后述字段应该只保留getter属性，不再存储
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
                return Global.ProjectResDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + "同志的考察材料" + Path.GetExtension(Local_SourceResFullpath);
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
        }  //整理后干呈件存放位置

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
                return Global.ProjectTempPDFDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + "同志的考察材料.pdf";
            }
        }
        public string Local_StorgeOtherPdfFullpath
        {
            get
            {
                return Global.ProjectTempPDFDirectory + ((XM + string.Empty).Trim().Length == 0 ? Guid.NewGuid().ToString() : this.XM) + "的其他文件.pdf";
            }
        }


        //public string LRMIMG { get; set; } //废弃字段 本地lrm图像存放位置
        //public string RESIMG { get; set; }  //废弃字段 本地任免表存放位置


        /// <summary>
        /// 非存储字段
        /// 所有的Lrm图像的存放位置
        /// </summary>
        public List<string> LrmImageFileFullPaths
        {
            get
            {
                List<string> ImageFilePaths = new List<string>();
                //if (this.LrmImageCount <= 0)
                //{
                //    return null;
                //}
                //else if (this.LrmImageCount == 1)
                //{
                //    ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, this.LrmImageFilename));
                //    return ImageFilePaths;
                //}
                //else
                //{
                //    string FilenameWithExtension = Path.GetFileNameWithoutExtension(Path.Combine(Global.ProjectOutputImgDirectory, this.LrmImageFilename));
                //    for (int i = 0; i < LrmImageCount; i++)
                //    {
                //        ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, string.Format("{0}_{1}.{2}", FilenameWithExtension,i, Global.ImgFormat.ToString().ToLower())));
                //    }
                //    return ImageFilePaths;
                //}
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
                //if (this.ResImageCount <= 0)
                //{
                //    return null;
                //}
                //else if (this.ResImageCount == 1)
                //{
                //    ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, this.ResImageFilename));
                //    return ImageFilePaths;
                //}
                //else
                //{
                //    string FilenameWithExtension = Path.GetFileNameWithoutExtension(Path.Combine(Global.ProjectOutputImgDirectory, this.ResImageFilename));
                //    for (int i = 0; i < ResImageCount; i++)
                //    {
                //        ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, string.Format("{0}_{1}.{2}", FilenameWithExtension, i,Global.ImgFormat.ToString().ToLower())));
                //    }
                //    return ImageFilePaths;
                //}

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
                //if (this.OtherImageCount <= 0)
                //{
                //    return null;
                //}
                //else if (this.OtherImageCount == 1)
                //{
                //    ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, this.OtherImageFilename));
                //    return ImageFilePaths;
                //}
                //else
                //{
                //    string FilenameWithExtension = Path.GetFileNameWithoutExtension(Path.Combine(Global.ProjectOutputImgDirectory, this.OtherImageFilename));
                //    for (int i = 0; i < OtherImageCount; i++)
                //    {
                //        ImageFilePaths.Add(Path.Combine(Global.ProjectOutputImgDirectory, string.Format("{0}_{1}.{2}", FilenameWithExtension, i,Global.ImgFormat.ToString().ToLower())));
                //    }
                //    return ImageFilePaths;
                //}
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
