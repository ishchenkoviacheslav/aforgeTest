using AForge.Imaging.Filters;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace aforgeTest
{
    internal class NextPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ColorOfPixel { get; set; }
    }
    internal class Program
    {
        private static void Main(string[] args)
        {
            Bitmap image = (Bitmap)Bitmap.FromFile(@"C:\Users\Slava\Downloads\garage1.jpg");
            // create grayscale filter (BT709)
            Grayscale grayscale = new Grayscale(0.2125, 0.7154, 0.0721);
            SISThreshold sISThreshold = new SISThreshold();

            Bitmap grayImage = grayscale.Apply(image);
            Bitmap bitmapImage = sISThreshold.Apply(grayImage);

            Bitmap pattern = new Bitmap(@"C:\Users\Slava\Downloads\garagePattern.png");
            //you should use the ToArgb method to compare two Colors
            int currentAssesment = 0;
            int bestAssesment = 0;
            int bestStartX = 0;
            int bestStartY = 0;
            int inc = 10;
            //prepare pattern bitmap
            //get and save different sizes
            int[,] originalPattern = new int[pattern.Width, pattern.Height];
            for (int w = 0; w < pattern.Width; w++)
            {
                for (int h = 0; h < pattern.Height; h++)
                {
                    originalPattern[w,h] = pattern.GetPixel(w, h).ToArgb();
                }
            }
            float[] sizes = new float[] { 0.5f, 0.75f, 1f, 1.25f, 1.5f };
            int[,] size05 = new int[(int)(originalPattern.GetLength(0) * sizes[0]),(int)(originalPattern.GetLength(1) * sizes[0])];
            int[,] size075 = new int[(int)(originalPattern.GetLength(0) * sizes[1]),(int)(originalPattern.GetLength(1) * sizes[1])];
            int[,] size1 = new int[(int)(originalPattern.GetLength(0) * sizes[2]),(int)(originalPattern.GetLength(1) * sizes[2])];
            int[,] size125 = new int[(int)(originalPattern.GetLength(0) * sizes[3]),(int)(originalPattern.GetLength(1) * sizes[3])];
            int[,] size15 = new int[(int)(originalPattern.GetLength(0) * sizes[4]),(int)(originalPattern.GetLength(1) * sizes[4])];
            List<int[,]> allSizes = new List<int[,]>() { size05, size075, size1, size125, size15 };
            List<NextPoint> allPoints = new List<NextPoint>();
            for (int i = 0; i < sizes.Length; i++)
            {
                bool firstIteration = true;
                int firstPixel = 0;
                int firstX = 0;
                int firstY = 0;

                bool fromBegin = true;
                //increase
                for (int w = 0; w < originalPattern.GetLength(0); w++)
                {
                    for (int h = 0; h < originalPattern.GetLength(1); h++)
                    {
                        //считываем одну линию (вертикаль) - высчитываем проценты сколько % черный и белый.
                        //нужно сделать тоже и для горизонтали
                        if (firstIteration == true)
                        {
                            firstIteration = false;
                            firstPixel = originalPattern[w, h];
                            allPoints.Add(new NextPoint() { ColorOfPixel = firstPixel, X = w, Y = h });
                            firstX = w;
                            firstY = h;
                        }
                        else//only from second interation
                        {
                            if(firstPixel != originalPattern[w,h])
                            {
                                allPoints.Add(new NextPoint() { ColorOfPixel = firstPixel, X = w, Y = h });
                                firstPixel = originalPattern[w, h];
                            }
                        }
                    }
                    for (int h = 0; h < originalPattern.GetLength(1); h++)
                    {
                        //заполнить allSizes теми же пикселями что и в allPoints, но только в пропорциях (для allSizes[i])
                        if (fromBegin == true)
                        {
                            allSizes[i][w, h] = allPoints.First().ColorOfPixel;

                        }
                    }
                }
            }
            //move pattern square inside image square
            for (int i = 0; i < 2; i++)
            {
                int borderByWidth = bitmapImage.Width - pattern.Width;
                int borderByHeight = bitmapImage.Height - pattern.Height;

                if (i == 1)
                {
                    inc = 1;
                    borderByWidth = (bestStartX + 10) > borderByWidth ? borderByWidth : (bestStartX + 10);
                    borderByHeight = (bestStartY + 10) > borderByHeight ? borderByHeight : (bestStartY + 10);
                }

                for (int startX = (i == 0) ? 0 : ((bestStartX - 10) < 0 ? 0 : (bestStartX - 10)); startX < borderByWidth; startX+=inc)
                {
                    for (int startY = (i == 0) ? 0 : ((bestStartY - 10) < 0 ? 0 : (bestStartY - 10)); startY < borderByHeight; startY+=inc)
                    {
                        for (int w = 0; w < pattern.Width; w++)
                        {
                            for (int h = 0; h < pattern.Height; h++)
                            {
                                int bitmapPixel = bitmapImage.GetPixel(w + startX, h + startY).ToArgb();
                                int patternPixel = pattern.GetPixel(w, h).ToArgb();

                                if (bitmapPixel == patternPixel)
                                {
                                    currentAssesment++;
                                }
                                //only this values
                                //if (r != -1 && r != -16777216)
                                //{
                                //    System.Console.WriteLine(x);
                                //}
                            }
                        }

                        if (bestAssesment < currentAssesment)
                        {
                            bestAssesment = currentAssesment;
                            bestStartX = startX;
                            bestStartY = startY;
                        }
                        currentAssesment = 0;
                    }
                    System.Console.Write($"{startX}");
                }
                System.Console.WriteLine($"done. best assesmet: {bestAssesment}, X: {bestStartX}, Y: {bestStartY}");
            }
        }
    }  
}