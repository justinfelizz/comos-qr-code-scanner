using System.Windows.Controls;

namespace COMOSQR_CodeScanner.View
{
    /// <summary>
    /// Interaktionslogik für DocPage.xaml
    /// </summary>
    public partial class DocPage : Page
    {
        public DocPage()
        {
            var vm = new ViewModel.DocViewModel();
            vm.Page = this;
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
