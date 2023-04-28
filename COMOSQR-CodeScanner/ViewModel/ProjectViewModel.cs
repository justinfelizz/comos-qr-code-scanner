using COMOSQR_CodeScanner.ComosAPI;
using COMOSQR_CodeScanner.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace COMOSQR_CodeScanner.ViewModel
{
    internal class ProjectViewModel : HTTPBaseViewModel
    {
        public Page Page { get; set; }
        public Window View { get; set; }
        
        public ProjectViewModel() 
        {
            Feedback = "";
            ProjectSearchModel = new Model.ProjectSearchModel()
            {
                DatabaseID = ConfigurationManager.AppSettings["databaseid"],
                GetProjects = new Commands.DelegateCommandGen<DataGrid>(canExecute: CheckGetProjects, execute: GetProjects),
                SaveProject = new Commands.DelegateCommand(canExecute: CheckSaveProject, execute: SaveProject)
            };
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


        private Model.ProjectSearchModel _projectSearchModel;
        public Model.ProjectSearchModel ProjectSearchModel
        {
            get
            {
                return _projectSearchModel;
            }
            set
            {
                if (_projectSearchModel != value)
                {
                    _projectSearchModel = value;
                    NotifyPropertyChanged();
                }

            }
        }

        private Model.ProjectModel _selectedProject;
        public Model.ProjectModel SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    NotifyPropertyChanged();
                }

            }
        }
        
        private async void GetProjects(DataGrid dataGrid)
        {
            Feedback = "Projekte werden geladen...";
            var projectResponse = await ProjectService.GetProjects(BaseHttpClient, ProjectSearchModel.DatabaseID);

            if (projectResponse.IsSuccessStatusCode)
            {
                var json = projectResponse.Content.ReadAsStringAsync().Result;
                var projects = JsonConvert.DeserializeObject<ObservableCollection<Model.ProjectModel>>(json);

                dataGrid.Visibility= Visibility.Visible;
                dataGrid.ItemsSource = projects;
                var count = projects.Count().ToString();

                Feedback = "Projekte laden erfolgreich! " + count + " Projekt(e) wurde(n) gefunden";
                BaseDBID = ProjectSearchModel.DatabaseID;

                View = GetParentView(dataGrid);
            }

            else
            {
                var error = ManageResponseCodes(projectResponse);
                Feedback = "Projekte laden Fehlgeschlagen: " + error;

            }
        }

        private Window GetParentView(DataGrid dataGrid)
        {
            var stackPanel = VisualTreeHelper.GetParent(dataGrid) as StackPanel;
            var grid = VisualTreeHelper.GetParent(stackPanel) as Grid;
            var border = VisualTreeHelper.GetParent(grid) as Border;
            var secondBorder = VisualTreeHelper.GetParent(border) as Border;
            var view = secondBorder.Parent;

            return view as Window;
        }

        private bool CheckGetProjects()
        {
            if(ProjectSearchModel.DatabaseID != "db1")
            {
                Feedback = "Bitte überprüfen sie die Datenbank-ID.";
                return false;
            }
            else
            {
                return true;
            }
        }
        private void SaveProject()
        {
            BaseProject = SelectedProject;
            Feedback = "Das ausgewählte Projekt wurde erfolgreich gespeichert!";

            Uri uri = new Uri("View/ScannerPage.xaml", UriKind.Relative);
            Page.NavigationService.Navigate(uri);
        }

        private bool CheckSaveProject()
        {
            return true;
        }

        



    }
}
