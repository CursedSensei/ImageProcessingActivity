using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessingActivity
{
    public partial class CameraDialog : Form
    {
        List<string> devices;
        public int index { get; private set; } = -1;

        public CameraDialog(List<string> devices)
        {
            InitializeComponent();

            this.devices = devices;

            devicesBox.DataSource = devices;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            index = devicesBox.SelectedIndex;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            index = -1;
            Close();
        }
    }
}
