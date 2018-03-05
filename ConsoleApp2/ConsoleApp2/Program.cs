using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string srcFolder = @"C:\Temp\moverTest\target\";
            string targetFolder = @"C:\Temp\moverTest\target2\";
            string gameName = "<Hier steht der Name des Spiels>";
            int counter = 0;

            IEnumerable<string> files = Directory.GetFiles(srcFolder);
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);
                string target = targetFolder + info.Name;

                try
                {
                    counter++;
                    CreateImageWithInfo(file, target, gameName);
                    Console.WriteLine("Processed file " + file + "  (" + counter.ToString() + " von " + files.Count().ToString() + ")");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
        

        static void CreateImageWithInfo(string source, string target, string gameName)
        {
            if (!File.Exists(source))
            {
                return;
            }

            DateTime createdDate = File.GetCreationTime(source);
            string createdDateString = createdDate.ToShortDateString() + " " + createdDate.ToShortTimeString();

            FileStream fileStream = new FileStream(source, FileMode.Open, FileAccess.Read);
            using (Image image = Image.FromStream(fileStream))
            {
                fileStream.Close();
                fileStream.Dispose();

                using (Bitmap bitmap = new Bitmap(image))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font font = new Font("Arial", 10))
                        {
                            int textSize = GetMaxTextWidth(new List<string>() { source, createdDateString, gameName }, graphics, font);

                            Color color = Color.FromArgb(100, Color.LightGray);
                            int rectHeight = font.Height * 3;
                            graphics.DrawRectangle(new Pen(color, rectHeight), new Rectangle(0, image.Size.Height - font.Height * 2 + 5, textSize, 1));

                            Point point = new Point(5, image.Size.Height - font.Height * 3 - 5);
                            graphics.DrawString(gameName, font, Brushes.Red, point);

                            point = new Point(5, image.Size.Height - font.Height * 2 - 5);
                            graphics.DrawString(createdDateString, font, Brushes.Red, point);

                            point = new Point(5, image.Size.Height - font.Height - 5);
                            graphics.DrawString(source, font, Brushes.Red, point);

                            bitmap.Save(target, System.Drawing.Imaging.ImageFormat.Jpeg);                            
                        }
                    }
                }
            }
        }

        static int GetMaxTextWidth(List<string> texts, Graphics graphics, Font font)
        {
            int result = 0;

            if (graphics != null && texts != null && font != null)
            {
                SizeF sizeText;
                int textSize = -1;

                foreach (string text in texts)
                {
                    sizeText = graphics.MeasureString(text, font);
                    textSize = (int)sizeText.Width + 1;

                    if (textSize > result)
                    {
                        result = textSize;
                    }
                }
            }

            return result;
        }
    }
}
