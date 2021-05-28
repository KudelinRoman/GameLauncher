using System.Windows;


namespace GameLauncher
{
	/// <summary>
	/// Логика взаимодействия для Password.xaml
	/// </summary>
	public partial class Password : Window
	{
		public Password()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Метод обрабатывающий нажатие по кнопке "Отмена"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		/// <summary>
		/// Метод обрабатывающий нажатие по кнопке "Ок", проверяет правильность введенного пароля и запускает панель администратора либо изменяет видимость некоторых объектов главной формы. Действия зависят от параметров создания экземпляра данного класса.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (GlobalParam.Password != passBox.Password)
			{
				MessageBox.Show("Не верный пароль.");
			}
			else
			{
				//проверяем является ли главное окно владельцем этого окна
				MainWindow main = this.Owner as MainWindow;
				if (main != null)
				{
					//если объекты были невидимы, то делаем их видимыми, либо наоборот
					if (main.AddGroup.IsVisible == false)
					{
						main.AddGroup.Visibility = Visibility.Visible;
						main.min_max.Visibility = Visibility.Visible;
						main.WindowState = WindowState.Maximized;
						GlobalParam.ContextMenuInGroup = true;
						main.UpdatePanelGroup();
					}
					else
					{
						main.AddGroup.Visibility = Visibility.Hidden;
						main.min_max.Visibility = Visibility.Hidden;
						main.WindowState = WindowState.Maximized;
						GlobalParam.ContextMenuInGroup = false;
						main.UpdatePanelGroup();
					}
					this.Close();
				}
				//если владелец не главное окно то открывает понель админа
				else
				{
					AdminPanel admin = new AdminPanel();
					admin.Show();
					this.Close();
				}
			}
			
		}
	}
}
