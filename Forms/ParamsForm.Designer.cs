namespace ImgLocation.Forms
{
    partial class ParamsForm
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
            this.GroupSearch = new System.Windows.Forms.GroupBox();
            this.radioFull = new System.Windows.Forms.RadioButton();
            this.radioInitial = new System.Windows.Forms.RadioButton();
            this.GroupModel = new System.Windows.Forms.GroupBox();
            this.groupRedModel = new System.Windows.Forms.GroupBox();
            this.tbM3Top = new System.Windows.Forms.TextBox();
            this.tbM2Top = new System.Windows.Forms.TextBox();
            this.tbM3Left = new System.Windows.Forms.TextBox();
            this.tbM2Left = new System.Windows.Forms.TextBox();
            this.tbM1Top = new System.Windows.Forms.TextBox();
            this.tbM1Left = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkAttachLrmRes = new System.Windows.Forms.CheckBox();
            this.checkAddRedTitle = new System.Windows.Forms.CheckBox();
            this.radioSWZZB = new System.Windows.Forms.RadioButton();
            this.radioZZB = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.checkShowWord = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkChooseSameCadre = new System.Windows.Forms.CheckBox();
            this.GroupSearch.SuspendLayout();
            this.GroupModel.SuspendLayout();
            this.groupRedModel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupSearch
            // 
            this.GroupSearch.Controls.Add(this.radioFull);
            this.GroupSearch.Controls.Add(this.radioInitial);
            this.GroupSearch.Location = new System.Drawing.Point(8, 8);
            this.GroupSearch.Margin = new System.Windows.Forms.Padding(2);
            this.GroupSearch.Name = "GroupSearch";
            this.GroupSearch.Padding = new System.Windows.Forms.Padding(2);
            this.GroupSearch.Size = new System.Drawing.Size(389, 52);
            this.GroupSearch.TabIndex = 0;
            this.GroupSearch.TabStop = false;
            this.GroupSearch.Text = "干部查询选项";
            // 
            // radioFull
            // 
            this.radioFull.AutoSize = true;
            this.radioFull.Location = new System.Drawing.Point(205, 20);
            this.radioFull.Margin = new System.Windows.Forms.Padding(2);
            this.radioFull.Name = "radioFull";
            this.radioFull.Size = new System.Drawing.Size(71, 16);
            this.radioFull.TabIndex = 1;
            this.radioFull.TabStop = true;
            this.radioFull.Text = "全文查找";
            this.radioFull.UseVisualStyleBackColor = true;
            // 
            // radioInitial
            // 
            this.radioInitial.AutoSize = true;
            this.radioInitial.Location = new System.Drawing.Point(20, 20);
            this.radioInitial.Margin = new System.Windows.Forms.Padding(2);
            this.radioInitial.Name = "radioInitial";
            this.radioInitial.Size = new System.Drawing.Size(71, 16);
            this.radioInitial.TabIndex = 0;
            this.radioInitial.TabStop = true;
            this.radioInitial.Text = "段首查找";
            this.radioInitial.UseVisualStyleBackColor = true;
            // 
            // GroupModel
            // 
            this.GroupModel.Controls.Add(this.groupRedModel);
            this.GroupModel.Controls.Add(this.checkAttachLrmRes);
            this.GroupModel.Controls.Add(this.checkAddRedTitle);
            this.GroupModel.Controls.Add(this.radioSWZZB);
            this.GroupModel.Controls.Add(this.radioZZB);
            this.GroupModel.Location = new System.Drawing.Point(8, 64);
            this.GroupModel.Margin = new System.Windows.Forms.Padding(2);
            this.GroupModel.Name = "GroupModel";
            this.GroupModel.Padding = new System.Windows.Forms.Padding(2);
            this.GroupModel.Size = new System.Drawing.Size(389, 264);
            this.GroupModel.TabIndex = 1;
            this.GroupModel.TabStop = false;
            this.GroupModel.Text = "文档转换选项";
            // 
            // groupRedModel
            // 
            this.groupRedModel.Controls.Add(this.tbM3Top);
            this.groupRedModel.Controls.Add(this.tbM2Top);
            this.groupRedModel.Controls.Add(this.tbM3Left);
            this.groupRedModel.Controls.Add(this.tbM2Left);
            this.groupRedModel.Controls.Add(this.tbM1Top);
            this.groupRedModel.Controls.Add(this.tbM1Left);
            this.groupRedModel.Controls.Add(this.label5);
            this.groupRedModel.Controls.Add(this.label4);
            this.groupRedModel.Controls.Add(this.label3);
            this.groupRedModel.Controls.Add(this.label2);
            this.groupRedModel.Controls.Add(this.label1);
            this.groupRedModel.Location = new System.Drawing.Point(20, 109);
            this.groupRedModel.Name = "groupRedModel";
            this.groupRedModel.Size = new System.Drawing.Size(348, 137);
            this.groupRedModel.TabIndex = 6;
            this.groupRedModel.TabStop = false;
            this.groupRedModel.Text = "中组部红头模板偏移量";
            // 
            // tbM3Top
            // 
            this.tbM3Top.Location = new System.Drawing.Point(240, 105);
            this.tbM3Top.Name = "tbM3Top";
            this.tbM3Top.Size = new System.Drawing.Size(100, 21);
            this.tbM3Top.TabIndex = 10;
            // 
            // tbM2Top
            // 
            this.tbM2Top.Location = new System.Drawing.Point(240, 74);
            this.tbM2Top.Name = "tbM2Top";
            this.tbM2Top.Size = new System.Drawing.Size(100, 21);
            this.tbM2Top.TabIndex = 9;
            // 
            // tbM3Left
            // 
            this.tbM3Left.Location = new System.Drawing.Point(121, 105);
            this.tbM3Left.Name = "tbM3Left";
            this.tbM3Left.Size = new System.Drawing.Size(100, 21);
            this.tbM3Left.TabIndex = 8;
            // 
            // tbM2Left
            // 
            this.tbM2Left.Location = new System.Drawing.Point(121, 74);
            this.tbM2Left.Name = "tbM2Left";
            this.tbM2Left.Size = new System.Drawing.Size(100, 21);
            this.tbM2Left.TabIndex = 7;
            // 
            // tbM1Top
            // 
            this.tbM1Top.Location = new System.Drawing.Point(240, 43);
            this.tbM1Top.Name = "tbM1Top";
            this.tbM1Top.Size = new System.Drawing.Size(100, 21);
            this.tbM1Top.TabIndex = 6;
            // 
            // tbM1Left
            // 
            this.tbM1Left.Location = new System.Drawing.Point(121, 43);
            this.tbM1Left.Name = "tbM1Left";
            this.tbM1Left.Size = new System.Drawing.Size(100, 21);
            this.tbM1Left.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(239, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "垂直偏移量（mm）";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(125, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "水平偏移量（mm）";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "干任字红头模板";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "组任字红头模板";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "请示件红头模板";
            // 
            // checkAttachLrmRes
            // 
            this.checkAttachLrmRes.AutoSize = true;
            this.checkAttachLrmRes.Location = new System.Drawing.Point(20, 86);
            this.checkAttachLrmRes.Name = "checkAttachLrmRes";
            this.checkAttachLrmRes.Size = new System.Drawing.Size(168, 16);
            this.checkAttachLrmRes.TabIndex = 5;
            this.checkAttachLrmRes.Text = "文档后附任免表和考察材料";
            this.checkAttachLrmRes.UseVisualStyleBackColor = true;
            // 
            // checkAddRedTitle
            // 
            this.checkAddRedTitle.AutoSize = true;
            this.checkAddRedTitle.Location = new System.Drawing.Point(20, 53);
            this.checkAddRedTitle.Name = "checkAddRedTitle";
            this.checkAddRedTitle.Size = new System.Drawing.Size(156, 16);
            this.checkAddRedTitle.TabIndex = 4;
            this.checkAddRedTitle.Text = "自动添加中组部红头背景";
            this.checkAddRedTitle.UseVisualStyleBackColor = true;
            // 
            // radioSWZZB
            // 
            this.radioSWZZB.AutoSize = true;
            this.radioSWZZB.Location = new System.Drawing.Point(205, 20);
            this.radioSWZZB.Margin = new System.Windows.Forms.Padding(2);
            this.radioSWZZB.Name = "radioSWZZB";
            this.radioSWZZB.Size = new System.Drawing.Size(107, 16);
            this.radioSWZZB.TabIndex = 1;
            this.radioSWZZB.TabStop = true;
            this.radioSWZZB.Text = "省委组织部模板";
            this.radioSWZZB.UseVisualStyleBackColor = true;
            // 
            // radioZZB
            // 
            this.radioZZB.AutoSize = true;
            this.radioZZB.Location = new System.Drawing.Point(20, 20);
            this.radioZZB.Margin = new System.Windows.Forms.Padding(2);
            this.radioZZB.Name = "radioZZB";
            this.radioZZB.Size = new System.Drawing.Size(107, 16);
            this.radioZZB.TabIndex = 0;
            this.radioZZB.TabStop = true;
            this.radioZZB.Text = "中央组织部模板";
            this.radioZZB.UseVisualStyleBackColor = true;
            this.radioZZB.CheckedChanged += new System.EventHandler(this.radioZZB_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(317, 436);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 27);
            this.button1.TabIndex = 2;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkShowWord
            // 
            this.checkShowWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkShowWord.AutoSize = true;
            this.checkShowWord.Location = new System.Drawing.Point(20, 29);
            this.checkShowWord.Name = "checkShowWord";
            this.checkShowWord.Size = new System.Drawing.Size(120, 16);
            this.checkShowWord.TabIndex = 5;
            this.checkShowWord.Text = "是否显示Word程序";
            this.checkShowWord.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkChooseSameCadre);
            this.groupBox1.Controls.Add(this.checkShowWord);
            this.groupBox1.Location = new System.Drawing.Point(8, 333);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(389, 99);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "其他转换选项";
            // 
            // checkChooseSameCadre
            // 
            this.checkChooseSameCadre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkChooseSameCadre.AutoSize = true;
            this.checkChooseSameCadre.Location = new System.Drawing.Point(20, 61);
            this.checkChooseSameCadre.Name = "checkChooseSameCadre";
            this.checkChooseSameCadre.Size = new System.Drawing.Size(96, 16);
            this.checkChooseSameCadre.TabIndex = 6;
            this.checkChooseSameCadre.Text = "是否开启提醒";
            this.checkChooseSameCadre.UseVisualStyleBackColor = true;
            // 
            // ParamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 474);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.GroupModel);
            this.Controls.Add(this.GroupSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ParamsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "参数管理";
            this.Load += new System.EventHandler(this.ParamsForm_Load);
            this.GroupSearch.ResumeLayout(false);
            this.GroupSearch.PerformLayout();
            this.GroupModel.ResumeLayout(false);
            this.GroupModel.PerformLayout();
            this.groupRedModel.ResumeLayout(false);
            this.groupRedModel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupSearch;
        private System.Windows.Forms.RadioButton radioFull;
        private System.Windows.Forms.RadioButton radioInitial;
        private System.Windows.Forms.GroupBox GroupModel;
        private System.Windows.Forms.RadioButton radioSWZZB;
        private System.Windows.Forms.RadioButton radioZZB;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkAddRedTitle;
        private System.Windows.Forms.CheckBox checkShowWord;
        private System.Windows.Forms.CheckBox checkAttachLrmRes;
        private System.Windows.Forms.GroupBox groupRedModel;
        private System.Windows.Forms.TextBox tbM3Top;
        private System.Windows.Forms.TextBox tbM2Top;
        private System.Windows.Forms.TextBox tbM3Left;
        private System.Windows.Forms.TextBox tbM2Left;
        private System.Windows.Forms.TextBox tbM1Top;
        private System.Windows.Forms.TextBox tbM1Left;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkChooseSameCadre;
    }
}