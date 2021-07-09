using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sklad.Windows;
using Sklad.BL;

namespace Sklad.Presenter
{
	public class AccountingPresenter
	{
		private readonly IAccountingWindow _view;

		public AccountingPresenter(IAccountingWindow view)
		{
			_view = view;

			// События
			_view.AccountingWindowLoaded += View_ProductAccountingWindowLoaded;
		}

		private void View_ProductAccountingWindowLoaded(object sender, EventArgs e)
		{
			List<BL.DB.Product> dbProducts;

			using (var db = new BL.DB.DbContext())
			{
				db.Database.Migrate();

				dbProducts = db.Products.ToList();
			}

			if (dbProducts.Count > 0)
			{
				List<BL.Product> listBoxProducts = new List<BL.Product>();

				foreach (var el in dbProducts)
				{

					listBoxProducts.Add(Utils.DbProductToCommonProduct(el));
				}

				_view.ProductsListBox.ItemsSource = listBoxProducts;
			}
		}
	}
}
