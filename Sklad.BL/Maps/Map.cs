using Sklad.BL.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sklad.BL.Maps
{
    public abstract class Map
    {
        public abstract event Action<string> SendMessage;

        public List<Charger> Chargers { get; set; }
        public List<Fridge> Fridges { get; set; }
        public List<ProductLoading> ProductLoadings { get; set; }
        public List<Road> Roads { get; set; }
        public List<Robot> Robots { get; set; }
        public List<Shelf> Shelves { get; set; }
        public List<StoreTransfer> StoreTransfers { get; set; }

        public List<ColumnDefinition> ColumnDefinitions { get; set; }
        public List<RowDefinition> RowDefinitions { get; set; }

        public Map()
        {
            // Инициализация полей
            ChargersInitialization();
            FridgesInitialization();
            ProductLoadingsInitialization();
            RoadsInitialization();
            RobotsInitialization();
            ShelvesInitialization();
            StoreTransfersInitialization();

            // Создание столбцов и строк
            CreateColumnsAndRows();
        }

        public abstract void ChargersInitialization();
        public abstract void FridgesInitialization();
        public abstract void ProductLoadingsInitialization();
        public abstract void RoadsInitialization();
        public abstract void RobotsInitialization();
        public abstract void ShelvesInitialization();
        public abstract void StoreTransfersInitialization();

        public abstract void CreateColumnsAndRows();

        public abstract Task SendProductsToShop(Robot robot1);
        public abstract Task ProductsUnloading(List<Product> products, Robot robot);

        public abstract Task GoToFridge(Robot robot, int number);
        public abstract Task GoToShelf(Robot robot, int number);
        public abstract Task GoToProductLoading(Robot robot);
        public abstract Task GoToTransfer(Robot robot);
        public abstract Task GoToCharger(Robot robot);
        public abstract Task GoToIdle<T>(Robot robot, int containerNumber = 0);
    }
}
