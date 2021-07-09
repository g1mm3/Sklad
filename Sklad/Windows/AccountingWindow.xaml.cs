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
	public interface IAccountingWindow
	{
		ListBox ProductsListBox { get; }
		event EventHandler AccountingWindowLoaded;
	}

	/// <summary>
	/// Логика взаимодействия для AccountingWindow.xaml
	/// </summary>
	public partial class AccountingWindow : Window, IAccountingWindow
	{
		public AccountingWindow()
		{
			InitializeComponent();

			// Объявление экземпляров нужных классов
			AccountingPresenter presenter = new AccountingPresenter(this);

			// События
			Loaded += AccountingWindow_Loaded;
		}

		#region Проброс событий
		private void AccountingWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (AccountingWindowLoaded != null) AccountingWindowLoaded(this, EventArgs.Empty);
		}
		#endregion

		#region AccountingWindow
		public ListBox ProductsListBox
		{
			get { return lbxProducts; }
		}

		public event EventHandler AccountingWindowLoaded;
		#endregion
	}
}
