using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GameLauncher
{
	/// <summary>
	/// Логика взаимодействия для NewGroup.xaml
	/// </summary>
	public partial class NewGroup : Window
	{
		private string filename = "";
		
		public NewGroup()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Конструктор выполняющий заполнение полей
		/// </summary>
		/// <param name="group">Объект группы программ</param>
		public NewGroup(GroupProgram group)
		{
			InitializeComponent();
			GlobalParam.GlobalInfoProg = group.ProgramInfo;

			Img1.Source = BitmapFrame.Create(new Uri(group.IconsGroup));
			Img2.Source = BitmapFrame.Create(new Uri(group.IconsGroup));
			Img3.Source = BitmapFrame.Create(new Uri(group.IconsGroup));

			NameGroup.Text = group.NameGroup;
			DescriptoinGrooup.Text = group.DescriptionGroup;
		}

		/// <summary>
		/// Обработчик нажатия кнопкой мыши по кнопке "Обзор", открывает проводник для выбора изображения для иконки группы.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.Filter = "Изображения (*.png)|*.png|Иконки (*.ico)|*.ico|Всё (*.*)|*.*";
			dialog.FilterIndex = 1;
			Nullable<bool> result = dialog.ShowDialog();

			if (result == true)
			{
				// Open document
				filename = dialog.FileName;
				try
				{
					Img1.Source = BitmapFrame.Create(new Uri(filename));
					Img2.Source = BitmapFrame.Create(new Uri(filename));
					Img3.Source = BitmapFrame.Create(new Uri(filename));
				}
				catch (Exception)
				{
					MessageBox.Show("Неверный формат изображения");
					filename = "";
				}
			}
		}
		/// <summary>
		/// Обработчик нажатия кнопкой мыши по кнопке "Сохранить", записывает созданную группу в список в классе GlobalParam. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				if (NameGroup.Text.Replace(" ", "") != "" && NameGroup.Text.Replace(" ", "").Length > 3)
				{
					if (filename == "") 
					{
						MessageBox.Show("Группа не может быть без иконки");
						return;
					}
					else
					{
						try
						{
							if (GlobalParam.GlobalInfoProg == null)
							{
								GroupProgram NewgroupProgram = new GroupProgram(NameGroup.Text, CopyImg(), DescriptoinGrooup.Text);
								GlobalParam.GlobalGroupProgram.Add(NewgroupProgram);
							}
							else
							{
								GroupProgram NewgroupProgram = new GroupProgram(NameGroup.Text, CopyImg(), DescriptoinGrooup.Text, GlobalParam.GlobalInfoProg);
								GlobalParam.GlobalGroupProgram.Add(NewgroupProgram);
								GlobalParam.GlobalInfoProg = null;
							}
						}
						catch (Exception)
						{
							MessageBox.Show("Неверный формат изображения");
							return;
						}
						
					}
					this.Close();
				}
				else
				{
					MessageBox.Show("Имя группы не должно быть пустым \n и должно содержать более 3 символов.");
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Произошла ошибка. \n Проверьте правильность заполнения всех полей.");
			}
			GlobalParam.SaveList();
		}
		/// <summary>
		/// Обработчик события нажатия кнопкой мыши по кнопке "Приложения", открывает форму со списком добавленных приложений
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ProgButAdd_Click(object sender, RoutedEventArgs e)
		{
			UpdateListProgram updateList = new UpdateListProgram();
			updateList.ShowDialog();
		}
		/// <summary>
		/// Метод копирующий картинку в каталог программы
		/// </summary>
		/// <returns> Полный путь к картинке</returns>
		private string CopyImg()
		{
			String filePath = AppDomain.CurrentDomain.BaseDirectory + @"groupImage\"+ NameGroup.Text + ".jpg" ;
			if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "groupImage")))
			{
				Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "groupImage"));
			}
			var encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create((BitmapSource)Img3.Source));
			using (FileStream stream = new FileStream(filePath, FileMode.Create))
				encoder.Save(stream);
			return filePath;
		}
	}
}
