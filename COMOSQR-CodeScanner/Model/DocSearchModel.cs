using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace COMOSQR_CodeScanner.Model
{
    internal class DocSearchModel
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string QueryId { get; set; }
        public ICommand GetDocuments { get; set; }
        public ICommand DownloadDocument { get; set; }
    }
}
