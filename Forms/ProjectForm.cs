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
using ImgLocation.Repository;
using ImgLocation.Models;
using ImgLocation;

namespace ImgLocation.Forms
{
    public partial class ProjectForm : Form
    {
        public ProjectForm()
        {
            InitializeComponent();
            RefreshData();
        }
        string path = "";
        private void RefreshData()
        {
            SystemRepository sr = new SystemRepository();
            GridProject.AutoGenerateColumns = false;
            GridProject.DataSource = sr.GetAllProjects();
        }
        public void LoadProject(Project p)
        {
            Global.RefreshParams();
            Global.RefreshDirectory(p,"Meeting1");
            SystemRepository sr = new SystemRepository();
            sr.WriteSystemConfig(201, "", p.id.ToString());//更改系统配置为当前项目；
            DataRepository dr = new DataRepository(Global.ProjectOutputDbPath);
            dr.ValidateDatabase();
            dr.AddZY(p.TITLE);
        }

        private void EditGridProjectRow()
        {
            DataGridViewRow r = GridProject.Rows[GridProject.CurrentRow.Index];
            SystemRepository sr = new SystemRepository();
            Project p = sr.GetProject(Convert.ToInt32(r.Cells["id"].Value.ToString()));
            tbid.Text = p.id.ToString();
            tbTitle.Text = p.TITLE;
            path= p.PATH;
            dateAddDate.Value = p.ADDDATE;
        }
        private void OpenProjectRecord()
        {
            Project p = new Project();
            if (tbid.Text.Trim().Length * path.Trim().Length * tbTitle.Text.Trim().Length > 0)
            {
                try
                {
                    SystemRepository sr = new SystemRepository();
                    p.id = Convert.ToInt32(tbid.Text.Trim());
                    p.TITLE = tbTitle.Text;
                    p.PATH =path;
                    p.ADDDATE = dateAddDate.Value;
                    sr.SaveProject(p);
                    RefreshData();
                    DialogResult dr = MessageBox.Show("保存成功，是否打开项目？", "保存成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        LoadProject(p);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发生错误：" + ex.Message, "保存项目错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("信息填写不完整！", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUse_Click(object sender, EventArgs e)
        {
            OpenProjectRecord();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            OpenProjectRecord();
        }
     
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string rp = Directory.GetCurrentDirectory();
            SystemRepository sr = new SystemRepository();
            tbid.Text = sr.NewProjectId().ToString();
            tbTitle.Text = string.Empty;
            path= rp.Substring(rp.Length - 1, 1) == @"\" 
                ? rp + @"Project\Project_" + sr.NewProjectId().ToString() + @"\" 
                : rp + @"\Project\Project_" + sr.NewProjectId().ToString() + @"\";
            dateAddDate.Value = DateTime.Now;
        }
  
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SystemRepository sr = new SystemRepository();
                DataGridViewRow r = GridProject.Rows[GridProject.CurrentRow.Index];
                int id = Convert.ToInt32(r.Cells["id"].Value.ToString());
                sr.DeleteProject(id);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "删除项目错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridProject_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditGridProjectRow();
        }

        private void GridProject_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditGridProjectRow();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditGridProjectRow();
        }

        private void ProjectForm_Load(object sender, EventArgs e)
        {

        }
    }
}
