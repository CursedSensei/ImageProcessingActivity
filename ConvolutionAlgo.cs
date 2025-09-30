using System.Drawing;
using ConvolutionMatrix;

namespace ImageAlgorithm
{
    public partial class ImageAlgorithm
    {
        public static void smooth(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Factor = 9;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }

        public static void gaussianBlur(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix(1, 2, 1, 2, 4, 2, 1, 2, 1);
            m.Factor = 16;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }

        public static void sharpen(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix(0, -2, 0, -2, 11, -2, 0, -2, 0);
            m.Factor = 3;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }

        public static void meanRemoval(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(-1);
            m.Pixel = 9;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }

        public static void embossLaplascian(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix(-1, 0, -1, 0, 4, 0, -1, 0, -1);
            m.Factor = 1;
            m.Offset = 127;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }

        public static void embossHorizontalVertical(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix(0, -1, 0, -1, 4, -1, 0, -1, 0);
            m.Factor = 1;
            m.Offset = 127;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }

        public static void embossAllDirections(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(-1);
            m.Pixel = 8;
            m.Factor = 1;
            m.Offset = 127;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }

        public static void embossLossy(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix(1, -2, 1, -2, 4, -2, -2, 1, -2);
            m.Factor = 1;
            m.Offset = 127;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }

        public static void embossHorizontal(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix(0, 0, 0, -1, 2, -1, 0, 0, 0);
            m.Factor = 1;
            m.Offset = 127;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }

        public static void embossVertical(Bitmap bitmapInput, ref Bitmap bitmapOutput)
        {
            ConvMatrix m = new ConvMatrix(0, -1, 0, 0, 0, 0, 0, 1, 0);
            m.Factor = 1;
            m.Offset = 127;
            m.Conv3x3(bitmapInput, bitmapOutput);
        }
    }
}
