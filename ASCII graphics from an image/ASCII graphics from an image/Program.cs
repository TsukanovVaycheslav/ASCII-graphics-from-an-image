using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace ASCII_graphics_from_an_image
{
    class Program
    {
        private const double WIDTH_OFFSET = 2.0;
        private const int maxWidth = 400;

        [STAThread]
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Cyan;

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images | *.png; *.bmp; *.jpg; *.JPEG"
            };

            openFileDialog.ShowDialog();

            while (true)
            {
                Console.ReadLine();
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    continue;
                Console.Clear();

                var bitmap = new Bitmap(openFileDialog.FileName);
                bitmap = ResizeBitmap(bitmap);
                bitmap.ToGrayScale();

                var converter = new BitmapToASCIIConverter(bitmap);
                var rows = converter.Convert();

                foreach (var row in rows)
                    Console.WriteLine(row);

                var rowNegative = converter.ConvertNegative();
                File.WriteAllLines("image.txt", rowNegative.Select(r=>new string(r)));

                Console.SetCursorPosition(0, 0);
            }
        }

        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            var newHeight = bitmap.Height / WIDTH_OFFSET * maxWidth / bitmap.Width;
            if (bitmap.Width > maxWidth || bitmap.Height > newHeight)
                bitmap = new Bitmap(bitmap, new Size(maxWidth, (int)newHeight));
            return bitmap;
        }
    }
}
