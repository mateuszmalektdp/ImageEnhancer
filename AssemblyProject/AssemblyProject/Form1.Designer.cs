using System.Windows.Forms;

namespace AssemblyProject
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioButtonC = new System.Windows.Forms.RadioButton();
            this.radioButtonASM = new System.Windows.Forms.RadioButton();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.trackBarSquareArea = new System.Windows.Forms.TrackBar();
            this.numberOfThreads = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.ExecutionTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSquareArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfThreads)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonC
            // 
            this.radioButtonC.AutoSize = true;
            this.radioButtonC.Location = new System.Drawing.Point(192, 18);
            this.radioButtonC.Name = "radioButtonC";
            this.radioButtonC.Size = new System.Drawing.Size(69, 17);
            this.radioButtonC.TabIndex = 0;
            this.radioButtonC.TabStop = true;
            this.radioButtonC.Text = "Using C#";
            this.radioButtonC.UseVisualStyleBackColor = true;
            this.radioButtonC.CheckedChanged += new System.EventHandler(this.radioButtonC_CheckedChanged);
            // 
            // radioButtonASM
            // 
            this.radioButtonASM.AutoSize = true;
            this.radioButtonASM.Location = new System.Drawing.Point(192, 41);
            this.radioButtonASM.Name = "radioButtonASM";
            this.radioButtonASM.Size = new System.Drawing.Size(98, 17);
            this.radioButtonASM.TabIndex = 1;
            this.radioButtonASM.TabStop = true;
            this.radioButtonASM.Text = "Using assembly";
            this.radioButtonASM.UseVisualStyleBackColor = true;
            this.radioButtonASM.CheckedChanged += new System.EventHandler(this.radioButtonASM_CheckedChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(314, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1024, 768);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // trackBarSquareArea
            // 
            this.trackBarSquareArea.Location = new System.Drawing.Point(8, 145);
            this.trackBarSquareArea.Margin = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.trackBarSquareArea.MaximumSize = new System.Drawing.Size(30, 0);
            this.trackBarSquareArea.MinimumSize = new System.Drawing.Size(300, 0);
            this.trackBarSquareArea.Name = "trackBarSquareArea";
            this.trackBarSquareArea.Size = new System.Drawing.Size(300, 45);
            this.trackBarSquareArea.TabIndex = 3;
            this.trackBarSquareArea.Scroll += new System.EventHandler(this.trackBarSquareArea_Scroll);
            // 
            // numberOfThreads
            // 
            this.numberOfThreads.Location = new System.Drawing.Point(11, 80);
            this.numberOfThreads.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numberOfThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfThreads.Name = "numberOfThreads";
            this.numberOfThreads.Size = new System.Drawing.Size(120, 20);
            this.numberOfThreads.TabIndex = 4;
            this.numberOfThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfThreads.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Number of Threads";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(300, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Square area size: 64x64 for value 1, ... 128x128 for value 2...\"";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(283, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "640";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Execution Time:";
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Location = new System.Drawing.Point(8, 12);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(151, 46);
            this.buttonLoadImage.TabIndex = 12;
            this.buttonLoadImage.Text = "Select Image";
            this.buttonLoadImage.UseVisualStyleBackColor = true;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // ExecutionTime
            // 
            this.ExecutionTime.AutoSize = true;
            this.ExecutionTime.Location = new System.Drawing.Point(97, 213);
            this.ExecutionTime.Name = "ExecutionTime";
            this.ExecutionTime.Size = new System.Drawing.Size(13, 13);
            this.ExecutionTime.TabIndex = 5;
            this.ExecutionTime.Text = "0";
            this.ExecutionTime.Click += new System.EventHandler(this.ExecutionTime_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 545);
            this.Controls.Add(this.buttonLoadImage);
            this.Controls.Add(this.ExecutionTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numberOfThreads);
            this.Controls.Add(this.trackBarSquareArea);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.radioButtonASM);
            this.Controls.Add(this.radioButtonC);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSquareArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfThreads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonC;
        private System.Windows.Forms.RadioButton radioButtonASM;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TrackBar trackBarSquareArea;
        private NumericUpDown numberOfThreads;
        private Label label1;
        private Label label3;
        private Label label2;
        private Label label4;
        private Label label5;
        private Button buttonLoadImage;
        private Label ExecutionTime;
    }
}

