using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace COMOSQR_CodeScanner.Model
{
    public class ProjectSearchModel
    {
        public string DatabaseID { get; set; }
        public ICommand GetProjects { get; set; }
        public ICommand SaveProject { get; set; }

    }
}
