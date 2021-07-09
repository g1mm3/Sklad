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
	public interface IStartWindow
	{
		void Close();
		event EventHandler ProgramStartClick;
		event EventHandler ProgramQuitClick;
	}

	/// <summary>
	/// Логика взаимодействия для StartWindow.xaml
	/// </summary>
	public partial class StartWindow : Window, IStartWindow
	{
		public StartWindow()
		{
			InitializeComponent();

			// Объявление экземпляров нужных классов
			WpfUtils wpfUtils = new WpfUtils();
			StartPresenter presenter = new StartPresenter(this, wpfUtils);

			// События
			btnProgramStart.Click += BtnProgramStart_Click;
			btnProgramQuit.Click += BtnProgramQuit_Click;
		}

		#region Проброс событий
		private void BtnProgramStart_Click(object sender, RoutedEventArgs e)
		{
			if (ProgramStartClick != null) ProgramStartClick(this, EventArgs.Empty);
		}

		private void BtnProgramQuit_Click(object sender, RoutedEventArgs e)
		{
			if (ProgramQuitClick != null) ProgramQuitClick(this, EventArgs.Empty);
		}
		#endregion

		#region IStartWindow
		public event EventHandler ProgramStartClick;
		public event EventHandler ProgramQuitClick;
		#endregion
	}
}
