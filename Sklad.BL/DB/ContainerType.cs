using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad.BL.DB
{
	public class ContainerType
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public List<Container> Containers { get; set; }
	}
}
