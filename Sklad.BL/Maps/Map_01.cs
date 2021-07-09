using Sklad.BL.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sklad.BL.Maps
{
	public class Map_01 : Map
	{
		// Событие для отправки сообщения в окно
		public override event Action<string> SendMessage;

		// Конструктор
		public Map_01() : base() { }

		public override string ToString()
		{
			return "Map_01";
		}

		public override void ChargersInitialization()
		{
			Chargers = new List<Charger>
			{
				new Charger(11, 0),
			};
		}

		public override void FridgesInitialization()
		{
			Fridges = new List<Fridge>
			{
				new Fridge(0, 4, 50, 1),
				new Fridge(0, 7, 50, 2),
				new Fridge(13, 4, 50, 3),
				new Fridge(13, 7, 50, 4),
			};
		}

		public override void ProductLoadingsInitialization()
		{
			ProductLoadings = new List<ProductLoading>
			{
				new ProductLoading(0, 0),
			};
		}

		public override void RoadsInitialization()
		{
			Roads = new List<Road>();

			Roads.Add(new Road(0, 3));


			for (int i = 3; i <= 12; i++)
				Roads.Add(new Road(1, i));

			for (int i = 3; i <= 12; i++)
				Roads.Add(new Road(2, i));

			for (int i = 0; i <= 9; i++)
				Roads.Add(new Road(3, i));

			for (int i = 0; i <= 12; i++)
				Roads.Add(new Road(4, i));

			for (int i = 0; i <= 12; i++)
				Roads.Add(new Road(5, i));

			for (int i = 1; i <= 9; i++)
				Roads.Add(new Road(6, i));

			for (int i = 1; i <= 12; i++)
				Roads.Add(new Road(7, i));

			for (int i = 0; i <= 12; i++)
				Roads.Add(new Road(8, i));

			for (int i = 0; i <= 9; i++)
				Roads.Add(new Road(9, i));

			for (int i = 0; i <= 12; i++)
				Roads.Add(new Road(10, i));


			for (int i = 2; i <= 12; i++)
				Roads.Add(new Road(11, i));

			for (int i = 2; i <= 12; i++)
				Roads.Add(new Road(12, i));


			Roads.Add(new Road(13, 2));
			Roads.Add(new Road(13, 3));
		}

		public override void RobotsInitialization()
		{
			Robots = new List<Robot>
			{
				new Robot(6, 6),
			};
		}

		public override void ShelvesInitialization()
		{
			Shelves = new List<Shelf>()
			{
				new Shelf(0, 10, 50, 1),
				new Shelf(3, 10, 50, 2),
				new Shelf(6, 10, 50, 3),
				new Shelf(9, 10, 50, 4),
				new Shelf(13, 10, 50, 5),
			};
		}

		public override void StoreTransfersInitialization()
		{
			StoreTransfers = new List<StoreTransfer>
			{
				new StoreTransfer(6, 0),
			};
		}

		public override void CreateColumnsAndRows()
		{
			ColumnDefinitions = new List<ColumnDefinition>(); // 14
			RowDefinitions = new List<RowDefinition>(); // 13

			for (int i = 1; i <= 14; i++)
			{
				if (i <= 13)
				{
					RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
				}

				ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
			}
		}

		/// <summary>
		/// Отправка продуктов с прошедшим наполовину сроком годности в магазин
		/// </summary>
		/// <param name="robot"></param>
		/// <returns></returns>
		public async override Task SendProductsToShop(Robot robot)
		{

			foreach (var fr in Fridges)
			{
				await SendProductsFromContainer(fr, robot);
			}

			foreach (var sh in Shelves)
			{
				await SendProductsFromContainer(sh, robot);
			}
		}

		/// <summary>
		/// Разгрузка продуктов
		/// </summary>
		/// <param name="products"></param>
		/// <param name="robot"></param>
		/// <returns></returns>
		public async override Task ProductsUnloading(List<Product> products, Robot robot)
		{
			SendMessage?.Invoke("Робот разгружает продукты из автомобиля");

			foreach (var el in products)
			{
				await GoToProductLoading(robot);
				robot.TakeProduct(el);
				await GoToIdle<ProductLoading>(robot);

				Fields.Container cnt = null;

				cnt = el.IsFrostNeed
					? (Fields.Container)Fridges.FirstOrDefault(x => x.RemaindWeight >= el.ValueKg)
					: (Fields.Container)Shelves.First(x => x.RemaindWeight >= el.ValueKg);



				if (cnt == null)
				{
					await GoToTransfer(robot);
					robot.TransferProductToShop();
					await GoToIdle<StoreTransfer>(robot);

					continue;
				}

				if (el.IsFrostNeed)
				{
					await GoToFridge(robot, cnt.Number);
					robot.PlaceProduct(cnt);
					await GoToIdle<Fridge>(robot, cnt.Number);
				}
				else
				{
					await GoToShelf(robot, cnt.Number);
					robot.PlaceProduct(cnt);
					await GoToIdle<Shelf>(robot, cnt.Number);
				}
			}
		}

		public async override Task GoToFridge(Robot robot, int number)
		{
			switch (number)
			{
				case 1: await GoToFridge1(robot); break;
				case 2: await GoToFridge2(robot); break;
				case 3: await GoToFridge3(robot); break;
				case 4: await GoToFridge4(robot); break;
			}
		}

		public async override Task GoToShelf(Robot robot, int number)
		{
			switch (number)
			{
				case 1: await GoToShelf1(robot); break;
				case 2: await GoToShelf2(robot); break;
				case 3: await GoToShelf3(robot); break;
				case 4: await GoToShelf4(robot); break;
				case 5: await GoToShelf5(robot); break;
			}
		}

		public async override Task GoToProductLoading(Robot robot)
		{
			for (int i = 1; i <= 9; i++)
			{
				if (i == 4)
				{
					await robot.TurnLeft();
					continue;
				}

				if (i == 9)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		public async override Task GoToTransfer(Robot robot)
		{
			for (int i = 1; i <= 5; i++)
			{
				await robot.MoveForward();
			}
		}

		public async override Task GoToCharger(Robot robot)
		{
			for (int i = 1; i <= 10; i++)
			{
				if (i == 6)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		public async override Task GoToIdle<T>(Robot robot, int containerNumber = 0)
		{

			if (typeof(T) == typeof(Charger))
			{
				await GoToIdleFromCharger(robot);
				return;
			}

			if (typeof(T) == typeof(Fridge))
			{
				await GoToIdleFromFridge(robot, containerNumber);
				return;
			}

			if (typeof(T) == typeof(ProductLoading))
			{
				await GoToIdleFromProductLoading(robot);
				return;
			}

			if (typeof(T) == typeof(Shelf))
			{
				await GoToIdleFromShelf(robot, containerNumber);
				return;
			}

			if (typeof(T) == typeof(StoreTransfer))
			{
				await GoToIdleFromStoreTransfer(robot);
				return;
			}
		}

		private async Task SendProductsFromContainer(Fields.Container container, Robot robot)
		{
			if (container.Products.Count > 0)
			{
				foreach (var pr in container.Products)
				{
					if (DateTime.Now.Subtract(Convert.ToDateTime(pr.ReceiptDate)).Ticks >=
						pr.ExpiryDate.Ticks / 2)
					{
						SendMessage?.Invoke("Робот переносит продукты в магазин");
						if (container is Fridge)
						{
							await GoToFridge(robot, container.Number);
							robot.TakeProductFromContainer(container, pr);
							await GoToIdle<Fridge>(robot, container.Number);
						}
						else if (container is Shelf)
						{
							await GoToShelf(robot, container.Number);
							robot.TakeProductFromContainer(container, pr);
							await GoToIdle<Shelf>(robot, container.Number);
						}
						else
						{
							// 
							continue;
						}

						await GoToTransfer(robot);
						robot.TransferProductToShop();
						await GoToIdle<StoreTransfer>(robot);
						return;
					}
				}
			}
		}

		private async Task GoToFridge1(Robot robot)
		{
			for (int i = 1; i <= 6; i++)
			{
				if (i == 1)
				{
					await robot.TurnLeft();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToFridge2(Robot robot)
		{
			for (int i = 1; i <= 8; i++)
			{
				if (i == 1)
				{
					await robot.Turn180Degrees();
					continue;
				}

				if (i == 3)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToFridge3(Robot robot)
		{
			for (int i = 1; i <= 7; i++)
			{
				if (i == 1)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToFridge4(Robot robot)
		{
			for (int i = 1; i <= 9; i++)
			{
				if (i == 1)
				{
					await robot.Turn180Degrees();
					continue;
				}

				if (i == 3)
				{
					await robot.TurnLeft();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToShelf1(Robot robot)
		{
			for (int i = 1; i <= 12; i++)
			{
				if (i == 1 || i == 7)
				{
					await robot.TurnLeft();
					continue;
				}

				if (i == 12)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToShelf2(Robot robot)
		{
			for (int i = 1; i <= 9; i++)
			{
				if (i == 1 || i == 4)
				{
					await robot.TurnLeft();
					continue;
				}

				if (i == 9)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToShelf3(Robot robot)
		{
			for (int i = 1; i <= 8; i++)
			{
				if (i == 1 || i == 3 || i == 8)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToShelf4(Robot robot)
		{
			for (int i = 1; i <= 11; i++)
			{
				if (i == 1 || i == 6 || i == 11)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToShelf5(Robot robot)
		{
			for (int i = 1; i <= 13; i++)
			{
				if (i == 1 || i == 8)
				{
					await robot.TurnRight();
					continue;
				}

				if (i == 13)
				{
					await robot.TurnLeft();
					continue;
				}


				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromShelf(Robot robot, int shelfNumber)
		{
			switch (shelfNumber)
			{
				case 1: await GoToIdleFromShelf1(robot); break;
				case 2: await GoToIdleFromShelf2(robot); break;
				case 3: await GoToIdleFromShelf3(robot); break;
				case 4: await GoToIdleFromShelf4(robot); break;
				case 5: await GoToIdleFromShelf5(robot); break;
			}
		}

		private async Task GoToIdleFromFridge(Robot robot, int fridgeNumber)
		{
			switch (fridgeNumber)
			{
				case 1: await GoToIdleFromFridge1(robot); break;
				case 2: await GoToIdleFromFridge2(robot); break;
				case 3: await GoToIdleFromFridge3(robot); break;
				case 4: await GoToIdleFromFridge4(robot); break;
			}
		}

		private async Task GoToIdleFromFridge1(Robot robot)
		{
			for (int i = 1; i <= 7; i++)
			{
				if (i == 1)
				{
					await robot.Turn180Degrees();
					continue;
				}

				if (i == 7)
				{
					await robot.TurnLeft();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromFridge2(Robot robot)
		{
			for (int i = 1; i <= 8; i++)
			{
				if (i == 1)
				{
					await robot.Turn180Degrees();
					continue;
				}

				if (i == 7)
				{
					await robot.TurnLeft();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromFridge3(Robot robot)
		{
			for (int i = 1; i <= 8; i++)
			{
				if (i == 1)
				{
					await robot.Turn180Degrees();
					continue;
				}

				if (i == 8)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromFridge4(Robot robot)
		{
			for (int i = 1; i <= 9; i++)
			{
				if (i == 1)
				{
					await robot.Turn180Degrees();
					continue;
				}

				if (i == 8)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromShelf1(Robot robot)
		{
			for (int i = 1; i <= 12; i++)
			{
				if (i == 1 || i == 6)
				{
					await robot.TurnRight();
					continue;
				}

				if (i == 12)
				{
					await robot.TurnLeft();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromShelf2(Robot robot)
		{
			for (int i = 1; i <= 9; i++)
			{
				if (i == 1 || i == 6)
				{
					await robot.TurnRight();
					continue;
				}

				if (i == 9)
				{
					await robot.TurnLeft();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromShelf3(Robot robot)
		{
			for (int i = 1; i <= 8; i++)
			{
				if (i == 1 || i == 8)
				{
					await robot.TurnRight();
					continue;
				}

				if (i == 6)
				{
					await robot.TurnLeft();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromShelf4(Robot robot)
		{
			for (int i = 1; i <= 11; i++)
			{
				if (i == 1 || i == 11)
				{
					await robot.TurnRight();
					continue;
				}

				if (i == 6)
				{
					await robot.TurnLeft();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromShelf5(Robot robot)
		{
			for (int i = 1; i <= 13; i++)
			{
				if (i == 1 || i == 6)
				{
					await robot.TurnLeft();
					continue;
				}

				if (i == 13)
				{
					await robot.TurnRight();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromStoreTransfer(Robot robot)
		{
			for (int i = 1; i <= 7; i++)
			{
				if (i == 1)
				{
					await robot.Turn180Degrees();
					continue;
				}

				if (i == 7)
				{
					await robot.Turn180Degrees();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromProductLoading(Robot robot)
		{
			for (int i = 1; i <= 10; i++)
			{
				if (i == 1 || i == 6)
				{
					await robot.TurnRight();
					continue;
				}

				if (i == 10)
				{
					await robot.Turn180Degrees();
					continue;
				}

				await robot.MoveForward();
			}
		}

		private async Task GoToIdleFromCharger(Robot robot)
		{
			for (int i = 1; i <= 12; i++)
			{
				if (i == 1)
				{
					await robot.Turn180Degrees();
					continue;
				}

				if (i == 6)
				{
					await robot.TurnLeft();
					continue;
				}

				if (i == 12)
				{
					await robot.Turn180Degrees();
					continue;
				}

				await robot.MoveForward();
			}
		}
	}
}
