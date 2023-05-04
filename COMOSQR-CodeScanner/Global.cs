using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMOSQR_CodeScanner
{
    public class Global
    {
        // Globale Variable, verhindert dauerhafte Instanziierung im ScannerViewModel
        public static VideoCaptureDevice CaptureDevice { get; set; }
    }
}
