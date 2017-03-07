using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing;
using Word = Microsoft.Office.Interop.Word;

namespace ImgLocation.Services
{
    public class WordHelper:IDisposable
    {
        #region　－　属性　－
        private object MissingValue = System.Reflection.Missing.Value;
        private Word._Document _oDocument = null;

        public WordHelper()
        {

        }
        public WordHelper(string WordDocumentPath,bool IsShowWordApplication)
        {
            this.OpenWordApplication(IsShowWordApplication);
            this.OpenDocument(WordDocumentPath, IsShowWordApplication);
        }

        public void Dispose()
        {
            if (oDoc != null)
            {
                object doNotSaveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                oDoc.Close(ref doNotSaveChanges, ref　MissingValue, ref　MissingValue);
            }
            if (oWordApplication != null)
            {
                object saveOption = Word.WdSaveOptions.wdDoNotSaveChanges;
                oWordApplication.Quit(ref　saveOption, ref　MissingValue, ref　MissingValue);
            }
           
        }

        public enum Orientation
        {
            Horizontal,
            Vertical
        }
        public enum Alignment
        {
            Left,
            Center,
            Right,
            Justify
        }

        public Word._Application oWordApplication = null;
        public Word._Document oDoc
        {
            get
            {
                if (_oDocument == null)
                {
                    _oDocument = oWordApplication.Documents.Add(ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue);
                }
                return _oDocument;
            }
            set
            {
                if (value != null)
                {
                    _oDocument = value;
                }
            }
        }



