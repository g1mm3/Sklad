using Sklad.BL;
using Sklad.BL.Fields;
using Sklad.BL.Maps;
using Sklad.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sklad.Presenter
{
	public class MainPresenter
	{
		private readonly IMainWindow _view;
		private readonly IWpfUtils _wpfUtils;

		private bool workStatus;
		private bool firstLaunch;
		private List<Map> maps;
		private Map currentMap;

		public MainPresenter(IMainWindow view, IWpfUtils wpfUtils)
		{
			_view = view;
			_wpfUtils = wpfUtils;

			workStatus = false;
			firstLaunch = true;

			// Список и комбо бокс с картами
			maps = new List<Map>
			{
				new Map_01(),
			};
			_view.MapsComboBox.ItemsSource = maps;

			// События
			_view.StartStopClick += View_StartStopClick;
			_view.ProductAccountingClick += View_ProductAccountingClick;
			_view.MapsSelectionChanged += View_MapsSelectionChanged;
		}

		private void View_StartStopClick(object sender, EventArgs e)
		{
			workStatus = !workStatus;

			_view.StartStopButton.Content = workStatus ? "Остановить" : "Запустить";
			_view.StartStopButton.Background = workStatus ? Brushes.DarkRed : Brushes.LimeGreen;

			string message = workStatus ? "Работает" : "Пауза";
			_view.SetStatusMessage(message);

			if (!workStatus)
			{
				MessageBox.Show("Если робот выполняет какую-либо задачу, то его работа будет остановлена как только он её (задачу) завершит");
			}

			// Если это первое нажатие кнопки старт
			if (firstLaunch && workStatus)
			{
				firstLaunch = false;
				_view.ProductAccountingButton.IsEnabled = true;

				FirstLaunchInitialization();
			}
		}

		private void View_ProductAccountingClick(object sender, EventArgs e)
		{
			AccountingWindow accountingWindow = new AccountingWindow();
			accountingWindow.Height = (_view as Window).ActualHeight / 1.4;
			accountingWindow.Width = (_view as Window).ActualWidth / 2;

			accountingWindow.Title = "Учёт продуктов";
			accountingWindow.WindowStartupLocation = (_view as Window).WindowStartupLocation;

			_wpfUtils.OpenWindow(_view as Window, accountingWindow, false, false);
		}

		private void View_MapsSelectionChanged(object sender, EventArgs e)
		{
			_view.StartStopButton.IsEnabled =
				_view.MapsComboBox.SelectedItem != null ? true : false;

			//_view.ProductAccountingButton.IsEnabled =
			//	_view.MapsComboBox.SelectedItem != null ? true : false;
		}

		// Движение робота и маршруты тут
		private async Task WorkProcess()
		{
			// Случайное определение времени ождиания автомобиля с продуктами
			Random rand = new Random();

			// Для более удобного тестирования, во времени прибытия первой машины изначально стоят меньшие значения
			int min = 10;
			int max = 15;
			int sec = rand.Next(min, max);

			// Перечисление всех роботов для дальнейшего использования
			Robot robot1 = currentMap.Robots[0];

			while (true)
			{
				if (workStatus)
				{
					sec--;
					_view.SetStatusMessage($"Автомобиль приедет через {sec} секунд");

					// Отправка продуктов с прошедшим наполовину сроком годности в магазин
					await currentMap.SendProductsToShop(robot1);

					// Зарядка
					if (robot1.Charge < 2000)
					{
						await currentMap.GoToCharger(robot1);
						await robot1.Charging();
						await currentMap.GoToIdle<Charger>(robot1);
					}

					if (sec == 0)
					{
						min = 100;
						max = 150;

						sec = rand.Next(min, max);

						_view.SetStatusMessage("Приехал автомобиль с продуктами");

						List<Product> products = Utils.GetProductData();

						// Разгрузка продуктов
						await currentMap.ProductsUnloading(products, robot1);

						_view.SetStatusMessage("Автомобиль был успешно разгружен");

						// Зарядка
						await currentMap.GoToCharger(robot1);
						await robot1.Charging();
						await currentMap.GoToIdle<Charger>(robot1);
					}
				}

				await Task.Delay(1000);
			}
		}

		private async void FirstLaunchInitialization()
		{
			_view.MapsComboBox.IsEnabled = false;
			currentMap = _view.MapsComboBox.SelectedItem as Map;

			// Инициализация столбцов и строк
			currentMap.ColumnDefinitions.ForEach(x => _view.MarkupGrid.ColumnDefinitions.Add(x));
			currentMap.RowDefinitions.ForEach(x => _view.MarkupGrid.RowDefinitions.Add(x));

			// События
			currentMap.Robots.ForEach(x => x.RefreshRobot += RefreshRobot);
			currentMap.Robots.ForEach(x => x.MoveRobot += MoveRobot);
			currentMap.Robots.ForEach(x => x.RefreshContainer += RefreshContainer);
			currentMap.Robots.ForEach(x => x.SendMessage += _view.SetStatusMessage);
			currentMap.SendMessage += _view.SetStatusMessage;


			// Загрузка полей
			LoadFields(currentMap.Chargers.ToList<Field>(), Properties.Resources.charger);
			LoadFields(currentMap.Fridges.ToList<Field>(), Properties.Resources.fridge);
			LoadFields(currentMap.ProductLoadings.ToList<Field>(), Properties.Resources.loading);
			LoadFields(currentMap.Roads.ToList<Field>(), Properties.Resources.road);
			LoadFields(currentMap.Robots.ToList<Field>(), Properties.Resources.robot_2);
			LoadFields(currentMap.Shelves.ToList<Field>(), Properties.Resources.shelf);
			LoadFields(currentMap.StoreTransfers.ToList<Field>(), Properties.Resources.transfer);


			await WorkProcess();
		}

		//void BreakCurrentTime()
		//{
		//	breakCurrentTime = true;
		//}

		private void LoadFields(List<Field> field, byte[] image)
		{
			foreach (var fld in field)
			{
				Image img = new Image
				{
					Source = _wpfUtils.ByteToImageSource(image),
					Stretch = Stretch.Fill
				};

				Grid.SetColumn(img, fld.Column);
				Grid.SetRow(img, fld.Row);

				if (fld.ColumnSpan > 1)
					Grid.SetColumnSpan(img, fld.ColumnSpan);

				if (fld.RowSpan > 1)
					Grid.SetRowSpan(img, fld.RowSpan);

				fld.Image = img;

				_view.MarkupGrid.Children.Add(img);

				if (fld is Container)
				{
					TextBlock tbl = new TextBlock
					{
						Text = $"{(fld as Container).RemaindWeight}/{(fld as Container).MaxWeightKg} | кг",
						Foreground = Brushes.Black,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center
					};

					Grid.SetColumn(tbl, fld.Column);
					Grid.SetRow(tbl, fld.Row);

					if (fld.ColumnSpan > 1)
						Grid.SetColumnSpan(tbl, fld.ColumnSpan);

					if (fld.RowSpan > 1)
						Grid.SetRowSpan(tbl, fld.RowSpan);

					(fld as Container).TextBlock = tbl;

					_view.MarkupGrid.Children.Add(tbl);
				}
			}
		}

		private void RefreshRobot(Robot robot)
		{
			switch (robot.Direction)
			{
				case Robot.Vector.Left: robot.Image.Source = _wpfUtils.ByteToImageSource(Properties.Resources.robot_1); break;
				case Robot.Vector.Top: robot.Image.Source = _wpfUtils.ByteToImageSource(Properties.Resources.robot_2); break;
				case Robot.Vector.Right: robot.Image.Source = _wpfUtils.ByteToImageSource(Properties.Resources.robot_3); break;
				case Robot.Vector.Bottom: robot.Image.Source = _wpfUtils.ByteToImageSource(Properties.Resources.robot_4); break;
			}
		}

		private void MoveRobot(Robot robot, int futureColumn, int futureRow)
		{
			foreach (var el in currentMap.Roads)
			{
				if (Grid.GetColumn(el.Image) == futureColumn &&
					Grid.GetRow(el.Image) == futureRow)
				{
					robot.Column = futureColumn;
					robot.Row = futureRow;
					Grid.SetColumn(robot.Image, futureColumn);
					Grid.SetRow(robot.Image, futureRow);
					return;
				}
			}
		}

		private void RefreshContainer(Container container)
		{
			container.TextBlock.Text = $"{container.RemaindWeight}/{container.MaxWeightKg} | кг";
		}
	}
}
