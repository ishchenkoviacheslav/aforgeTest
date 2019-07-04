using AForge.Imaging.Filters;
using System.Drawing;
using System.IO;
using System.Linq;

namespace aforgeTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Bitmap image = (Bitmap)Bitmap.FromFile(@"C:\Users\Slava\Downloads\garage2.jpg");
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
            //move pattern square inside image squate
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