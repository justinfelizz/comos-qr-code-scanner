using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZXing;

namespace COMOSQR_CodeScanner.ViewModel
{
    internal class ScannerViewModel : BaseViewModel
    {
        // Variablen, die benötigt werden
        private int _selectedIndex;
        private DispatcherTimer _timer = new DispatcherTimer();
        private BitmapImage _image;
        public FilterInfoCollection CameraDevices { get;  set; }
        public List<string> CameraDevicesList { get; set; } = new List<string>();
        public Page Page { get; set; }
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                
                _selectedIndex = value;
                StartScan();
                NotifyPropertyChanged(nameof(SelectedIndex));
            }
        }
        public BitmapImage Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                NotifyPropertyChanged(nameof(Image));
            }
        }
        
        // Konstruktor, führt Methode aus
        public ScannerViewModel()
        {
            LoadCameraDevices();
        }

        private string _feedback;

        public string Feedback
        {
            get
            {
                return _feedback;
            }
            set
            {
                if (_feedback != value)
                {
                    _feedback = value;
                    NotifyPropertyChanged();
                }
            }
        }

        //Konvertierung des Bildes in BitmapImageSource
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        //Verfügbare Kameras in Combobox laden
        private void LoadCameraDevices()
        {
            CameraDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo filterinfo in CameraDevices)
                CameraDevicesList.Add(filterinfo.Name);
            SelectedIndex = 0;
        }

        // Decoding des QR-Codes, wenn einer erkannt wurde.
        private void Timer_Tick(object sender, EventArgs e)
        {
            if(Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((BitmapImage)Image);

                if(result != null)
                {
                    _timer.Stop();
                    string baseQRUID = result.ToString();
                    HTTPBaseViewModel.BaseQRUID = baseQRUID;
                    Uri uri = new Uri("View/DocPage.xaml", UriKind.Relative);
                    Page.NavigationService.Navigate(uri);
                }
            }
            else
            {
                Feedback = "Keine Kameras gefunden!";
            }
        }

        //Neuer frame wird dauerhaft hinzugefügt --> Livefeed
        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Image = BitmapToImageSource(eventArgs.Frame.Clone() as Bitmap);
            });
        }

        //Scanprozess wird gestartet
        private void StartScan()
        {
            if (Global.CaptureDevice == null)
            {
                Global.CaptureDevice = new VideoCaptureDevice(CameraDevices[SelectedIndex].MonikerString);
                Global.CaptureDevice.NewFrame += CaptureDevice_NewFrame;
                Global.CaptureDevice.Start();
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += Timer_Tick;
                _timer.Start();
                Feedback = "Bitte scannen Sie den QR-Code";
            }
        }
    }
}
