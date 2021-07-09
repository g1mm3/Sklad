using Sklad.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sklad.Windows
{
	public interface IMainWindow
	{
		Button StartStopButton { get; }
		Button ProductAccountingButton { get; }
		ComboBox MapsComboBox { get; }
		Grid MarkupGrid { get; }

		void SetStatusMessage(string message);

		event EventHandler StartStopClick;
		event EventHandler ProductAccountingClick;
		event EventHandler MapsSelectionChanged;
	}

	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IMainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			// Объявление экземпляров нужных классов
			WpfUtils wpfUtils = new WpfUtils();
			MainPresenter presenter = new MainPresenter(this, wpfUtils);

			// События
			btnStartStop.Click += BtnStartStop_Click;
			btnProductAccounting.Click += BtnProductAccounting_Click;
			cmbMaps.SelectionChanged += CmbMaps_SelectionChanged;
		}

		#region Проброс событий
		private void BtnStartStop_Click(object sender, RoutedEventArgs e)
		{
			if (StartStopClick != null) StartStopClick(this, EventArgs.Empty);
		}

		private void BtnProductAccounting_Click(object sender, RoutedEventArgs e)
		{
			if (ProductAccountingClick != null) ProductAccountingClick(this, EventArgs.Empty);
		}

		private void CmbMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (MapsSelectionChanged != null) MapsSelectionChanged(this, EventArgs.Empty);
		}
		#endregion

		#region IMainWindow
		public Button StartStopButton
		{
			get { return btnStartStop; }
		}

		public Button ProductAccountingButton
		{
			get { return btnProductAccounting; }
		}

		public ComboBox MapsComboBox
		{
			get { return cmbMaps; }
		}

		public Grid MarkupGrid
		{
			get { return markupGrid; }
		}

		public void SetStatusMessage(string message) => tblStatus.Text = message;

		public event EventHandler StartStopClick;
		public event EventHandler ProductAccountingClick;
		public event EventHandler MapsSelectionChanged;
		#endregion
	}
}
