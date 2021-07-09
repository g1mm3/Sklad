using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sklad
{
	public interface IWpfUtils
	{
		void OpenWindow(Window currentWindow, Window windowToOpen, bool isCloseCurrentWindow = true,
			bool isCopyPreviousParameters = true);

		ImageSource ByteToImageSource(byte[] imageData);
	}

	public class WpfUtils : IWpfUtils
	{
		/// <summary>
		/// Метод, отвечающий за открытие нового окна
		/// </summary>
		/// <param name="currentWindow"></param>
		/// <param name="windowToOpen"></param>
		/// <param name="isCloseCurrentWindow"></param>
		/// <param name="isCopyPreviousParameters"></param>
		public void OpenWindow(Window currentWindow, Window windowToOpen, bool isCloseCurrentWindow = true,
			bool isCopyPreviousParameters = true)
		{
			if (isCopyPreviousParameters)
			{
				windowToOpen.Title = currentWindow.Title;
				windowToOpen.WindowStartupLocation = currentWindow.WindowStartupLocation;
				windowToOpen.WindowState = currentWindow.WindowState;
			}

			if (isCloseCurrentWindow)
				currentWindow.Close();

			windowToOpen.Show();
		}


		public ImageSource ByteToImageSource(byte[] imageData)
		{
			BitmapImage biImg = new BitmapImage();
			MemoryStream ms = new MemoryStream(imageData);
			biImg.BeginInit();
			biImg.StreamSource = ms;
			biImg.EndInit();

			return biImg as ImageSource;
		}
	}
}
