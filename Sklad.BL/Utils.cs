using System;
using System.Collections.Generic;

namespace Sklad.BL
{
	public static class Utils
	{
		private static Random random;

		static Utils()
		{
			random = new Random();
		}

		public static List<Product> GetProductData()
		{
			List<Product> products = new List<Product>();

			int productsCount = random.Next(4, 8);

			for (int i = 1; i <= productsCount; i++)
			{
				products.Add(GetRandomProduct());
			}

			return products;
		}

		private static Product GetRandomProduct()
		{
			// Типы продуктов (все должны быть в БД):
			// 1 - Бакалея, 2 - Молочные, 3 - Мясные, 4 - Полуфабрикаты,
			// 5 - Овощи, 6 - Фрукты, 7 - Кондитерские, 8 - Хлебо-булочные

			List<Product> pr = new List<Product>()
			{
				new Product { Title = "Макароны", ValueKg = 10, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(4), ProductType = "Бакалея" },
				new Product { Title = "Перловка", ValueKg = 10, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(4), ProductType = "Бакалея" },
				new Product { Title = "Молоко", ValueKg = 15, IsFrostNeed = true, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(5), ProductType = "Молочные" },
				new Product { Title = "Масло сливочное", ValueKg = 15, IsFrostNeed = true, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(4), ProductType = "Молочные" },
				new Product { Title = "Куриное филе", ValueKg = 10, IsFrostNeed = true, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(3), ProductType = "Мясные" },
				new Product { Title = "Говяжий фарш", ValueKg = 15, IsFrostNeed = true, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(3), ProductType = "Мясные" },
				new Product { Title = "Наггетсы", ValueKg = 15, IsFrostNeed = true, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(5), ProductType = "Полуфабрикаты" },
				new Product { Title = "Пельмени", ValueKg = 15, IsFrostNeed = true, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(5), ProductType = "Полуфабрикаты" },
				new Product { Title = "Огурцы", ValueKg = 15, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(6), ProductType = "Овощи" },
				new Product { Title = "Помидоры", ValueKg = 15, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(6), ProductType = "Овощи" },
				new Product { Title = "Яблоки", ValueKg = 15, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(6), ProductType = "Фрукты" },
				new Product { Title = "Груши", ValueKg = 15, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(6), ProductType = "Фрукты" },
				new Product { Title = "Вафли", ValueKg = 10, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(8), ProductType = "Кондитерские" },
				new Product { Title = "Печенье", ValueKg = 15, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(8), ProductType = "Кондитерские" },
				new Product { Title = "Хлеб пшеничный", ValueKg = 25, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(4), ProductType = "Хлебо-булочные" },
				new Product { Title = "Хлеб ржаной", ValueKg = 25, IsFrostNeed = false, ReceiptDate = DateTime.Now.ToString(), ExpiryDate = TimeSpan.FromMinutes(4), ProductType = "Хлебо-булочные" },
			};

			return pr[random.Next(0, 15)];
		}

		public static DB.Product CommonProductToDbProduct(Product product)
		{
			(int productTypeId, int containerId) infoTuple =
				GetDbIdsByProductAndContainerInfo(product.ProductType, product.ContainerType, product.ContainerNumber);

			DB.Product dbProduct = new DB.Product
			{
				Title = product.Title,
				ValueKg = product.ValueKg,
				IsFrostNeed = product.IsFrostNeed,
				ReceiptDate = product.ReceiptDate,
				ExpiryDate = product.ExpiryDate,
				ContainerId = infoTuple.containerId,
				ProductTypeId = infoTuple.productTypeId
			};

			return dbProduct;
		}

		public static Product DbProductToCommonProduct(DB.Product dbProduct)
		{
			(string productType, string containerType, int containerNumber) infoTuple =
				GetProductAndContainerInfoByDbIds(dbProduct.ProductTypeId, dbProduct.ContainerId);

			Product product = new Product
			{
				Title = dbProduct.Title,
				ValueKg = dbProduct.ValueKg,
				IsFrostNeed = dbProduct.IsFrostNeed,
				ReceiptDate = dbProduct.ReceiptDate,
				ExpiryDate = dbProduct.ExpiryDate,
				ProductType = infoTuple.productType,
				ContainerType = infoTuple.containerType,
				ContainerNumber = infoTuple.containerNumber
			};

			return product;
		}

		private static (int, int) GetDbIdsByProductAndContainerInfo
			(string productType, string containerType, int containerNumber)
		{
			int productTypeId, containerId;

			switch (productType)
			{
				case "Бакалея": productTypeId = 1; break;
				case "Молочные": productTypeId = 2; break;
				case "Мясные": productTypeId = 3; break;
				case "Полуфабрикаты": productTypeId = 4; break;
				case "Овощи": productTypeId = 5; break;
				case "Фрукты": productTypeId = 6; break;
				case "Кондитерские": productTypeId = 7; break;
				case "Хлебо-булочные": productTypeId = 8; break;
				default: productTypeId = 0; break;
			}

			switch (containerType)
			{
				case "Холодильник":
					switch (containerNumber)
					{
						case 1: containerId = 1; break;
						case 2: containerId = 2; break;
						case 3: containerId = 3; break;
						case 4: containerId = 4; break;
						default: containerId = 0; break;
					}
					break;

				case "Полка":
					switch (containerNumber)
					{
						case 1: containerId = 5; break;
						case 2: containerId = 6; break;
						case 3: containerId = 7; break;
						case 4: containerId = 8; break;
						case 5: containerId = 9; break;
						default: containerId = 0; break;
					}
					break;

				default: containerId = 0; break;
			}

			return (productTypeId, containerId);
		}

		private static (string, string, int) GetProductAndContainerInfoByDbIds
			(int productTypeId, int containerId)
		{
			string productType, containerType;
			int containerNumber;

			switch (productTypeId)
			{
				case 1: productType = "Бакалея"; break;
				case 2: productType = "Молочные"; break;
				case 3: productType = "Мясные"; break;
				case 4: productType = "Полуфабрикаты"; break;
				case 5: productType = "Овощи"; break;
				case 6: productType = "Фрукты"; break;
				case 7: productType = "Кондитерские"; break;
				case 8: productType = "Хлебо-булочные"; break;
				default: throw new Exception("Типа продуктов с таким id нет в базе данных!");
			}

			switch (containerId)
			{
				case 1: containerType = "Холодильник"; containerNumber = 1; break;
				case 2: containerType = "Холодильник"; containerNumber = 2; break;
				case 3: containerType = "Холодильник"; containerNumber = 3; break;
				case 4: containerType = "Холодильник"; containerNumber = 4; break;
				case 5: containerType = "Полка"; containerNumber = 1; break;
				case 6: containerType = "Полка"; containerNumber = 2; break;
				case 7: containerType = "Полка"; containerNumber = 3; break;
				case 8: containerType = "Полка"; containerNumber = 4; break;
				case 9: containerType = "Полка"; containerNumber = 5; break;
				default: throw new Exception("Контейнера с таким id нет в базе данных!");
			}

			return (productType, containerType, containerNumber);
		}
	}
}
