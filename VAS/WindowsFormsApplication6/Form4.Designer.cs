namespace WindowsFormsApplication6
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            this.vlc1 = new AxAXVLC.AxVLCPlugin2();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textMin = new System.Windows.Forms.TextBox();
            this.textSec = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vlc1)).BeginInit();
            this.SuspendLayout();
            // 
            // vlc1
            // 
            this.vlc1.Enabled = true;
            this.vlc1.Location = new System.Drawing.Point(16, 47);
            this.vlc1.Name = "vlc1";
            this.vlc1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("vlc1.OcxState")));
            this.vlc1.Size = new System.Drawing.Size(746, 405);
            this.vlc1.TabIndex = 0;
            this.vlc1.MediaPlayerTimeChanged += new AxAXVLC.DVLCEvents_MediaPlayerTimeChangedEventHandler(this.vlc1_MediaPlayerTimeChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(683, 463);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "確定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 11F);
            this.label1.Location = new System.Drawing.Point(14, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "請選擇影片中空白球場之影格";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(499, 474);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "時間:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(571, 474);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "分";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(628, 474);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "秒 處";
            // 
            // textMin
            // 
            this.textMin.Location = new System.Drawing.Point(537, 464);
            this.textMin.Name = "textMin";
            this.textMin.Size = new System.Drawing.Size(28, 22);
            this.textMin.TabIndex = 7;
            // 
            // textSec
            // 
            this.textSec.Location = new System.Drawing.Point(594, 464);
            this.textSec.Name = "textSec";
            this.textSec.Size = new System.Drawing.Size(28, 22);
            this.textSec.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 463);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 498);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textSec);
            this.Controls.Add(this.textMin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.vlc1);
            this.Name = "Form4";
            this.Text = "Form4";
            ((System.ComponentModel.ISupportInitialize)(this.vlc1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxAXVLC.AxVLCPlugin2 vlc1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textSec;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textMin;
    }
}