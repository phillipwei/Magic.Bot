using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Autobot
{
    public class FastAccessImage
    {
        private byte[] bytes;
        private int stride;

        public FastAccessImage(Bitmap bmp)
        {
            if (bmp.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new NotImplementedException("Only Format32bppArgb supported");
            }

            this.Width = bmp.Width;
            this.Height = bmp.Height;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int numberOfBytes = bmpData.Stride * bmp.Height;
            this.stride = bmpData.Stride;
            byte[] rgbValues = new byte[numberOfBytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, numberOfBytes);
            this.bytes = rgbValues;
            bmp.UnlockBits(bmpData);
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public static FastAccessImage FromPath(string imagePath)
        {
            using (Bitmap bmp = (Bitmap)Bitmap.FromFile(imagePath))
            {
                return new FastAccessImage(bmp);
            }
        }

        public Pixel GetPixel(int x, int y)
        {
            if (x < 0 || y < 0 || x > this.Width || y > this.Height)
            {
                throw new ArgumentException(string.Format("GetPixel({0},{1}) on an image of size [{2},{3}]", x, y, this.Width, this.Height));
            }

            int baseIndex = (y * this.stride) + (x * 4);
            return new Pixel(
                this.bytes[baseIndex + 2], 
                this.bytes[baseIndex + 1], 
                this.bytes[baseIndex],
                this.bytes[baseIndex + 3]);
        }

        public bool Matches(FastAccessImage image, Rectangle rectangle)
        {
            for (int i = 0; i < image.Width && i < rectangle.Width; i++)
            {
                for (int j = 0; j < image.Height && j < rectangle.Height; j++)
                {
                    int x = i + rectangle.X;
                    int y = j + rectangle.Y;
                    Pixel referencePixel = image.GetPixel(i, j);
                    if (!referencePixel.IsTransparent) 
                    {
                        if (!referencePixel.Equals(this.GetPixel(x, y)))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool CloselyMatches(FastAccessImage image, Rectangle rectangle, int channelThreshold, double percentageMatch)
        {
            int unmatchedPixels = 0;
            for (int i = 0; i < image.Width && i < rectangle.Width; i++)
            {
                for (int j = 0; j < image.Height && j < rectangle.Height; j++)
                {
                    int x = i + rectangle.X;
                    int y = j + rectangle.Y;
                    Pixel referencePixel = image.GetPixel(i, j);
                    if (!referencePixel.IsTransparent)
                    {
                        if (!referencePixel.CloselyMatches(this.GetPixel(x, y), channelThreshold))
                        {
                            unmatchedPixels++;
                            if (unmatchedPixels / (double)(image.Width * image.Height) > (1 - percentageMatch))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public struct Pixel
        {
            public byte Red, Green, Blue, Alpha;

            public Pixel(byte r, byte g, byte b, byte a)
            {
                this.Red = r;
                this.Green = g;
                this.Blue = b;
                this.Alpha = a;
            }

            public bool IsTransparent
            {
                get { return this.Alpha == 0; }
            }

            public override int GetHashCode()
            {
                int hash = 17;
                hash = (hash * 23) + this.Red.GetHashCode();
                hash = (hash * 23) + this.Green.GetHashCode();
                hash = (hash * 23) + this.Blue.GetHashCode();
                hash = (hash * 23) + this.Alpha.GetHashCode();
                return hash;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Pixel))
                {
                    return false;
                }

                Pixel other = (Pixel)obj;
                return this.Red == other.Red &&
                    this.Green == other.Green &&
                    this.Blue == other.Blue &&
                    this.Alpha == other.Alpha;
            }

            public bool CloselyMatches(Pixel other, int channelThreshold)
            {
                return Math.Abs(this.Red - other.Red) < channelThreshold &&
                    Math.Abs(this.Green - other.Green) < channelThreshold &&
                    Math.Abs(this.Blue - other.Blue) < channelThreshold;
            }
        }
    }
}
