using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad.BL.DB
{
	public class Container
	{
		public int Id { get; set; }
		public int Number { get; set; }

		public int ContainerTypeId { get; set; }
		public ContainerType ContainerType { get; set; }
	}
}
