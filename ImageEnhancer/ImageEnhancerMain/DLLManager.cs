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

namespace ImageEnhancerMain
{
    internal class DLLManager
    {
        // Assembly DLL load for GaussianBlur and LaplacianFilter operations
        [DllImport(@"..\\..\\..\\..\\x64\\Debug\\ImageEnhancerASM.dll")]
        public static extern unsafe void GaussianBlurASM(int[] rgbArray, int[] newArray, int startPosition, int endPosition);

        [DllImport(@"..\\..\\..\\..\\x64\\Debug\\ImageEnhancerASM.dll")]
        public static extern unsafe void LaplacianFilterASM(int[] rgbArray, int[] newArray, int startPosition, int endPosition);
        Thread[] threads;
        object[] dllResult;
        public int max = 0;
        public DLLManager()
        {
            threads = new Thread[64];
        }
        
        public void PrepareLaplacianFilterCS(int[][] rgbArrays, int threadsNumber)
        {
            string path = @"..\..\..\..\ImageEnhancerCS\bin\Debug\ImageEnhancerCS.dll";
            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("ImageEnhancerCS.ImageEnhancerCS");
            var activator = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("LaplacianFilterCS");
            dllResult = new object[threadsNumber];
            int computedSize = rgbArrays.Length / threadsNumber;

            try
            {
                for (int i = 0; i < threadsNumber; i++)
                {
                    int threadIndex = i; // To avoid the closure loop issue
                    threads[i] = new Thread(
                    () =>
                    {
                        int start = threadIndex * computedSize;
                        int end = (threadIndex + 1) * computedSize;

                        max = end - start;
                        if (threadIndex == threadsNumber - 1)
                        {
                            // The last thread handles the remainder
                            end = rgbArrays.Length;
                        }

                        // Processing each function within the specific array range
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
                // Exception handling
            }
        }

        public void PrepareGaussianBlurCS(int[][] rgbArrays, int threadsNumber)
        {
            string path = @"..\..\..\..\ImageEnhancerCS\bin\Debug\ImageEnhancerCS.dll";
            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("ImageEnhancerCS.ImageEnhancerCS");
            var activator = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("GaussianBlurCS");
            dllResult = new object[threadsNumber];
            int computedSize = rgbArrays.Length / threadsNumber;
            try
            {
                for (int i = 0; i < threadsNumber; i++)
                {
                    int threadIndex = i; // To avoid the closure loop issue
                    threads[i] = new Thread(
                    () =>
                    {
                        int start = threadIndex * computedSize;
                        int end = (threadIndex + 1) * computedSize;

                        max = end - start;
                        if (threadIndex == threadsNumber - 1)
                        {
                            // The last thread handles the remainder
                            end = rgbArrays.Length;
                        }

                        // Processing each function within the specific array range
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
                // Exception handling
            }
        }   

        public void PrepareLaplacianFilterASM(int[][] rgbArrays, int threadsNumber)
        {
            int computedSize = rgbArrays.Length / threadsNumber;

            try
            {
                for (int i = 0; i < threadsNumber; i++)
                {
                    int threadIndex = i; // To avoid the closure loop issue
                    threads[i] = new Thread(
                        () =>
                        {
                            int start = threadIndex * computedSize;
                            int end = (threadIndex + 1) * computedSize;

                            if (threadIndex == threadsNumber - 1)
                            {
                                // The last thread handles the remainder
                                end = rgbArrays.Length;
                            }

                            // Processing each function within the specific array range
                            for (int j = start; j < end; j++)
                            {
                                int[] newArray = new int[100];
                                newArray = rgbArrays[j];
                                LaplacianFilterASM(rgbArrays[j], newArray, 10, 89);
                                rgbArrays[j] = newArray;
                            }
                        }

                    );
                }
            }
            catch (Exception)
            {
                // Exception handling
            }
        }
        public void PrepareGaussianBlurASM(int[][] rgbArrays, int threadsNumber)
        {
            int computedSize = rgbArrays.Length / threadsNumber;

            try
            {
                for (int i = 0; i < threadsNumber; i++)
                {
                    int threadIndex = i; // To avoid the closure loop issue
                    threads[i] = new Thread(
                    () =>
                    {
                        int start = threadIndex * computedSize;
                        int end = (threadIndex + 1) * computedSize;

                        max = end - start;
                        if (threadIndex == threadsNumber - 1)
                        {
                            // The last thread handles the remainder
                            end = rgbArrays.Length;
                        }

                        // Processing each function within the specific array range
                        for (int j = start; j < end; j++)
                        {
                            int[] newArray = new int[100];
                            newArray = rgbArrays[j];
                            GaussianBlurASM(rgbArrays[j], newArray, 10, 89);
                            rgbArrays[j] = newArray;

                        }
                    }
                );
                }
            }
            catch (Exception)
            {
                // Exception handling
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
