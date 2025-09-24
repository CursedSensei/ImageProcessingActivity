using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using WebCamLib;

namespace WindowsCamera
{
    class Camera
    {
        private VideoCapture device;
        private Mat frame;
        private int newWidth = 0, newHeight = 0, index;

        public Camera(int index, int width, int height)
        {
            this.index = index;
            device = new VideoCapture(index);

            if (!device.IsOpened())
            {
                throw new Exception("Cannot open camera!");
            }

            device.Set(VideoCaptureProperties.FrameWidth, width);
            device.Set(VideoCaptureProperties.FrameHeight, height);

            frame = new Mat();
        }

        ~Camera()
        {
            this.Close();
        }

        public void resize(int width, int height)
        {
            newWidth = width;
            newHeight = height;
        }

        public Bitmap GetBitmap()
        {
            if (newWidth != 0 || newHeight != 0)
            {
                device = new VideoCapture(index);
                device.Set(VideoCaptureProperties.FrameWidth, newWidth);
                device.Set(VideoCaptureProperties.FrameHeight, newHeight);
                frame = new Mat();
                newWidth = 0;
                newHeight = 0;
            }

            if (device.IsOpened())
            {
                device.Read(frame);
            }

            if (frame.Empty())
            {
                return new Bitmap(device.FrameWidth, device.FrameHeight);
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