using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad.BL.Fields
{
	public class StoreTransfer : Field
	{
		public StoreTransfer(int column, int row) : base(column, row)
		{
			ColumnSpan = 2;
			RowSpan = 1;
		}
	}
}
