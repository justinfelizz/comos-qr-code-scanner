using System.Windows.Controls;

namespace COMOSQR_CodeScanner.View
{
    /// <summary>
    /// Interaktionslogik für LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            var vm = new ViewModel.LoginViewModel();
            vm.Page = this;
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
