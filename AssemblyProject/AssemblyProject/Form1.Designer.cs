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
            this.labelMin = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.ExecutionTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelSquareArea = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelImageArea = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSquareArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfThreads)).BeginInit();
            this.panel1.SuspendLayout();
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
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(300, 300);
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
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.Location = new System.Drawing.Point(12, 177);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(13, 13);
            this.labelMin.TabIndex = 8;
            this.labelMin.Text = "0";
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.Location = new System.Drawing.Point(283, 177);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(27, 13);
            this.labelMax.TabIndex = 9;
            this.labelMax.Text = "N/A";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 251);
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
            this.ExecutionTime.Location = new System.Drawing.Point(97, 251);
            this.ExecutionTime.Name = "ExecutionTime";
            this.ExecutionTime.Size = new System.Drawing.Size(13, 13);
            this.ExecutionTime.TabIndex = 5;
            this.ExecutionTime.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Square area size:";
            // 
            // labelSquareArea
            // 
            this.labelSquareArea.AutoSize = true;
            this.labelSquareArea.Location = new System.Drawing.Point(97, 226);
            this.labelSquareArea.Name = "labelSquareArea";
            this.labelSquareArea.Size = new System.Drawing.Size(24, 13);
            this.labelSquareArea.TabIndex = 14;
            this.labelSquareArea.Text = "0x0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(249, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Square area size: 2+(64*X) where X is Slider Value\"";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Total image area:";
            // 
            // labelImageArea
            // 
            this.labelImageArea.AutoSize = true;
            this.labelImageArea.Location = new System.Drawing.Point(97, 202);
            this.labelImageArea.Name = "labelImageArea";
            this.labelImageArea.Size = new System.Drawing.Size(24, 13);
            this.labelImageArea.TabIndex = 16;
            this.labelImageArea.Text = "0x0";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(8, 290);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(151, 46);
            this.buttonSave.TabIndex = 17;
            this.buttonSave.Text = "Save Image";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Location = new System.Drawing.Point(316, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1600, 900);
            this.panel1.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 545);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelImageArea);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelSquareArea);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonLoadImage);
            this.Controls.Add(this.ExecutionTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelMax);
            this.Controls.Add(this.labelMin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numberOfThreads);
            this.Controls.Add(this.trackBarSquareArea);
            this.Controls.Add(this.radioButtonASM);
            this.Controls.Add(this.radioButtonC);
            this.Name = "Form1";
            this.Text = "Image Enhancer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSquareArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfThreads)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private Label labelMin;
        private Label labelMax;
        private Label label5;
        private Button buttonLoadImage;
        private Label ExecutionTime;
        private Label label6;
        private Label labelSquareArea;
        private Label label3;
        private Label label2;
        private Label labelImageArea;
        private Button buttonSave;
        private Panel panel1;
    }
}

