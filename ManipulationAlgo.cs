using System;
using System.Drawing;

namespace ImageAlgorithm
{
    public partial class ImageAlgorithm
    {
        public delegate void AlgorithmAction(Bitmap bitmapInput, ref Bitmap bitmapOutput);

        public static void copy(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            for (int x = 0; x < bitmapInput.Width; x++)
            {
                for (int y = 0; y < bitmapInput.Height; y++)
                {
                    bitmapOutput.SetPixel(x, y, bitmapInput.GetPixel(x, y));
                }
            }
        }

        public static void greyscale(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            for (int x = 0; x < bitmapInput.Width; x++)
            {
                for (int y = 0; y < bitmapInput.Height; y++)
                {
                    Color pixel = bitmapInput.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    bitmapOutput.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }
            }
        }

        public static void inversion(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            for (int x = 0; x < bitmapInput.Width; x++)
            {
                for (int y = 0; y < bitmapInput.Height; y++)
                {
                    Color pixel = bitmapInput.GetPixel(x, y);
                    bitmapOutput.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
            }
        }

        public static void histogram(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            // ignore last
            bitmapOutput.Dispose();

            int max = 0;
            int[] ctr = new int[256];

            for (int x = 0; x < bitmapInput.Width; x++)
            {
                for (int y = 0; y < bitmapInput.Height; y++)
                {
                    Color pixel = bitmapInput.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    if (max < ++ctr[grey])
                    {
                        max = ctr[grey];
                    }
                }
            }

            bitmapOutput = new Bitmap(256, max);
            Color gray = Color.FromArgb(100, 100, 100);

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < ctr[x]; y++)
                {
                    bitmapOutput.SetPixel(x, max - y - 1, gray);
                }
            }
        }

        public static void sepia(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            for (int x = 0; x < bitmapInput.Width; x++)
            {
                for (int y = 0; y < bitmapInput.Height; y++)
                {
                    Color pixel = bitmapInput.GetPixel(x, y);
                    int red = Math.Min(255, (int)(pixel.R * 0.393 + pixel.G * 0.769 + pixel.B * 0.189));
                    int green = Math.Min(255, (int)(pixel.R * 0.349 + pixel.G * 0.686 + pixel.B * 0.168));
                    int blue = Math.Min(255, (int)(pixel.R * 0.272 + pixel.G * 0.534 + pixel.B * 0.131));
                    bitmapOutput.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
        }
    }
}
