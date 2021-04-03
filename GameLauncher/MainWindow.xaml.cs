using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameLauncher
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Process[] localAll;
		public MainWindow()
		{
			InitializeComponent();
			//Bitmap bmp = default(Bitmap);
			//bmp = new Bitmap(System.Drawing.Icon.ExtractAssociatedIcon(@"C:\Program Files (x86)\Steam\steam.exe").ToBitmap());
			//var brush = new ImageBrush();
			//using (Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(@"C:\Program Files (x86)\Steam\steam.exe"))
			//{
			//	brush.ImageSource = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			//}
			//Steam.Background = brush;
			localAll = Process.GetProcesses();
			string s = "";
			List<int> idProc = new List<int>();
			int k = 0;
			foreach (Process p in localAll)
			{
				if (p.MainWindowTitle.ToString() != "")
				{
					s += p.MainWindowTitle.ToString() + "\n";
					idProc.Add(k);
				}
				k++;
			}
			k = 0;
			PanelTasks.ColumnDefinitions.Add(new ColumnDefinition());
			foreach (int i in idProc)
			{
				Button b = new Button();
				b.Width = 33;
				b.Height = 33;
				b.ToolTip = localAll[i].MainWindowTitle;
				b.Tag = i;
				PanelTasks.Children.Add(b);
				b.HorizontalAlignment = HorizontalAlignment.Left;
				PanelTasks.ColumnDefinitions.Add(new ColumnDefinition());
				Grid.SetColumn(b, k+1);
				k++;
				b.MouseLeftButtonDown += ButtonProcess_MouseLeftClick;
				var brush = new ImageBrush();
				//using (Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(localAll[i].MainModule.FileName.ToString()))
				//{
				//	brush.ImageSource = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				//}
				//b.Background = brush;
			}
		}
		private void MyWindow_Loaded(object sender, RoutedEventArgs e)
		{
			foreach(GroupProgram group in GlobalParam.GlobalGroupProgram)
			{
				Button button = AddGroup;
				button.ToolTip = group.NameGroup;

				Bitmap bitmap = new Bitmap(group.IconsGroup);
				
				var brush = new ImageBrush();
				brush.ImageSource = Convert(bitmap);
				button.Background = brush;
				button.VerticalAlignment = VerticalAlignment.Top;
			}
		}
		private void ButtonProcess_MouseLeftClick(object sender, RoutedEventArgs e)
		{
			Button b = (Button) sender;
			Process process = localAll[(int)b.Tag];
			
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			
		}
		//System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Steam\steam.exe");
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
		/// Конвертирует картинку
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public BitmapImage Convert(Bitmap src)
		{
			MemoryStream ms = new MemoryStream();
			((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
			BitmapImage image = new BitmapImage();
			image.BeginInit();
			ms.Seek(0, SeekOrigin.Begin);
			image.StreamSource = ms;
			image.EndInit();
			return image;
		}

		/// <summary>
		/// Метод обновляющий список групп в правой панели
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
				//var brushBorder = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,255,255,255));
				//button.BorderBrush = brushBorder;
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
