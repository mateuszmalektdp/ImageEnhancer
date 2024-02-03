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
        public static extern unsafe void GaussianBlurASM(int[] rgbArray, int startPosition, int endPosition, int[] newArray);

        [DllImport(@"..\\..\\..\\..\\x64\\Debug\\JAAsm.dll")]
        public static extern unsafe void LaplacianFilterASM(int[] rgbArray, int startPosition, int endPosition, int[] newArray);
        Thread[] threads;
        object[] dllResult;
        public int max = 0;
        public DLLManager()
        {
            threads = new Thread[64];
        }
        
        public void PrepareLaplacianFilterCS(int[][] rgbArrays, int threadsNumber)
        {
            string path = @"..\..\..\..\JACs\bin\Debug\JACs.dll";
            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("JACs.JACs");
            var activator = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("LaplacianFilterCS");
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

                        max = end - start;
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
                            method.Invoke(activator, new object[] { rgbArrays[j], newArray });
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

        public void PrepareGaussianBlurCS(int[][] rgbArrays, int threadsNumber)
        {
            string path = @"..\..\..\..\JACs\bin\Debug\JACs.dll";
            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("JACs.JACs");
            var activator = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("GaussianBlurCS");
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

                        max = end - start;
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
                            method.Invoke(activator, new object[] { rgbArrays[j], newArray });
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

        public void PrepareLaplacianFilterASM(int[][] rgbArrays, int threadsNumber)
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
                                LaplacianFilterASM(rgbArrays[j], 10, 89, newArray);
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
        public void PrepareGaussianBlurASM(int[][] rgbArrays, int threadsNumber)
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

                        max = end - start;
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
                            GaussianBlurASM(rgbArrays[j], 10, 89, newArray);
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
    }
}
