using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad.BL.Fields
{
	public class Charger : Field
	{
		public Charger(int column, int row) : base(column, row)
		{
			ColumnSpan = 3;
			RowSpan = 2;
		}
	}
}
