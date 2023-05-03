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
using System.IO;

namespace COMOSQR_CodeScanner.ViewModel
{
    internal class DocViewModel : HTTPBaseViewModel
    {
        private string _filePath;
        public Window View { get; set; }
        public Page Page { get; set; }

        public DocViewModel()
        {
            Feedback = "";
            _filePath = ConfigurationManager.AppSettings["filepath"];

            DocSearchModel = new Model.DocSearchModel()
            {
                ProjectId = BaseProject.UID,
                ProjectName = BaseProject.Name,
                GetDocuments = new Commands.DelegateCommandGen<DataGrid>(canExecute: CheckGetDocument, execute: GetDocuments),
                DownloadDocument = new Commands.DelegateCommand(canExecute: CheckSelectDocument, execute: DownloadDocument),
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

        public async void GetDocuments(DataGrid datagrid)
        {
            Feedback = "Dokumente werden geladen...";



            var documentResponse = await DocService.GetDocuments(BaseHttpClient, BaseDBID, DocSearchModel.ProjectId, BaseQRUID);

            if (documentResponse.IsSuccessStatusCode)
            {
                var json = documentResponse.Content.ReadAsStringAsync().Result;
                var documents = JsonConvert.DeserializeObject<Model.Root>(json);

                var datatable = CreateDocumentDataTable(documents);

                datagrid.Visibility = Visibility.Visible;
                datagrid.ItemsSource = datatable.AsDataView();
                var count = datatable.Rows.Count;

                Feedback = "Dokumente laden war erfolgreich! " + count + " Dokument(e) wurde(n) gefunden";

                View = GetParentView(datagrid);
            }

            else
            {
                Feedback = "Dokumente holen fehlgeschlagen: " + documentResponse.ReasonPhrase;
            }

        }



        private DataTable CreateDocumentDataTable(Model.Root documents)
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

        public async void DownloadDocument()
        {
            Feedback = "Dokument wird heruntergeladen...";

            var docID = "U:29:" + SelectedDocument.Row.ItemArray[0] as string;


            var response = await DocService.GetDocumentStream(BaseHttpClient, BaseDBID, BaseProject.UID, docID);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                OpenDocument(stream);
            }
            else
            {
                var error = ManageResponseCodes(response);
                Feedback = "Download fehlgeschlagen: " + error;
            }
        }

        private void OpenDocument(Stream stream)
        {
            Feedback = "Dokument wird geöffnet...";

            if (!String.IsNullOrEmpty(_filePath))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    try
                    {
                        using (var pdfStream = new FileStream(_filePath, FileMode.Create))
                        {
                            stream.CopyTo(pdfStream);
                            System.Diagnostics.Process.Start(_filePath);

                            Feedback = "Dokument öffnen war erfolgreich!";
                        }
                    }
                    catch
                    {
                        Feedback = "Das Dokument konnte nicht geöffnet werden. Bitte versuchen Sie es erneut oder überprüfen Sie den Dateipfad.";
                    }
                }
            }
            else
            {
                Feedback = "Das gewünschte Dokument wurde nicht gefunden, bitte überprüfe den Dateipfad in App.config.";
            }
        }

        private bool CheckGetDocument()
        {

            if (!String.IsNullOrEmpty(BaseDBID) && BaseDBID == "db1")
            {
                return true;
            }
            else
            {
                Feedback = "Entweder wurde keine DatenbankID gefunden oder die DatenbankID ist falsch!";
                return false;
            }
        }

        private bool CheckSelectDocument()
        {
            if (SelectedDocument != null)
            {
                return true;
            }
            else
            {
                return false;
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
    }
}
