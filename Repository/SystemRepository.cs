using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using ImgLocation.Models;


namespace ImgLocation.Repository
{
    public class SystemRepository
    {
        private SQLiteDbHelper sqlite;
        public SystemRepository()
        {
            string rootPath = Directory.GetCurrentDirectory();
            string dbPath = rootPath.Substring(rootPath.Length - 1, 1) == @"\" ? rootPath + "sys.db" : rootPath + @"\sys.db";
            this.sqlite = new SQLiteDbHelper(dbPath);
            string sqlCreateTableProject = "CREATE TABLE IF NOT EXISTS t_Project (id INT,TITLE VARCHART,PATH VARCHAR,ADDDATE DATETIME,LASTDATAID VARCHAR )";
            sqlite.ExecuteNonQuery(sqlCreateTableProject);
            string sqlCreateTableSystem = "CREATE TABLE IF NOT EXISTS t_System (id INT,NAME VARCHART,VALUE VARCHAR)";
            sqlite.ExecuteNonQuery(sqlCreateTableSystem);
            string sqlCreateTablePadInfo = "CREATE TABLE IF NOT EXISTS t_PadInfo (id INT,SERIAL VARCHART,DATAID VARCHART,HASDATA VARCHART)";
            sqlite.ExecuteNonQuery(sqlCreateTablePadInfo);
        }

