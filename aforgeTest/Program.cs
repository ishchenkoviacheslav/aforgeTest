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

            //no restriction if pattern largest than source bitmap
            for (int posX = 0; posX < borderByWidth; posX++)
            {
                for (int posY = 0; posY < borderByHeight; posY++)
                {
                    for (int start = 0; start < coordinates.Count; start++)
                    {
                        int currentPixel = bitmapImage.GetPixel(posX + coordinates[start].X, posY + coordinates[0].Y).ToArgb();

                        if (currentPixel == -16777216)
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