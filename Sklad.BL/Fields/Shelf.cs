using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad.BL.Fields
{
	public class Shelf : Container
	{
		public Shelf(int column, int row, double maxWeight, int number) : base(column, row, maxWeight, number)
		{
			ColumnSpan = 1;
			RowSpan = 3;
		}
	}
}