        public IEnumerable<Project> GetAllProjects()
        {
            List<Project> ps = new List<Project>();
            string sqlSelectProject = "SELECT * FROM t_Project ORDER BY id";
            DataTable dt = sqlite.ExecuteDataTable(sqlSelectProject);
            foreach (DataRow dr in dt.Rows)
            {
                ps.Add(new Project
                    {
                        id = Convert.ToInt32(dr["id"]),
                        TITLE = dr["TITLE"].ToString(),
                        PATH = dr["PATH"].ToString(),
                        ADDDATE = Convert.ToDateTime(dr["ADDDATE"].ToString()),
                    });
            }
            return ps;
        }
        public Project GetProject(int id)
        {

            string sqlSelectProject = string.Format("SELECT * FROM t_Project WHERE id={0}", id.ToString());
            DataTable dt = sqlite.ExecuteDataTable(sqlSelectProject);
            DataRow dr = dt.Rows[0];
            Project p = new Project
                 {
                     id = Convert.ToInt32(dr["id"]),
                     TITLE = dr["TITLE"].ToString(),
                     PATH = dr["PATH"].ToString(),
                     ADDDATE = Convert.ToDateTime(dr["ADDDATE"].ToString()),
                     LASTDATAID = dr["LASTDATAID"].ToString(),
                 };
            return p;
        }
        public Project AddProject(Project p)
        {
            string sqlInsertProject = string.Format("INSERT INTO t_Project(id,TITLE,PATH,ADDDATE,LASTDATAID) VALUES({0},'{1}','{2}',{3},'{4}')", p.id, p.TITLE, p.PATH, p.ADDDATE.ToString("yyyyMMddHHmmss"),p.LASTDATAID);
            sqlite.ExecuteNonQuery(sqlInsertProject);
            return p;
        }
        public Project EditProject(Project p)
        {
            string sqlUpdateProject = string.Format("UPDATE t_Project SET id={0},TITLE='{1}',PATH='{2}',ADDDATE={3},LASTDATAID='{4}' WHERE id={0}", p.id, p.TITLE, p.PATH, p.ADDDATE.ToString("yyyyMMddHHmmss"),p.LASTDATAID);
            sqlite.ExecuteNonQuery(sqlUpdateProject);
            return p;
        }
        public void DeleteProject(int id)
        {
            string sqlUpdateProject = string.Format("DELETE FROM t_Project WHERE id={0}", id);
            sqlite.ExecuteNonQuery(sqlUpdateProject);
        }
        public bool ExistProject(int id)
        {
            string sqlSelectProject =string.Format( "SELECT * FROM t_Project WHERE id={0}",id.ToString());
            DataTable dt = sqlite.ExecuteDataTable(sqlSelectProject);
            return dt.Rows.Count>0;
        }
        public int NewProjectId()
        {
            string sqlSelectProject = "SELECT * FROM t_Project ORDER BY id DESC";
            DataTable dt = sqlite.ExecuteDataTable(sqlSelectProject);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["id"].ToString()) + 1;
            }
            else
            {
                return 101;
            }
        }
        public Project SaveProject(Project p)
        {
            if(ExistProject(p.id))
            {
                return EditProject(p);
            }
            else
            {
                return AddProject(p);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PadInfo GetPadInfo(int id)
        {

            string sqlSelectPadInfo = string.Format("SELECT * FROM t_PadInfo WHERE id={0}", id.ToString());
            DataTable dt = sqlite.ExecuteDataTable(sqlSelectPadInfo);
            if(dt.Rows.Count>0)
            {
                DataRow dr = dt.Rows[0];
                PadInfo p = new PadInfo
                {
                    id = Convert.ToInt32(dr["id"]),
                    SERIAL = dr["SERIAL"].ToString(),
                    DATAID = dr["DATAID"].ToString(),
                    HASDATA = dr["HASDATA"].ToString(),
                };
                return p;
            }
            else
            {
                return null;
            }
        }
        public PadInfo AddPadInfo(PadInfo p)
        {
            string sqlInsertPadInfo = string.Format("INSERT INTO t_PadInfo(id,SERIAL,DATAID,HASDATA) VALUES({0},'{1}','{2}','{3}')", p.id, p.SERIAL, p.DATAID, p.HASDATA);
            sqlite.ExecuteNonQuery(sqlInsertPadInfo);
            return p;
        }
        public PadInfo EditPadInfo(PadInfo p)
        {
            string sqlUpdatePadInfo = string.Format("UPDATE t_PadInfo SET id={0},SERIAL='{1}',DATAID='{2}',HASDATA='{3}' WHERE id={0}", p.id, p.SERIAL, p.DATAID, p.HASDATA);
            sqlite.ExecuteNonQuery(sqlUpdatePadInfo);
            return p;
        }
        public void DeletePadInfo(int id)
        {
            string sqlUpdatePadInfo = string.Format("DELETE FROM t_PadInfo WHERE id={0}", id);
            sqlite.ExecuteNonQuery(sqlUpdatePadInfo);
        }
        public bool ExistPadInfo(int id)
        {
            string sqlSelectPadInfo = string.Format("SELECT * FROM t_PadInfo WHERE id={0}", id.ToString());
            DataTable dt = sqlite.ExecuteDataTable(sqlSelectPadInfo);
            return dt.Rows.Count > 0;
        }


        public string ReadSystemConfig(int id)
        {
            string sqlSelectSystem = string.Format("SELECT * FROM t_System WHERE id={0}", id.ToString());
            DataTable dt = sqlite.ExecuteDataTable(sqlSelectSystem);
            if(dt.Rows.Count>0)
            {
                return dt.Rows[0]["VALUE"].ToString();
            }
            else
            {
                return "";
            }
        }
        public void WriteSystemConfig(int id,string Name,string Value)
        {
             string sqlSelectSystem = string.Format("SELECT * FROM t_System WHERE id={0}", id.ToString());
            DataTable dt = sqlite.ExecuteDataTable(sqlSelectSystem);
            if(dt.Rows.Count>0)
            {
                string sqlUpdateSystem = string.Format("UPDATE t_System SET Name='{0}',Value='{1}' WHERE id={2}", Name, Value, id);
                sqlite.ExecuteNonQuery(sqlUpdateSystem);
            }
            else
            {
                string sqlInsertSystem = string.Format("INSERT INTO t_System(id,NAME,VALUE) VALUES({0},'{1}','{2}')", id, Name, Value);
                sqlite.ExecuteNonQuery(sqlInsertSystem);
            }
        }
    }
}
