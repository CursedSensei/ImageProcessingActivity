using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsCamera;
using System.Threading;
using Timer = System.Threading.Timer;

namespace ImageProcessingActivity
{
    public partial class SubtractionForm : Form
    {
        private Bitmap imageA, imageB;
        private Camera device = null;
        bool processSubtractFlag = false;
        System.Threading.Timer cameraThread;

        public SubtractionForm()
        {
            InitializeComponent();

            cameraThread = new Timer(new TimerCallback(cameraTimer_Tick));
        }

        private void SubtractionForm_Load(object sender, EventArgs e)
        {
            imageA = new Bitmap(800, 800);
            imageB = new Bitmap(800, 800);

            pictureBoxImage.Image = imageB;
            pictureBoxBackground.Image = imageA;
            pictureBoxOutput.Image = new Bitmap(800, 800);
        }

        private void returnToPart1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (device != null)
                {
                    stopTimer();
                    device.Close();
                    device = null;
                }

                imageB = new Bitmap(openFileDialog1.FileName);
                pictureBoxImage.Image = imageB;
            }
        }

        private void buttonBackground_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageA = new Bitmap(openFileDialog1.FileName);
                pictureBoxBackground.Image = imageA;

                if (device != null)
                {
                    device.resize(imageA.Width, imageA.Height);
                }
            }
        }

        private void buttonCamera_Click(object sender, EventArgs e)
        {
            CameraDialog dialog = new CameraDialog(Camera.GetWindowsDeviceNames());
            dialog.ShowDialog();

            if (dialog.index == -1)
            {
                return;
            }

            try
            {
                device = new Camera(dialog.index, imageA.Width, imageA.Height);
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot open camera");
                return;
            }

            imageB = device.GetBitmap();
            pictureBoxImage.Image = imageB;
            processSubtractFlag = false;
            startTimer();
        }

        private void cameraTimer_Tick(object state)
        {
            Bitmap frame = device.GetBitmap();
            imageB = new Bitmap(frame);

            this.Invoke(new Action(() => 
            {
                pictureBoxImage.Image = imageB;
            }));

            if (processSubtractFlag)
            {
                Bitmap oldImage = new Bitmap(imageA);
                int width = Math.Min(oldImage.Width, frame.Width);
                int height = Math.Min(oldImage.Height, frame.Height);
                Bitmap resultImage = new Bitmap(width, height);

                Color mygreen = Color.FromArgb(0, 0, 255);
                int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
                int threshold = 5;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color pixel = frame.GetPixel(x, y);
                        Color backpixel = oldImage.GetPixel(x, y);
                        int grey = (pixel.R + pixel.G + pixel.B) / 3;
                        int subtractvalue = Math.Abs(grey - greygreen);

                        if (subtractvalue > threshold)
                        {
                            resultImage.SetPixel(x, y, backpixel);
                        }
                        else
                        {
                            resultImage.SetPixel(x, y, pixel);
                        }
                    }
                }


                this.Invoke(new Action(() => pictureBoxOutput.Image = resultImage));
            }
        }

        private void buttonSubtract_Click(object sender, EventArgs e)
        {
            if (sender != null && device != null)
            {
                processSubtractFlag = true;
                return;
            }

            int width = Math.Min(imageA.Width, imageB.Width);
            int height = Math.Min(imageA.Height, imageB.Height);
            Bitmap resultImage = new Bitmap(width, height);

            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);

                    if (subtractvalue <= threshold)
                    {
                        resultImage.SetPixel(x, y, backpixel);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, pixel);
                    }
                }
            }

            pictureBoxOutput.Image = resultImage;
        }

        private void startTimer()
        {
            cameraThread.Change(0, 100);
        }

        private void stopTimer()
        {
            cameraThread.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
