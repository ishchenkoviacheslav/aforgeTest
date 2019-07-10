using AForge.Imaging.Filters;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace aforgeTest
{
    class XY
    {
        public XY(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
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

            List<XY> coordinates = new List<XY>();

            for (int pX = 0; pX < pattern.Width; pX++)
            {
                for (int pY = 0; pY < pattern.Height; pY++)
                {
                    if (pattern.GetPixel(pX, pY).ToArgb() == -1)
                    {
                        coordinates.Add(new XY(pX, pY));
                    }
                }
            }

            int maxX = coordinates.Max(s => s.X);
            int maxY = coordinates.Max(s => s.Y);

            //you should use the ToArgb method to compare two Colors
            int currentAssesment = 0;
            int bestAssesment = 0;
            int bestStartX = 0;
            int bestStartY = 0;
            int borderByWidth = bitmapImage.Width - maxX;
            int borderByHeight = bitmapImage.Height - maxY;
            float[] multiple = new float[] { 0.8f, 0.9f, 1f, 1.1f, 1.2f, 1.3f };
            for (int repeat = 0; repeat < 7; repeat++)
            {
                //no check if pattern largest than source bitmap
                for (int posX = repeat != 6 ? 0 : bestStartX; posX < borderByWidth; posX += repeat != 6 ? 10 : 1)
                {
                    for (int posY = repeat != 6 ? 0 : bestStartY; posY < borderByHeight; posY += repeat != 6 ? 10 : 1)
                    {
                        for (int start = 0; start < coordinates.Count; start++)
                        {
                            int currX = posX + (int)(coordinates[start].X * multiple[repeat]);
                            int currY = posY + (int)(coordinates[0].Y * multiple[repeat]);
                            int currentPixel = bitmapImage.GetPixel(currX > borderByWidth ? borderByWidth : currX, currY > borderByHeight ? borderByHeight : currY).ToArgb();

                            if (currentPixel == -1)
                            {
                                currentAssesment++;
                            }
                        }

                        //-1 - white; -16777216 - black
                        if (bestAssesment < currentAssesment)
                        {
                            bestAssesment = currentAssesment;
                            bestStartX = posX;
                            bestStartY = posY;
                        }
                        currentAssesment = 0;
                    }
                    System.Console.Write($"{posX}");
                }
                System.Console.WriteLine($"done. best assesmet: {bestAssesment}, X: {bestStartX}, Y: {bestStartY}");
            }
        }
    }
}