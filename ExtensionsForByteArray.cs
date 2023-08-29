using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace AppEnd
{
    public static class ExtensionsForByteArray
    {
        public static byte[] ResizeImage(this byte[] imageFile, int targetSize)
        {
            //return imageFile;

            using (Image oldImage = Image.FromStream(new MemoryStream(imageFile), true, true))
            {
                Size newSize = CalculateIntelligentDimensions(oldImage.Size, targetSize);
                using (Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb))
                {
                    using (Graphics canvas = Graphics.FromImage(newImage))
                    {
                        canvas.SmoothingMode = SmoothingMode.HighQuality;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize));
                        MemoryStream m = new MemoryStream();

                        ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                        EncoderParameters encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 255L);
                        newImage.Save(m, info[1], encoderParams);

                        return m.GetBuffer();
                    }
                }
            }
        }

        private static Size CalculateIntelligentDimensions(Size oldSize, int targetSize)
        {
            Size newSize = new Size();
            if (oldSize.Height > oldSize.Width)
            {
                newSize.Width = (int)(oldSize.Width * ((float)targetSize / (float)oldSize.Height));
                newSize.Height = targetSize;
            }
            else
            {
                newSize.Width = targetSize;
                newSize.Height = (int)(oldSize.Height * ((float)targetSize / (float)oldSize.Width));
            }
            return newSize;
        }
    }
}