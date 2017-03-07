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
            this.button1 = new System.Windows.Forms.Button();
            this.checkShowWord = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkChooseSameCadre = new System.Windows.Forms.CheckBox();
            this.GroupSearch.SuspendLayout();
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
            this.checkShowWord.CheckedChanged += new System.EventHandler(this.checkShowWord_CheckedChanged);
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
            this.Controls.Add(this.GroupSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ParamsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "参数管理";
            this.Load += new System.EventHandler(this.ParamsForm_Load);
            this.GroupSearch.ResumeLayout(false);
            this.GroupSearch.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupSearch;
        private System.Windows.Forms.RadioButton radioFull;
        private System.Windows.Forms.RadioButton radioInitial;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkShowWord;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkChooseSameCadre;
    }
}