using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using WebCamLib;
using System.Threading;

namespace WindowsCamera
{
    class Camera
    {
        private VideoCapture device;
        private Mat frame;
        private int width, height;

        public Camera(int index, int width, int height)
        {
            device = new VideoCapture(index);

            if (!device.IsOpened())
            {
                throw new Exception("Cannot open camera!");
            }

            resize(width, height);

            frame = new Mat();
        }

        ~Camera()
        {
            this.Close();
        }

        public void resize(int width, int height)
        {
            if (width % 2 != 0)
            {
                width--;
            }

            int remainder = height % 3;
            if (remainder != 0)
            {
                height -= remainder;
            }

            lock (this)
            {
                device.Set(VideoCaptureProperties.FrameWidth, width);
                device.Set(VideoCaptureProperties.FrameHeight, height);

                this.width = width;
                this.height = height;
            }
        }

        public Bitmap GetBitmap()
        {
            lock (this)
            {
                if (device.IsOpened())
                {
                    device.Read(frame);
                }

                if (frame.Empty())
                {
                    return new Bitmap(width, height);
                }
            }

            return frame.ToBitmap();
        }

        public void Close()
        {
            device.Dispose();
            frame.Dispose();
        }

        public static List<string> GetWindowsDeviceNames()
        {
            List<string> deviceNames = new List<string>();
            
            foreach (Device i in DeviceManager.GetAllDevices())
            {
                deviceNames.Add(i.Name);
            }

            return deviceNames;
        }
    }
}