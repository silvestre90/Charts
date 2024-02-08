using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Spectrogram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Width = 800;
        private const int Height = 400;
        private const int NumBins = 256; // Number of frequency bins
        private const int NumFrames = 50; // Number of time frames
        private byte[,] spectrogramData; // Intensity values for each bin/frame
        private DispatcherTimer timer;
        private int currentFrame = 0;
        private WriteableBitmap writeableBitmap;
        public MainWindow()
        {
            InitializeComponent();
            GenerateSpectrogramData();
            InitializeWriteableBitmap();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); // Adjust the interval as needed
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Simulate receiving new data and updating the spectrogram
            byte[,] newData = GenerateNewData();
            UpdateSpectrogram(newData);
        }

        private void InitializeWriteableBitmap()
        {
            writeableBitmap = new WriteableBitmap(NumFrames, NumBins, 96, 96, PixelFormats.Gray8, null);
            spectrogramImage.Source = writeableBitmap;
        }

        private void GenerateSpectrogramData()
        {
            // Example: Generate random spectrogram data
            Random random = new Random();
            spectrogramData = new byte[NumBins, NumFrames];

            for (int bin = 0; bin < NumBins; bin++)
            {
                for (int frame = 0; frame < NumFrames; frame++)
                {
                    spectrogramData[bin, frame] = (byte)random.Next(0, 255);
                }
            }
        }

        private byte[,] GenerateNewData()
        {
            // Example: Generate new random data for each frame
            Random random = new Random();
            byte[,] newData = new byte[NumBins, 1];

            for (int bin = 0; bin < NumBins; bin++)
            {
                newData[bin, 0] = (byte)random.Next(0, 255);
            }

            return newData;
        }

        //private void UpdateSpectrogram(byte[,] newData)
        //{
        //    for (int bin = 0; bin < NumBins; bin++)
        //    {
        //        byte intensity = newData[bin, 0];
        //        writeableBitmap.WritePixels(new Int32Rect(currentFrame, bin, 1, 1), new byte[] { intensity }, 1, 0);
        //    }

        //    currentFrame = (currentFrame + 1) % NumFrames;
        //}

        private void UpdateSpectrogram(byte[,] newData)
        {
            // Shift existing data to make room for new data
            if(currentFrame >= NumFrames - 1)
            {
                for (int frame = 0; frame < NumFrames - 1; frame++)
                {
                    for (int bin = 0; bin < NumBins; bin++)
                    {
                        spectrogramData[bin, frame] = spectrogramData[bin, frame + 1];
                    }
                }

                // Copy new data to the last frame
                for (int bin = 0; bin < NumBins; bin++)
                {
                    spectrogramData[bin, NumFrames - 1] = newData[bin, 0];
                }

            }


            // Update the WriteableBitmap
            if (currentFrame < NumFrames - 1)
            {
                for (int bin = 0; bin < NumBins; bin++)
                {
                    byte intensity = spectrogramData[bin, currentFrame];
                    writeableBitmap.WritePixels(new Int32Rect(currentFrame, bin, 1, 1), new byte[] { intensity }, 1, 0);
                }

            }
            else
            {
                // Shift existing data to the left
                //for (int frame = 0; frame < NumFrames - 1; frame++)
                //{
                //Int32Rect sourceRect = new Int32Rect(1, 0, NumFrames -1, NumBins);
                //Int32Rect destRect = new Int32Rect(0, 0, NumFrames -1, NumBins);

                //// Create an intermediate buffer
                //var bufferSize = sourceRect.Width * sourceRect.Height;
                //byte[] buffer = new byte[bufferSize];
                //writeableBitmap.CopyPixels(sourceRect, buffer, bufferSize, 0);

                //writeableBitmap.WritePixels(destRect, buffer, bufferSize, 0);
                ////}

                //// Copy new data to the last frame
                //for (int bin = 0; bin < NumBins; bin++)
                //{
                //    spectrogramData[bin, NumFrames - 1] = newData[bin, 0];
                //}

                //// Update the last frame with the new data
                //Int32Rect newFrameRect = new Int32Rect(NumFrames - 1, 0, 1, NumBins);
                //for (int bin = 0; bin < NumBins; bin++)
                //{
                //    byte intensity = spectrogramData[bin, NumFrames - 1];
                //    writeableBitmap.WritePixels(newFrameRect, new byte[] { intensity }, 1, 0);
                //}

                for (int frame = 0; frame < NumFrames; frame++)
                {
                    for (int bin = 0; bin < NumBins; bin++)
                    {
                        byte intensity = spectrogramData[bin, frame];
                        writeableBitmap.WritePixels(new Int32Rect(frame, bin, 1, 1), new byte[] { intensity }, 1, 0);
                    }
                }
            }


            if (currentFrame >= NumFrames -1)
            {
                currentFrame = NumFrames -1;
            }
            else
            {
                currentFrame = (currentFrame + 1);
            }
        }
    }
}