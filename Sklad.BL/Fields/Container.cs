using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sklad.BL.Fields
{
    public abstract class Container : Field
    {
        public int Number { get; private set; }
        public double MaxWeightKg { get; private set; }
        public double RemaindWeight { get; set; }
        public TextBlock TextBlock { get; set; }
        public List<Product> Products { get; set; }

        public Container(int column, int row, double maxWeight, int number) : base(column, row)
        {
            Number = number;
            MaxWeightKg = maxWeight;
            RemaindWeight = maxWeight;
            Products = new List<Product>();
        }
    }
}
