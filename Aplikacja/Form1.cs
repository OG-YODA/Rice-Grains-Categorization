using System.Windows.Forms;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using AForge.Math;

namespace Aplikacja
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();


            openFile.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
            openFile.FilterIndex = 1;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(openFile.FileName);
                }
                catch
                {
                    MessageBox.Show("Unable to open file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TransformButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1 != null)
            {

                Bitmap inputImage = new Bitmap(pictureBox1.Image);
                int threshold = OtsuThreshold(inputImage);

                Bitmap outputImage = new Bitmap(inputImage.Width, inputImage.Height);
                for (int y = 0; y < inputImage.Height; y++)
                {
                    for (int x = 0; x < inputImage.Width; x++)
                    {
                        Color pixelColor = inputImage.GetPixel(x, y);
                        int intensity = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                        if (intensity < threshold)
                        {
                            outputImage.SetPixel(x, y, Color.Black);
                        }
                        else
                        {
                            outputImage.SetPixel(x, y, Color.White);
                        }
                    }
                }
                //pictureBox2.Image = outputImage;
                //Rotate(outputImage);
                pictureBox2.Image = Crop(outputImage);

            }
        }

        private void SaveToFile_Click(object sender, EventArgs e)
        {
            string filename = "param.txt";
            if (textBox1 != null)
            {
                WriteDataToFile(filename, CountWidth(), CountHeight(), CountMeasurements(), textBox1.Text);
            }
            else WriteDataToFile(filename, CountWidth(), CountHeight(), CountMeasurements(), "None");

        }

        /*public void ToBinArray()
        {
            Bitmap input = new Bitmap(pictureBox2.Image);
            int[,] arr = new int[input.Width, input.Height];
            for (int i = 0; i < input.Width; i++)
            {
                for (int j = 0; j < input.Height; j++)
                {
                    Color c = input.GetPixel(i, j);
                    int brightness = (c.R + c.G + c.B) / 3;
                    arr[i, j] = brightness < 128 ? 1 : 0; // 1 for black, 0 for white
                }
            }

            using (StreamWriter sw = new StreamWriter("o.txt"))
            {
                for (int y = 0; y < input.Height; y++)
                {
                    for (int x = 0; x < input.Width; x++)
                    {
                        sw.Write(arr[x, y]);
                    }
                    sw.WriteLine();
                }
            }
        }*/

        public double Compare()
        {
            return 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public static int OtsuThreshold(Bitmap bmp)
        {
            int[] hist = new int[256];
            double[] prob = new double[256];
            double[] varBetween = new double[256];

            int totalPixels = bmp.Width * bmp.Height;
            byte[] pixels = GetGrayscalePixels(bmp);

            // Calculate histogram
            for (int i = 0; i < totalPixels; i++)
            {
                hist[pixels[i]]++;
            }

            // Calculate probability of each intensity level
            for (int i = 0; i < 256; i++)
            {
                prob[i] = (double)hist[i] / totalPixels;
            }

            // Calculate variance between classes for each threshold
            for (int t = 0; t < 256; t++)
            {
                double w1 = 0, w2 = 0, u1 = 0, u2 = 0;
                for (int i = 0; i < t; i++)
                {
                    w1 += prob[i];
                    u1 += i * prob[i];
                }
                for (int i = t; i < 256; i++)
                {
                    w2 += prob[i];
                    u2 += i * prob[i];
                }

                if (w1 == 0 || w2 == 0)
                {
                    varBetween[t] = 0;
                }
                else
                {
                    u1 /= w1;
                    u2 /= w2;
                    varBetween[t] = w1 * w2 * Math.Pow(u1 - u2, 2);
                }
            }

            // Find threshold with maximum variance between classes
            int maxIndex = 0;
            double maxVar = varBetween[0];
            for (int t = 1; t < 256; t++)
            {
                if (varBetween[t] > maxVar)
                {
                    maxIndex = t;
                    maxVar = varBetween[t];
                }
            }

            return maxIndex;
        }

        private static byte[] GetGrayscalePixels(Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixels = new byte[bmpData.Stride * bmp.Height];

            Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);

            bmp.UnlockBits(bmpData);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                byte gray = (byte)(0.299 * pixels[i + 2] + 0.587 * pixels[i + 1] + 0.114 * pixels[i]);
                pixels[i] = pixels[i + 1] = pixels[i + 2] = gray;
            }

            return pixels;
        }

        public static Bitmap Crop(Bitmap image)
        {
            // Создание объекта для обнаружения контуров
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.ProcessImage(image);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            if (blobs.Length > 0)
            {
                // Поиск наибольшего контура
                int maxAreaIndex = 0;
                for (int i = 1; i < blobs.Length; i++)
                {
                    if (blobs[i].Area > blobs[maxAreaIndex].Area)
                        maxAreaIndex = i;
                }

                // Получение прямоугольной области, охватывающей контур
                Rectangle objectRect = blobs[maxAreaIndex].Rectangle;

                // Обрезка изображения до размеров объекта
                Bitmap croppedImage = image.Clone(objectRect, image.PixelFormat);

                // Сохранение обрезанного изображения
                //croppedImage.Save("cropped_image.png");

                return croppedImage;
            }
            return null;
        }

        public double CountWidth()
        {
            Bitmap input = new Bitmap(pictureBox2.Image);
            // Создание объекта BlobCounter для подсчета объектов на изображении
            var blobCounter = new BlobCounter();
            blobCounter.ProcessImage(input);
            var blobs = blobCounter.GetObjectsInformation();
            double width = 0;
            // Получение ширины и длины первого найденного объекта
            if (blobs.Length > 0)
            {
                width = blobs[0].Rectangle.Width;
            }
            return width;
        }

        public double CountHeight()
        {
            Bitmap input = new Bitmap(pictureBox2.Image);
            // Создание объекта BlobCounter для подсчета объектов на изображении
            var blobCounter = new BlobCounter();
            blobCounter.ProcessImage(input);
            var blobs = blobCounter.GetObjectsInformation();
            double height = 0;
            // Получение ширины и длины первого найденного объекта
            if (blobs.Length > 0)
            {
                height = blobs[0].Rectangle.Height;
            }
            return height;
        }

        public double CountMeasurements()
        {
            Bitmap input = new Bitmap(pictureBox2.Image);
            // Создание объекта BlobCounter для подсчета объектов на изображении
            var blobCounter = new BlobCounter();
            blobCounter.ProcessImage(input);
            var blobs = blobCounter.GetObjectsInformation();
            double coeff = 0;
            // Получение ширины и длины первого найденного объекта
            if (blobs.Length > 0)
            {
                double width = blobs[0].Rectangle.Width;
                double height = blobs[0].Rectangle.Height;
                coeff = width / height;
            }
            return coeff;
        }

        public void WriteDataToFile(string filePath, double width, double height, double coeff, string word)
        {
            // Открываем файл для добавления данных в конец
            using (StreamWriter writer = File.AppendText(filePath))
            {
                // Записываем число и слово, разделенные пробелом
                writer.WriteLine(width + "/" + height + "/" + coeff + "/" + word);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void categorizeGrainsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }

        /*public void Rotate(Bitmap bmp)
        {
            Bitmap input = new Bitmap(bmp);
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.ProcessImage(input);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // Нахождение наибольшего контура
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            foreach (Blob blob in blobs)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blob);
                List<IntPoint> cornerPoints;

                // Проверка, является ли контур прямоугольником
                if (shapeChecker.IsQuadrilateral(edgePoints, out cornerPoints))
                {
                    // Проверка, является ли контур горизонтальным
                    if (IsHorizontalRectangle(cornerPoints))
                    {
                        // Вычисление центра контура
                        AForge.Point center = ComputeCenter(cornerPoints);

                        // Вычисление угла между центром контура и одной из вершин
                        double angle = Math.Atan2(center.Y - cornerPoints[0].Y, center.X - cornerPoints[0].X) * 180.0 / Math.PI;

                        // Поворот изображения на противоположный угол
                        RotateBilinear rotationFilter = new RotateBilinear(-angle, true);
                        Bitmap rotatedImage = rotationFilter.Apply(input);

                        // Сохранение повернутого изображения
                        pictureBox2.Image = rotatedImage;
                        break;
                    }
                }
            }

            // Проверка, является ли контур прямоугольником и горизонтальным
            bool IsHorizontalRectangle(List<IntPoint> points)
            {
                double angleThreshold = 10.0; // Пороговое значение для горизонтальности

                // Вычисление угла между первой и второй вершиной
                double angle = Math.Atan2(points[1].Y - points[0].Y, points[1].X - points[0].X) * 180.0 / Math.PI;

                // Проверка, близок ли угол к 0 градусам (горизонтальному положению)
                return Math.Abs(angle) < angleThreshold;
            }

            // Вычисление центра контура
            AForge.Point ComputeCenter(List<IntPoint> points)
            {
                int totalX = 0;
                int totalY = 0;

                foreach (IntPoint point in points)
                {
                    totalX += point.X;
                    totalY += point.Y;
                }

                int centerX = totalX / points.Count;
                int centerY = totalY / points.Count;

                return new AForge.Point(centerX, centerY);
            }

        }*/
    }
}