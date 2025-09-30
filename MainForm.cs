using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsCamera;
using System.Threading;
using Timer = System.Threading.Timer;
using Algorithm = ImageAlgorithm.ImageAlgorithm;
using AlgorithmAction = ImageAlgorithm.ImageAlgorithm.AlgorithmAction;

namespace ImageProcessingActivity
{
    public partial class MainForm : Form
    {
        private AlgorithmAction cameraAction = null;
        private Timer cameraThread;
        private Camera device = null;

        public MainForm()
        {
            InitializeComponent();

            cameraThread = new Timer(new TimerCallback(cameraTimerTick));
        }

        private void Main_Load(object sender, EventArgs e)
        {
            pictureBoxInput.Image = new Bitmap(800, 800);
            pictureBoxOutput.Image = new Bitmap(800, 800);
        }

        private void handleToolStrip(AlgorithmAction action)
        {
            if (device != null)
            {
                cameraAction = action;
                return;
            }

            Bitmap bitmapInput = pictureBoxInput.Image as Bitmap;
            Bitmap bitmapOutput = new Bitmap(bitmapInput.Width, bitmapInput.Height);

            action(bitmapInput, ref bitmapOutput);

            setOutputImage(bitmapOutput);
        }

        private void cameraTimerTick(object state)
        {
            Bitmap frame = device.GetBitmap();

            AlgorithmAction action = cameraAction;

            if (action != null)
            {
                Bitmap output = new Bitmap(frame.Width, frame.Height);
                action(frame, ref output);

                try
                {
                    this.Invoke(new Action(() => {
                        pictureBoxInput.Image.Dispose();
                        pictureBoxInput.Image = frame;
                        setOutputImage(output);
                    }));
                }
                catch (InvalidOperationException)
                {
                    frame.Dispose();
                    output.Dispose();
                }
            }
            else
            {
                try
                {
                    this.Invoke(new Action(() => {
                        pictureBoxInput.Image.Dispose();
                        pictureBoxInput.Image = frame;
                    }));
                }
                catch (InvalidOperationException)
                {
                    frame.Dispose();
                }
            }
        }

        private void loadImageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                stopTimer();

                pictureBoxInput.Image.Dispose();
                pictureBoxInput.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void openCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraDialog dialog = new CameraDialog(Camera.GetWindowsDeviceNames());
            dialog.ShowDialog();

            if (dialog.index == -1)
            {
                return;
            }

            try
            {
                device = new Camera(dialog.index, 800, 800);
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot open camera");
                return;
            }

            cameraAction = null;
            startTimer();
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBoxOutput.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.copy);
        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.greyscale);
        private void inversionToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.inversion);
        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.sepia);

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (device != null)
            {
                cameraAction = Algorithm.histogram;
                return;
            }

            Bitmap bitmapInput = pictureBoxInput.Image as Bitmap;
            Bitmap bitmapOutput = new Bitmap(1, 1);

            Algorithm.histogram(bitmapInput, ref bitmapOutput);

            setOutputImage(bitmapOutput);
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

        private void setOutputImage(Bitmap outputImage)
        {
            pictureBoxOutput.Image.Dispose();
            pictureBoxOutput.Image = outputImage;
        }

        private void startTimer()
        {
            cameraThread.Change(0, 100);
        }

        private void stopTimer()
        {
            if (device == null) return;

            cameraThread.Change(Timeout.Infinite, Timeout.Infinite);
            device.Close();
            device = null;
        }

        private void smoothToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.smooth);
        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.gaussianBlur);
        private void shapenToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.sharpen);
        private void meanRemovalToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.meanRemoval);
        private void laplascianToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.embossLaplascian);
        private void horizontalVerticalToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.embossHorizontalVertical);
        private void allDirectionsToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.embossAllDirections);
        private void lossyToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.embossLossy);
        private void horizontalOnlyToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.embossHorizontal);
        private void verticalOnlyToolStripMenuItem_Click(object sender, EventArgs e) => handleToolStrip(Algorithm.embossVertical);
    }
}
