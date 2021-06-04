using System.Diagnostics;
using System.Windows;

namespace GameLauncher
{
	/// <summary>
	/// Логика взаимодействия для LockWindow.xaml
	/// </summary>
	public partial class LockWindow : Window
	{
		public LockWindow()
		{
			InitializeComponent();
			this.WindowState = WindowState.Maximized;
			this.Topmost = true;
		}
		/// <summary>
		/// Метод срабатывающий при событии клик по кнопке "Выключить"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("shutdown", "/s /t 0");
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (Passwrd.Password != GlobalParam.Password)
			{
				Passwrd.Password = "";
				MessageBox.Show("Пароль неверный.");
			}
			else
			{
				this.Close();
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (Passwrd.Password != GlobalParam.Password)
			{
				e.Cancel = true;
			}
		}
	}
}
