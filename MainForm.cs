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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            pictureBoxInput.Image = new Bitmap(800, 800);
            pictureBoxOutput.Image = new Bitmap(800, 800);
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBoxInput.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBoxOutput.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapInput = pictureBoxInput.Image as Bitmap;
            Bitmap bitmapOutput = new Bitmap(bitmapInput.Width, bitmapInput.Height);

            for (int x = 0; x < bitmapInput.Width; x++)
            {
                for (int y = 0; y < bitmapInput.Height; y++)
                {
                    bitmapOutput.SetPixel(x, y, bitmapInput.GetPixel(x, y));
                }
            }

            pictureBoxOutput.Image = bitmapOutput;
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapInput = pictureBoxInput.Image as Bitmap;
            Bitmap bitmapOutput = new Bitmap(bitmapInput.Width, bitmapInput.Height);

            for (int x = 0; x < bitmapInput.Width; x++)
            {
                for (int y = 0; y < bitmapInput.Height; y++)
                {
                    Color pixel = bitmapInput.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    bitmapOutput.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }
            }

            pictureBoxOutput.Image = bitmapOutput;
        }

        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapInput = pictureBoxInput.Image as Bitmap;
            Bitmap bitmapOutput = new Bitmap(bitmapInput.Width, bitmapInput.Height);

            for (int x = 0; x < bitmapInput.Width; x++)
            {
                for (int y = 0; y < bitmapInput.Height; y++)
                {
                    Color pixel = bitmapInput.GetPixel(x, y);
                    bitmapOutput.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
            }

            pictureBoxOutput.Image = bitmapOutput;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapInput = pictureBoxInput.Image as Bitmap;
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

            Bitmap bitmapOutput = new Bitmap(256, max);
            Color gray = Color.FromArgb(100, 100, 100);

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < ctr[x]; y++)
                {
                    bitmapOutput.SetPixel(x, max - y - 1, gray);
                }
            }

            pictureBoxOutput.Image = bitmapOutput;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapInput = pictureBoxInput.Image as Bitmap;
            Bitmap bitmapOutput = new Bitmap(bitmapInput.Width, bitmapInput.Height);

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

            pictureBoxOutput.Image = bitmapOutput;
        }

        private void subtractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            SubtractionForm nextForm = new SubtractionForm();
            if (nextForm.ShowDialog() == DialogResult.OK)
            {
                this.Show();
            }
            else
            {
                this.Close();
            }
        }
    }
}
