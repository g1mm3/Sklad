using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sklad.BL.Fields
{
    public abstract class Field
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public int ColumnSpan { get; set; }
        public int RowSpan { get; set; }
        public Image Image { get; set; }

        public Field(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}
