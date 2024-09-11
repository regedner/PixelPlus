using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace pixelPlus
{
    public partial class Form1 : Form
    {

        private double currentScale = 1.0;
        private Bitmap image;
        public Form1()
        {

            InitializeComponent();
        }
        private void UpdateLabelVisibility()
        {
            if (pictureBox1.Image == null)
            {
                label1.Visible = true;

            }
            else
            {
                label1.Visible = false;
            }

            if (pictureBox2.Image == null)
            {
                label2.Visible = true;

            }
            else
            {
                label2.Visible = false;
            }

            if (pictureBox3.Image == null)
            {
                label3.Visible = true;

            }
            else
            {
                label3.Visible = false;
            }

        }
        private void button1_Click(object sender, EventArgs e) // resimyükle1
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImage = openFileDialog.FileName;
                    image = new Bitmap(selectedImage);


                    pictureBox1.Image = new Bitmap(image, pictureBox1.Width, pictureBox1.Height);
                }

            }
            label4.Text = "Original Image";
            label5.Text = "";
            UpdateLabelVisibility();
        }

        private void button2_Click(object sender, EventArgs e) // resimyükle2
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImage = openFileDialog.FileName;
                    Bitmap image = new Bitmap(selectedImage);


                    pictureBox2.Image = new Bitmap(image, pictureBox2.Width, pictureBox2.Height);
                }

            }
            label5.Text = "Second Original Image";
            UpdateLabelVisibility();
        }


        private void button3_Click(object sender, EventArgs e) //ali binary
        {


            if (pictureBox1.Image != null)
            {
                Bitmap orgImage = (Bitmap)pictureBox1.Image;

                Bitmap binaryImage = new Bitmap(orgImage.Width, orgImage.Height);
                for (int x = 0; x < orgImage.Width; x++)
                {
                    for (int y = 0; y < orgImage.Height; y++)
                    {
                        Color pixel = orgImage.GetPixel(x, y);
                        int threshold = 127;


                        Color bwPixel = (pixel.R + pixel.G + pixel.B) / 3 > threshold
                            ? Color.White
                            : Color.Black;

                        binaryImage.SetPixel(x, y, bwPixel);
                    }
                }
                pictureBox3.Image = binaryImage;
                UpdateLabelVisibility();
                label6.Text = "Binary Conversion";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }



        private void button4_Click(object sender, EventArgs e) //alper döndürme
        {
            if (pictureBox1.Image != null)
            {
                Bitmap originalImage = new Bitmap(pictureBox1.Image);
                Bitmap rotatedImage = RotateImage(originalImage, 45);
                pictureBox1.Image = rotatedImage;
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;


                int newWidth = pictureBox1.Width;
                int newHeight = pictureBox1.Height;



            }
            else { MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }


        private Bitmap RotateImage(Bitmap image, float angle)
        {

            Matrix matrix = new Matrix();
            matrix.Rotate(angle);


            Bitmap rotatedImage = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.TranslateTransform(image.Width / 2, image.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-image.Width / 2, -image.Height / 2);
                g.DrawImage(image, new Point(0, 0));
            }
            return rotatedImage;
        }






        public int[] CalculateHistogram(Bitmap image) //alper histogram hesaplama 
        {
            int[] histogram = new int[256];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int grayscaleValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    histogram[grayscaleValue]++;
                }
            }

            return histogram;
        }
        public Bitmap StretchHistogram(Bitmap image)//alper histogram germe
        {
            int[] histogram = CalculateHistogram(image);
            int minValue = 0;
            int maxValue = 255;
            for (int i = 0; i < 256; i++)
            {
                if (histogram[i] > 0)
                {
                    minValue = i;
                    break;
                }
            }
            for (int i = 255; i >= 0; i--)
            {
                if (histogram[i] > 0)
                {
                    maxValue = i;
                    break;
                }
            }

            Bitmap stretchedImage = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int grayscaleValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    int newPixelValue = (int)((grayscaleValue - minValue) * (255.0 / (maxValue - minValue)));
                    newPixelValue = Math.Max(0, Math.Min(255, newPixelValue));
                    Color newColor = Color.FromArgb(newPixelValue, newPixelValue, newPixelValue);
                    stretchedImage.SetPixel(x, y, newColor);
                }
            }

            return stretchedImage;
        }

        public Bitmap genisletHistogram(Bitmap image)//alper histogram genisletme
        {
            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    histogramR[pixelColor.R]++;
                    histogramG[pixelColor.G]++;
                    histogramB[pixelColor.B]++;
                }
            }
            int[] newHistR = new int[128];
            int[] newHistG = new int[128];
            int[] newHistB = new int[128];

            for (int i = 0; i < 128; i++)
            {
                newHistR[i] = histogramR[2 * i] + histogramR[2 * i + 1];
                newHistG[i] = histogramG[2 * i] + histogramG[2 * i + 1];
                newHistB[i] = histogramB[2 * i] + histogramB[2 * i + 1];
            }

            int minR = Array.IndexOf(newHistR, newHistR.Min());
            int maxR = Array.IndexOf(newHistR, newHistR.Max());
            int minG = Array.IndexOf(newHistG, newHistG.Min());
            int maxG = Array.IndexOf(newHistG, newHistG.Max());
            int minB = Array.IndexOf(newHistB, newHistB.Min());
            int maxB = Array.IndexOf(newHistB, newHistB.Max());

            Bitmap stretchedImage = new Bitmap(image.Width, image.Height);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int newR = (int)((pixelColor.R - minR) * 255.0 / (maxR - minR));
                    int newG = (int)((pixelColor.G - minG) * 255.0 / (maxG - minG));
                    int newB = (int)((pixelColor.B - minB) * 255.0 / (maxB - minB));

                    newR = Math.Max(0, Math.Min(255, newR));
                    newG = Math.Max(0, Math.Min(255, newG));
                    newB = Math.Max(0, Math.Min(255, newB));

                    Color newColor = Color.FromArgb(newR, newG, newB);
                    stretchedImage.SetPixel(x, y, newColor);
                }
            }
            return stretchedImage;
        }

        private void DisplayHistogram(int[] histogram, PictureBox pictureBox)//alper histogram cizme
        {


            int pictureBoxWidth = pictureBox.Width;
            int pictureBoxHeight = pictureBox.Height;


            int graphWidth = pictureBoxWidth - 60;
            int graphHeight = pictureBoxHeight - 60;


            Bitmap graph = new Bitmap(pictureBoxWidth, pictureBoxHeight);
            using (Graphics g = Graphics.FromImage(graph))
            {

                g.Clear(Color.White);


                Pen axisPen = new Pen(Color.Black);
                g.DrawLine(axisPen, 40, graphHeight + 40, 40, 40);
                g.DrawLine(axisPen, 40, graphHeight + 40, graphWidth + 40, graphHeight + 40);


                float barWidth = (float)graphWidth / histogram.Length;
                float maxValue = histogram.Max();
                for (int i = 0; i < histogram.Length; i++)
                {
                    float barHeight = (histogram[i] / maxValue) * graphHeight;
                    RectangleF barRect = new RectangleF(40 + i * barWidth, graphHeight + 40 - barHeight, barWidth, barHeight);
                    g.FillRectangle(Brushes.Black, barRect);
                }


                Font font = new Font("Arial", 8);
                Brush brush = Brushes.Black;
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                for (int i = 0; i < histogram.Length; i += 50)
                {

                    g.DrawString(i.ToString(), font, brush, 40 + i * barWidth - 5, graphHeight + 45, format);
                }


                int stepSize = (int)Math.Ceiling(maxValue / 5);
                for (int i = 0; i <= 5; i++)
                {
                    int value = stepSize * i;
                    g.DrawString(value.ToString(), font, brush, 20, graphHeight + 40 - (value / maxValue) * graphHeight, format);
                }
            }


            pictureBox3.Image = graph;
        }

        private void button36_Click(object sender, EventArgs e)//alper orj histo
        {
            if (pictureBox1.Image != null)
            {
                Bitmap orgImage = new Bitmap(pictureBox1.Image);


                int[] originalHistogram = CalculateHistogram(orgImage);
                DisplayHistogram(originalHistogram, pictureBox3);
                UpdateLabelVisibility();
                label6.Text = "Histogram of Original Image";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void button5_Click(object sender, EventArgs e) //alper histogram germe 
        {
            if (pictureBox1.Image != null)
            {
                Bitmap orgImage = new Bitmap(pictureBox1.Image);
                Bitmap stretchedImage = StretchHistogram(orgImage);


                int[] stretchedHistogram = CalculateHistogram(stretchedImage);
                DisplayHistogram(stretchedHistogram, pictureBox3);
                UpdateLabelVisibility();
                label6.Text = "Stretched Histogram of Original Image";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void button23_Click(object sender, EventArgs e)//alper histogram genisletme
        {
            if (pictureBox1.Image != null)
            {
                Bitmap orgImage = new Bitmap(pictureBox1.Image);
                Bitmap equalizedImage = genisletHistogram(orgImage);


                int[] equalizedHistogram = CalculateHistogram(equalizedImage);
                DisplayHistogram(equalizedHistogram, pictureBox3);
                UpdateLabelVisibility();
                label6.Text = "Expanded Histogram of Original Image";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }



        private Bitmap UnsharpMask(Bitmap image, float amount, int radius, int threshold)//alper unsharp
        {
            Bitmap blurred = GaussianBlur(image, radius);
            Bitmap sharpened = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color originalColor = image.GetPixel(x, y);
                    Color blurredColor = blurred.GetPixel(x, y);

                    int diffR = originalColor.R - blurredColor.R;
                    int diffG = originalColor.G - blurredColor.G;
                    int diffB = originalColor.B - blurredColor.B;

                    if (Math.Abs(diffR) > threshold || Math.Abs(diffG) > threshold || Math.Abs(diffB) > threshold)
                    {
                        int newR = (int)(originalColor.R + amount * diffR);
                        int newG = (int)(originalColor.G + amount * diffG);
                        int newB = (int)(originalColor.B + amount * diffB);

                        newR = Math.Max(0, Math.Min(255, newR));
                        newG = Math.Max(0, Math.Min(255, newG));
                        newB = Math.Max(0, Math.Min(255, newB));

                        sharpened.SetPixel(x, y, Color.FromArgb(newR, newG, newB));
                    }
                    else
                    {
                        sharpened.SetPixel(x, y, originalColor);
                    }
                }
            }

            return sharpened;
        }

        private Bitmap GaussianBlur(Bitmap image, int radius)//alper unsharp
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);
            int[,] kernel = CreateGaussianKernel(radius);
            int kernelSize = 2 * radius + 1;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int r = 0, g = 0, b = 0, weightSum = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        int offsetX = x + k;
                        if (offsetX >= 0 && offsetX < image.Width)
                        {
                            Color pixel = image.GetPixel(offsetX, y);
                            int weight = kernel[radius + k, radius];
                            r += pixel.R * weight;
                            g += pixel.G * weight;
                            b += pixel.B * weight;
                            weightSum += weight;
                        }
                    }
                    if (weightSum == 0) weightSum = 1;

                    r = Math.Max(0, Math.Min(255, r / weightSum));
                    g = Math.Max(0, Math.Min(255, g / weightSum));
                    b = Math.Max(0, Math.Min(255, b / weightSum));

                    blurred.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            Bitmap finalBlurred = new Bitmap(image.Width, image.Height);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int r = 0, g = 0, b = 0, weightSum = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        int offsetY = y + k;
                        if (offsetY >= 0 && offsetY < image.Height)
                        {
                            Color pixel = blurred.GetPixel(x, offsetY);
                            int weight = kernel[radius, radius + k];
                            r += pixel.R * weight;
                            g += pixel.G * weight;
                            b += pixel.B * weight;
                            weightSum += weight;
                        }
                    }
                    if (weightSum == 0) weightSum = 1;
                    r /= weightSum;
                    g /= weightSum;
                    b /= weightSum;
                    r = Math.Max(0, Math.Min(255, r));
                    g = Math.Max(0, Math.Min(255, g));
                    b = Math.Max(0, Math.Min(255, b));
                    finalBlurred.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return finalBlurred;
        }

        private int[,] CreateGaussianKernel(int radius)//alper unsharp
        {
            int size = 2 * radius + 1;
            int[,] kernel = new int[size, size];
            double sigma = radius / 3.0;
            double twoSigmaSquare = 2 * sigma * sigma;
            double sigmaRoot = Math.Sqrt(twoSigmaSquare * Math.PI);
            double total = 0;

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    double distance = x * x + y * y;
                    kernel[y + radius, x + radius] = (int)(Math.Exp(-distance / twoSigmaSquare) / sigmaRoot);
                    total += kernel[y + radius, x + radius];
                }
            }

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    kernel[y, x] = (int)(kernel[y, x] * (1.0 / total));
                }
            }

            return kernel;
        }

        private void button6_Click(object sender, EventArgs e) //alper unsharp
        {
            if (pictureBox1.Image != null)
            {
                Bitmap orgImage = new Bitmap(pictureBox1.Image);
                float amount = 0.3f;
                int radius = 5;
                int threshold = 10;

                Bitmap unsharpMaskedImage = UnsharpMask(orgImage, amount, radius, threshold);


                pictureBox3.Image = unsharpMaskedImage;
                UpdateLabelVisibility();
                label6.Text = "Unsharp Filter";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button7_Click(object sender, EventArgs e)//alper+
        {
            if (pictureBox1.Image != null)
            {
                Bitmap image1 = new Bitmap(pictureBox1.Image);
                Bitmap image2 = (Bitmap)pictureBox2.Image?.Clone();

                if (image2 != null)
                {

                    int newWidth1 = (int)(pictureBox1.Width * currentScale);
                    int newHeight1 = (int)(pictureBox1.Height * currentScale);

                    Bitmap newImage1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                    using (Graphics g1 = Graphics.FromImage(newImage1))
                    {
                        g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g1.DrawImage(image1, new Rectangle((pictureBox1.Width - newWidth1) / 2, (pictureBox1.Height - newHeight1) / 2, newWidth1, newHeight1), new Rectangle(0, 0, image1.Width, image1.Height), GraphicsUnit.Pixel);
                    }


                    int newWidth2 = (int)(newWidth1 * (double)image2.Width / image1.Width);
                    int newHeight2 = (int)(newHeight1 * (double)image2.Height / image1.Height);

                    Bitmap newImage2 = new Bitmap(newWidth2, newHeight2);

                    using (Graphics g2 = Graphics.FromImage(newImage2))
                    {
                        g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g2.DrawImage(image2, new Rectangle(0, 0, newWidth2, newHeight2), new Rectangle(0, 0, image2.Width, image2.Height), GraphicsUnit.Pixel);
                    }


                    int resultWidth = Math.Min(newImage1.Width, newImage2.Width);
                    int resultHeight = Math.Min(newImage1.Height, newImage2.Height);
                    Bitmap resultImage = new Bitmap(resultWidth, resultHeight);

                    for (int y = 0; y < resultHeight; y++)
                    {
                        for (int x = 0; x < resultWidth; x++)
                        {
                            Color pixel1 = newImage1.GetPixel(x, y);
                            Color pixel2 = newImage2.GetPixel(x, y);

                            int r = 0, g = 0, b = 0;
                            r = Math.Min(pixel1.R + pixel2.R, 255);
                            g = Math.Min(pixel1.G + pixel2.G, 255);
                            b = Math.Min(pixel1.B + pixel2.B, 255);
                            resultImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }

                    pictureBox3.Image = resultImage;
                    UpdateLabelVisibility();
                    label6.Text = "Addition of 1st Image and 2nd Image";
                }
                else
                {
                    MessageBox.Show("Please upload image 2 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        private void button8_Click(object sender, EventArgs e)//alper-
        {
            if (pictureBox1.Image != null)
            {
                Bitmap image1 = new Bitmap(pictureBox1.Image);
                Bitmap image2 = (Bitmap)pictureBox2.Image?.Clone();

                if (image2 != null)
                {

                    int newWidth1 = (int)(pictureBox1.Width * currentScale);
                    int newHeight1 = (int)(pictureBox1.Height * currentScale);

                    Bitmap newImage1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                    using (Graphics g1 = Graphics.FromImage(newImage1))
                    {
                        g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g1.DrawImage(image1, new Rectangle((pictureBox1.Width - newWidth1) / 2, (pictureBox1.Height - newHeight1) / 2, newWidth1, newHeight1), new Rectangle(0, 0, image1.Width, image1.Height), GraphicsUnit.Pixel);
                    }

                    int newWidth2 = (int)(newWidth1 * (double)image2.Width / image1.Width);
                    int newHeight2 = (int)(newHeight1 * (double)image2.Height / image1.Height);

                    Bitmap newImage2 = new Bitmap(newWidth2, newHeight2);

                    using (Graphics g2 = Graphics.FromImage(newImage2))
                    {
                        g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g2.DrawImage(image2, new Rectangle(0, 0, newWidth2, newHeight2), new Rectangle(0, 0, image2.Width, image2.Height), GraphicsUnit.Pixel);
                    }

                    int resultWidth = Math.Min(newImage1.Width, newImage2.Width);
                    int resultHeight = Math.Min(newImage1.Height, newImage2.Height);
                    Bitmap resultImage = new Bitmap(resultWidth, resultHeight);

                    for (int y = 0; y < resultHeight; y++)
                    {
                        for (int x = 0; x < resultWidth; x++)
                        {
                            Color pixel1 = newImage1.GetPixel(x, y);
                            Color pixel2 = newImage2.GetPixel(x, y);

                            int r = 0, g = 0, b = 0;
                            r = Math.Max(pixel1.R - pixel2.R, 0);
                            g = Math.Max(pixel1.G - pixel2.G, 0);
                            b = Math.Max(pixel1.B - pixel2.B, 0);
                            resultImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }

                    pictureBox3.Image = resultImage;
                    UpdateLabelVisibility();
                    label6.Text = "Substraction between 1st and 2nd image";
                }
                else
                {
                    MessageBox.Show("Please upload image 2 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button9_Click(object sender, EventArgs e)//alper*
        {
            if (pictureBox1.Image != null)
            {
                Bitmap image1 = new Bitmap(pictureBox1.Image);
                Bitmap image2 = (Bitmap)pictureBox2.Image?.Clone();

                if (image2 != null)
                {

                    int newWidth1 = (int)(pictureBox1.Width * currentScale);
                    int newHeight1 = (int)(pictureBox1.Height * currentScale);

                    Bitmap newImage1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                    using (Graphics g1 = Graphics.FromImage(newImage1))
                    {
                        g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g1.DrawImage(image1, new Rectangle((pictureBox1.Width - newWidth1) / 2, (pictureBox1.Height - newHeight1) / 2, newWidth1, newHeight1), new Rectangle(0, 0, image1.Width, image1.Height), GraphicsUnit.Pixel);
                    }


                    int newWidth2 = (int)(newWidth1 * (double)image2.Width / image1.Width);
                    int newHeight2 = (int)(newHeight1 * (double)image2.Height / image1.Height);

                    Bitmap newImage2 = new Bitmap(newWidth2, newHeight2);

                    using (Graphics g2 = Graphics.FromImage(newImage2))
                    {
                        g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g2.DrawImage(image2, new Rectangle(0, 0, newWidth2, newHeight2), new Rectangle(0, 0, image2.Width, image2.Height), GraphicsUnit.Pixel);
                    }


                    int resultWidth = Math.Min(newImage1.Width, newImage2.Width);
                    int resultHeight = Math.Min(newImage1.Height, newImage2.Height);
                    Bitmap resultImage = new Bitmap(resultWidth, resultHeight);

                    for (int y = 0; y < resultHeight; y++)
                    {
                        for (int x = 0; x < resultWidth; x++)
                        {
                            Color pixel1 = newImage1.GetPixel(x, y);
                            Color pixel2 = newImage2.GetPixel(x, y);

                            int r = 0, g = 0, b = 0;
                            r = Math.Min((pixel1.R * pixel2.R) / 255, 255);
                            g = Math.Min((pixel1.G * pixel2.G) / 255, 255);
                            b = Math.Min((pixel1.B * pixel2.B) / 255, 255);
                            resultImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }

                    pictureBox3.Image = resultImage;
                    UpdateLabelVisibility();
                    label6.Text = "Multiplying Image 1 by Image 2";
                }
                else
                {
                    MessageBox.Show("Please upload image 2 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void button10_Click(object sender, EventArgs e)//alper:
        {
            if (pictureBox1.Image != null)
            {
                Bitmap image1 = new Bitmap(pictureBox1.Image);
                Bitmap image2 = (Bitmap)pictureBox2.Image?.Clone();

                if (image2 != null)
                {

                    int newWidth1 = (int)(pictureBox1.Width * currentScale);
                    int newHeight1 = (int)(pictureBox1.Height * currentScale);

                    Bitmap newImage1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                    using (Graphics g1 = Graphics.FromImage(newImage1))
                    {
                        g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g1.DrawImage(image1, new Rectangle((pictureBox1.Width - newWidth1) / 2, (pictureBox1.Height - newHeight1) / 2, newWidth1, newHeight1), new Rectangle(0, 0, image1.Width, image1.Height), GraphicsUnit.Pixel);
                    }


                    int newWidth2 = (int)(newWidth1 * (double)image2.Width / image1.Width);
                    int newHeight2 = (int)(newHeight1 * (double)image2.Height / image1.Height);

                    Bitmap newImage2 = new Bitmap(newWidth2, newHeight2);

                    using (Graphics g2 = Graphics.FromImage(newImage2))
                    {
                        g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g2.DrawImage(image2, new Rectangle(0, 0, newWidth2, newHeight2), new Rectangle(0, 0, image2.Width, image2.Height), GraphicsUnit.Pixel);
                    }

                    int resultWidth = Math.Min(newImage1.Width, newImage2.Width);
                    int resultHeight = Math.Min(newImage1.Height, newImage2.Height);
                    Bitmap resultImage = new Bitmap(resultWidth, resultHeight);

                    for (int y = 0; y < resultHeight; y++)
                    {
                        for (int x = 0; x < resultWidth; x++)
                        {
                            Color pixel1 = newImage1.GetPixel(x, y);
                            Color pixel2 = newImage2.GetPixel(x, y);

                            int r = 0, g = 0, b = 0;
                            if (pixel2.R != 0)
                                r = Math.Min((pixel1.R / pixel2.R) * 255, 255);
                            if (pixel2.G != 0)
                                g = Math.Min((pixel1.G / pixel2.G) * 255, 255);
                            if (pixel2.B != 0)
                                b = Math.Min((pixel1.B / pixel2.B) * 255, 255);
                            resultImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }

                    pictureBox3.Image = resultImage;
                    UpdateLabelVisibility();
                    label6.Text = "Division of Image 1 and Image 2";
                }
                else
                {
                    MessageBox.Show("Please upload image 2 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }





        private void button11_Click(object sender, EventArgs e)//alizoom+ 
        {
            currentScale *= 1.1;

            if (pictureBox1.Image != null)
            {

                Bitmap currentImage = (Bitmap)pictureBox1.Image;


                int newWidth = (int)(pictureBox1.Width * currentScale);
                int newHeight = (int)(pictureBox1.Height * currentScale);


                Bitmap newImage = new Bitmap(newWidth, newHeight);

                using (Graphics g = Graphics.FromImage(newImage))
                {

                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(currentImage, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, currentImage.Width, currentImage.Height), GraphicsUnit.Pixel);
                }


                pictureBox1.Image = newImage;


                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            else { MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void button12_Click(object sender, EventArgs e)//alizoom-
        {
            currentScale /= 1.1;


            if (currentScale < 1.0)
            {
                currentScale = 1.0;
                return;
            }

            if (pictureBox1.Image != null)
            {

                Bitmap currentImage = (Bitmap)pictureBox1.Image;


                int newWidth = (int)(pictureBox1.Width * currentScale);
                int newHeight = (int)(pictureBox1.Height * currentScale);


                Bitmap newImage = new Bitmap(newWidth, newHeight);

                using (Graphics g = Graphics.FromImage(newImage))
                {

                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(currentImage, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, currentImage.Width, currentImage.Height), GraphicsUnit.Pixel);
                }

                pictureBox1.Image = newImage;

                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            else { MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void button13_Click(object sender, EventArgs e)//eren konv
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            float[,] kernel = new float[,]
            {
            { -1, 0, 1 },
            { -2, 0, 2 },
            { -1, 0, 1 }
            };

            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            Bitmap filteredImage = Convolve(originalImage, kernel);


            pictureBox3.Image = filteredImage;
            UpdateLabelVisibility();
            label6.Text = "Convolution";
        }

        private Bitmap Convolve(Bitmap originalImage, float[,] kernel)//eren konv
        {
            int kernelWidth = kernel.GetLength(1);
            int kernelHeight = kernel.GetLength(0);
            int imageWidth = originalImage.Width;
            int imageHeight = originalImage.Height;
            Bitmap resultImage = new Bitmap(imageWidth, imageHeight);


            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    float r = 0, g = 0, b = 0;


                    for (int i = 0; i < kernelHeight; i++)
                    {
                        for (int j = 0; j < kernelWidth; j++)
                        {
                            int pixelX = x - kernelWidth / 2 + j;
                            int pixelY = y - kernelHeight / 2 + i;

                            if (pixelX >= 0 && pixelX < imageWidth && pixelY >= 0 && pixelY < imageHeight)
                            {
                                Color pixel = originalImage.GetPixel(pixelX, pixelY);
                                r += pixel.R * kernel[i, j];
                                g += pixel.G * kernel[i, j];
                                b += pixel.B * kernel[i, j];
                            }
                        }
                    }

                    r = Math.Min(Math.Max(r, 0), 255);
                    g = Math.Min(Math.Max(g, 0), 255);
                    b = Math.Min(Math.Max(b, 0), 255);

                    resultImage.SetPixel(x, y, Color.FromArgb((int)r, (int)g, (int)b));
                }
            }

            return resultImage;

        }

        private void button14_Click(object sender, EventArgs e)//eren kontrast+
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            pictureBox1.Image = IncreaseContrast((Bitmap)pictureBox1.Image);
            UpdateLabelVisibility();
            label4.Text = "Increased Contrast of Original Image";



        }

        private void button40_Click(object sender, EventArgs e)//eren kontrast-
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            pictureBox1.Image = DecreaseContrast((Bitmap)pictureBox1.Image);
            label4.Text = "Original Image Contrast Reduced";
            UpdateLabelVisibility();
        }

        private Bitmap IncreaseContrast(Bitmap image)//eren kontrast+
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            float contrast = 1.5f;


            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color oldColor = image.GetPixel(x, y);
                    float newRed = (((oldColor.R / 255.0f) - 0.5f) * contrast + 0.5f) * 255.0f;
                    float newGreen = (((oldColor.G / 255.0f) - 0.5f) * contrast + 0.5f) * 255.0f;
                    float newBlue = (((oldColor.B / 255.0f) - 0.5f) * contrast + 0.5f) * 255.0f;


                    newRed = Math.Max(0, Math.Min(255, newRed));
                    newGreen = Math.Max(0, Math.Min(255, newGreen));
                    newBlue = Math.Max(0, Math.Min(255, newBlue));

                    Color newColor = Color.FromArgb((int)newRed, (int)newGreen, (int)newBlue);
                    newImage.SetPixel(x, y, newColor);
                }
            }

            return newImage;
        }

        private Bitmap DecreaseContrast(Bitmap image)//eren kontrast-
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);


            double factor = 0.5;



            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color originalColor = image.GetPixel(x, y);

                    int r = originalColor.R;
                    int g = originalColor.G;
                    int b = originalColor.B;

                    r = (int)((r - 128) * factor + 128);
                    g = (int)((g - 128) * factor + 128);
                    b = (int)((b - 128) * factor + 128);

                    r = Math.Max(0, Math.Min(255, r));
                    g = Math.Max(0, Math.Min(255, g));
                    b = Math.Max(0, Math.Min(255, b));

                    Color newColor = Color.FromArgb(r, g, b);
                    newImage.SetPixel(x, y, newColor);
                }
            }

            return newImage;

        }



        private void AddSaltPepperNoise(Bitmap image, float noiseProbability)//eren tuzbiber
        {
            Random rand = new Random();

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (rand.NextDouble() < noiseProbability)
                    {

                        image.SetPixel(i, j, rand.Next(2) == 0 ? Color.Black : Color.White);
                    }
                }
            }



        }

        private Bitmap MeanFilter(Bitmap image)// eren mean
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            for (int i = 1; i < image.Width - 1; i++)
            {
                for (int j = 1; j < image.Height - 1; j++)
                {
                    Color[] neighbors = {
                        image.GetPixel(i - 1, j - 1), image.GetPixel(i, j - 1), image.GetPixel(i + 1, j - 1),
                        image.GetPixel(i - 1, j),     image.GetPixel(i, j),     image.GetPixel(i + 1, j),
                        image.GetPixel(i - 1, j + 1), image.GetPixel(i, j + 1), image.GetPixel(i + 1, j + 1)
                    };

                    int sumR = 0, sumG = 0, sumB = 0;

                    foreach (Color neighbor in neighbors)
                    {
                        sumR += neighbor.R;
                        sumG += neighbor.G;
                        sumB += neighbor.B;
                    }

                    int avgR = sumR / 9;
                    int avgG = sumG / 9;
                    int avgB = sumB / 9;

                    result.SetPixel(i, j, Color.FromArgb(avgR, avgG, avgB));
                }
            }

            return result;
        }


        private Bitmap MedianFilter(Bitmap image)//eren median
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            for (int i = 1; i < image.Width - 1; i++)
            {
                for (int j = 1; j < image.Height - 1; j++)
                {
                    Color[] neighbors = {
                        image.GetPixel(i - 1, j - 1), image.GetPixel(i, j - 1), image.GetPixel(i + 1, j - 1),
                        image.GetPixel(i - 1, j),     image.GetPixel(i, j),     image.GetPixel(i + 1, j),
                        image.GetPixel(i - 1, j + 1), image.GetPixel(i, j + 1), image.GetPixel(i + 1, j + 1)
                    };

                    int[] redValues = new int[9];
                    int[] greenValues = new int[9];
                    int[] blueValues = new int[9];

                    for (int k = 0; k < 9; k++)
                    {
                        redValues[k] = neighbors[k].R;
                        greenValues[k] = neighbors[k].G;
                        blueValues[k] = neighbors[k].B;
                    }

                    Array.Sort(redValues);
                    Array.Sort(greenValues);
                    Array.Sort(blueValues);

                    int medianR = redValues[4];
                    int medianG = greenValues[4];
                    int medianB = blueValues[4];

                    result.SetPixel(i, j, Color.FromArgb(medianR, medianG, medianB));
                }
            }

            return result;
        }

        private void button15_Click(object sender, EventArgs e)//eren tuzbiber
        {
            if (pictureBox1.Image != null)
            {
                Bitmap image = new Bitmap(pictureBox1.Image);


                AddSaltPepperNoise(image, 0.05f);
                pictureBox2.Image = image;
                UpdateLabelVisibility();
                label5.Text = "Salt & Pepper Noise";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button17_Click(object sender, EventArgs e) //eren mean
        {
            if (pictureBox2.Image != null)
            {
                Bitmap noisyImage = new Bitmap(pictureBox2.Image);


                Bitmap cleanedImage = MeanFilter(noisyImage);


                pictureBox3.Image = cleanedImage;
                UpdateLabelVisibility();
                label6.Text = "Tuz&Biber Gürültüsü Mean ile Temizlendi";
            }
            else
            {
                MessageBox.Show("Please create noise on an image first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button16_Click(object sender, EventArgs e)//eren median
        {
            if (pictureBox2.Image != null)
            {
                Bitmap noisyImage = new Bitmap(pictureBox2.Image);


                Bitmap cleanedImage = MedianFilter(noisyImage);


                pictureBox3.Image = cleanedImage;
                UpdateLabelVisibility();
                label6.Text = "Salt & Pepper Noise Cleared with Median";
            }
            else
            {
                MessageBox.Show("Please create noise on an image first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        private Bitmap DilateWithDiskKernel(Bitmap image, int radius)//eren morf genisleme
        {
            Bitmap result = new Bitmap(image.Width, image.Height);


            int diameter = radius * 2;
            int[,] kernel = new int[diameter + 1, diameter + 1];
            for (int x = 0; x <= diameter; x++)
            {
                for (int y = 0; y <= diameter; y++)
                {
                    if (Math.Sqrt(Math.Pow(x - radius, 2) + Math.Pow(y - radius, 2)) <= radius)
                    {
                        kernel[x, y] = 1;
                    }
                    else
                    {
                        kernel[x, y] = 0;
                    }
                }
            }


            int[,] greyPixels = new int[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    greyPixels[x, y] = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                }
            }


            for (int x = radius; x < image.Width - radius; x++)
            {
                for (int y = radius; y < image.Height - radius; y++)
                {
                    int max = 0;
                    for (int i = -radius; i <= radius; i++)
                    {
                        for (int j = -radius; j <= radius; j++)
                        {
                            int val = greyPixels[x + i, y + j] + kernel[i + radius, j + radius];
                            max = Math.Max(max, Math.Min(255, val));
                        }
                    }
                    result.SetPixel(x, y, Color.FromArgb(max, max, max));
                }
            }
            return result;
        }

        private void button18_Click(object sender, EventArgs e)//eren morf genisleme
        {
            if (pictureBox1.Image != null)
            {

                Bitmap dilatedImage = DilateWithDiskKernel(new Bitmap(pictureBox1.Image), 3);
                pictureBox3.Image = dilatedImage;
                UpdateLabelVisibility();
                label6.Text = "Morphological Dilation";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Bitmap ErodeWithDiskKernel(Bitmap image, int radius)//eren morf asinma
        {
            Bitmap result = new Bitmap(image.Width, image.Height);


            int diameter = radius * 2;
            int[,] kernel = new int[diameter + 1, diameter + 1];
            for (int x = 0; x <= diameter; x++)
            {
                for (int y = 0; y <= diameter; y++)
                {
                    if (Math.Sqrt(Math.Pow(x - radius, 2) + Math.Pow(y - radius, 2)) <= radius)
                    {
                        kernel[x, y] = 1;
                    }
                    else
                    {
                        kernel[x, y] = 0;
                    }
                }
            }


            int[,] greyPixels = new int[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    greyPixels[x, y] = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                }
            }


            for (int x = radius; x < image.Width - radius; x++)
            {
                for (int y = radius; y < image.Height - radius; y++)
                {
                    int min = 255;
                    for (int i = -radius; i <= radius; i++)
                    {
                        for (int j = -radius; j <= radius; j++)
                        {
                            int val = greyPixels[x + i, y + j] - kernel[i + radius, j + radius];
                            min = Math.Min(min, Math.Max(0, val));
                        }
                    }
                    result.SetPixel(x, y, Color.FromArgb(min, min, min));
                }
            }
            return result;
        }

        private void button19_Click(object sender, EventArgs e)//eren morf asinma
        {
            if (pictureBox1.Image != null)
            {

                Bitmap erodedImage = ErodeWithDiskKernel(new Bitmap(pictureBox1.Image), 3);
                pictureBox3.Image = erodedImage;
                UpdateLabelVisibility();
                label6.Text = "Morphological Erosion";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Bitmap OpeningWithDiskKernel(Bitmap image, int radius)//eren morf acma
        {
            Bitmap result = ErodeWithDiskKernel(image, radius);
            result = DilateWithDiskKernel(result, radius);
            return result;
        }


        private void button20_Click(object sender, EventArgs e)//eren morf acma
        {
            if (pictureBox1.Image != null)
            {

                Bitmap openedImage = OpeningWithDiskKernel(new Bitmap(pictureBox1.Image), 7);
                pictureBox3.Image = openedImage;
                UpdateLabelVisibility();
                label6.Text = "Morphological Opening";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Bitmap ClosingWithDiskKernel(Bitmap image, int radius)//eren morf kapama
        {
            Bitmap result = DilateWithDiskKernel(image, radius);
            result = ErodeWithDiskKernel(result, radius);
            return result;
        }


        private void button21_Click(object sender, EventArgs e)//eren morf kapama
        {
            if (pictureBox1.Image != null)
            {

                Bitmap closedImage = ClosingWithDiskKernel(new Bitmap(pictureBox1.Image), 10);
                pictureBox3.Image = closedImage;
                UpdateLabelVisibility();
                label6.Text = "Morphological Closing";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button22_Click(object sender, EventArgs e)//sýfýrla
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            UpdateLabelVisibility();
            label4.Text = "";
            label5.Text = "PROGRAM HAS BEEN RESET SUCCESSFULLY";
            label6.Text = "";
        }


        private Bitmap ToGrayscale(Bitmap originalImage)//seher gri dönüþüm
        {
            Bitmap grayImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int x = 0; x < originalImage.Width; x++)
            {
                for (int y = 0; y < originalImage.Height; y++)
                {
                    Color pixelColor = originalImage.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    Color grayColor = Color.FromArgb(grayValue, grayValue, grayValue);
                    grayImage.SetPixel(x, y, grayColor);
                }
            }

            return grayImage;
        }

        private Bitmap PrewittEdgeDetection(Bitmap grayImage)//ali prewitt
        {
            Bitmap edgesImage = new Bitmap(grayImage.Width, grayImage.Height);

            int[,] maskX = { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
            int[,] maskY = { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };

            for (int x = 1; x < grayImage.Width - 1; x++)
            {
                for (int y = 1; y < grayImage.Height - 1; y++)
                {
                    int sumX = 0, sumY = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Color pixelColor = grayImage.GetPixel(x + i, y + j);
                            int grayValue = pixelColor.R;

                            sumX += grayValue * maskX[i + 1, j + 1];
                            sumY += grayValue * maskY[i + 1, j + 1];
                        }
                    }

                    int edgeValue = (int)Math.Sqrt(sumX * sumX + sumY * sumY);
                    edgeValue = Math.Min(255, edgeValue);
                    Color edgeColor = Color.FromArgb(edgeValue, edgeValue, edgeValue);
                    edgesImage.SetPixel(x, y, edgeColor);
                }
            }

            return edgesImage;
        }

        private void button24_Click(object sender, EventArgs e)//ali prewitt
        {


            if (pictureBox1.Image != null)
            {
                Bitmap originalImage = (Bitmap)pictureBox1.Image;


                Bitmap grayImage = ToGrayscale(originalImage);


                Bitmap edgesImage = PrewittEdgeDetection(grayImage);


                pictureBox3.Image = edgesImage;
                UpdateLabelVisibility();
                label6.Text = "Prewitt Edge Finding";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button26_Click(object sender, EventArgs e)//seher gri dönüþüm
        {

            if (pictureBox1.Image != null)
            {
                Bitmap originalImage = (Bitmap)pictureBox1.Image;
                Bitmap grayImage = ToGrayscale(originalImage);

                pictureBox2.Image = grayImage;
                UpdateLabelVisibility();
                label5.Text = "Grey Transformation";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Bitmap ApplyThresholding(Bitmap originalImage, int thresholdValue)//ali eþikleme
        {
            Bitmap resultImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int x = 0; x < originalImage.Width; x++)
            {
                for (int y = 0; y < originalImage.Height; y++)
                {
                    Color pixelColor = originalImage.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);


                    if (grayValue >= thresholdValue)
                    {
                        resultImage.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return resultImage;
        }
        private void button25_Click(object sender, EventArgs e)//ali eþikleme
        {

            if (pictureBox1.Image != null)
            {
                Bitmap originalImage = (Bitmap)pictureBox1.Image;


                Bitmap thresholdedImage = ApplyThresholding(originalImage, 125);


                pictureBox3.Image = thresholdedImage;
                UpdateLabelVisibility();
                label6.Text = "Thresholding";
            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static class ColorConversion//seher renk uzay
        {
            public static Bitmap RgbToHsi(Bitmap originalImage)
            {
                Bitmap hsiImage = new Bitmap(originalImage.Width, originalImage.Height);
                for (int y = 0; y < originalImage.Height; y++)
                {
                    for (int x = 0; x < originalImage.Width; x++)
                    {
                        Color rgbColor = originalImage.GetPixel(x, y);
                        double r = rgbColor.R / 255.0;
                        double g = rgbColor.G / 255.0;
                        double b = rgbColor.B / 255.0;

                        double intensity = (r + g + b) / 3.0;

                        double min = Math.Min(Math.Min(r, g), b);
                        double saturation = (intensity == 0) ? 0 : 1 - (min / intensity);

                        double hue = 0;
                        if (saturation != 0)
                        {
                            double num = 0.5 * ((r - g) + (r - b));
                            double den = Math.Sqrt((r - g) * (r - g) + (r - b) * (g - b));
                            hue = Math.Acos(Math.Max(-1, Math.Min(1, num / den)));
                            if (b > g)
                            {
                                hue = 2 * Math.PI - hue;
                            }
                        }

                        int h = (int)(hue * 180 / Math.PI);
                        int s = (int)(saturation * 255);
                        int i = (int)(intensity * 255);

                        h = Math.Max(0, Math.Min(255, h));
                        s = Math.Max(0, Math.Min(255, s));
                        i = Math.Max(0, Math.Min(255, i));

                        hsiImage.SetPixel(x, y, Color.FromArgb(h, s, i));
                    }
                }
                return hsiImage;
            }

            public static Bitmap HsiToRgb(Bitmap hsiImage)
            {
                Bitmap rgbImage = new Bitmap(hsiImage.Width, hsiImage.Height);
                for (int y = 0; y < hsiImage.Height; y++)
                {
                    for (int x = 0; x < hsiImage.Width; x++)
                    {
                        Color hsiColor = hsiImage.GetPixel(x, y);
                        double h = hsiColor.R * Math.PI / 180;
                        double s = hsiColor.G / 255.0;
                        double i = hsiColor.B / 255.0;

                        double r = 0, g = 0, b = 0;

                        if (h < 2 * Math.PI / 3)
                        {
                            b = i * (1 - s);
                            r = i * (1 + s * Math.Cos(h) / Math.Cos(Math.PI / 3 - h));
                            g = 3 * i - (r + b);
                        }
                        else if (h < 4 * Math.PI / 3)
                        {
                            h -= 2 * Math.PI / 3;
                            r = i * (1 - s);
                            g = i * (1 + s * Math.Cos(h) / Math.Cos(Math.PI / 3 - h));
                            b = 3 * i - (r + g);
                        }
                        else
                        {
                            h -= 4 * Math.PI / 3;
                            g = i * (1 - s);
                            b = i * (1 + s * Math.Cos(h) / Math.Cos(Math.PI / 3 - h));
                            r = 3 * i - (g + b);
                        }

                        int red = Math.Max(0, Math.Min(255, (int)(r * 255)));
                        int green = Math.Max(0, Math.Min(255, (int)(g * 255)));
                        int blue = Math.Max(0, Math.Min(255, (int)(b * 255)));

                        rgbImage.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }
                return rgbImage;
            }

            public static Bitmap RgbToXyz(Bitmap originalImage)
            {
                Bitmap xyzImage = new Bitmap(originalImage.Width, originalImage.Height);
                for (int y = 0; y < originalImage.Height; y++)
                {
                    for (int x = 0; x < originalImage.Width; x++)
                    {
                        Color rgbColor = originalImage.GetPixel(x, y);
                        double r = rgbColor.R / 255.0;
                        double g = rgbColor.G / 255.0;
                        double b = rgbColor.B / 255.0;

                        double xVal = r * 0.4124 + g * 0.3576 + b * 0.1805;
                        double yVal = r * 0.2126 + g * 0.7152 + b * 0.0722;
                        double zVal = r * 0.0193 + g * 0.1192 + b * 0.9505;


                        int xInt = (int)(Math.Max(0, Math.Min(1, xVal)) * 255);
                        int yInt = (int)(Math.Max(0, Math.Min(1, yVal)) * 255);
                        int zInt = (int)(Math.Max(0, Math.Min(1, zVal)) * 255);

                        xyzImage.SetPixel(x, y, Color.FromArgb(xInt, yInt, zInt));
                    }
                }
                return xyzImage;
            }
            public static Bitmap XyzToRgb(Bitmap xyzImage)
            {
                Bitmap rgbImage = new Bitmap(xyzImage.Width, xyzImage.Height);
                for (int y = 0; y < xyzImage.Height; y++)
                {
                    for (int x = 0; x < xyzImage.Width; x++)
                    {
                        Color xyzColor = xyzImage.GetPixel(x, y);
                        double X = xyzColor.R / 255.0;
                        double Y = xyzColor.G / 255.0;
                        double Z = xyzColor.B / 255.0;


                        double[,] xyzToRgbMatrix = new double[,]
                        {
                    {3.2406, -1.5372, -0.4986},
                    {-0.9689, 1.8758, 0.0415},
                    {0.0557, -0.2040, 1.0570}
                        };


                        double R = X * xyzToRgbMatrix[0, 0] + Y * xyzToRgbMatrix[0, 1] + Z * xyzToRgbMatrix[0, 2];
                        double G = X * xyzToRgbMatrix[1, 0] + Y * xyzToRgbMatrix[1, 1] + Z * xyzToRgbMatrix[1, 2];
                        double B = X * xyzToRgbMatrix[2, 0] + Y * xyzToRgbMatrix[2, 1] + Z * xyzToRgbMatrix[2, 2];

                        int RInt = Math.Max(0, Math.Min(255, (int)(R * 255)));
                        int GInt = Math.Max(0, Math.Min(255, (int)(G * 255)));
                        int BInt = Math.Max(0, Math.Min(255, (int)(B * 255)));

                        rgbImage.SetPixel(x, y, Color.FromArgb(RInt, GInt, BInt));
                    }
                }
                return rgbImage;
            }
            public static Bitmap XyzToLuv(Bitmap xyzImage)
            {
                Bitmap luvImage = new Bitmap(xyzImage.Width, xyzImage.Height);
                for (int y = 0; y < xyzImage.Height; y++)
                {
                    for (int x = 0; x < xyzImage.Width; x++)
                    {
                        Color xyzColor = xyzImage.GetPixel(x, y);
                        double xVal = xyzColor.R / 255.0;
                        double yVal = xyzColor.G / 255.0;
                        double zVal = xyzColor.B / 255.0;

                        double uPrime = 4 * xVal / (xVal + 15 * yVal + 3 * zVal);
                        double vPrime = 9 * yVal / (xVal + 15 * yVal + 3 * zVal);

                        double L = yVal > 0.008856 ? 116 * Math.Pow(yVal, 1.0 / 3) - 16 : 903.3 * yVal;
                        double u = 13 * L * (uPrime - 0.2009);
                        double v = 13 * L * (vPrime - 0.4610);


                        int LInt = Math.Max(0, Math.Min(255, (int)L));
                        int uInt = Math.Max(0, Math.Min(255, (int)(u + 134)));
                        int vInt = Math.Max(0, Math.Min(255, (int)(v + 140)));

                        luvImage.SetPixel(x, y, Color.FromArgb(LInt, uInt, vInt));
                    }
                }
                return luvImage;
            }

            public static Bitmap LuvToXyz(Bitmap luvImage)
            {
                Bitmap xyzImage = new Bitmap(luvImage.Width, luvImage.Height);
                for (int y = 0; y < luvImage.Height; y++)
                {
                    for (int x = 0; x < luvImage.Width; x++)
                    {
                        Color luvColor = luvImage.GetPixel(x, y);
                        double L = luvColor.R;
                        double u = (luvColor.G - 128) / 255.0 * 255;
                        double v = (luvColor.B - 128) / 255.0 * 255;

                        double Y;
                        if (L > 7.9996)
                        {
                            Y = Math.Pow((L + 16) / 116, 3);
                        }
                        else
                        {
                            Y = L / 903.3;
                        }


                        double X = 0, Z = 0;
                        if (L > 0)
                        {
                            double a = (1.0 / 3.0) * ((52.0 * L) / (u + 13.0 * L * 0.197849) - 1.0);
                            double b = -5 * Y;
                            double c = -1.0 / 3.0;
                            double d = Y * (((39.0 * L) / (v + 13.0 * L * 0.468351)) - 5.0);

                            X = (d - b) / (a - c);
                            Z = X * a + b;
                        }


                        X = Math.Max(0, Math.Min(1, X));
                        Y = Math.Max(0, Math.Min(1, Y));
                        Z = Math.Max(0, Math.Min(1, Z));


                        int xInt = (int)(X * 255);
                        int yInt = (int)(Y * 255);
                        int zInt = (int)(Z * 255);


                        xInt = Math.Max(0, Math.Min(255, xInt));
                        yInt = Math.Max(0, Math.Min(255, yInt));
                        zInt = Math.Max(0, Math.Min(255, zInt));

                        xyzImage.SetPixel(x, y, Color.FromArgb(xInt, yInt, zInt));
                    }
                }
                return xyzImage;
            }


            public static Bitmap GrayToRgb(Bitmap grayImage)
            {
                Bitmap rgbImage = new Bitmap(grayImage.Width, grayImage.Height);
                for (int y = 0; y < grayImage.Height; y++)
                {
                    for (int x = 0; x < grayImage.Width; x++)
                    {
                        Color grayColor = grayImage.GetPixel(x, y);
                        int grayValue = grayColor.R; // Gri tonlamalý bir görüntü olduðunu varsayýyoruz.

                        // Gri tonlamayý kullanarak RGB bileþenlerini ayarlama
                        rgbImage.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                    }
                }
                return rgbImage;
            }


            public static Bitmap RgbToHsv(Bitmap rgbImage)
            {
                Bitmap hsvImage = new Bitmap(rgbImage.Width, rgbImage.Height);
                for (int y = 0; y < rgbImage.Height; y++)
                {
                    for (int x = 0; x < rgbImage.Width; x++)
                    {
                        Color rgbColor = rgbImage.GetPixel(x, y);
                        double r = rgbColor.R / 255.0;
                        double g = rgbColor.G / 255.0;
                        double b = rgbColor.B / 255.0;

                        double max = Math.Max(r, Math.Max(g, b));
                        double min = Math.Min(r, Math.Min(g, b));
                        double delta = max - min;

                        double h = 0;
                        if (delta != 0)
                        {
                            if (max == r)
                            {
                                h = (g - b) / delta + (g < b ? 6 : 0);
                            }
                            else if (max == g)
                            {
                                h = (b - r) / delta + 2;
                            }
                            else if (max == b)
                            {
                                h = (r - g) / delta + 4;
                            }
                            h /= 6;
                        }

                        double s = max == 0 ? 0 : delta / max;
                        double v = max;

                        hsvImage.SetPixel(x, y, Color.FromArgb((int)(h * 255), (int)(s * 255), (int)(v * 255)));
                    }
                }
                return hsvImage;
            }
        }




        private void button27_Click(object sender, EventArgs e)//seher rgb to hsi
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            Bitmap hsiImage = ColorConversion.RgbToHsi(originalImage);


            pictureBox2.Image = hsiImage;
            UpdateLabelVisibility();
            label4.Text = "RGB";
            label5.Text = "HSI";
        }

        private void button28_Click(object sender, EventArgs e)//seher hsi to rgb
        {
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("Please upload HSI image or perform RGB to HSI conversion in box 2.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Bitmap originalImage = new Bitmap(pictureBox2.Image);
            Bitmap rgbImage = ColorConversion.HsiToRgb(originalImage);


            pictureBox3.Image = rgbImage;
            UpdateLabelVisibility();

            label6.Text = "RGB";
        }

        private void button29_Click(object sender, EventArgs e)//seher rgb to xyz
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            Bitmap xyzimage = ColorConversion.RgbToXyz(originalImage);


            pictureBox2.Image = xyzimage;
            UpdateLabelVisibility();
            label4.Text = "RGB";
            label5.Text = "XYZ";
        }

        private void button30_Click(object sender, EventArgs e)//seher xyz to luv
        {
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("Please upload XYZ image to box 2 or perform RGB - XYZ conversion.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Bitmap originalImage = new Bitmap(pictureBox2.Image);
            Bitmap luvimage = ColorConversion.XyzToLuv(originalImage);


            pictureBox3.Image = luvimage;
            UpdateLabelVisibility();

            label6.Text = "LUV";
        }

        private void button32_Click(object sender, EventArgs e)//seher xyz to rgb
        {
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("Please upload XYZ image or perform RGB - XYZ conversion in box 2.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Bitmap originalImage = new Bitmap(pictureBox2.Image);
            Bitmap rgbimage = ColorConversion.XyzToRgb(originalImage);


            pictureBox3.Image = rgbimage;
            UpdateLabelVisibility();

            label6.Text = "RGB";

        }

        private void button31_Click(object sender, EventArgs e)//seher luv to xyz
        {



            if (pictureBox3.Image == null)
            {
                MessageBox.Show("Lütfen önce bir görsel yükleyin.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Bitmap originalImage = new Bitmap(pictureBox3.Image);
            Bitmap xyzimage = ColorConversion.LuvToXyz(originalImage);


            pictureBox3.Image = xyzimage;
            UpdateLabelVisibility();

            label6.Text = "XYZ";
        }

        private Bitmap ApplyColormap(Bitmap grayscaleImage)//seher gri to rgb
        {
            Bitmap coloredImage = new Bitmap(grayscaleImage.Width, grayscaleImage.Height);

            for (int y = 0; y < grayscaleImage.Height; y++)
            {
                for (int x = 0; x < grayscaleImage.Width; x++)
                {
                    Color originalColor = grayscaleImage.GetPixel(x, y);
                    int intensity = originalColor.R;

                    Color newColor = GetColormapColor(intensity);
                    coloredImage.SetPixel(x, y, newColor);
                }
            }

            return coloredImage;
        }

        private Color GetColormapColor(int intensity)//seher gri to rgb
        {

            int r = 0, g = 0, b = 0;

            if (intensity < 128)
            {
                r = 0;
                g = (int)(255 * (intensity / 127.0));
                b = 255;
            }
            else
            {
                r = (int)(255 * ((intensity - 128) / 127.0));
                g = 255 - r;
                b = 255 - r;
            }

            return Color.FromArgb(r, g, b);
        }


        private void button33_Click(object sender, EventArgs e)//seher gray to rgb
        {
            if (pictureBox2.Image != null)
            {
                Bitmap grayscaleImage = new Bitmap(pictureBox2.Image);
                Bitmap coloredImage = ApplyColormap(grayscaleImage);
                pictureBox3.Image = coloredImage;
                label6.Text = "Renklendirilmiþ Görsel";

                UpdateLabelVisibility();
            }
            else if (pictureBox1.Image != null)
            {
                Bitmap grayscaleImage = new Bitmap(pictureBox1.Image);
                Bitmap coloredImage = ApplyColormap(grayscaleImage);
                pictureBox3.Image = coloredImage;
                label6.Text = "Renklendirilmiþ Görsel";
                UpdateLabelVisibility();
            }
            else
            {
                MessageBox.Show("Please upload image 1 first or do Gray Transform.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



        }
        private void button34_Click(object sender, EventArgs e)//seher rgb to hsv
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            Bitmap hsvimage = ColorConversion.RgbToHsv(originalImage);


            pictureBox2.Image = hsvimage;
            UpdateLabelVisibility();
            label4.Text = "RGB";
            label5.Text = "HSV";
        }

        private void Crop(PictureBox sourcePictureBox, PictureBox targetPictureBox, double ratioWidth, double ratioHeight)//seher crop
        {
            if (sourcePictureBox.Image != null)
            {
                Bitmap originalImage = new Bitmap(sourcePictureBox.Image);


                int targetWidth, targetHeight;
                if (originalImage.Width / ratioWidth > originalImage.Height / ratioHeight)
                {

                    targetHeight = originalImage.Height;
                    targetWidth = (int)(originalImage.Height * ratioWidth / ratioHeight);
                }
                else
                {

                    targetWidth = originalImage.Width;
                    targetHeight = (int)(originalImage.Width * ratioHeight / ratioWidth);
                }


                Rectangle cropRect = new Rectangle(
                    (originalImage.Width - targetWidth) / 2,
                    (originalImage.Height - targetHeight) / 2,
                    targetWidth,
                    targetHeight
                );


                Bitmap croppedImage = new Bitmap(targetWidth, targetHeight);
                using (Graphics g = Graphics.FromImage(croppedImage))
                {
                    g.DrawImage(originalImage, new Rectangle(0, 0, targetWidth, targetHeight), cropRect, GraphicsUnit.Pixel);
                }


                Bitmap finalImage = new Bitmap(targetPictureBox.Width, targetPictureBox.Height);
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    g.Clear(Color.Transparent);
                    int x = (targetPictureBox.Width - targetWidth) / 2;
                    int y = (targetPictureBox.Height - targetHeight) / 2;
                    g.DrawImage(croppedImage, x, y, targetWidth, targetHeight);
                }


                targetPictureBox.Image = finalImage;
                label5.Text = "Cropped Image";
                UpdateLabelVisibility();

            }
            else
            {
                MessageBox.Show("Please upload image 1 first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void button35_Click(object sender, EventArgs e)//seher crop 4:3
        {

            Crop(pictureBox1, pictureBox2, 4.0, 3.0);



        }

        private void button37_Click(object sender, EventArgs e)//seher crop 16:9
        {

            Crop(pictureBox1, pictureBox2, 16.0, 9.0);

        }

        private void button38_Click(object sender, EventArgs e)//seher crop 3:2
        {

            Crop(pictureBox1, pictureBox2, 3.0, 2.0);

        }

        private void button39_Click(object sender, EventArgs e)//seher crop 1:1
        {

            Crop(pictureBox1, pictureBox2, 1.0, 1.0);

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}