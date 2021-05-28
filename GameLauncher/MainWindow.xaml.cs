using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;

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
		#region Dll для запуска приложений
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);
		#endregion
		/// <summary>
		/// Переменная процесса
		/// </summary>
		private Process process;
		private Process _childp;
		private IntPtr _appWin;

		//Константы
		/// <summary>
		/// Z - последовательность
		/// </summary>
		private const int SWP_ZOZORDER = 0x0004;
		/// <summary>
		/// Не активное окно. Если этот флаг не установлен, окно активируется и перемещается в начало либо самой верхней, либо не самой верхней группы
		/// </summary>
		private const int SWP_NOACTIVATE = 0x0010;
		/// <summary>
		/// Устанавливает новый стиль окна
		/// </summary>
		private const int GWL_STYLE = (-16);
		/// <summary>
		/// Окно имеет строку заголовка
		/// </summary>
		private const int WS_CAPTION = 0x00C00000;
		/// <summary>
		/// Окно изначально видно.
		/// </summary>
		private const int WS_VISIBLE = 0x10000000;
		/// <summary>
		/// Окно имеет рамку для изменения размера
		/// </summary>
		private const int WS_THICKFRAME = 0x00040000;
		const string patran = "patran";

		public MainWindow()
		{
			InitializeComponent();
			//Счтываем сохраненные файлы
			GlobalParam.LoadLists();
			UpdatePanelGroup();
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
					b.Click += ButtonProcess_MouseLeftClick;
					b.ContextMenu = (ContextMenu)Resources["contextMenuTassk"];
					PanelTasks.Children.Add(b);
					b.HorizontalAlignment = HorizontalAlignment.Center;
					ColumnDefinition c = new ColumnDefinition();
					c.MinWidth = 37;
					c.MaxWidth = 37;
					PanelTasks.ColumnDefinitions.Add(c);
					Grid.SetColumn(b, k + 1);
					k++;
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
			MinimizeOrMaximizeWindow m = new MinimizeOrMaximizeWindow();
			m.BringWindowToFront(process);
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
			Button b = new Button();
			b = (Button)sender;
			UpdateCanvasProg((int)b.Tag);
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
				button.ContextMenu = (ContextMenu)Resources["contextMenuGroupProgramm"];
				var brush = new ImageBrush();
				brush.ImageSource = new BitmapImage(new Uri(groupProgram.IconsGroup, UriKind.Relative));
				brush.TileMode = TileMode.None;
				button.Background = brush;
				button.OpacityMask = null;
				button.Tag = k;
				button.Click += Group_Click;
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
		/// <summary>
		/// Метод возвращающий кнопку, у которой было вызвано контекстное меню
		/// </summary>
		/// <param name="sender">объект MenuItem</param>
		/// <returns>Кнопка у которой было вызвано контекстное меню</returns>
		private static Button FindClickedItem(object sender)
		{
			var mi = sender as MenuItem;
			if (mi == null)
			{
				return null;
			}
			var cm = mi.CommandParameter as ContextMenu;
			if (cm == null)
			{
				return null;
			}
			return cm.PlacementTarget as Button;
		}
		/// <summary>
		/// Обработчик кнопки контекстного меню, закрывания задачи
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Close_OnClick(object sender, RoutedEventArgs e)
		{
			var clickedItem = FindClickedItem(sender);
			if (clickedItem != null)
			{
				Button b = clickedItem;
				localAll[(int)b.Tag].Kill();
			}
		}
		/// <summary>
		/// Обработчик кнопки контекстного меню, удаления группы программ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Delete_OnClick(object sender, RoutedEventArgs e)
		{
			var clickedItem = FindClickedItem(sender);
			if (clickedItem != null)
			{
				Button b = clickedItem;
				int k = 0;
				foreach (GroupProgram groupProgram in GlobalParam.GlobalGroupProgram.ToArray())
				{
					if (k ==(int)b.Tag)
					{
						GlobalParam.GlobalGroupProgram.Remove(groupProgram);
					}
					k++;
				}
				UpdatePanelGroup();
			}
		}
		/// <summary>
		/// Обработчик кнопки контекстного меню, редактирования группы программ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Edit_OnClick(object sender, RoutedEventArgs e)
		{
			var clickedItem = FindClickedItem(sender);
			if (clickedItem != null)
			{
				Button b = clickedItem;
				int k = 0;
				foreach (GroupProgram groupProgram in GlobalParam.GlobalGroupProgram.ToArray())
				{
					if (k == (int)b.Tag)
					{
						NewGroup newGroup = new NewGroup(groupProgram);
						newGroup.ShowDialog();
					}
					k++;
				}
				UpdatePanelGroup();
			}
		}
		/// <summary>
		/// Метод обновляющий список программ в соответствии с индексом выбранной группы
		/// </summary>
		/// <param name="index">Индекс выбранной группы</param>
		private void UpdateCanvasProg(int index)
		{
			//Удаляем содержимое стекпанел
			CanvasProg.Children.Clear();
			CanvasProg.Orientation = Orientation.Horizontal;
			CanvasProg.CanHorizontallyScroll = true;
			double countProg = GlobalParam.GlobalGroupProgram[index].ProgramInfo.Count;
			double widthCanvas = CanvasProg.ActualWidth;
			double headthCanvas = CanvasProg.ActualHeight/2;
			foreach(InformationProgramm informationProgramm in GlobalParam.GlobalGroupProgram[index].ProgramInfo)
			{
				Grid grid = new Grid();
				grid.Height = headthCanvas + 90;
				grid.VerticalAlignment = VerticalAlignment.Center;
				Button pri = new Button();
				pri.ToolTip = informationProgramm.DescriptionProgram;
				pri.Height = headthCanvas;
				pri.Width = headthCanvas;
				var brush = new ImageBrush();
				brush.ImageSource = new BitmapImage(new Uri(informationProgramm.IconsProg, UriKind.Relative));
				brush.TileMode = TileMode.None;
				pri.OpacityMask = null;
				pri.Background = brush;
				pri.Padding = new Thickness(10);
				pri.Margin = new Thickness(10);
				pri.BorderThickness = new Thickness(0);
				pri.Effect = new System.Windows.Media.Effects.DropShadowEffect();
				pri.Click += Start_Pril_Click;
				pri.Tag = informationProgramm.LocationExeFile;

				Label namePri = new Label();
				namePri.FontFamily = new System.Windows.Media.FontFamily("MV Boli");
				namePri.FontSize = 26;
				namePri.Content = informationProgramm.NameProgramm;
				namePri.HorizontalAlignment = HorizontalAlignment.Center;
				namePri.VerticalAlignment = VerticalAlignment.Bottom;
				namePri.Effect = new System.Windows.Media.Effects.DropShadowEffect();
				grid.Children.Add(pri);
				grid.Children.Add(namePri);
				CanvasProg.Children.Add(grid);

			}
			CanvasProg.UpdateLayout();
		}
		/// <summary>
		/// Обработчик события "Клик" кнопки перезагрузки. Перезагружает ПК.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("shutdown", "/r /t 0");
		}
		/// <summary>
		/// Обработчик события "Клик" кнопки выключения. Выключает ПК.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			Process.Start("shutdown", "/s /t 0");
		}
		/// <summary>
		/// Обработчик кнопки запуска приложения
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Start_Pril_Click(object sender, RoutedEventArgs e)
		{
			//извлекаем из тега нажатой кнопки путь к исполняемому файлу
			Button b = (Button)sender;
			string pathExeFile = b.Tag.ToString();

			string exeName = pathExeFile;
			var procInfo = new System.Diagnostics.ProcessStartInfo(exeName);
			procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(exeName);
			procInfo.WindowStyle = ProcessWindowStyle.Normal;
			// Start the process 
			_childp = Process.Start(procInfo);
			System.Windows.Forms.Panel _pnlSched = new System.Windows.Forms.Panel();
			WindowsFormsHost windowsFormsHost1 = new WindowsFormsHost();

			windowsFormsHost1.Child = _pnlSched;

			ApplicationDock.Children.Add(windowsFormsHost1);

			// Wait for process to be created and enter idle condition 
			// _childp.WaitForInputIdle(); 
			// The main window handle may be unavailable for a while, just wait for it 
			while (_childp.MainWindowHandle == IntPtr.Zero)
			{
				Thread.Yield();
			}

			// Get the main handle 
			_appWin = _childp.MainWindowHandle;
			//  PR.WaitForInputIdle(); // true if the associated process has reached an idle state. 
			SetParent(_appWin, _pnlSched.Handle); // loading exe to the wpf window. 


			//this.process = Process.Start(pathExeFile);
			//this.process.WaitForInputIdle();

			//var helper = new WindowInteropHelper(GetWindow(this.ApplicationDock));

			//SetParent(this.process.MainWindowHandle, helper.Handle);

			//int style = GetWindowLong(this.process.MainWindowHandle, GWL_STYLE);
			//style = style & ~WS_CAPTION & ~WS_THICKFRAME;
			//SetWindowLong(this.process.MainWindowHandle, GWL_STYLE, style);
			//ResizeEmbeddedApp();
		}

		private void ResizeEmbeddedApp()
		{
			if (this.process == null)
			{
				return;
			}

			UIElement container = VisualTreeHelper.GetParent(this.ApplicationDock) as UIElement;
			System.Windows.Point relativeLocation = this.ApplicationDock.TranslatePoint(new System.Windows.Point(0, 0), container);
		}

		protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
		{
			System.Windows.Size size = base.MeasureOverride(availableSize);
			ResizeEmbeddedApp();
			return size;
		}
		/// <summary>
		/// Открытие формы ввода пароля
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			Password p = new Password();
			p.ShowDialog();
		}
	}
}
