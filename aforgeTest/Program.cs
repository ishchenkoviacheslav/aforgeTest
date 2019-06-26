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
            
            for (int x = 0; x < bitmapImage.Width; x++)
            {
                for (int y = 0; y < bitmapImage.Height; y++)
                {
                    Color c = bitmapImage.GetPixel(x, y);
                    int r = c.ToArgb();
                    //int x = 30;
                    //if (Enumerable.Range(1, 100).Contains(x))
                    //    //true

                    //if (x >= 1 && x <= 100)
                    //        //true
                    if (r != -1)
                    {
                        System.Console.WriteLine(x);
                    }
                }
            }

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