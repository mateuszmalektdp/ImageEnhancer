using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ImageEnhancerMain
{
    public partial class Form1 : Form
    {
        Stopwatch stopwatch;
        DLLManager dllManager;
        BitmapManager bitmapManager;
        string fileToConvert;
        int[] imageToArray;
        int[] rectToArray;
        int[][] splittedArray;
        Bitmap image;

        public Form1()
        {
            InitializeComponent();
            dllManager = new DLLManager();
            bitmapManager = new BitmapManager();
            RunTimerFirstTime();
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Title = "Select images to convert";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileToConvert = openFileDialog.FileName;
                image = new Bitmap(fileToConvert);
                pictureBox.Image = image;
                imageToArray = bitmapManager.ImageToArrayFunc(image);
                labelImageArea.Text = image.Width + "x" + image.Height;
                trackBarSquareArea.Maximum = Math.Min((image.Width-2) / 64, (image.Height-2) / 64);
                labelMax.Text = Math.Min((image.Width - 2 )/ 64 * 64 + 2, (image.Height - 2)/ 64 * 64 + 2).ToString();
            }
        }
        private void RunTimerFirstTime()
        {
            StartTimer();
            StopTimer();
            ExecutionTime.Text = "0";
        }
        public void StartTimer()
        {
            stopwatch = Stopwatch.StartNew();
        }

        public void StopTimer()
        {
            stopwatch.Stop();
            long ticks = stopwatch.ElapsedTicks;
            if (ExecutionTime.Text.Split('\n').Length > 4)
            {
                string[] times = ExecutionTime.Text.Split('\n');
                ExecutionTime.Text = times[0] + "\n" + times[1] + "\n" + times[2] + "\n" + times[3];
            }

            if (ExecutionTime.Text == "0")
            {
                ExecutionTime.Text = ticks.ToString();
            }
            else
            {
                ExecutionTime.Text = ticks.ToString() + "\n" + ExecutionTime.Text;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if  (trackBarSquareArea.Value != 0) { 
                MouseEventArgs me = (MouseEventArgs)e;

                // Get the value from the slider
                int squareSize = trackBarSquareArea.Value * 32;

                // Determine the coordinates of the square area
                int x = Math.Max(0, me.X - squareSize - 1); // X top left corner of the square (with border)
                int y = Math.Max(0, me.Y - squareSize - 1); // Y top left corner of the square (with border)

                if (x + 2 * squareSize + 2 > pictureBox.Width) // Check the right boundary with border
                {
                    x = pictureBox.Width - 2 * squareSize - 2;
                }

                if (y + 2 * squareSize + 2 > pictureBox.Height) // Check the bottom boundary with border
                {
                    y = pictureBox.Height - 2 * squareSize - 2;
                }

                // Determine the width and height of the square (with border)
                int width = squareSize * 2 + 2; // Width of the square (with border)
                int height = squareSize * 2 + 2; // Height of the square (with border)

                Rectangle rect = new Rectangle(x, y, width, height);
                rectToArray = bitmapManager.RectToArrayFunc(image, rect);
                splittedArray = bitmapManager.SplitRectArray(rectToArray, width);
                FunctionSelect(splittedArray, me);
                rectToArray = bitmapManager.CombineArrays(splittedArray);
                imageToArray = bitmapManager.ReplacePixelInImageArray(imageToArray, rectToArray, x, y, width);
                image = bitmapManager.ConvertToImage(imageToArray);
                pictureBox.Image = image;
            }
        }
        public void FunctionSelect(int[][] splittedArray, MouseEventArgs me)
        {
            if (radioButtonASM.Checked && me.Button == MouseButtons.Left)
            {
                dllManager.PrepareLaplacianFilterASM(splittedArray, (int)numberOfThreads.Value);
                StartTimer();
                dllManager.Run(splittedArray, (int)numberOfThreads.Value);
                StopTimer();
            }
            else if (radioButtonC.Checked && me.Button == MouseButtons.Left)
            {
                dllManager.PrepareLaplacianFilterCS(splittedArray, (int)numberOfThreads.Value);
                StartTimer();
                dllManager.Run(splittedArray, (int)numberOfThreads.Value);
                StopTimer();
            }
            else if (radioButtonASM.Checked && me.Button == MouseButtons.Right)
            {
                dllManager.PrepareGaussianBlurASM(splittedArray, (int)numberOfThreads.Value);
                StartTimer();
                dllManager.Run(splittedArray, (int)numberOfThreads.Value);
                StopTimer();
            }
            else if (radioButtonC.Checked && me.Button == MouseButtons.Right)
            {
                dllManager.PrepareGaussianBlurCS(splittedArray, (int)numberOfThreads.Value);
                StartTimer();
                dllManager.Run(splittedArray, (int)numberOfThreads.Value);
                StopTimer();
            }
        }

        private void trackBarSquareArea_Scroll(object sender, EventArgs e)
        {
            if (trackBarSquareArea.Value != 0)
            { 
                labelSquareArea.Text = (trackBarSquareArea.Value * 64 + 2) + "x" + (trackBarSquareArea.Value * 64 + 2);
            }
            else
            { 
                labelSquareArea.Text = "0x0";
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (image == null)
            {
                MessageBox.Show("No image to save.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG Image|*.jpg";
            saveFileDialog.Title = "Save an Image File";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.InitialDirectory = "c:\\";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load path selected by user
                string savePath = saveFileDialog.FileName;

                // Save image from bitmap as JPEG
                image.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                MessageBox.Show("No image to save.");
            }
        }
    }
}
