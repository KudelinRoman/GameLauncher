using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	}
}
