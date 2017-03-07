using ImgLocation.Models;
//using CadrePoint.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImgLocation.Repository
{
    public class DataRepository:IDisposable
    {
        private SQLiteDbHelper sqlite;

        public DataRepository(string dbFilePath)
        {
            this.sqlite = new SQLiteDbHelper(dbFilePath.ToString());
        }
        public void Dispose(bool disposing)
        {
            if(disposing)
            {
                //清理托管资源
            }
            //清理非托管资源
        }
        public void Dispose()
        {
            //调用带参数的Dispose方法，释放托管和非托管资源
            Dispose(true);
            //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
            //不需要再调用本对象的Finalize方法
            GC.SuppressFinalize(this);
        }
        ~DataRepository()
        {
            Dispose();
        }

        public void InitialDatabase()
        {
            //20170216 此对象数据库结构已经更新
            string sqlDropTableGB = "drop table if exists [t_GB]";
            sqlite.ExecuteNonQuery(sqlDropTableGB);
            string sqlCreateTableGB = "CREATE TABLE t_GB (id VARCHAR(100),DWID VARCHAR(100),XM VARCHAR(100),Lrm_Guid VARCHAR,Res_Guid VARCHAR, Other_Guid VARCHAR,Rank INT,LrmImageFilename VARCHAR,LrmImageCount INT,ResImageFilename VARCHAR,ResImageCount INT,OtherImageFilename VARCHAR,OtherImageCount INT,DocumentImagePageNumber INT,DocumentImageFilename VARCHAR,TouchStartPointY DOUBLE,TouchEndPointY DOUBLE,TouchStartPointX DOUBLE,TouchEndPointX DOUBLE,TouchStartPointY2 DOUBLE,TouchEndPointY2 DOUBLE,TouchStartPointX2 DOUBLE,TouchEndPointX2 DOUBLE, Local_SourceLrmFullpath VARCHAR,Local_SourcePicFullpath VARCHAR,Local_SourceResFullpath VARCHAR,Local_SourceOtherFullpath VARCHAR)";
            sqlite.ExecuteNonQuery(sqlCreateTableGB);

            //string sqlDropTableDI = "drop table if exists [t_DI]";
            //sqlite.ExecuteNonQuery(sqlDropTableDI);
            //string sqlCreateTableDI = "CREATE TABLE t_DI (id VARCHAR(100),DWID VARCHAR(100),SX INT,DI VARCHAR,DIMG VARCHAR,P INT,W INT, H INT,DIUUID VARCHAR)";
            //sqlite.ExecuteNonQuery(sqlCreateTableDI);

            string sqlDropTableDW = "drop table if exists [t_DW]";
            sqlite.ExecuteNonQuery(sqlDropTableDW);
            string sqlCreateTableDW = "CREATE TABLE t_DW (id VARCHAR(100),MC VARCHAR,WH VARCHAR,XH VARCHAR ,Rank INT,DocumentImageFilename VARCHAR, DocumentImageCount INT,Local_SourceDocumnetFullpath VARCHAR,PageNumberStopLookCadre INT)";
            sqlite.ExecuteNonQuery(sqlCreateTableDW);

            string sqlDropTableZY = "drop table if exists [t_ZY]";
            sqlite.ExecuteNonQuery(sqlDropTableZY);
            string sqlCreateTableZY = "CREATE TABLE t_ZY (ZYXX VARCHAR)";
            sqlite.ExecuteNonQuery(sqlCreateTableZY);
        }

        public void ValidateDatabase()
        {
            //20170216校验数据库语句已经根据新对象属性更新
            string sqlCreateTableGB = "CREATE TABLE IF NOT EXISTS t_GB (id VARCHAR(100),DWID VARCHAR(100),XM VARCHAR(100),Lrm_Guid VARCHAR,Res_Guid VARCHAR, Other_Guid VARCHAR,Rank INT,LrmImageFilename VARCHAR,LrmImageCount INT,ResImageFilename VARCHAR,ResImageCount INT,OtherImageFilename VARCHAR,OtherImageCount INT,DocumentImagePageNumber INT,DocumentImageFilename VARCHAR,TouchStartPointY DOUBLE,TouchEndPointY DOUBLE,TouchStartPointX DOUBLE,TouchEndPointX DOUBLE,TouchStartPointY2 DOUBLE,TouchEndPointY2 DOUBLE,TouchStartPointX2 DOUBLE,TouchEndPointX2 DOUBLE, Local_SourceLrmFullpath VARCHAR,Local_SourcePicFullpath VARCHAR,Local_SourceResFullpath VARCHAR,Local_SourceOtherFullpath VARCHAR)";
            sqlite.ExecuteNonQuery(sqlCreateTableGB);

            //string sqlCreateTableDI = "CREATE TABLE IF NOT EXISTS t_DI (id VARCHAR(100),DWID VARCHAR(100),SX INT,DI VARCHAR,DIMG VARCHAR,P INT,W INT,H INT,DIUUID VARCHAR)";
            //sqlite.ExecuteNonQuery(sqlCreateTableDI);

            string sqlCreateTableDW = "CREATE TABLE IF NOT EXISTS t_DW (id VARCHAR(100),MC VARCHAR,WH VARCHAR,XH VARCHAR ,Rank INT,DocumentImageFilename VARCHAR, DocumentImageCount INT,Local_SourceDocumnetFullpath VARCHAR,PageNumberStopLookCadre INT)";
            sqlite.ExecuteNonQuery(sqlCreateTableDW);

            string sqlCreateTableZY = "CREATE TABLE IF NOT EXISTS t_ZY (ZYXX VARCHAR)";
            sqlite.ExecuteNonQuery(sqlCreateTableZY);
        }

        public DW EditDW(DW d)
        {
            string sqlSelectData = string.Format("SELECT * FROM t_DW WHERE id='{0}'", d.id);
            DataTable dt = this.sqlite.ExecuteDataTable(sqlSelectData);
            if (dt.Rows.Count > 0)
            {
                string sqlInsertData = string.Format("UPDATE t_DW SET id='{0}',MC='{1}',WH='{2}',XH='{3}',Rank={4},DocumentImageFilename='{5}',DocumentImageCount={6},Local_SourceDocumnetFullpath='{7}',PageNumberStopLookCadre={8} WHERE id='{0}'", d.id, d.MC, d.WH, d.XH, d.Rank, d.DocumentImageFilename, d.DocumentImageCount, d.Local_SourceDocumnetFullpath,d.PageNumberStopLookCadre);
                this.sqlite.ExecuteNonQuery(sqlInsertData);
                //string sqlDeleteDataDI = string.Format("DELETE FROM t_DI WHERE DWID='{0}' ", d.id);
                //this.sqlite.ExecuteNonQuery(sqlDeleteDataDI);
                //foreach (DI di in d.DIS)
                //{
                //    EditDI(di);
                //}
            }
            else
            {
                string sqlInsertData = string.Format("INSERT INTO t_DW VALUES ('{0}','{1}','{2}','{3}',{4},'{5}',{6},'{7}',{8})", d.id, d.MC, d.WH, d.XH, d.Rank, d.DocumentImageFilename, d.DocumentImageCount, d.Local_SourceDocumnetFullpath,d.PageNumberStopLookCadre);
                this.sqlite.ExecuteNonQuery(sqlInsertData);
                //string sqlDeleteDataDI = string.Format("DELETE FROM t_DI WHERE DWID='{0}' ", d.id);
                //this.sqlite.ExecuteNonQuery(sqlDeleteDataDI);
                //foreach (DI di in d.DIS)
                //{
                //    EditDI(di);
                //}
            }
            return d;
        }

        public int RemoveDW(DW d)
        {
            //string sqlDeleteDataDI = string.Format("DELETE FROM t_DI WHERE DWID='{0}' ", d.id);
            //this.sqlite.ExecuteNonQuery(sqlDeleteDataDI);
            string sqlDeleteDataGB = string.Format("DELETE FROM t_GB WHERE DWID='{0}' ", d.id);
            this.sqlite.ExecuteNonQuery(sqlDeleteDataGB);
            string sqlDeleteData = string.Format("DELETE FROM t_DW WHERE id='{0}' ", d.id);
            return this.sqlite.ExecuteNonQuery(sqlDeleteData);
        }


        //public DI EditDI(DI d)
        //{
        //    string sqlSelectData = string.Format("SELECT * FROM t_DI WHERE id='{0}'", d.id);
        //    DataTable dt = this.sqlite.ExecuteDataTable(sqlSelectData);
        //    if (dt.Rows.Count > 0)
        //    {
        //        string sqlInsertData = string.Format("UPDATE t_DI SET id='{0}',DWID='{1}',SX={2},DI='{3}',DIMG='{4}',DI={5},W={6},H={7},DIUUID='{8}' WHERE id='{0}'", d.id, d.DWID, d.SX, d.DDI, d.Local_StorgeDocumentImageFullpath,d.P,d.W,d.H,d.DIUUID);
        //        this.sqlite.ExecuteNonQuery(sqlInsertData);
        //    }
        //    else
        //    {
        //        string sqlInsertData = string.Format("INSERT INTO t_DI VALUES ('{0}','{1}',{2},'{3}','{4}',{5},{6},{7},'{8}')", d.id, d.DWID, d.SX, d.DDI, d.Local_StorgeDocumentImageFullpath,d.P,d.W,d.H,d.DIUUID);
        //        this.sqlite.ExecuteNonQuery(sqlInsertData);
        //    }
        //    return d;
        //}

        //public int RemoveDI(DI d)
        //{
        //    string sqlDeleteData = string.Format("DELETE FROM t_DI WHERE id='{0}' ", d.id);
        //    return this.sqlite.ExecuteNonQuery(sqlDeleteData);
        //}

        public bool MoveUp(DW d)
        {
            OrderSX();
            bool re = false;
            string sqlSelectDWup = string.Format("SELECT * FROM t_DW WHERE Rank='{0}'", d.Rank-1);
            DataTable dwup = this.sqlite.ExecuteDataTable(sqlSelectDWup);

            if (dwup.Rows.Count > 0)
            {
                string dwid = d.id;
                string upid = dwup.Rows[0]["id"].ToString();

                string sqlUpdateDW = string.Format("UPDATE t_DW SET Rank=Rank-1,XH='{0}' WHERE id='{1}'", d.Rank-1, d.id);
                this.sqlite.ExecuteNonQuery(sqlUpdateDW);
                //string sqlUpdateDI = string.Format("UPDATE t_DI SET SX=SX-100 WHERE DWID='{0}'", dwid);
                //this.sqlite.ExecuteNonQuery(sqlUpdateDI);

                string sqlUpdateDWup = string.Format("UPDATE t_DW SET Rank=Rank+1,XH='{0}' WHERE id='{1}'", d.Rank, upid);
                this.sqlite.ExecuteNonQuery(sqlUpdateDWup);
                //string sqlUpdateDIup = string.Format("UPDATE t_DI SET SX=SX+100 WHERE DWID='{0}'", upid);
                //this.sqlite.ExecuteNonQuery(sqlUpdateDIup);

                re = true;
            }
            return re;
        }

        public bool MoveDown(DW d)
        {
            OrderSX();
            bool re = false;
            string sqlSelectDWdown = string.Format("SELECT * FROM t_DW WHERE Rank='{0}'", d.Rank + 1);
            DataTable dwdown = this.sqlite.ExecuteDataTable(sqlSelectDWdown);
            if (dwdown.Rows.Count > 0)
            {
                string dwid = d.id;
                string downid = dwdown.Rows[0]["id"].ToString();

                string sqlUpdateDW = string.Format("UPDATE t_DW SET Rank=Rank+1,XH='{0}' WHERE id='{1}'", d.Rank+1, d.id);
                this.sqlite.ExecuteNonQuery(sqlUpdateDW);
                //string sqlUpdateDI = string.Format("UPDATE t_DI SET SX=SX+100 WHERE DWID='{0}'", dwid);
                //this.sqlite.ExecuteNonQuery(sqlUpdateDI);

                string sqlUpdateDWdown = string.Format("UPDATE t_DW SET Rank=Rank-1,XH='{0}' WHERE id='{1}'", d.Rank, downid);
                this.sqlite.ExecuteNonQuery(sqlUpdateDWdown);
                //string sqlUpdateDIdown = string.Format("UPDATE t_DI SET SX=SX-100 WHERE DWID='{0}'", downid);
                //this.sqlite.ExecuteNonQuery(sqlUpdateDIdown);
                re = true;
            }
            return re;
        }

        
        public void CoverSX(string OrderType)
        {
            string sqlSelectDW = "SELECT * FROM t_DW ORDER BY "+OrderType;
            DataTable dtDW = this.sqlite.ExecuteDataTable(sqlSelectDW);
            for (int i = 0; i < dtDW.Rows.Count;i++ )
            {
                DataRow dr = dtDW.Rows[i];
                string id = dr["id"].ToString();
                string sqlUpdateDW = string.Format("UPDATE t_DW SET Rank={0} WHERE id='{1}'", (i+101).ToString(), id);
                this.sqlite.ExecuteNonQuery(sqlUpdateDW);
                //string sqlUpdateDI = string.Format("UPDATE t_DI SET SX=SX+({0}-{1})*100 WHERE DWID='{2}'", (i + 101).ToString(), Convert.ToInt32(dr["SX"] + ""), id);
                //this.sqlite.ExecuteNonQuery(sqlUpdateDI);
            }
        }
        public void OrderSX()
        {
            string sqlSelectDW = "SELECT * FROM t_DW ORDER BY Rank";
            DataTable dtDW = this.sqlite.ExecuteDataTable(sqlSelectDW);
            for (int i = 0; i < dtDW.Rows.Count; i++)
            {
                DataRow dr = dtDW.Rows[i];
                string id = dr["id"].ToString();
                string sqlUpdateDW = string.Format("UPDATE t_DW SET Rank={0},XH='{1}' WHERE id='{2}'", (i + 101).ToString(),(i+1) ,id);
                this.sqlite.ExecuteNonQuery(sqlUpdateDW);
                //string sqlUpdateDI = string.Format("UPDATE t_DI SET Rank=Rank+({0}-{1})*100 WHERE DWID='{2}'", (i + 101).ToString(), Convert.ToInt32(dr["SX"] + ""), id);
                //this.sqlite.ExecuteNonQuery(sqlUpdateDI);
            }
        }

        public int GetMaxSX()
        {
            string sqlSelectData = "SELECT MAX(Rank) FROM t_DW";
            DataTable dt = this.sqlite.ExecuteDataTable(sqlSelectData);
            if (dt.Rows[0][0] == null)
            {
                return 100;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0][0].ToString());
            }
        }

        public GB EditGB(GB g)
        {
            string sqlSelectData = string.Format("SELECT * FROM t_GB WHERE id='{0}' ", g.id);
            DataTable dt = this.sqlite.ExecuteDataTable(sqlSelectData);
            if (dt.Rows.Count > 0)
            {
                ///20170216已根据新GB对象属性更新
                string sqlInsertData = string.Format("UPDATE t_GB SET id='{0}',DWID='{1}',XM='{2}',Lrm_Guid='{3}',Res_Guid='{4}',Other_Guid='{5}',Rank={6},LrmImageFilename='{7}',LrmImageCount={8},ResImageFilename='{9}',ResImageCount={10},OtherImageFilename='{11}',OtherImageCount={12},DocumentImagePageNumber={13},DocumentImageFilename='{14}',TouchStartPointY={15},TouchEndPointY={16},TouchStartPointX={17},TouchEndPointX={18},TouchStartPointY2={19},TouchEndPointY2={20},TouchStartPointX2={21},TouchEndPointX2={22},Local_SourceLrmFullpath='{23}',Local_SourcePicFullpath='{24}',Local_SourceResFullpath='{25}',Local_SourceOtherFullpath='{26}' WHERE id='{0}'",
                    g.id, g.DWID, g.XM, g.Lrm_Guid, g.Res_Guid, g.Other_Guid, g.Rank, g.LrmImageFilename, g.LrmImageCount, g.ResImageFilename, g.ResImageCount, g.OtherImageFilename, g.OtherImageCount, g.DocumentImagePageNumber, g.DocumentImageFilename,g.TouchStartPointY,g.TouchEndPointY,g.TouchStartPointX,g.TouchEndPointX, g.TouchStartPointY2, g.TouchEndPointY2, g.TouchStartPointX2, g.TouchEndPointX2, g.Local_SourceLrmFullpath, g.Local_SourcePicFullpath, g.Local_SourceResFullpath, g.Local_SourceOtherFullpath);
                this.sqlite.ExecuteNonQuery(sqlInsertData);
            }
            else
            {
                string sqlInsertData = string.Format("INSERT INTO t_GB VALUES('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}',{8},'{9}',{10},'{11}',{12},{13},'{14}',{15},{16},{17},{18},{19},{20},{21},{22},'{23}','{24}','{25}','{26}')",
                    g.id, g.DWID, g.XM, g.Lrm_Guid, g.Res_Guid, g.Other_Guid, g.Rank, g.LrmImageFilename, g.LrmImageCount, g.ResImageFilename, g.ResImageCount, g.OtherImageFilename, g.OtherImageCount, g.DocumentImagePageNumber, g.DocumentImageFilename,g.TouchStartPointY,g.TouchEndPointY,g.TouchStartPointX,g.TouchEndPointX, g.TouchStartPointY2, g.TouchEndPointY2, g.TouchStartPointX2, g.TouchEndPointX2, g.Local_SourceLrmFullpath, g.Local_SourcePicFullpath, g.Local_SourceResFullpath, g.Local_SourceOtherFullpath);
                this.sqlite.ExecuteNonQuery(sqlInsertData);
            }
            return g;
        }

        public int RemoveGB(GB g)
        {
            string sqlDeleteData = string.Format("DELETE FROM t_GB WHERE id='{0}' ", g.id);
            return this.sqlite.ExecuteNonQuery(sqlDeleteData);
        }

        public void AddZY(string zy)
        {
            string sqlSelectData = string.Format("DELETE FROM t_ZY ");
            this.sqlite.ExecuteNonQuery(sqlSelectData);

            string sqlInsertData = string.Format("INSERT INTO t_ZY VALUES('{0}')", zy);
            this.sqlite.ExecuteNonQuery(sqlInsertData);
        }
        public DataTable GetAllGBs()
        {
            string sqlSelectData = "SELECT * FROM t_GB";
            return this.sqlite.ExecuteDataTable(sqlSelectData);
        }

        public List<DW> GetAllDWs()
        {
            List<DW> dws = new List<DW>();
            string sqlSelectData = "SELECT * FROM t_DW";
            DataTable dt = this.sqlite.ExecuteDataTable(sqlSelectData);
            foreach (DataRow dr in dt.Rows)
            {
                DW d = new DW();
                d.id = dr["id"] + "";
                d.MC = dr["MC"] + "";
                d.WH = dr["WH"] + "";
                d.XH = dr["XH"] + "";
                d.Rank = Convert.ToInt32(dr["Rank"] + "");
                d.DocumentImageFilename = dr["DocumentImageFilename"] + "";
                d.DocumentImageCount = Convert.ToInt32(dr["DocumentImageCount"] + "");
                //d.IMG = dr["IMG"] + "";
                //d.Local_StorgeDocumentPdfFullpath = dr["PDF"] + "";
                //d.Local_StorgeDocumentFullpath = dr["SAVE"] + "";
                //d.W = Convert.ToInt32(dr["W"] + "");

                d.Local_SourceDocumnetFullpath = dr["Local_SourceDocumnetFullpath"] + "";
                d.PageNumberStopLookCadre = Convert.ToInt32(dr["PageNumberStopLookCadre"] + "");

                d.GBS = new List<GB>();
                string sqlSelectGBData = string.Format("SELECT * FROM t_GB WHERE DWID='{0}'", d.id);
                DataTable dgs = this.sqlite.ExecuteDataTable(sqlSelectGBData);
                {
                    foreach (DataRow dg in dgs.Rows)
                    {
                        GB g = new GB();
                        g.id = dg["id"] + string.Empty;
                        g.DWID = d.id;
                        g.XM = dg["XM"] +string.Empty;
                        g.Lrm_Guid = dg["Lrm_Guid"] + string.Empty;
                        g.Res_Guid = dg["Res_Guid"] + string.Empty;
                        g.Other_Guid = dg["Other_Guid"] + string.Empty;
                        g.Rank = Convert.ToInt32(dg["Rank"] + string.Empty);
                        //g.LrmImageFilename = dg["LrmImageFilename"] + string.Empty;
                        g.LrmImageCount = Convert.ToInt32(dg["LrmImageCount"] + string.Empty);
                        //g.ResImageFilename = dg["ResImageFilename"] + string.Empty;
                        g.ResImageCount = Convert.ToInt32(dg["ResImageCount"] + string.Empty);
                        //g.OtherImageFilename = dg["OtherImageFilename"] + string.Empty;
                        g.OtherImageCount = Convert.ToInt32(dg["OtherImageCount"] + string.Empty);
                        g.DocumentImagePageNumber = Convert.ToInt32(dg["DocumentImagePageNumber"] + string.Empty);
                        g.DocumentImageFilename = dg["DocumentImageFilename"] + string.Empty;
                        g.TouchStartPointY = Convert.ToDouble(dg["TouchStartPointY"] + string.Empty);
                        g.TouchEndPointY = Convert.ToDouble(dg["TouchEndPointY"] + string.Empty);
                        g.TouchStartPointX = Convert.ToDouble(dg["TouchStartPointX"] + string.Empty);
                        g.TouchEndPointX = Convert.ToDouble(dg["TouchEndPointX"] + string.Empty);
                        g.TouchStartPointY2 = Convert.ToDouble(dg["TouchStartPointY2"] + string.Empty);
                        g.TouchEndPointY2 = Convert.ToDouble(dg["TouchEndPointY2"] + string.Empty);
                        g.TouchStartPointX2 = Convert.ToDouble(dg["TouchStartPointX2"] + string.Empty);
                        g.TouchEndPointX2 = Convert.ToDouble(dg["TouchEndPointX2"] + string.Empty);
                        g.Local_SourceLrmFullpath = dg["Local_SourceLrmFullpath"] + string.Empty;
                        g.Local_SourcePicFullpath = dg["Local_SourcePicFullpath"] + string.Empty;
                        g.Local_SourceResFullpath = dg["Local_SourceResFullpath"] + string.Empty;
                        g.Local_SourceOtherFullpath= dg["Local_SourceOtherFullpath"] + string.Empty;
                        
                        //g.ResImageCount = Convert.ToInt32(dg["CLSL"] + "");
                        ////g.E = Convert.ToDouble(dg["E"] + "");
                        //g.ResImageFilename = dg["KCCL"] + "";
                        //g.Local_StorgeLrmFullpath = dg["LRM"] + "";
                        //g.LRMIMG = dg["LRMIMG"] + "";
                        //g.Local_StorgeLrmPdfFullpath = dg["LRMPDF"] + "";
                        //g.Local_StorgePicFullpath = dg["PIC"] + "";
                        //g.Local_StorgeResFullpath = dg["RES"] + "";
                        //g.RESIMG = dg["RESIMG"] + "";
                        //g.Local_StorgeResPdfFullPath = dg["RESPDF"] + "";
                        ////g.S = Convert.ToDouble(dg["S"] + ""); ;
                        //g.Local_StorgeDocumentWordFullpath = dg["WORD"] + "";
                        //g.DIID = dg["DIID"] + "";
                        //g.XSD = Convert.ToDouble(dg["XSD"] + "");

                        d.GBS.Add(g);
                    }
                }
                //d.DIS = new List<DI>();
                //string sqlSelectDIData = string.Format("SELECT * FROM t_DI WHERE DWID='{0}'", d.id);
                //DataTable dis = this.sqlite.ExecuteDataTable(sqlSelectDIData);
                //{
                //    foreach (DataRow di in dis.Rows)
                //    {
                //        DI i = new DI();
                //        i.id = di["id"] + "";
                //        i.SX = Convert.ToInt32(di["SX"] + "");
                //        i.P = Convert.ToInt32(di["P"] + "");
                //        i.W = Convert.ToInt32(di["W"] + "");
                //        i.H = Convert.ToInt32(di["H"] + "");
                //        i.DDI = di["DI"] + "";
                //        i.DWID = d.id;
                //        i.Local_StorgeDocumentImageFullpath = di["DIMG"] + "";
                //        i.DIUUID = di["DIUUID"] + "";
                //        d.DIS.Add(i);
                //    }
                //}
                dws.Add(d);
            }
            return dws;
        }
    }
}