        #endregion
        #region　－　基本操作　－
        public bool OpenWordApplication(bool isVisible)
        {
            try
            {
                oWordApplication = new Microsoft.Office.Interop.Word.Application();
                oWordApplication.Visible = isVisible;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool OpenWordApplication()
        {
            return OpenWordApplication(true);
        }

        public bool OpenDocument(string filePath, bool isVisible, bool isReadonly)
        {
            try
            {
                oWordApplication.Visible = isVisible;
                object path = filePath;
                object readOnly = isReadonly;
                oDoc = oWordApplication.Documents.Open(ref　path, ref　MissingValue, ref　readOnly, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool OpenDocument(string filePath, bool isVisible)
        {
            try
            {
                oWordApplication.Visible = isVisible;
                object path = filePath;
                oDoc = oWordApplication.Documents.Open(ref　path, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool OpenDocument(string filePath)
        {
            return OpenDocument(filePath, true);
        }

        public void CreateDocument()
        {
            oDoc = oWordApplication.Documents.Add(ref　MissingValue, ref　MissingValue, ref　MissingValue, ref　MissingValue);
        }

        public bool SaveDocument()
        {
            return SaveDocument(false);
        }
        public bool SaveDocument(bool isClose)
        {
            if (oDoc == null)
            {
                oDoc = oWordApplication.ActiveDocument;
            }
            oDoc.Save();
            if (isClose)
            {
                return CloseDocument();
            }
            return true;
        }

        public bool SaveDocumentAs(string savePath)
        {
            return SaveDocumentAs(savePath, false);
        }
        public bool SaveDocumentAs(string savePath, bool isClose)
        {
            try
            {
                object fileName = savePath;
                oDoc.SaveAs(ref　fileName, ref　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue, ref　　MissingValue);
                if (isClose)
                {
                    return CloseDocument();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveDocumentAsPDF(string savePath)
        {
            try
            {
                Word.Selection oCoverSelection = oWordApplication.Selection;
                int iCoverParagraph = oDoc.Paragraphs.Count;
                oDoc.Paragraphs[iCoverParagraph].Range.Select();
                object iCoverPageNumber = oCoverSelection.get_Information(Word.WdInformation.wdActiveEndPageNumber);
                oDoc.ExportAsFixedFormat(savePath.ToString(), Word.WdExportFormat.wdExportFormatPDF,
                      false, Word.WdExportOptimizeFor.wdExportOptimizeForPrint, Word.WdExportRange.wdExportFromTo, 1,
                      (int)iCoverPageNumber, Word.WdExportItem.wdExportDocumentContent, false, true,
                      Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks, true, true, false, ref MissingValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SaveDocumentAsXPS(string savePath)
        {
            try
            {
                Word.Selection oCoverSelection = oWordApplication.Selection;
                int iCoverParagraph = oDoc.Paragraphs.Count;
                oDoc.Paragraphs[iCoverParagraph].Range.Select();
                object iCoverPageNumber = oCoverSelection.get_Information(Word.WdInformation.wdActiveEndPageNumber);
                oDoc.ExportAsFixedFormat(savePath.ToString(), Word.WdExportFormat.wdExportFormatXPS,
                      false, Word.WdExportOptimizeFor.wdExportOptimizeForPrint, Word.WdExportRange.wdExportFromTo, 1,
                      (int)iCoverPageNumber, Word.WdExportItem.wdExportDocumentContent, false, true,
                      Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks, true, true, false, ref MissingValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CloseDocument()
        {
            try
            {
                object doNotSaveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                oDoc.Close(ref　doNotSaveChanges, ref　MissingValue, ref　MissingValue);
                oDoc = null; return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool QuitWordApplication()
        {
            try
            {
                object saveOption = Word.WdSaveOptions.wdDoNotSaveChanges;
                oWordApplication.Quit(ref　saveOption, ref　MissingValue, ref　MissingValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region　－　获得表格　－
        public Word.Table GetTable(int tableindex)
        {
            return oDoc.Tables[tableindex];
        }

        public IEnumerable<Word.Table> GetTable()
        {
            List<Word.Table> tables = new List<Word.Table>();
            foreach(Word.Table t in oDoc.Tables)
            {
                tables.Add(t);
            }
            return tables;
        }
        #endregion
        #region　-　插入表格　-
        public bool InsertTable(DataTable dt, bool haveBorder, double[] colWidths)
        {
            try
            {
                object Nothing = System.Reflection.Missing.Value;
                int lenght = oDoc.Characters.Count - 1;
                object start = lenght;
                object end = lenght;
                //表格起始坐标　　　　　　
                Microsoft.Office.Interop.Word.Range tableLocation = oDoc.Range(ref　start, ref　end);
                //添加Word表格　　　　　　　
                Microsoft.Office.Interop.Word.Table table = oDoc.Tables.Add(tableLocation, dt.Rows.Count, dt.Columns.Count, ref　Nothing, ref　Nothing);
                if (colWidths != null)
                {
                    for (int i = 0; i < colWidths.Length; i++)
                    {
                        table.Columns[i + 1].Width = (float)(28.5F * colWidths[i]);
                    }
                }
                ///设置TABLE的样式　　　　
                table.Rows.HeightRule = Microsoft.Office.Interop.Word.WdRowHeightRule.wdRowHeightAtLeast;
                table.Rows.Height = oWordApplication.CentimetersToPoints(float.Parse("0.8"));
                table.Range.Font.Size = 10.5F;
                table.Range.Font.Name = "宋体";
                table.Range.Font.Bold = 0;
                table.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                table.Range.Cells.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                if (haveBorder == true)
                {
                    //设置外框样式　　　　
                    table.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                    //样式设置结束　　　　
                }
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        table.Cell(row + 1, col + 1).Range.Text = dt.Rows[row][col].ToString();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }

        }
        public bool InsertTable(DataTable dt, bool haveBorder)
        {
            return InsertTable(dt, haveBorder, null);
        }
        public bool InsertTable(DataTable dt)
        {
            return InsertTable(dt, false, null);
        }
        #endregion
        #region　-　插入文本　-
        public bool InsertText(string strText, System.Drawing.Font font, Alignment alignment, bool isAftre)
        {
            try
            {
                Word.Range rng = oDoc.Content;
                int lenght = oDoc.Characters.Count - 1;
                object start = lenght;
                object end = lenght;
                rng = oDoc.Range(ref　start, ref　end);
                if (isAftre == true)
                {

                    strText += "\r\n";
                }
                rng.Text = strText;
                rng.Font.Name = font.Name;
                rng.Font.Size = font.Size;
                if (font.Style == System.Drawing.FontStyle.Bold)
                { rng.Font.Bold = 1; }
                //设置单元格中字体为粗体　　
                SetAlignment(rng, alignment);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool InsertText(string strText)
        {
            return InsertText(strText, new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold), Alignment.Left, false);
        }
        #endregion
        #region　-　设置对齐方式　-
        private Microsoft.Office.Interop.Word.WdParagraphAlignment SetAlignment(Microsoft.Office.Interop.Word.Range rng, Alignment alignment)
        {
            rng.ParagraphFormat.Alignment = SetAlignment(alignment);
            return SetAlignment(alignment);
        }

        private Microsoft.Office.Interop.Word.WdParagraphAlignment SetAlignment(Alignment alignment)
        {
            if (alignment == Alignment.Center)
            {
                return Word.WdParagraphAlignment.wdAlignParagraphCenter;
            }
            else if (alignment == Alignment.Left)
            {
                return Word.WdParagraphAlignment.wdAlignParagraphLeft;
            }
            else if (alignment == Alignment.Justify)
            {
                return Word.WdParagraphAlignment.wdAlignParagraphRight;
            }
            else
            {
                return Word.WdParagraphAlignment.wdAlignParagraphJustify;
            }
        }
        #endregion
        #region　-　插入分页符　-
        public void InsertBreak()
        {
            Word.Paragraph para;
            para = oDoc.Content.Paragraphs.Add(ref　MissingValue);
            object pBreak = (int)Word.WdBreakType.wdSectionBreakNextPage;
            para.Range.InsertBreak(ref　pBreak);
        }
        #endregion
        #region　-　插入页脚　-
        public bool InsertPageFooter(string text, System.Drawing.Font font, Alignment alignment)
        {
            try
            {
                oWordApplication.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekCurrentPageFooter;//页脚
                oWordApplication.Selection.InsertAfter(text);
                GetWordFont(oWordApplication.Selection.Font, font);
                SetAlignment(oWordApplication.Selection.Range, alignment); return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool InsertPageFooterNumber(System.Drawing.Font font, Alignment alignment)
        {
            try
            {
                oWordApplication.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekCurrentPageHeader;
                oWordApplication.Selection.WholeStory();
                oWordApplication.Selection.ParagraphFormat.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleNone; oWordApplication.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekMainDocument; oWordApplication.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekCurrentPageFooter;//页脚　　
                oWordApplication.Selection.TypeText("-第");
                object page = Word.WdFieldType.wdFieldPage;
                oWordApplication.Selection.Fields.Add(oWordApplication.Selection.Range, ref　page, ref　MissingValue, ref　MissingValue);
                oWordApplication.Selection.TypeText("页- -共");
                object pages = Word.WdFieldType.wdFieldNumPages;
                oWordApplication.Selection.Fields.Add(oWordApplication.Selection.Range, ref　pages, ref　MissingValue, ref　MissingValue);
                oWordApplication.Selection.TypeText("页-");
                GetWordFont(oWordApplication.Selection.Font, font);
                SetAlignment(oWordApplication.Selection.Range, alignment);
                oWordApplication.ActiveWindow.View.SeekView = Word.WdSeekView.wdSeekMainDocument;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region　-　字体格式设定　-
        public void GetWordFont(Microsoft.Office.Interop.Word.Font wordFont, System.Drawing.Font font)
        {
            wordFont.Name = font.Name;
            wordFont.Size = font.Size;
            if (font.Bold) { wordFont.Bold = 1; }
            if (font.Italic) { wordFont.Italic = 1; }
            if (font.Underline == true)
            {
                wordFont.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
            }
            wordFont.UnderlineColor = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
            if (font.Strikeout)
            {
                wordFont.StrikeThrough = 1;//删除线　　　　　
            }
        }
        #endregion
        #region　-　插入图片　-
        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="wdWrapType"></param>
        public void InsertImage(string fileName, float Left, float Top, float Width, float Height, object Anchor)
        {
            //插入图片   
            string FileName = fileName; //图片所在路径   
            object LinkToFile = false;
            object SaveWithDocument = true;
            oDoc.InlineShapes.AddPicture(fileName, ref LinkToFile, ref SaveWithDocument, ref Anchor);
            Word.Shape s = oDoc.Application.ActiveDocument.InlineShapes[oDoc.Application.ActiveDocument.InlineShapes.Count].ConvertToShape();
            s.WrapFormat.Type = Word.WdWrapType.wdWrapSquare;
            s.Width = Width;//图片宽度 
            s.Height = Height;//图片高度  
            s.Left = Left;
            s.Top = Top;
        }
        #endregion
        #region　-　页面设置　-
        public void SetPage(Orientation orientation, double width, double height, double topMargin, double leftMargin, double rightMargin, double bottomMargin)
        {
            oDoc.PageSetup.PageWidth = oWordApplication.CentimetersToPoints((float)width);
            oDoc.PageSetup.PageHeight = oWordApplication.CentimetersToPoints((float)height);
            if (orientation == Orientation.Horizontal)
            {
                oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
            }
            oDoc.PageSetup.TopMargin = (float)(topMargin * 25);//上边距　　　　
            oDoc.PageSetup.LeftMargin = (float)(leftMargin * 25);//左边距　　　
            oDoc.PageSetup.RightMargin = (float)(rightMargin * 25);//右边距　　
            oDoc.PageSetup.BottomMargin = (float)(bottomMargin * 25);//下边距　
        }
        public void SetPage(Orientation orientation, double topMargin, double leftMargin, double rightMargin, double bottomMargin)
        {
            SetPage(orientation, 21, 29.7, topMargin, leftMargin, rightMargin, bottomMargin);
        }
        public void SetPage(double topMargin, double leftMargin, double rightMargin, double bottomMargin)
        {
            SetPage(Orientation.Vertical, 21, 29.7, topMargin, leftMargin, rightMargin, bottomMargin);
        }
        #endregion
    }
}
