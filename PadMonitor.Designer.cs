namespace ImgLocation
{
    partial class PadMonitor
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PadMonitor));
            this.Imgs = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblNo = new System.Windows.Forms.Label();
            this.bkpm = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imgPushedData = new System.Windows.Forms.PictureBox();
            this.imgPlugined = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgPushedData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgPlugined)).BeginInit();
            this.SuspendLayout();
            // 
            // Imgs
            // 
            this.Imgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Imgs.ImageStream")));
            this.Imgs.TransparentColor = System.Drawing.Color.Transparent;
            this.Imgs.Images.SetKeyName(0, "2015030410383954_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(1, "signal_null.png");
            this.Imgs.Images.SetKeyName(2, "Connect_32px_529297_easyicon.net.png");
            this.Imgs.Images.SetKeyName(3, "folder_send.png");
            this.Imgs.Images.SetKeyName(4, "20150304104644980_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(5, "20150304104241354_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(6, "20150307023126717_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(7, "20150307023055713_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(8, "20150307023056544_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(9, "20150307023058730_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(10, "20150307023059206_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(11, "compile_32px_1086020_easyicon.net.png");
            this.Imgs.Images.SetKeyName(12, "compile_error_32px_1086021_easyicon.net.png");
            this.Imgs.Images.SetKeyName(13, "compile_warning_32px_1086022_easyicon.net.png");
            this.Imgs.Images.SetKeyName(14, "20150307024309954_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(15, "20150304104202689_easyicon_net_32.png");
            this.Imgs.Images.SetKeyName(16, "20150307023456606_easyicon_net_32.png");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "连接：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(124, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "数据：";
            // 
            // lblNo
            // 
            this.lblNo.AutoSize = true;
            this.lblNo.BackColor = System.Drawing.Color.Transparent;
            this.lblNo.Font = new System.Drawing.Font("黑体", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNo.Location = new System.Drawing.Point(23, 17);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(25, 15);
            this.lblNo.TabIndex = 1;
            this.lblNo.Text = "20";
            this.lblNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNo.Click += new System.EventHandler(this.lblNo_Click);
            // 
            // bkpm
            // 
            this.bkpm.WorkerReportsProgress = true;
            this.bkpm.WorkerSupportsCancellation = true;
            this.bkpm.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkpm_DoWork);
            this.bkpm.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bkpm_ProgressChanged);
            this.bkpm.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkpm_RunWorkerCompleted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(4, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // imgPushedData
            // 
            this.imgPushedData.Location = new System.Drawing.Point(164, 1);
            this.imgPushedData.Name = "imgPushedData";
            this.imgPushedData.Size = new System.Drawing.Size(32, 32);
            this.imgPushedData.TabIndex = 11;
            this.imgPushedData.TabStop = false;
            this.imgPushedData.Click += new System.EventHandler(this.imgPushedData_Click);
            // 
            // imgPlugined
            // 
            this.imgPlugined.Location = new System.Drawing.Point(84, 1);
            this.imgPlugined.Name = "imgPlugined";
            this.imgPlugined.Size = new System.Drawing.Size(32, 32);
            this.imgPlugined.TabIndex = 10;
            this.imgPlugined.TabStop = false;
            // 
            // PadMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblNo);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.imgPushedData);
            this.Controls.Add(this.imgPlugined);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "PadMonitor";
            this.Size = new System.Drawing.Size(205, 34);
            this.Load += new System.EventHandler(this.PadMonitor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgPushedData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgPlugined)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList Imgs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox imgPlugined;
        private System.Windows.Forms.PictureBox imgPushedData;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblNo;
        private System.ComponentModel.BackgroundWorker bkpm;
    }
}
