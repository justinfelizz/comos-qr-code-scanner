using COMOSQR_CodeScanner.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace COMOSQR_CodeScanner.ViewModel
{
    internal class DocViewModel : HTTPBaseViewModel
    {
        public Page Page { get; set; }
        public Window View { get; set; }
        private string _filePath;

        public DocViewModel()
        {
            Feedback = "";
            _filePath = ConfigurationManager.AppSettings["filepath"];

            DocSearchModel = new Model.DocSearchModel()
            {
                ProjectId = BaseProject.UID,
                ProjectName = BaseProject.Name,
                GetDocuments = new Commands.DelegateCommandGen<DataGrid>(canExecute: CheckGetDocument, execute: GetDocuments),
                DownloadDocument = new Commands.DelegateCommand(canExecute: CheckSelectDocument, execute: DownloadDocument)
            };
        }

       

        private Model.DocSearchModel _docSearchModel;

        public Model.DocSearchModel DocSearchModel
        {
            get
            {
                return _docSearchModel;
            }
            set
            {
                if (_docSearchModel != value)
                {
                    _docSearchModel = value;
                    NotifyPropertyChanged();
                }
            }
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

        private DataRowView _selectedDocument;
        public DataRowView SelectedDocument
        {
            get
            {
                return _selectedDocument;
            }
            set
            {
                if (_selectedDocument != value)
                {
                    _selectedDocument = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public async void GetDocuments(DataGrid dataGrid)
        {
            Feedback = "Dokumente werden geladen...";

            var documentResponse = await DocService.GetDocuments(BaseHttpClient, BaseDBID, DocSearchModel.ProjectId, BaseQRUID);

            if (documentResponse.IsSuccessStatusCode)
            {
                string json = documentResponse.Content.ReadAsStringAsync().Result;
                var documents = JsonConvert.DeserializeObject<Model.Root>(json);

                var dataTable = CreateDocumentDataTable(documents);


                dataGrid.Visibility = Visibility.Visible;
                dataGrid.ItemsSource = dataTable.AsDataView();
                var count = dataTable.Rows.Count;

                Feedback = "Fetching documents was successfull " + count + " documents were found";

                View = GetParentView(dataGrid);
            }

            else
            {
                Feedback = "Dokumente Laden fehlgeschlagen!";
            }
        }



        private DataTable CreateDocumentDataTable(Root documents)
        {
            var table = new DataTable();

            foreach (var column in documents.Columns)
            {
                table.Columns.Add(column.DisplayDescription);
            }

            foreach (var row in documents.Rows)
            {
                DataRow datarow = table.NewRow();

                for (int i = 0; i < row.Items.Count; i++)
                {
                    datarow[i] = row.Items[i].Text;
                }

                table.Rows.Add(datarow);

            }
            return table;
        }

        private bool CheckGetDocument()
        {
            if (!String.IsNullOrEmpty(BaseDBID) && BaseDBID == "db1")
            {
                return true;
            }

            else
            {
                Feedback = "Entweder wurde keine Datenbank gefunden oder die Datenbank ID ist falsch!";
                return false;
            }
        } private void DownloadDocument()
        {
            throw new NotImplementedException();
        }

        private bool CheckSelectDocument()
        {
            throw new NotImplementedException();
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
    }
}
