namespace ImgLocation.Forms
{
    partial class CadreForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CadreForm));
            this.checkSPB = new System.Windows.Forms.CheckBox();
            this.lbllrm = new System.Windows.Forms.Label();
            this.lblres = new System.Windows.Forms.Label();
            this.checkKCCL = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnres = new System.Windows.Forms.Button();
            this.btnlrm = new System.Windows.Forms.Button();
            this.btnpic = new System.Windows.Forms.Button();
            this.lblpic = new System.Windows.Forms.Label();
            this.gSPB = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupKCCL = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupOther = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblOther = new System.Windows.Forms.Label();
            this.btnOther = new System.Windows.Forms.Button();
            this.checkOther = new System.Windows.Forms.CheckBox();
            this.gSPB.SuspendLayout();
            this.groupKCCL.SuspendLayout();
            this.groupOther.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkSPB
            // 
            this.checkSPB.AutoSize = true;
            this.checkSPB.Location = new System.Drawing.Point(12, 12);
            this.checkSPB.Name = "checkSPB";
            this.checkSPB.Size = new System.Drawing.Size(84, 16);
            this.checkSPB.TabIndex = 0;
            this.checkSPB.Text = "任免审批表";
            this.checkSPB.UseVisualStyleBackColor = true;
            this.checkSPB.CheckedChanged += new System.EventHandler(this.checkLrm_CheckedChanged);
            // 
            // lbllrm
            // 
            this.lbllrm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbllrm.Location = new System.Drawing.Point(39, 24);
            this.lbllrm.Name = "lbllrm";
            this.lbllrm.Size = new System.Drawing.Size(525, 26);
            this.lbllrm.TabIndex = 1;
            this.lbllrm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblres
            // 
            this.lblres.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblres.Location = new System.Drawing.Point(39, 22);
            this.lblres.Name = "lblres";
            this.lblres.Size = new System.Drawing.Size(525, 26);
            this.lblres.TabIndex = 4;
            this.lblres.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkKCCL
            // 
            this.checkKCCL.AutoSize = true;
            this.checkKCCL.Location = new System.Drawing.Point(11, 151);
            this.checkKCCL.Name = "checkKCCL";
            this.checkKCCL.Size = new System.Drawing.Size(72, 16);
            this.checkKCCL.TabIndex = 3;
            this.checkKCCL.Text = "考察材料";
            this.checkKCCL.UseVisualStyleBackColor = true;
            this.checkKCCL.CheckedChanged += new System.EventHandler(this.checkRes_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(548, 359);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(467, 359);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnres
            // 
            this.btnres.Image = global::ImgLocation.Properties.Resources.comment_edit;
            this.btnres.Location = new System.Drawing.Point(570, 19);
            this.btnres.Name = "btnres";
            this.btnres.Size = new System.Drawing.Size(30, 30);
            this.btnres.TabIndex = 5;
            this.btnres.UseVisualStyleBackColor = true;
            this.btnres.Click += new System.EventHandler(this.btnres_Click);
            // 
            // btnlrm
            // 
            this.btnlrm.Image = global::ImgLocation.Properties.Resources.user_edit;
            this.btnlrm.Location = new System.Drawing.Point(570, 21);
            this.btnlrm.Name = "btnlrm";
            this.btnlrm.Size = new System.Drawing.Size(30, 30);
            this.btnlrm.TabIndex = 2;
            this.btnlrm.UseVisualStyleBackColor = true;
            this.btnlrm.Click += new System.EventHandler(this.btnlrm_Click);
            // 
            // btnpic
            // 
            this.btnpic.Image = global::ImgLocation.Properties.Resources.user_edit;
            this.btnpic.Location = new System.Drawing.Point(570, 53);
            this.btnpic.Name = "btnpic";
            this.btnpic.Size = new System.Drawing.Size(30, 30);
            this.btnpic.TabIndex = 9;
            this.btnpic.UseVisualStyleBackColor = true;
            this.btnpic.Click += new System.EventHandler(this.btnpic_Click);
            // 
            // lblpic
            // 
            this.lblpic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblpic.Location = new System.Drawing.Point(39, 55);
            this.lblpic.Name = "lblpic";
            this.lblpic.Size = new System.Drawing.Size(525, 26);
            this.lblpic.TabIndex = 8;
            this.lblpic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gSPB
            // 
            this.gSPB.Controls.Add(this.label3);
            this.gSPB.Controls.Add(this.label2);
            this.gSPB.Controls.Add(this.btnpic);
            this.gSPB.Controls.Add(this.lbllrm);
            this.gSPB.Controls.Add(this.lblpic);
            this.gSPB.Controls.Add(this.btnlrm);
            this.gSPB.Location = new System.Drawing.Point(11, 33);
            this.gSPB.Name = "gSPB";
            this.gSPB.Size = new System.Drawing.Size(612, 101);
            this.gSPB.TabIndex = 10;
            this.gSPB.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "pic：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "lrm：";
            // 
            // groupKCCL
            // 
            this.groupKCCL.Controls.Add(this.label4);
            this.groupKCCL.Controls.Add(this.lblres);
            this.groupKCCL.Controls.Add(this.btnres);
            this.groupKCCL.Location = new System.Drawing.Point(11, 173);
            this.groupKCCL.Name = "groupKCCL";
            this.groupKCCL.Size = new System.Drawing.Size(612, 62);
            this.groupKCCL.TabIndex = 11;
            this.groupKCCL.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "word：";
            // 
            // groupOther
            // 
            this.groupOther.Controls.Add(this.label1);
            this.groupOther.Controls.Add(this.lblOther);
            this.groupOther.Controls.Add(this.btnOther);
            this.groupOther.Location = new System.Drawing.Point(11, 277);
            this.groupOther.Name = "groupOther";
            this.groupOther.Size = new System.Drawing.Size(612, 62);
            this.groupOther.TabIndex = 13;
            this.groupOther.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "pdf：";
            // 
            // lblOther
            // 
            this.lblOther.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblOther.Location = new System.Drawing.Point(39, 22);
            this.lblOther.Name = "lblOther";
            this.lblOther.Size = new System.Drawing.Size(525, 26);
            this.lblOther.TabIndex = 4;
            this.lblOther.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOther
            // 
            this.btnOther.Image = global::ImgLocation.Properties.Resources.comment_edit;
            this.btnOther.Location = new System.Drawing.Point(570, 19);
            this.btnOther.Name = "btnOther";
            this.btnOther.Size = new System.Drawing.Size(30, 30);
            this.btnOther.TabIndex = 5;
            this.btnOther.UseVisualStyleBackColor = true;
            this.btnOther.Click += new System.EventHandler(this.btnOther_Click);
            // 
            // checkOther
            // 
            this.checkOther.AutoSize = true;
            this.checkOther.Location = new System.Drawing.Point(11, 255);
            this.checkOther.Name = "checkOther";
            this.checkOther.Size = new System.Drawing.Size(72, 16);
            this.checkOther.TabIndex = 12;
            this.checkOther.Text = "其他文件";
            this.checkOther.UseVisualStyleBackColor = true;
            this.checkOther.CheckedChanged += new System.EventHandler(this.checkOther_CheckedChanged);
            // 
            // CadreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 401);
            this.Controls.Add(this.groupOther);
            this.Controls.Add(this.checkOther);
            this.Controls.Add(this.groupKCCL);
            this.Controls.Add(this.checkKCCL);
            this.Controls.Add(this.checkSPB);
            this.Controls.Add(this.gSPB);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CadreForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "干部信息选择";
            this.Load += new System.EventHandler(this.CadreForm_Load);
            this.gSPB.ResumeLayout(false);
            this.gSPB.PerformLayout();
            this.groupKCCL.ResumeLayout(false);
            this.groupKCCL.PerformLayout();
            this.groupOther.ResumeLayout(false);
            this.groupOther.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkSPB;
        private System.Windows.Forms.Label lbllrm;
        private System.Windows.Forms.Button btnlrm;
        private System.Windows.Forms.Button btnres;
        private System.Windows.Forms.Label lblres;
        private System.Windows.Forms.CheckBox checkKCCL;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnpic;
        private System.Windows.Forms.Label lblpic;
        private System.Windows.Forms.GroupBox gSPB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupKCCL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupOther;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOther;
        private System.Windows.Forms.Button btnOther;
        private System.Windows.Forms.CheckBox checkOther;
    }
}