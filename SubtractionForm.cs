using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageProcessingActivity
{
    public partial class SubtractionForm : Form
    {
        private Bitmap imageA, imageB;

        public SubtractionForm()
        {
            InitializeComponent();
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
            }
        }

        private void buttonSubtract_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = new Bitmap(imageB.Width, imageB.Height);

            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
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
    }
}
