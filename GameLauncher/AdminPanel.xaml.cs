using System;
using System.Diagnostics;
using System.Windows;
namespace GameLauncher
{
	/// <summary>
	/// Логика взаимодействия для AdminPanel.xaml
	/// </summary>
	public partial class AdminPanel : Window
	{
		/// <summary>
		/// Конструктор класса, в котором из GlobalParam устанавливаются значения в объекты на форме
		/// </summary>
		public AdminPanel()
		{
			InitializeComponent();
			if (GlobalParam.Shell == true)
			{
				ThisProgShell.IsChecked = true;
			}
			else
			{
				WindowsShell.IsChecked = true;
			}
			Pass.Password = GlobalParam.Password;
			dublePass.Password = GlobalParam.Password;
			hoursStart.Text = GlobalParam.MassTime[0, 0].ToString();
			hoursEnd.Text = GlobalParam.MassTime[1, 0].ToString();
			minuteStart.Text = GlobalParam.MassTime[0, 1].ToString();
			minuteEnd.Text = GlobalParam.MassTime[1, 1].ToString();
		}
		/// <summary>
		/// Метод обрабатывающий событие клик, по кнопку "Сохранить", проверяет правильность заполнения полей и записывает их значение в GlobalParam
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (Pass.Password!= dublePass.Password)
			{
				MessageBox.Show("Пароли не совпадают!");
				return;
			}
			else
			{
				int[,] masTime = new int[2,2];
				try
				{
					masTime[0, 0] = Convert.ToInt32(hoursStart.Text);
					masTime[0, 1] = Convert.ToInt32(minuteStart.Text);
					masTime[1, 0] = Convert.ToInt32(hoursEnd.Text);
					masTime[1, 1] = Convert.ToInt32(minuteEnd.Text);
					GlobalParam.MassTime = masTime;
				}
				catch (Exception)
				{
					MessageBox.Show("Ошибка в задании времени");
					return;
				}
				if (ThisProgShell.IsChecked == true)
				{
					GlobalParam.Shell = true;
					GlobalParam.ContextMenuInGroup = false;
				}
				else
				{
					GlobalParam.Shell = false;
					Process.Start("explorer.exe");
					GlobalParam.ContextMenuInGroup = true;
				}
				GlobalParam.Password = Pass.Password;
				GlobalParam.SaveList();

				this.Close();
			}
		}
	}
}
