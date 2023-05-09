using System.Windows.Controls;

namespace COMOSQR_CodeScanner.View
{
    /// <summary>
    /// Interaktionslogik für ScannerPage.xaml
    /// </summary>
    public partial class ScannerPage : Page
    {
        public ScannerPage()
        {
            var vm = new ViewModel.ScannerViewModel();
            vm.Page = this;
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
