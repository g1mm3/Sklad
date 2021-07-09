using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad.BL
{
	public class Product
	{
		public string Title { get; set; }
		public int ValueKg { get; set; } // Вес в кг
		public bool IsFrostNeed { get; set; } // Нужен ли холодильник
		public string ReceiptDate { get; set; } // Дата получения продукта
		public TimeSpan ExpiryDate { get; set; } // Дата времени хранения (в данном случае в минутах - демонстрации ради)
		public string ProductType { get; set; }
		public string ContainerType { get; set; }
		public int ContainerNumber { get; set; }
	}
}
