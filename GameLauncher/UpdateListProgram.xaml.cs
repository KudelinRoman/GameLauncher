using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
	/// Логика взаимодействия для UpdateListProgram.xaml
	/// </summary>
	public partial class UpdateListProgram : Window
	{
		Grid grid = new Grid();
		public UpdateListProgram()
		{
			InitializeComponent();	
			grid.Width = BoxProg.Width;
			grid.Height = BoxProg.Height;
			grid.Margin = new Thickness(0);
			BoxProg.Content = grid;
			ColumnDefinition oneColumn = new ColumnDefinition();
			oneColumn.Width = new GridLength(30);
			//oneColumn.MinWidth = 16;
			grid.ColumnDefinitions.Add(oneColumn);
			ColumnDefinition twoColumn = new ColumnDefinition();
			twoColumn.Width = new GridLength(150);
			//twoColumn.MinWidth = 150;
			grid.ColumnDefinitions.Add(twoColumn);
			ColumnDefinition thriColumn = new ColumnDefinition();
			thriColumn.Width = new GridLength(50);
			//thriColumn.MinWidth = 50;
			grid.ColumnDefinitions.Add(thriColumn);
			ColumnDefinition fourColumn = new ColumnDefinition();
			fourColumn.Width = new GridLength(50);
			//fourColumn.MinWidth = 50;
			grid.ColumnDefinitions.Add(fourColumn);
			ColumnDefinition fColumn = new ColumnDefinition();
			fColumn.Width = GridLength.Auto;
			grid.ColumnDefinitions.Add(fColumn);
		}
		/// <summary>
		/// Метод срабатывающий при клике на кнопку "Выбрать приложение". Открывает окно для создания нового объекта.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			InformProgram informProgram = new InformProgram();
			informProgram.ShowDialog();
			Updat();
		}
		/// <summary>
		/// Метод, выводящий все объекты в бокс для отображения.
		/// </summary>
		public void Updat()
		{
			//Удаляем все объекты находящиеся на сетке
			grid.Children.Clear();
			//Удаляем все созданные ранее строки
			grid.RowDefinitions.Clear();
			int i = 0;
			RowDefinition f = new RowDefinition();
			f.MinHeight = 35;
			f.MaxHeight = 35;
			grid.RowDefinitions.Add(f);
			//Цикл в котором выводятся все объекты в бокс для отображения
			foreach (InformationProgramm infr in GlobalParam.GlobalInfoProg)
			{
				//Добавляем строку в которую будем добавлять элементы
				RowDefinition row = new RowDefinition();
				row.MinHeight = 35;
				row.MaxHeight = 35;
				grid.RowDefinitions.Add(row);
				//Создание картинки для отображения иконки
				System.Windows.Controls.Image img = new System.Windows.Controls.Image();
				img.Source = BitmapFrame.Create(new Uri(infr.IconsProg));
				img.IsEnabled = true;
				img.Visibility = Visibility.Visible;
				img.Width = 30;
				img.Height = 30;
				img.Stretch = Stretch.Fill;
				Grid.SetRow(img, i );
				Grid.SetColumn(img, 0);
				//Создание лейбла для названия программы
				Label nameProg = new Label();
				nameProg.Content = infr.NameProgramm;
				nameProg.HorizontalAlignment = HorizontalAlignment.Left;
				grid.Children.Add(nameProg);				
				Grid.SetRow(nameProg, i);
				Grid.SetColumn(nameProg, 1);
				//Создание кнопки для удаления объекта из списка
				Button buttonDelete = new Button();
				buttonDelete.Content = "Delet";
				buttonDelete.Margin = new Thickness(4);
				buttonDelete.Click += ButtonDelete_Click;
				//записываем в тег индекс объекта в листе, для дальнейшей работы с ним
				buttonDelete.Tag = i;
				grid.Children.Add(buttonDelete);
				Grid.SetRow(buttonDelete, i );
				Grid.SetColumn(buttonDelete, 2);
				//Создание кнопки для редактирования объекта 
				Button buttonUpdate = new Button();
				buttonUpdate.Content = "Updat";
				buttonUpdate.Margin = new Thickness(4);
				buttonUpdate.Click += ButtonUpdate_Click;
				//записываем в тег индекс объекта в листе, для дальнейшей работы с ним
				buttonUpdate.Tag = i;
				grid.Children.Add(buttonUpdate);
				Grid.SetRow(buttonUpdate, i );
				Grid.SetColumn(buttonUpdate, 3);
				i++;
			}			
		}


		/// <summary>
		/// Метод, удаляющий программу из списка
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonDelete_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			GlobalParam.GlobalInfoProg.RemoveAt((int)button.Tag);
			Updat();
		}
		/// <summary>
		/// Метод, открывающий форму для редактирования
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			InformProgram inf = new InformProgram(GlobalParam.GlobalInfoProg[(int)button.Tag]);
			inf.ShowDialog();
		}
		/// <summary>
		/// Конвертирует картинку
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public BitmapImage Convert(Bitmap src)
		{
			MemoryStream ms = new MemoryStream();
			((Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
			BitmapImage image = new BitmapImage();
			image.BeginInit();
			ms.Seek(0, SeekOrigin.Begin);
			image.StreamSource = ms;
			image.EndInit();
			return image;
		}
	}
}
