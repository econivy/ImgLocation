namespace ImgLocation.Forms
{
    partial class PushDataForm
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
            this.components = new System.ComponentModel.Container();
            this.TimerDelayLoad = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.ProgressPushData = new System.Windows.Forms.ProgressBar();
            this.lblPad = new System.Windows.Forms.Label();
            this.bk = new System.ComponentModel.BackgroundWorker();
            this.tbOut = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // TimerDelayLoad
            // 
            this.TimerDelayLoad.Tick += new System.EventHandler(this.TimerDelayLoad_Tick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tbOut);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.ProgressPushData);
            this.panel1.Controls.Add(this.lblPad);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1151, 544);
            this.panel1.TabIndex = 0;
            // 
            // ProgressPushData
            // 
            this.ProgressPushData.Location = new System.Drawing.Point(69, 35);
            this.ProgressPushData.Maximum = 10000;
            this.ProgressPushData.Name = "ProgressPushData";
            this.ProgressPushData.Size = new System.Drawing.Size(1069, 25);
            this.ProgressPushData.TabIndex = 7;
            // 
            // lblPad
            // 
            this.lblPad.AutoSize = true;
            this.lblPad.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPad.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblPad.Location = new System.Drawing.Point(67, 10);
            this.lblPad.Name = "lblPad";
            this.lblPad.Size = new System.Drawing.Size(191, 22);
            this.lblPad.TabIndex = 6;
            this.lblPad.Text = "正在向平板{0}推送数据...";
            // 
            // bk
            // 
            this.bk.WorkerReportsProgress = true;
            this.bk.WorkerSupportsCancellation = true;
            this.bk.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkPushData_DoWork);
            this.bk.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorkPushData_ProgressChanged);
            this.bk.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkPushData_RunWorkerCompleted);
            // 
            // tbOut
            // 
            this.tbOut.Location = new System.Drawing.Point(12, 67);
            this.tbOut.Multiline = true;
            this.tbOut.Name = "tbOut";
            this.tbOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOut.Size = new System.Drawing.Size(1126, 464);
            this.tbOut.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ImgLocation.Properties.Resources.loading;
            this.pictureBox1.Location = new System.Drawing.Point(11, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // PushDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 544);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PushDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据推送";
            this.Load += new System.EventHandler(this.PushDataForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer TimerDelayLoad;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ProgressBar ProgressPushData;
        private System.Windows.Forms.Label lblPad;
        private System.ComponentModel.BackgroundWorker bk;
        private System.Windows.Forms.TextBox tbOut;
    }
}