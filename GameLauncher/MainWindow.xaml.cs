using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;

namespace GameLauncher
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Массив существующих процессов
		/// </summary>
		private Process[] localAll;
		/// <summary>
		/// Лист с процессами у которых есть открытые окна
		/// </summary>
		private List<int> idProc= new List<int>();
		public MainWindow()
		{
			InitializeComponent();
			//Создаю поток в котором будет отслеживаться состояние процессов
			Thread ThreadUpdatePanelTask = new Thread(new ThreadStart(SearchProccess));
			ThreadUpdatePanelTask.Start();
		}
		/// <summary>
		/// Метод актуальность содержимого панели задач
		/// </summary>
		private void SearchProccess()
		{
			//Использую бесконечный цикл в котором будет происходить обновление панели задач каждую секунду
			while (true)
			{
				//Получаю все процессы
				localAll = Process.GetProcesses();
				string s = "";
				List<int> idProc1 = new List<int>();
				int k = 0;
				//Выбираю процессы у которых имеются окна
				foreach (Process p in localAll)
				{
					if (p.MainWindowTitle.ToString() != "")
					{
						s += p.MainWindowTitle.ToString() + "\n";
						idProc1.Add(k);
					}
					k++;
				}
				//Если два листа не равны, значит появились/закылись процессы
				//Перезаписываю idProc и вызываю метод обновления панели задач
				if (idProc.Count!= idProc1.Count)
				{
					idProc.Clear();
					foreach(int proc in idProc1)
					{
						idProc.Add(proc);
					}
					//Вызываю метод обновления панели задач
					UpdatePanelTassk();
				}
				Thread.Sleep(1000);
			}
		}
		/// <summary>
		/// Метод обновления панели задач
		/// </summary>
		private void UpdatePanelTassk()
		{
			this.Dispatcher.Invoke(new Action(() =>
			{
				int k = 0;
				PanelTasks.ColumnDefinitions.Clear();
				PanelTasks.ColumnDefinitions.Add(new ColumnDefinition());
				PanelTasks.Children.Clear();
				if (MenuPusk.Parent != null)
				{
					var parent = (Panel)MenuPusk.Parent;
					parent.Children.Remove(MenuPusk);
				}
				PanelTasks.Children.Add(MenuPusk);
				Grid.SetColumn(MenuPusk, k);

				foreach (int i in idProc)
				{
					Button b = new Button();
					b.Width = 33;
					b.Height = 33;
					b.ToolTip = localAll[i].MainWindowTitle;
					b.Tag = i;
					PanelTasks.Children.Add(b);
					b.HorizontalAlignment = HorizontalAlignment.Center;
					ColumnDefinition c = new ColumnDefinition();
					c.MinWidth = 37;
					c.MaxWidth = 37;
					PanelTasks.ColumnDefinitions.Add(c);
					Grid.SetColumn(b, k + 1);
					k++;
					b.MouseLeftButtonDown += ButtonProcess_MouseLeftClick;
					var brush = new ImageBrush();
					try
					{
						localAll[i].StartInfo.UseShellExecute = false;
						using (Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(localAll[i].MainModule.FileName))
						{
							brush.ImageSource = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
						}
						b.Background = brush;
					}
					catch (Exception)
					{

					}
				}
				PanelTasks.ColumnDefinitions.Add(new ColumnDefinition());
			}));
		}
		private void MyWindow_Loaded(object sender, RoutedEventArgs e)
		{
			UpdatePanelGroup();
		}
		private void ButtonProcess_MouseLeftClick(object sender, RoutedEventArgs e)
		{
			Button b = (Button) sender;
			Process process = localAll[(int)b.Tag];
			process.Close();
			
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			
		}
		
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var brush = new ImageBrush();
			if (this.WindowState != WindowState.Maximized)
			{
				brush.ImageSource = new BitmapImage(new Uri("Icons/min.png", UriKind.Relative));
				min_max.Background = brush;
				this.WindowState = WindowState.Maximized;
			}
			else
			{
				brush.ImageSource = new BitmapImage(new Uri("Icons/max.png", UriKind.Relative));
				min_max.Background = brush;
				this.WindowState = WindowState.Normal;
			}
		}
		/// <summary>
		/// Обработчик клика по кнопке "добавить группу" (+)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddGroup_Click(object sender, RoutedEventArgs e)
		{
			NewGroup newGroup = new NewGroup();
			newGroup.ShowDialog();
			UpdatePanelGroup();
		}
		/// <summary>
		/// Обработчик клика по группе
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Group_Click(object sender, RoutedEventArgs e)
		{
			
		}
		/// <summary>
		/// Метод обновляющий список групп в левой панели
		/// </summary>
		public void UpdatePanelGroup()
		{
			PanelGroup.Children.Clear();
			PanelGroup.RowDefinitions.Clear();
			int k = 0;
			foreach(GroupProgram groupProgram in GlobalParam.GlobalGroupProgram)
			{
				Button button = new Button();
				button.MinHeight = AddGroup.MinHeight;
				button.MinWidth = AddGroup.MinWidth;
				button.MaxHeight = AddGroup.MaxHeight;
				button.MaxWidth = AddGroup.MaxWidth;
				button.Margin = new Thickness(3);
				button.HorizontalAlignment =  HorizontalAlignment.Center;
				button.VerticalAlignment = VerticalAlignment.Top;
				button.ToolTip = groupProgram.NameGroup;
				var brush = new ImageBrush();
				brush.ImageSource = new BitmapImage(new Uri(groupProgram.IconsGroup, UriKind.Relative));
				brush.TileMode = TileMode.None;
				button.Background = brush;
				button.OpacityMask = null;
				PanelGroup.Children.Add(button);
				RowDefinition f = new RowDefinition();
				f.MinHeight = 40;
				f.MaxHeight = 50;
				PanelGroup.RowDefinitions.Add(f);
				Grid.SetRow(button, k);
				k++;				
			}
			PanelGroup.Children.Add(AddGroup);
			PanelGroup.RowDefinitions.Add(new RowDefinition());
			Grid.SetRow(AddGroup, k+1);
		}
	}
}
