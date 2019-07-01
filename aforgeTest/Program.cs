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
            Bitmap image = (Bitmap)Bitmap.FromFile(@"C:\Users\Slava\Downloads\Arm.jpg");
            // create grayscale filter (BT709)
            Grayscale grayscale = new Grayscale(0.2125, 0.7154, 0.0721);
            SISThreshold sISThreshold = new SISThreshold();

            Bitmap grayImage = grayscale.Apply(image);
            Bitmap bitmapImage = sISThreshold.Apply(grayImage);

            Bitmap pattern = new Bitmap(@"C:\Users\Slava\Downloads\Pattern.png");
            //you should use the ToArgb method to compare two Colors
            int totalAssesment = pattern.Width * pattern.Height;
            int currentAssesment = 0;
            int bestAssesment = 0;

            int increaseByWidth = bitmapImage.Width - pattern.Width;
            int increaseByHeight = bitmapImage.Height - pattern.Height;

            int currentX = 0;
            int currentY = 0;

            //for
            
            if(currentX >= increaseByWidth)
            {
                currentX = increaseByWidth;
            }

            if (currentY >= increaseByHeight)
            {
                currentY = increaseByHeight;
            }

            for (int x = 0; x < pattern.Width; x++)
            {
                for (int y = 0; y < pattern.Height; y++)
                {
                    int bitmapPixel = bitmapImage.GetPixel(x + currentX, y + currentY).ToArgb();
                    int patternPixel = pattern.GetPixel(x, y).ToArgb();
                    
                    if(bitmapPixel == patternPixel)
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

            if(bestAssesment < currentAssesment)
            {
                bestAssesment = currentAssesment;
            }
            currentX++;
            currentY++;
            //Bitmap result;
            //using (var ms = new MemoryStream(patternArray))
            //{
            //    result = new Bitmap(ms);
            //}
            //result.Save(@"C:\Users\Slava\Downloads\PatternFromArray.jpg");

            //HitAndMiss hFilter = new HitAndMiss(patternArray, HitAndMiss.Modes.HitAndMiss);
            //image = hFilter.Apply(image);

            //image.Save(@"C:\Users\Slava\Downloads\Arm2.jpg");
        }

        //public static byte[] ImageToByte2(Image img)
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        return stream.ToArray();
        //    }
        //}
    }
}


//// First we declare the variable that will make the strig that will be printed in the textbox    
//string texto = "";  
////The we make the cicles to read pixel by pixel and we make the comparation with the withe color.    
//for (int i = 0; i<img.Height; i++) {  
//    for (int j = 0; j<img.Width; j++) {  
//        //When we add the value to the string we should invert the  
//        order because the images are reading from top to bottom
//        //and the textBox is write from left to right.    
//        if (img.GetPixel(j, i).A.ToString() == "255" && img.GetPixel(j, i).B.ToString() == "255" && img.GetPixel(j, i).G.ToString() == "255" && img.GetPixel(j, i).R.ToString() == "255") {  
//            texto = texto + "0";  
//        } else {  
//            texto = texto + "1";  
//        }  
//    }  
//    texto = texto + "\r\n"; // this is to make the enter between lines    
//}  
////And finally we put the string to the textbox    
//txtArreglo.Text = texto;  