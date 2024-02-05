namespace ImageEnhancerMain
{
    partial class ImageChart
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
            this.labelRed = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelRed
            // 
            this.labelRed.AutoSize = true;
            this.labelRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 50.6F);
            this.labelRed.ForeColor = System.Drawing.Color.Red;
            this.labelRed.Location = new System.Drawing.Point(50, 460);
            this.labelRed.MinimumSize = new System.Drawing.Size(360, 20);
            this.labelRed.Name = "labelRed";
            this.labelRed.Size = new System.Drawing.Size(360, 78);
            this.labelRed.TabIndex = 0;
            this.labelRed.Text = "RED";
            this.labelRed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelRed.Click += new System.EventHandler(this.label1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 50.6F);
            this.label1.ForeColor = System.Drawing.Color.LimeGreen;
            this.label1.Location = new System.Drawing.Point(460, 460);
            this.label1.MinimumSize = new System.Drawing.Size(360, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 78);
            this.label1.TabIndex = 1;
            this.label1.Text = "GREEN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 50.6F);
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(870, 460);
            this.label2.MinimumSize = new System.Drawing.Size(360, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(360, 78);
            this.label2.TabIndex = 2;
            this.label2.Text = "BLUE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImageChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelRed);
            this.Name = "ImageChart";
            this.Text = "ImageChart";
            this.Load += new System.EventHandler(this.ImageChart_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}