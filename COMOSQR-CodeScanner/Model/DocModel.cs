using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMOSQR_CodeScanner.Model
{
    public class Column
    {
        public int Id { get; set; }
        public int Alignment { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }
        public string DisplayDescription { get; set; }
        public bool Numeric { get; set; }
        public bool WrapText { get; set; }
        public bool IsDate { get; set; }
    }

    public class Item
    {
        public string UID { get; set; }
        public string Text { get; set; }
        public double NumericValue { get; set; }
    }

    public class Root
    {
        public List<Column> Columns { get; set; }
        public List<Row> Rows { get; set; }
    }

    public class Row
    {
        public string UID { get; set; }
        public List<Item> Items { get; set; }
    }
}
