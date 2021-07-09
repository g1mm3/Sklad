using Sklad.BL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad.BL.Fields
{
	public class Robot : Field
	{
		// Векторы перемещения робота
		public enum Vector
		{
			Left = 1,
			Top,
			Right,
			Bottom
		}

		// Событие для обновления расположения робота
		public event Action<Robot> RefreshRobot;

		// Событие для обновления расположения робота
		public event Action<Robot, int, int> MoveRobot;

		// Событие для обновления состояния хранилища
		public event Action<Fields.Container> RefreshContainer;

		// Событие для отправки сообщения в окно
		public event Action<string> SendMessage;

		// Robot
		// public bool IsEnabled { get; set; }
		public Vector Direction { get; set; }
		public int Charge { get; set; }
		public Product CurrentProduct { get; set; }

		public Robot(int column, int row) : base(column, row)
		{
			ColumnSpan = 1;
			RowSpan = 1;

			Direction = Vector.Top;
			Charge = 2500;
		}

		public async Task Charging()
		{
			while (Charge < 2500)
			{
				Charge += 5;
				SendMessage?.Invoke($"Идёт зарядка {Charge}/2500");
				await Task.Delay(50);
			}
			SendMessage?.Invoke("Робот успешно заряжен!");
		}

		public void TakeProductFromContainer(Fields.Container container, Product product)
		{
			using (DbContext db = new DbContext())
			{
				// Поиск нужного продукта в базе данных для последующего удаления
				DB.Product neededProduct = db.Products
					.Where(x => x.Title == product.Title)
					.Where(x => x.ReceiptDate == product.ReceiptDate)
					.FirstOrDefault();

				db.Products.Remove(neededProduct);
				db.SaveChanges();
			}

			container.Products.Remove(product);
			container.RemaindWeight += product.ValueKg;
			TakeProduct(product);

			RefreshContainer?.Invoke(container);
		}

		public void TakeProduct(Product product)
		{
			CurrentProduct = product;
		}

		public void PlaceProduct(Fields.Container container)
		{
			int containerId = 0;
			if (container is Fridge)
			{
				switch (container.Number)
				{
					case 1: containerId = 1; break;
					case 2: containerId = 2; break;
					case 3: containerId = 3; break;
					case 4: containerId = 4; break;
				}
			}
			else if (container is Shelf)
			{
				switch (container.Number)
				{
					case 1: containerId = 5; break;
					case 2: containerId = 6; break;
					case 3: containerId = 7; break;
					case 4: containerId = 8; break;
					case 5: containerId = 9; break;
				}
			}

			DB.Product dbProduct = Utils.CommonProductToDbProduct(CurrentProduct);
			dbProduct.ContainerId = containerId;

			using (DbContext db = new DbContext())
			{
				db.Products.Add(dbProduct);
				db.SaveChanges();
			}

			container.Products.Add(CurrentProduct);
			container.RemaindWeight -= CurrentProduct.ValueKg;
			CurrentProduct = null;

			RefreshContainer?.Invoke(container);
		}

		public void TransferProductToShop()
		{
			CurrentProduct = null;
		}

		public async Task Move(bool isForward)
		{
			int futureColumn = -1;
			int futureRow = -1;

			switch (Direction)
			{
				case Vector.Left: futureColumn = isForward ? Column - 1 : Column + 1; break;
				case Vector.Top: futureRow = isForward ? Row - 1 : Row + 1; break;
				case Vector.Right: futureColumn = isForward ? Column + 1 : Column - 1; break;
				case Vector.Bottom: futureRow = isForward ? Row + 1 : Row - 1; break;
			}

			if (futureColumn == -1)
				futureColumn = Column;

			if (futureRow == -1)
				futureRow = Row;

			Charge -= 5;

			RefreshRobot?.Invoke(this);
			MoveRobot?.Invoke(this, futureColumn, futureRow);
			await Task.Delay(500);
		}

		public async Task MoveForward()
		{
			await Move(true);
		}

		public async Task MoveBackward()
		{
			await Move(false);
		}

		public async Task TurnLeft()
		{
			switch (Direction)
			{
				case Vector.Left: Direction = Vector.Bottom; break;
				case Vector.Top: Direction = Vector.Left; break;
				case Vector.Right: Direction = Vector.Top; break;
				case Vector.Bottom: Direction = Vector.Right; break;
			}

			Charge -= 5;

			RefreshRobot?.Invoke(this);
			await Task.Delay(200);
		}

		public async Task TurnRight()
		{
			switch (Direction)
			{
				case Vector.Left: Direction = Vector.Top; break;
				case Vector.Top: Direction = Vector.Right; break;
				case Vector.Right: Direction = Vector.Bottom; break;
				case Vector.Bottom: Direction = Vector.Left; break;
			}

			Charge -= 5;

			RefreshRobot?.Invoke(this);
			await Task.Delay(200);
		}

		public async Task Turn180Degrees()
		{
			await TurnLeft();
			await TurnLeft();
		}
	}
}
