using System.Windows.Controls;

namespace COMOSQR_CodeScanner.View
{
    /// <summary>
    /// Interaktionslogik für ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        public ProjectPage()
        {
            var vm = new ViewModel.ProjectViewModel();
            vm.Page = this;
            this.DataContext = vm;  
            InitializeComponent();
        }
    }
}
