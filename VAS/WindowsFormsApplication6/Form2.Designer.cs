namespace WindowsFormsApplication6
{
    partial class Form2
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Alpha_trackBar = new System.Windows.Forms.TrackBar();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.resultPictureBox = new System.Windows.Forms.PictureBox();
            this.ad_pictureBox = new System.Windows.Forms.PictureBox();
            this.adTrans_pictureBox = new System.Windows.Forms.PictureBox();
            this.Alpha_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Alpha_trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ad_pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adTrans_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(40, 444);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "上一步";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(753, 444);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "下一步";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox1.Location = new System.Drawing.Point(307, 37);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(97, 28);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "預覽畫面";
            // 
            // Alpha_trackBar
            // 
            this.Alpha_trackBar.Location = new System.Drawing.Point(40, 367);
            this.Alpha_trackBar.Maximum = 100;
            this.Alpha_trackBar.Name = "Alpha_trackBar";
            this.Alpha_trackBar.Size = new System.Drawing.Size(195, 45);
            this.Alpha_trackBar.TabIndex = 5;
            this.Alpha_trackBar.Scroll += new System.EventHandler(this.Alpha_trackBar_Scroll);
            this.Alpha_trackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Alpha_trackBar_MouseUp);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox3.Location = new System.Drawing.Point(40, 333);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(97, 28);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "透明度:";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // resultPictureBox
            // 
            this.resultPictureBox.Location = new System.Drawing.Point(307, 82);
            this.resultPictureBox.Name = "resultPictureBox";
            this.resultPictureBox.Size = new System.Drawing.Size(521, 330);
            this.resultPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.resultPictureBox.TabIndex = 9;
            this.resultPictureBox.TabStop = false;
            // 
            // ad_pictureBox
            // 
            this.ad_pictureBox.BackColor = System.Drawing.SystemColors.Control;
            this.ad_pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ad_pictureBox.Location = new System.Drawing.Point(40, 37);
            this.ad_pictureBox.Name = "ad_pictureBox";
            this.ad_pictureBox.Size = new System.Drawing.Size(223, 103);
            this.ad_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ad_pictureBox.TabIndex = 10;
            this.ad_pictureBox.TabStop = false;
            // 
            // adTrans_pictureBox
            // 
            this.adTrans_pictureBox.BackColor = System.Drawing.SystemColors.Control;
            this.adTrans_pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.adTrans_pictureBox.Location = new System.Drawing.Point(40, 174);
            this.adTrans_pictureBox.Name = "adTrans_pictureBox";
            this.adTrans_pictureBox.Size = new System.Drawing.Size(223, 107);
            this.adTrans_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.adTrans_pictureBox.TabIndex = 11;
            this.adTrans_pictureBox.TabStop = false;
            // 
            // Alpha_label
            // 
            this.Alpha_label.AutoSize = true;
            this.Alpha_label.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Alpha_label.Location = new System.Drawing.Point(231, 367);
            this.Alpha_label.Name = "Alpha_label";
            this.Alpha_label.Size = new System.Drawing.Size(32, 16);
            this.Alpha_label.TabIndex = 12;
            this.Alpha_label.Text = "100";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 479);
            this.Controls.Add(this.Alpha_label);
            this.Controls.Add(this.adTrans_pictureBox);
            this.Controls.Add(this.ad_pictureBox);
            this.Controls.Add(this.resultPictureBox);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.Alpha_trackBar);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::WindowsFormsApplication6.Properties.Settings.Default, "oo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Location = global::WindowsFormsApplication6.Properties.Settings.Default.oo;
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Alpha_trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ad_pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adTrans_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TrackBar Alpha_trackBar;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.PictureBox resultPictureBox;
        private System.Windows.Forms.PictureBox ad_pictureBox;
        private System.Windows.Forms.PictureBox adTrans_pictureBox;
        private System.Windows.Forms.Label Alpha_label;
    }
}