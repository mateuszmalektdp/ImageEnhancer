using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEnhancerMain
{
    public partial class ImageChart : Form
    {
        private HistogramBox histogramBoxR = new HistogramBox();
        private HistogramBox histogramBoxG = new HistogramBox();
        private HistogramBox histogramBoxB = new HistogramBox();
        Bitmap inputImage;
        public ImageChart(Bitmap inputImage)
        {
            InitializeComponent();
            this.inputImage = inputImage;
            histogramBoxR.Size = new Size(360, 360);
            histogramBoxR.Location = new Point(50, 100); // Lewy górny róg

            // Ustawienie rozmiaru i pozycji histogramBoxG
            histogramBoxG.Size = new Size(360, 360);
            histogramBoxG.Location = new Point(460, 100); // Środek

            // Ustawienie rozmiaru i pozycji histogramBoxB
            histogramBoxB.Size = new Size(360, 360);
            histogramBoxB.Location = new Point(870, 100); // Prawy górny róg

            // Dodanie histogramów do formularza
            this.Controls.Add(histogramBoxR);
            this.Controls.Add(histogramBoxG);
            this.Controls.Add(histogramBoxB);

        }
        public void GenerateHistograms(Bitmap inputImage)
        {

            Image<Bgr, byte> image = inputImage.ToImage<Bgr, byte>();
            Image<Gray, byte> Red = image[0];
            Image<Gray, byte> Green = image[1];
            Image<Gray, byte> Blue = image[2];

            histogramBoxR.ClearHistogram();
            histogramBoxR.GenerateHistograms(Red, 256);
            histogramBoxR.Refresh();

            histogramBoxG.ClearHistogram();
            histogramBoxG.GenerateHistograms(Blue, 256);
            histogramBoxG.Refresh();

            histogramBoxB.ClearHistogram();
            histogramBoxB.GenerateHistograms(Green, 256);
            histogramBoxB.Refresh();

        }

        private void ImageChart_Load(object sender, EventArgs e)
        {

        }

        private void ImageChart_Load_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
