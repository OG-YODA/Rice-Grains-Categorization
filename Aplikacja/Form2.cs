using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplikacja
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
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

        private void CategorizeButton_Click(object sender, EventArgs e)
        {
            double minValue = 100;//постійно змінюється
            double maxValue = 0;//постійно змінюється
            string filename = "param.txt";
            int count = 0;

            if (pictureBox1.Image != null)
            {
                Sort();

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
                pictureBox1.Image = Crop(outputImage);

                if (File.Exists(filename))
                {
                    List<Grain> grains = new List<Grain>();
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] values = line.Split('/');
                            if (values.Length == 4
                                && int.TryParse(values[0], out int number1)
                                && int.TryParse(values[1], out int number2)
                                && double.TryParse(values[2], out double number3))
                            {
                                string type = values[3];
                                Grain grain = new Grain(number1, number2, number3, type);
                                grains.Add(grain);
                                textBox1.AppendText("Обьект " + count + "успешно создан!" + Environment.NewLine);
                                count++;
                            }
                        }
                    }

                    List<TypeValues> types = new List<TypeValues>();
                    Grain first = grains[0];//стартовий об'єкт
                    string pretype = first.getType();//попередній сорт

                    while (grains.Count > 0)//доки список не буде порожній
                    {

                        Grain grain = grains[0];//кожен цикл створюється новий об'єкт типу Grain
                        if (pretype.Equals(grain.getType())) //якщо той самий сорт
                        {
                            textBox1.AppendText("Новый цикл для сорта " + grain.getType() + "." + Environment.NewLine);
                            if (minValue > grain.getCoeff())
                            {
                                minValue = grain.getCoeff();
                                textBox1.AppendText("Добавлен новый минимальный порог: " + minValue
                                    + " для сорта " + grain.getType() + "." + Environment.NewLine);
                            }
                            if (maxValue < grain.getCoeff())
                            {
                                maxValue = grain.getCoeff();
                                textBox1.AppendText("Добавлен новый максимальный порог: " + minValue
                                    + " для сорта " + grain.getType() + "." + Environment.NewLine);
                            }
                        }
                        else//якщо сорт закінчивя
                        {
                            textBox1.AppendText("Конец сорта!" + Environment.NewLine);
                            TypeValues typeValues = new TypeValues(minValue, maxValue, pretype);//утворюється об'єкт з пороговими значеннями для сорту
                            types.Add(typeValues);//об'єкт зберігається в список таких самих об'єктів (порогові значення - сорт)
                            textBox1.AppendText("Сохранение сорта в список. Чистка значений." + Environment.NewLine);
                            pretype = grain.getType();//перехід до нового сорту
                            minValue = 100;//скидування порогу
                            maxValue = 0;//скидування порогу

                            textBox1.AppendText("Обработка нового сорта " + pretype + "." + Environment.NewLine);
                            if (minValue > grain.getCoeff())
                            {
                                minValue = grain.getCoeff();
                                textBox1.AppendText("Добавлен новый минимальный порог: " + minValue
                                    + " для сорта " + grain.getType() + "." + Environment.NewLine);
                            }
                            if (maxValue < grain.getCoeff())
                            {
                                maxValue = grain.getCoeff();
                                textBox1.AppendText("Добавлен новый максимальный порог: " + minValue
                                    + " для сорта " + grain.getType() + "." + Environment.NewLine);
                            }

                        }

                        grains.RemoveAt(0);
                    }
                    textBox1.AppendText("Конец списка! Сохранение последнего сорта: " + pretype + "." + Environment.NewLine);
                    TypeValues last = new TypeValues(minValue, maxValue, pretype);
                    types.Add(last);
                    string str = string.Join(Environment.NewLine, types);
                    textBox1.AppendText("Список содержит: " + str + Environment.NewLine);


                    double sampleCoeff = CountMeasurements();
                    int numberOfSimilarities = 0;
                    TypeValues firstTypeValue = types[0];
                    pretype = firstTypeValue.getFamily();

                    textBox1.AppendText("Обьект с коеффициэнтом: " + CountMeasurements() + Environment.NewLine);
                    while (types.Count > 0)
                    {
                        TypeValues typeValues = types[0];

                        if (pretype.Equals(typeValues.getFamily()))
                        {
                            textBox1.AppendText("Начало сравнивания с сортом: " + pretype + "." + Environment.NewLine);
                            if (CountMeasurements() > typeValues.getMinValue()
                                && CountMeasurements() < typeValues.getMaxValue())
                            {
                                textBox1.AppendText("Найдено сходство с сортом: " + pretype + "." + Environment.NewLine);
                                numberOfSimilarities++;
                            }
                        }
                        else
                        {
                            pretype = typeValues.getFamily();
                            textBox1.AppendText("Начало сравнивания с сортом: " + pretype + "." + Environment.NewLine);
                            if (CountMeasurements() > typeValues.getMinValue()
                                && CountMeasurements() < typeValues.getMaxValue())
                            {
                                textBox1.AppendText("Найдено сходство с сортом: " + pretype + "." + Environment.NewLine);
                                numberOfSimilarities++;
                            }
                        }

                        types.RemoveAt(0);
                    }
                    textBox1.AppendText("Конец сравнивания." + Environment.NewLine);
                    if(numberOfSimilarities == 0)
                    {
                        textBox1.AppendText("Сорт неизвестен." + Environment.NewLine);
                    }

                }
            }
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
            if (pictureBox1.Image != null)
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
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
            return 0;
        }

        public double CountHeight()
        {
            if (pictureBox1.Image != null)
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
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
            return 0;
        }

        public double CountMeasurements()
        {
            if (pictureBox1.Image != null)
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
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
            return 0;
        }

        public void Sort()
        {
            string filePath = "param.txt";

            // Читаем все строки из файла
            string[] lines = File.ReadAllLines(filePath);

            // Создаем список для хранения строк
            List<string> sortedLines = new List<string>(lines);

            // Сортируем список по слову
            sortedLines.Sort((a, b) =>
            {
                string[] partsA = a.Split('/');
                string[] partsB = b.Split('/');

                // Получаем последнюю часть строки (слово)
                string wordA = partsA[partsA.Length - 1];
                string wordB = partsB[partsB.Length - 1];

                // Сортируем по алфавиту
                return wordA.CompareTo(wordB);
            });

            // Путь к файлу для сохранения отсортированных строк
            string sortedFilePath = "param.txt";

            // Записываем отсортированные строки в новый файл
            File.WriteAllLines(sortedFilePath, sortedLines);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sort();
        }
    }
}
