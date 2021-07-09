using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad.BL.Fields
{
	public class Road : Field
	{
		public Road(int column, int row) : base(column, row)
		{
			ColumnSpan = 1;
			RowSpan = 1;
		}
	}
}
