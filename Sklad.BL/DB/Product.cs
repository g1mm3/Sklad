using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad.BL.DB
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ValueKg { get; set; } // Вес в кг
        public bool IsFrostNeed { get; set; } // Нужен ли холодильник
        public string ReceiptDate { get; set; } // Дата получения продукта
        public TimeSpan ExpiryDate { get; set; } // Дата времени хранения (в данном случае в минутах - демонстрации ради)

        public int ContainerId { get; set; }
        public Container Container { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
