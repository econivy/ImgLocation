namespace ImgLocation.Forms
{
    partial class DocumentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbXH = new System.Windows.Forms.TextBox();
            this.tbMC = new System.Windows.Forms.TextBox();
            this.tbWH = new System.Windows.Forms.TextBox();
            this.groupCover = new System.Windows.Forms.GroupBox();
            this.tbPageRange = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnWord = new System.Windows.Forms.Button();
            this.tbWord = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkCover = new System.Windows.Forms.CheckBox();
            this.btnLrm = new System.Windows.Forms.Button();
            this.btnRes = new System.Windows.Forms.Button();
            this.tbLrm = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbRes = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCovert = new System.Windows.Forms.Button();
            this.tbid = new System.Windows.Forms.Label();
            this.groupRelocation = new System.Windows.Forms.GroupBox();
            this.checkRelocationCadre = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupCover.SuspendLayout();
            this.groupRelocation.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "编号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "文号：";
            // 
            // tbXH
            // 
            this.tbXH.Location = new System.Drawing.Point(55, 54);
            this.tbXH.Name = "tbXH";
            this.tbXH.Size = new System.Drawing.Size(45, 21);
            this.tbXH.TabIndex = 4;
            // 
            // tbMC
            // 
            this.tbMC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMC.Location = new System.Drawing.Point(106, 54);
            this.tbMC.Name = "tbMC";
            this.tbMC.Size = new System.Drawing.Size(799, 21);
            this.tbMC.TabIndex = 5;
            // 
            // tbWH
            // 
            this.tbWH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWH.Location = new System.Drawing.Point(106, 82);
            this.tbWH.Name = "tbWH";
            this.tbWH.Size = new System.Drawing.Size(799, 21);
            this.tbWH.TabIndex = 6;
            // 
            // groupCover
            // 
            this.groupCover.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCover.Controls.Add(this.label8);
            this.groupCover.Controls.Add(this.tbPageRange);
            this.groupCover.Controls.Add(this.label7);
            this.groupCover.Controls.Add(this.btnWord);
            this.groupCover.Controls.Add(this.tbWord);
            this.groupCover.Controls.Add(this.label5);
            this.groupCover.Location = new System.Drawing.Point(14, 178);
            this.groupCover.Name = "groupCover";
            this.groupCover.Size = new System.Drawing.Size(929, 135);
            this.groupCover.TabIndex = 7;
            this.groupCover.TabStop = false;
            // 
            // tbPageRange
            // 
            this.tbPageRange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPageRange.Location = new System.Drawing.Point(106, 64);
            this.tbPageRange.Name = "tbPageRange";
            this.tbPageRange.Size = new System.Drawing.Size(799, 21);
            this.tbPageRange.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "替换页码范围：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnWord
            // 
            this.btnWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWord.Image = global::ImgLocation.Properties.Resources.folder_page;
            this.btnWord.Location = new System.Drawing.Point(879, 24);
            this.btnWord.Name = "btnWord";
            this.btnWord.Size = new System.Drawing.Size(25, 25);
            this.btnWord.TabIndex = 15;
            this.btnWord.UseVisualStyleBackColor = true;
            this.btnWord.Click += new System.EventHandler(this.btnWord_Click);
            // 
            // tbWord
            // 
            this.tbWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWord.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbWord.Location = new System.Drawing.Point(106, 24);
            this.tbWord.Name = "tbWord";
            this.tbWord.Size = new System.Drawing.Size(767, 25);
            this.tbWord.TabIndex = 10;
            this.tbWord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "呈批件原始路径：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkCover
            // 
            this.checkCover.AutoSize = true;
            this.checkCover.Location = new System.Drawing.Point(14, 166);
            this.checkCover.Name = "checkCover";
            this.checkCover.Size = new System.Drawing.Size(180, 16);
            this.checkCover.TabIndex = 10;
            this.checkCover.Text = "替换呈批件或者呈批件指定页";
            this.checkCover.UseVisualStyleBackColor = true;
            this.checkCover.CheckedChanged += new System.EventHandler(this.checkCover_CheckedChanged);
            // 
            // btnLrm
            // 
            this.btnLrm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLrm.Image = global::ImgLocation.Properties.Resources.folder_picture;
            this.btnLrm.Location = new System.Drawing.Point(879, 33);
            this.btnLrm.Name = "btnLrm";
            this.btnLrm.Size = new System.Drawing.Size(25, 25);
            this.btnLrm.TabIndex = 17;
            this.btnLrm.UseVisualStyleBackColor = true;
            this.btnLrm.Click += new System.EventHandler(this.btnLrm_Click);
            // 
            // btnRes
            // 
            this.btnRes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRes.Image = global::ImgLocation.Properties.Resources.folder_table;
            this.btnRes.Location = new System.Drawing.Point(879, 62);
            this.btnRes.Name = "btnRes";
            this.btnRes.Size = new System.Drawing.Size(25, 25);
            this.btnRes.TabIndex = 16;
            this.btnRes.UseVisualStyleBackColor = true;
            this.btnRes.Click += new System.EventHandler(this.btnRes_Click);
            // 
            // tbLrm
            // 
            this.tbLrm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLrm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbLrm.Location = new System.Drawing.Point(106, 33);
            this.tbLrm.Name = "tbLrm";
            this.tbLrm.Size = new System.Drawing.Size(767, 25);
            this.tbLrm.TabIndex = 14;
            this.tbLrm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "任免审批表目录：";
            // 
            // tbRes
            // 
            this.tbRes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbRes.Location = new System.Drawing.Point(106, 62);
            this.tbRes.Name = "tbRes";
            this.tbRes.Size = new System.Drawing.Size(767, 25);
            this.tbRes.TabIndex = 12;
            this.tbRes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "干部考察材料目录：";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(870, 474);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCovert
            // 
            this.btnCovert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCovert.Location = new System.Drawing.Point(789, 474);
            this.btnCovert.Name = "btnCovert";
            this.btnCovert.Size = new System.Drawing.Size(75, 23);
            this.btnCovert.TabIndex = 9;
            this.btnCovert.Text = "确定";
            this.btnCovert.UseVisualStyleBackColor = true;
            this.btnCovert.Click += new System.EventHandler(this.btnCovert_Click);
            // 
            // tbid
            // 
            this.tbid.AutoSize = true;
            this.tbid.Location = new System.Drawing.Point(56, 30);
            this.tbid.Name = "tbid";
            this.tbid.Size = new System.Drawing.Size(17, 12);
            this.tbid.TabIndex = 11;
            this.tbid.Text = "id";
            this.tbid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupRelocation
            // 
            this.groupRelocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupRelocation.Controls.Add(this.btnLrm);
            this.groupRelocation.Controls.Add(this.label6);
            this.groupRelocation.Controls.Add(this.label4);
            this.groupRelocation.Controls.Add(this.tbRes);
            this.groupRelocation.Controls.Add(this.btnRes);
            this.groupRelocation.Controls.Add(this.tbLrm);
            this.groupRelocation.Location = new System.Drawing.Point(14, 348);
            this.groupRelocation.Name = "groupRelocation";
            this.groupRelocation.Size = new System.Drawing.Size(929, 114);
            this.groupRelocation.TabIndex = 16;
            this.groupRelocation.TabStop = false;
            // 
            // checkRelocationCadre
            // 
            this.checkRelocationCadre.AutoSize = true;
            this.checkRelocationCadre.Location = new System.Drawing.Point(15, 335);
            this.checkRelocationCadre.Name = "checkRelocationCadre";
            this.checkRelocationCadre.Size = new System.Drawing.Size(168, 16);
            this.checkRelocationCadre.TabIndex = 10;
            this.checkRelocationCadre.Text = "重新检索页码范围内的干部";
            this.checkRelocationCadre.UseVisualStyleBackColor = true;
            this.checkRelocationCadre.CheckedChanged += new System.EventHandler(this.checkRelocationCadre_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.tbid);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.tbXH);
            this.groupBox3.Controls.Add(this.tbMC);
            this.groupBox3.Controls.Add(this.tbWH);
            this.groupBox3.Location = new System.Drawing.Point(14, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(927, 131);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "呈批件信息修改";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(8, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(525, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "注意：如果替换的页面中有干部信息，请务必选中“重新检索页码范围内的干部”复选框。";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 512);
            this.Controls.Add(this.checkRelocationCadre);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupRelocation);
            this.Controls.Add(this.checkCover);
            this.Controls.Add(this.btnCovert);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupCover);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DocumentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文档选择";
            this.Load += new System.EventHandler(this.DocumentForm_Load);
            this.groupCover.ResumeLayout(false);
            this.groupCover.PerformLayout();
            this.groupRelocation.ResumeLayout(false);
            this.groupRelocation.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbXH;
        private System.Windows.Forms.TextBox tbMC;
        private System.Windows.Forms.TextBox tbWH;
        private System.Windows.Forms.GroupBox groupCover;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCovert;
        private System.Windows.Forms.Button btnLrm;
        private System.Windows.Forms.Button btnRes;
        private System.Windows.Forms.Button btnWord;
        private System.Windows.Forms.Label tbLrm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label tbRes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label tbWord;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkCover;
        private System.Windows.Forms.Label tbid;
        private System.Windows.Forms.GroupBox groupRelocation;
        private System.Windows.Forms.CheckBox checkRelocationCadre;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbPageRange;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}