using System;
using System.Drawing;

// Author: Mateusz Malek
// Silesian University of Technology 2023/24
// Assembly Project v1.0

namespace ImageEnhancerCS
{
    public class ImageEnhancerCS
    {
        public ImageEnhancerCS() { }

        private const int ImageWidth = 10;
        private const int ImageHeight = 10;
        private static readonly int[] RedMask = { 0xFF0000 };
        private static readonly int[] GreenMask = { 0x00FF00 };
        private static readonly int[] BlueMask = { 0x0000FF };

        //Pixel format is 0xAARRGGBB
        public static void LaplacianFilterCS(int[] oldArray, int[] newArray)
        {
            float minus = -0.5f;
            float laplace = 5.0f;

            for (int y = 1; y < ImageHeight - 1; y++)
            {
                for (int x = 1; x < ImageWidth - 1; x++)
                {
                    float[] newPixel = new float[3]; // RGB

                    // Process each neighboring pixel
                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            int pixel = oldArray[(y + ky) * ImageWidth + (x + kx)];
                            float weight = (ky == 0 && kx == 0) ? laplace : minus;

                            newPixel[0] += ((pixel & RedMask[0]) >> 16) * weight;
                            newPixel[1] += ((pixel & GreenMask[0]) >> 8) * weight;
                            newPixel[2] += (pixel & BlueMask[0]) * weight;
                        }
                    }

                    //Set new pixel value
                    byte newR = (byte)ClampColorValue(newPixel[0]);
                    byte newG = (byte)ClampColorValue(newPixel[1]);
                    byte newB = (byte)ClampColorValue(newPixel[2]);

                    newArray[y * ImageWidth + x] = (newR << 16) | (newG << 8) | newB;
                }
            }
        }
        public static void GaussianBlurCS(int[] oldArray, int[] newArray)
        {
            float corner = 1.0f;
            float diag = 2.0f;
            float center = 4.0f;
            float divide = 16.0f;

            for (int y = 1; y < ImageHeight - 1; y++)
            {
                for (int x = 1; x < ImageWidth - 1; x++)
                {
                    float[] newPixel = new float[3]; // RGB

                    // Process each neighboring pixel
                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            int pixel = oldArray[(y + ky) * ImageWidth + (x + kx)];
                            float weight = (ky == 0 && kx == 0) ? center : (Math.Abs(ky) == Math.Abs(kx) ? diag : corner);

                            newPixel[0] += ((pixel & RedMask[0]) >> 16) * weight;
                            newPixel[1] += ((pixel & GreenMask[0]) >> 8) * weight;
                            newPixel[2] += (pixel & BlueMask[0]) * weight;
                        }
                    }

                    // Divide by total weight and set new pixel value
                    byte newR = (byte)ClampColorValue(newPixel[0] / divide);
                    byte newG = (byte)ClampColorValue(newPixel[1] / divide);
                    byte newB = (byte)ClampColorValue(newPixel[2] / divide);

                    newArray[y * ImageWidth + x] = (newR << 16) | (newG << 8) | newB;
                }
            }
        }
        private static int ClampColorValue(float value)
        {
            return (int)Math.Max(0, Math.Min(255, value));
        }

    }
}