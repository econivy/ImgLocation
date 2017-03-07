namespace ImgLocation.Forms
{
    partial class FolderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderForm));
            this.label1 = new System.Windows.Forms.Label();
            this.tbWord = new System.Windows.Forms.Label();
            this.tbRes = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbLrm = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.GroupAdd = new System.Windows.Forms.GroupBox();
            this.RadioAdd = new System.Windows.Forms.RadioButton();
            this.RadioOver = new System.Windows.Forms.RadioButton();
            this.btnLrm = new System.Windows.Forms.Button();
            this.btnRes = new System.Windows.Forms.Button();
            this.btnWord = new System.Windows.Forms.Button();
            this.GroupAdd.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "呈批件：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbWord
            // 
            this.tbWord.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbWord.Location = new System.Drawing.Point(96, 9);
            this.tbWord.Name = "tbWord";
            this.tbWord.Size = new System.Drawing.Size(450, 25);
            this.tbWord.TabIndex = 1;
            this.tbWord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbRes
            // 
            this.tbRes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbRes.Location = new System.Drawing.Point(96, 67);
            this.tbRes.Name = "tbRes";
            this.tbRes.Size = new System.Drawing.Size(450, 25);
            this.tbRes.TabIndex = 3;
            this.tbRes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "考察材料：";
            // 
            // tbLrm
            // 
            this.tbLrm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbLrm.Location = new System.Drawing.Point(96, 38);
            this.tbLrm.Name = "tbLrm";
            this.tbLrm.Size = new System.Drawing.Size(450, 25);
            this.tbLrm.TabIndex = 5;
            this.tbLrm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "任免审批表：";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(497, 153);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(416, 153);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 25);
            this.btnConvert.TabIndex = 10;
            this.btnConvert.Text = "确定";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // GroupAdd
            // 
            this.GroupAdd.Controls.Add(this.RadioAdd);
            this.GroupAdd.Controls.Add(this.RadioOver);
            this.GroupAdd.Location = new System.Drawing.Point(95, 96);
            this.GroupAdd.Name = "GroupAdd";
            this.GroupAdd.Size = new System.Drawing.Size(233, 51);
            this.GroupAdd.TabIndex = 11;
            this.GroupAdd.TabStop = false;
            this.GroupAdd.Text = "追加/覆盖";
            // 
            // RadioAdd
            // 
            this.RadioAdd.Checked = true;
            this.RadioAdd.Location = new System.Drawing.Point(63, 17);
            this.RadioAdd.Name = "RadioAdd";
            this.RadioAdd.Size = new System.Drawing.Size(55, 24);
            this.RadioAdd.TabIndex = 1;
            this.RadioAdd.TabStop = true;
            this.RadioAdd.Text = "追加";
            this.RadioAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RadioAdd.UseVisualStyleBackColor = true;
            // 
            // RadioOver
            // 
            this.RadioOver.Location = new System.Drawing.Point(6, 17);
            this.RadioOver.Name = "RadioOver";
            this.RadioOver.Size = new System.Drawing.Size(51, 24);
            this.RadioOver.TabIndex = 0;
            this.RadioOver.Text = "覆盖";
            this.RadioOver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RadioOver.UseVisualStyleBackColor = true;
            // 
            // btnLrm
            // 
            this.btnLrm.Image = global::ImgLocation.Properties.Resources.folder_picture;
            this.btnLrm.Location = new System.Drawing.Point(552, 38);
            this.btnLrm.Name = "btnLrm";
            this.btnLrm.Size = new System.Drawing.Size(25, 25);
            this.btnLrm.TabIndex = 8;
            this.btnLrm.UseVisualStyleBackColor = true;
            this.btnLrm.Click += new System.EventHandler(this.btnLrm_Click);
            // 
            // btnRes
            // 
            this.btnRes.Image = global::ImgLocation.Properties.Resources.folder_table;
            this.btnRes.Location = new System.Drawing.Point(552, 67);
            this.btnRes.Name = "btnRes";
            this.btnRes.Size = new System.Drawing.Size(25, 25);
            this.btnRes.TabIndex = 7;
            this.btnRes.UseVisualStyleBackColor = true;
            this.btnRes.Click += new System.EventHandler(this.btnRes_Click);
            // 
            // btnWord
            // 
            this.btnWord.Image = global::ImgLocation.Properties.Resources.folder_page;
            this.btnWord.Location = new System.Drawing.Point(552, 9);
            this.btnWord.Name = "btnWord";
            this.btnWord.Size = new System.Drawing.Size(25, 25);
            this.btnWord.TabIndex = 6;
            this.btnWord.UseVisualStyleBackColor = true;
            this.btnWord.Click += new System.EventHandler(this.btnWord_Click);
            // 
            // FolderForm
            // 
            this.AcceptButton = this.btnConvert;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 188);
            this.Controls.Add(this.GroupAdd);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLrm);
            this.Controls.Add(this.btnRes);
            this.Controls.Add(this.btnWord);
            this.Controls.Add(this.tbLrm);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbRes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbWord);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FolderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "目录导入";
            this.Load += new System.EventHandler(this.FolderForm_Load);
            this.GroupAdd.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label tbWord;
        private System.Windows.Forms.Label tbRes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label tbLrm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnWord;
        private System.Windows.Forms.Button btnRes;
        private System.Windows.Forms.Button btnLrm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.GroupBox GroupAdd;
        private System.Windows.Forms.RadioButton RadioAdd;
        private System.Windows.Forms.RadioButton RadioOver;
    }
}