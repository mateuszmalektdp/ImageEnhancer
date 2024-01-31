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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace AssemblyProject
{
    public partial class Form1 : Form
    {
        Stopwatch stopwatch;
        DLLManager dllManager;
        string fileToConvert;
        int[] imageToArray;
        int[] rectToArray;
        int[][] splittedArray;
        Bitmap image;

        public Form1()
        {
            InitializeComponent();
            dllManager = new DLLManager();
            //RunTimerFirstTime();
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
                imageToArray = dllManager.ImageToArrayFunc(image);
            }
        }
        //private void RunTimerFirstTime()
        //{
        //    StartTimer();
        //    StopTimer();
        //    ExecutionTime.Text = "0";
        //}
        //public void StartTimer()
        //{
        //    stopwatch = Stopwatch.StartNew();
        //}

        //public void StopTimer()
        //{
        //    stopwatch.Stop();
        //    long ticks = stopwatch.ElapsedTicks;
        //    if (ExecutionTime.Text.Split("\n").Length > 4)
        //    {
        //        string[] times = ExecutionTime.Text.Split("\n");
        //        ExecutionTime.Text = times[0] + "\n" + times[1] + "\n" + times[2] + "\n" + times[3];
        //    }

        //    if (ExecutionTime.Text == "0")
        //    {
        //        ExecutionTime.Text = ticks.ToString();
        //    }
        //    else
        //    {
        //        ExecutionTime.Text = ticks.ToString() + "\n" + ExecutionTime.Text;
        //    }
        //}
           
        private void pictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            // Pobranie wartości z suwaka
            int squareSize = trackBarSquareArea.Value * 32;

            // Ustalenie współrzędnych obszaru kwadratowego
            int x = Math.Max(0, me.X - squareSize - 1); // X górny lewy róg kwadratu (z ramką)
            int y = Math.Max(0, me.Y - squareSize - 1); // Y górny lewy róg kwadratu (z ramką)

            if (x + 2 * squareSize + 2 > pictureBox.Width) // Sprawdzenie prawej granicy z ramką
            {
                x = pictureBox.Width - 2 * squareSize - 2;
            }

            if (y + 2 * squareSize + 2 > pictureBox.Height) // Sprawdzenie dolnej granicy z ramką
            {
                y = pictureBox.Height - 2 * squareSize - 2;
            }

            // Ustalenie szerokości i wysokości kwadratu (z ramką) w zakresie od 66x66 do 642x642
            int width = squareSize * 2 + 2; // Szerokość kwadratu (z ramką)
            int height = squareSize * 2 + 2; // Wysokość kwadratu (z ramką)

            // Opcjonalnie: Ustalenie obszaru dla wyostrzenia obrazu (możesz użyć innego obrazu zamiast pictureBox.Image)
            Rectangle rect = new Rectangle(x, y, width, height);
            rectToArray = dllManager.RectToArrayFunc(image, rect);
            splittedArray = dllManager.SplitRectArray(rectToArray, width);
            FunctionSelect(splittedArray, me);
            rectToArray = dllManager.CombineArrays(splittedArray);
            imageToArray = dllManager.ReplacePixelInImageArray(imageToArray,rectToArray, x, y, width);
            image = dllManager.ConvertToImage(imageToArray);
            pictureBox.Image = image;
        }

        public void FunctionSelect(int[][] splittedArray, MouseEventArgs me)
        {
            if (radioButtonASM.Checked && me.Button == MouseButtons.Left)
            {
                dllManager.PrepareAssemblerLaplacianFilter(splittedArray, (int)numberOfThreads.Value);
                //StartTimer();
                dllManager.Run(splittedArray, (int)numberOfThreads.Value);
                //StopTimer();
            }
            else if (radioButtonC.Checked && me.Button == MouseButtons.Left)
            {
                //dllManager.PrepareCsharpGaussianBlur(imageToArray, (int)numberOfThreads.Value);
                //StartTimer();
                //dllManager.Run(imageToArray, (int)numberOfThreads.Value);
                //StopTimer();
            }
            else if (radioButtonASM.Checked && me.Button == MouseButtons.Right)
            {
                dllManager.PrepareAssemblerGaussianBlur(splittedArray, (int)numberOfThreads.Value);
                //StartTimer();
                dllManager.Run(splittedArray, (int)numberOfThreads.Value);
                //StopTimer();
            }
            else if (radioButtonC.Checked && me.Button == MouseButtons.Right)
            {
                //dllManager.PrepareCsharpGaussianBlur(splittedArray, (int)numberOfThreads.Value);
                //StartTimer();
                dllManager.Run(splittedArray, (int)numberOfThreads.Value);
                //StopTimer();
            }
        }
      
        private void trackBarSquareArea_Scroll(object sender, EventArgs e)
        {
            int value = trackBarSquareArea.Value * 60;
            Console.WriteLine(value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ExecutionTime_Click(object sender, EventArgs e)
        {

        }
        private void radioButtonC_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonASM_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
