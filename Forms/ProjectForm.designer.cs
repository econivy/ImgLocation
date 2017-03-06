namespace ImgLocation.Forms
{
    partial class ProjectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectForm));
            this.GroupProject = new System.Windows.Forms.GroupBox();
            this.dateAddDate = new System.Windows.Forms.DateTimePicker();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.tbid = new System.Windows.Forms.TextBox();
            this.lbladddate = new System.Windows.Forms.Label();
            this.lbltitle = new System.Windows.Forms.Label();
            this.lblid = new System.Windows.Forms.Label();
            this.btnUse = new System.Windows.Forms.Button();
            this.GridProject = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TITLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.GroupProject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridProject)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupProject
            // 
            this.GroupProject.Controls.Add(this.dateAddDate);
            this.GroupProject.Controls.Add(this.tbTitle);
            this.GroupProject.Controls.Add(this.tbid);
            this.GroupProject.Controls.Add(this.lbladddate);
            this.GroupProject.Controls.Add(this.lbltitle);
            this.GroupProject.Controls.Add(this.lblid);
            this.GroupProject.Location = new System.Drawing.Point(12, 229);
            this.GroupProject.Name = "GroupProject";
            this.GroupProject.Size = new System.Drawing.Size(381, 100);
            this.GroupProject.TabIndex = 1;
            this.GroupProject.TabStop = false;
            this.GroupProject.Text = "项目详情";
            // 
            // dateAddDate
            // 
            this.dateAddDate.Location = new System.Drawing.Point(78, 66);
            this.dateAddDate.Name = "dateAddDate";
            this.dateAddDate.Size = new System.Drawing.Size(297, 21);
            this.dateAddDate.TabIndex = 7;
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(78, 42);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(297, 21);
            this.tbTitle.TabIndex = 5;
            // 
            // tbid
            // 
            this.tbid.Location = new System.Drawing.Point(78, 18);
            this.tbid.Name = "tbid";
            this.tbid.Size = new System.Drawing.Size(297, 21);
            this.tbid.TabIndex = 4;
            // 
            // lbladddate
            // 
            this.lbladddate.AutoSize = true;
            this.lbladddate.Location = new System.Drawing.Point(8, 70);
            this.lbladddate.Name = "lbladddate";
            this.lbladddate.Size = new System.Drawing.Size(65, 12);
            this.lbladddate.TabIndex = 3;
            this.lbladddate.Text = "添加时间：";
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.Location = new System.Drawing.Point(7, 46);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(65, 12);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "项目标题：";
            // 
            // lblid
            // 
            this.lblid.AutoSize = true;
            this.lblid.Location = new System.Drawing.Point(7, 21);
            this.lblid.Name = "lblid";
            this.lblid.Size = new System.Drawing.Size(65, 12);
            this.lblid.TabIndex = 0;
            this.lblid.Text = "项目编号：";
            // 
            // btnUse
            // 
            this.btnUse.Location = new System.Drawing.Point(318, 335);
            this.btnUse.Name = "btnUse";
            this.btnUse.Size = new System.Drawing.Size(75, 23);
            this.btnUse.TabIndex = 3;
            this.btnUse.Text = "打开";
            this.btnUse.UseVisualStyleBackColor = true;
            this.btnUse.Click += new System.EventHandler(this.btnUse_Click);
            // 
            // GridProject
            // 
            this.GridProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.GridProject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.TITLE});
            this.GridProject.Location = new System.Drawing.Point(12, 12);
            this.GridProject.MultiSelect = false;
            this.GridProject.Name = "GridProject";
            this.GridProject.ReadOnly = true;
            this.GridProject.RowTemplate.Height = 23;
            this.GridProject.Size = new System.Drawing.Size(381, 184);
            this.GridProject.TabIndex = 4;
            this.GridProject.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridProject_CellDoubleClick);
            this.GridProject.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridProject_CellMouseClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 50;
            // 
            // TITLE
            // 
            this.TITLE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TITLE.DataPropertyName = "TITLE";
            this.TITLE.HeaderText = "项目名称";
            this.TITLE.Name = "TITLE";
            this.TITLE.ReadOnly = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::ImgLocation.Properties.Resources.pencil1;
            this.btnEdit.Location = new System.Drawing.Point(289, 202);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(30, 30);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::ImgLocation.Properties.Resources.cross;
            this.btnDelete.Location = new System.Drawing.Point(325, 202);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(30, 30);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            //this.btnAdd.Image = global::ImgLocation.Properties.Resources.edit_add;
            this.btnAdd.Location = new System.Drawing.Point(253, 202);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(30, 30);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::ImgLocation.Properties.Resources.disk;
            this.btnSave.Location = new System.Drawing.Point(361, 202);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(30, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 369);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.GridProject);
            this.Controls.Add(this.btnUse);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.GroupProject);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "项目管理";
            this.Load += new System.EventHandler(this.ProjectForm_Load);
            this.GroupProject.ResumeLayout(false);
            this.GroupProject.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridProject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupProject;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUse;
        private System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Label lblid;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lbladddate;
        private System.Windows.Forms.DateTimePicker dateAddDate;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.TextBox tbid;
        private System.Windows.Forms.DataGridView GridProject;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn TITLE;
    }
}