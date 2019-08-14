using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace aforgeTest
{
    internal class NextPoint
    {
        public int Counter { get; set; }
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

            float[] sizes = new float[] { 0.5f, 0.75f, 1f, 1.25f, 1.5f };
            List<Bitmap> patternSizes = new List<Bitmap>();
            for (int i = 0; i < sizes.Length; i++)
            {
                Bitmap btm = new Bitmap((int)(pattern.Width * sizes[i]), (int)(pattern.Height * sizes[i]));
                patternSizes.Add(btm);
                Graphics graphics = Graphics.FromImage(btm);
                graphics.DrawImage(pattern, 0, 0, btm.Width, btm.Height);
                graphics.Dispose();
                //btm.Save($"{sizes[i].ToString()}.png", ImageFormat.Png);
            }

            for (int currsize = 0; currsize < sizes.Length; currsize++)
            {
                Console.WriteLine($"{sizes[currsize]}");
                int currentAssesment = 0;
                int bestAssesment = 0;
                int bestStartX = 0;
                int bestStartY = 0;
                int inc = 10;

                int onePercent = (patternSizes[currsize].Width * patternSizes[currsize].Height) / 100;
                //move pattern square inside image square
                for (int i = 0; i < 2; i++)
                {
                    int borderByWidth = bitmapImage.Width - patternSizes[currsize].Width;
                    int borderByHeight = bitmapImage.Height - patternSizes[currsize].Height;

                    if (i == 1)
                    {
                        inc = 1;
                        borderByWidth = (bestStartX + 10) > borderByWidth ? borderByWidth : (bestStartX + 10);
                        borderByHeight = (bestStartY + 10) > borderByHeight ? borderByHeight : (bestStartY + 10);
                    }

                    for (int startX = (i == 0) ? 0 : ((bestStartX - 10) < 0 ? 0 : (bestStartX - 10)); startX < borderByWidth; startX += inc)
                    {
                        for (int startY = (i == 0) ? 0 : ((bestStartY - 10) < 0 ? 0 : (bestStartY - 10)); startY < borderByHeight; startY += inc)
                        {
                            for (int w = 0; w < patternSizes[currsize].Width; w++)
                            {
                                for (int h = 0; h < patternSizes[currsize].Height; h++)
                                {
                                    int bitmapPixel = bitmapImage.GetPixel(w + startX, h + startY).ToArgb();
                                    int patternPixel = patternSizes[currsize].GetPixel(w, h).ToArgb();

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
                    System.Console.WriteLine($"done. best assesmet: {(bestAssesment / onePercent)}, X: {bestStartX}, Y: {bestStartY}");
                }
            }
        }
    }
}