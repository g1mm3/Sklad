using Sklad.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sklad.Presenter
{
	public class StartPresenter
	{
		private readonly IStartWindow _view;
		private readonly IWpfUtils _wpfUtils;

		public StartPresenter(IStartWindow view, IWpfUtils wpfUtils)
		{
			_view = view;
			_wpfUtils = wpfUtils;

			// События
			_view.ProgramStartClick += View_ProgramStartClick;
			_view.ProgramQuitClick += View_ProgramQuitClick;
		}

		private void View_ProgramStartClick(object sender, EventArgs e)
		{
			_wpfUtils.OpenWindow(_view as Window, new MainWindow());
		}

		private void View_ProgramQuitClick(object sender, EventArgs e)
		{
			//messageService.ShowExclamation("Программа закрыта");
			_view.Close();
		}
	}
}
