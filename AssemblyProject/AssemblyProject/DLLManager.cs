using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssemblyProject
{
    internal class DLLManager
    {

        [DllImport(@"..\\..\\..\\..\\x64\\Debug\\JAAsm.dll")]
        public static extern unsafe void GaussianBlur(int[] rgbArray, int startPosition, int endPosition, int[] newArray);

        [DllImport(@"..\\..\\..\\..\\x64\\Debug\\JAAsm.dll")]
        public static extern unsafe void LaplacianFilter(int[] rgbArray, int startPosition, int endPosition, int[] newArray);
        Thread[] threads;
        object[] dllResult;
        public DLLManager()
        {
            threads = new Thread[64];
        }
        private int height;
        private int width;
        public void PrepareCsharpGaussianBlur(int[][] rgbArrays, int threadsNumber)
        {
            string path = @"..\..\..\..\JACs\bin\Debug\net6.0\JACs.dll";
            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("JACs.JACs");
            var activator = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("ConvertToNegative");
            dllResult = new object[threadsNumber];
            int computedSize = rgbArrays.Length / threadsNumber;
            try
            {
                for (int i = 0; i < threadsNumber; i++)
                {
                    int threadIndex = i; // Aby uniknąć problemu zamknięcia pętli
                    threads[i] = new Thread(
                        () =>
                        {
                            int start = threadIndex * computedSize;
                            int end = (threadIndex + 1) * computedSize;

                            if (threadIndex == threadsNumber - 1)
                            {
                                // Ostatni wątek obsługuje resztę
                                end = rgbArrays.Length;
                            }

                            // Przetwarzanie każdej funkcji w odpowiednim zakresie tablicy
                            for (int j = start; j < end; j++)
                            {
                                method.Invoke(activator, new object[] { rgbArrays[j], 9, 55 });
                            }
                        }
                    );
                }
            }
            catch (Exception)
            {
                // Obsługa wyjątków
            }
        }
        public void PrepareAssemblerGaussianBlur(int[][] rgbArrays, int threadsNumber)
{
    int computedSize = rgbArrays.Length / threadsNumber;

    try
    {
        for (int i = 0; i < threadsNumber; i++)
        {
            int threadIndex = i; // Aby uniknąć problemu zamknięcia pętli
            threads[i] = new Thread(
                () =>
                {
                    int start = threadIndex * computedSize;
                    int end = (threadIndex + 1) * computedSize;

                    if (threadIndex == threadsNumber - 1)
                    {
                        // Ostatni wątek obsługuje resztę
                        end = rgbArrays.Length;
                    }

                    // Przetwarzanie każdej funkcji w odpowiednim zakresie tablicy
                    for (int j = start; j < end; j++)
                    {
                        int[] newArray = new int[100];
                        /*for (int k = 0; k < newArray.Length; k++)
                        {
                         //   newArray[k] = 333;
                        }*/
                        newArray = rgbArrays[j];
                        GaussianBlur(rgbArrays[j], 10, 89, newArray);
                        rgbArrays[j] = newArray;
                    }
                }
            );
        }
    }
    catch (Exception)
    {
        // Obsługa wyjątków
    }
}

        public void PrepareAssemblerLaplacianFilter(int[][] rgbArrays, int threadsNumber)
        {
            int computedSize = rgbArrays.Length / threadsNumber;

            try
            {
                for (int i = 0; i < threadsNumber; i++)
                {
                    int threadIndex = i; // Aby uniknąć problemu zamknięcia pętli
                    threads[i] = new Thread(
                        () =>
                        {
                            int start = threadIndex * computedSize;
                            int end = (threadIndex + 1) * computedSize;

                            if (threadIndex == threadsNumber - 1)
                            {
                                // Ostatni wątek obsługuje resztę
                                end = rgbArrays.Length;
                            }

                            // Przetwarzanie każdej funkcji w odpowiednim zakresie tablicy
                            for (int j = start; j < end; j++)
                            {
                                int[] newArray = new int[100];
                                newArray = rgbArrays[j];
                                LaplacianFilter(rgbArrays[j], 10, 89, newArray);
                                rgbArrays[j] = newArray;
                            }
                        }
                    );
                }
            }
            catch (Exception)
            {
                // Obsługa wyjątków
            }
        }





        public int[] ImageToArrayFunc(Bitmap bitmap)
        {
            width = bitmap.Width;
            height = bitmap.Height;
            //r << 16, g << 8, b
            uint size = (uint)(height * width);
            int[] imageToArray = new int[size];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    imageToArray[y * width + x] = pixel.R << 16 | pixel.G << 8 | pixel.B;
                }
            }
            return imageToArray;
        }

        public int[] RectToArrayFunc(Bitmap bitmap, Rectangle rect)
        {
            //r << 16, g << 8, b
            uint size = (uint)(rect.Height * rect.Width);
            int[] rectToArray = new int[size];
            for (int y = 0; y < rect.Height; y++)
            {
                for (int x = 0; x < rect.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(rect.Left + x, rect.Top + y);
                    rectToArray[y * rect.Width + x] = pixel.R << 16 | pixel.G << 8 | pixel.B;
                }
            }
            return rectToArray;
        }
        public int[][] SplitRectArray(int[] rectToArray, int rectSize)
        {
            int numOfSmallArrays = (rectSize - 2) / 8;
            int[][] splittedArray = new int[numOfSmallArrays * numOfSmallArrays][];

            for (int i = 0; i < numOfSmallArrays; i++)
            {
                for (int j = 0; j < numOfSmallArrays; j++)
                {
                    int smallArrayIndex = i * numOfSmallArrays + j;
                    splittedArray[smallArrayIndex] = new int[100]; // 10x10 array

                    for (int y = 0; y < 10; y++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            int indexInRectArray = (i * 8 + y) * rectSize + j * 8 + x;
                            splittedArray[smallArrayIndex][y * 10 + x] = rectToArray[indexInRectArray];
                        }
                    }
                }
            }

            return splittedArray;
        }

        public int[] CombineArrays(int[][] splittedArray)
        {
            int numOfSmallArrays = (int)Math.Sqrt(splittedArray.Length);
            int tableSize = 8 * numOfSmallArrays + 2; // Adjusted table size
            int[] rectToArray = new int[tableSize * tableSize];

            for (int i = 0; i < numOfSmallArrays; i++)
            {
                for (int j = 0; j < numOfSmallArrays; j++)
                {
                    int smallArrayIndex = i * numOfSmallArrays + j;

                    for (int y = 0; y < 10; y++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            int indexInRectArray = (i * 8 + y) * tableSize + j * 8 + x;
                            // Avoid overwriting shared edges
                            if (!(x == 9 && j < numOfSmallArrays - 1) && !(y == 9 && i < numOfSmallArrays - 1))
                            {
                                rectToArray[indexInRectArray] = splittedArray[smallArrayIndex][y * 10 + x];
                            }
                        }
                    }
                }
            }

            return rectToArray;
        }

        public int[] ReplacePixelInImageArray(int[] imageToArray, int[] rectToArray, int rectX, int rectY, int rectSize)
        {
            for (int y = 0; y < rectSize; y++)
            {
                for (int x = 0; x < rectSize; x++)
                {
                    int targetY = rectY + y;
                    int targetX = rectX + x;

                    // Dodajemy dodatkowe sprawdzenie, aby uniknąć przekroczenia zakresu tablicy
                    if (targetY < height && targetX < width)
                    {
                        imageToArray[targetY * width + targetX] = rectToArray[y * rectSize + x];
                    }
                }
            }

        return imageToArray;
        }

        public int[][] Run(int[][] splittedArray, int threadsNumber)
        {
            for (int i = 0; i < threadsNumber; i++)
            {
                int index = i;
                threads[index].Start();
            }
            for (int i = 0; i < threadsNumber; i++)
            {
                int index = i;
                threads[index].Join();
            }
            return splittedArray;
        }
        public Bitmap ConvertToImage(int[] imageToArray)
        {
            Bitmap result = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (uint i = 0; i < height * width; i++)
            {
                result.SetPixel((int)i % width, (int)i / width, Color.FromArgb((byte)255, (byte)(imageToArray[i] >> 16), (byte)(imageToArray[i] >> 8), (byte)(imageToArray[i])));
            }
            return result;
        }

    }
}
