using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace GameLauncher
{
	/// <summary>
	/// Логика взаимодействия для InformProgram.xaml
	/// </summary>
	public partial class InformProgram : Window
	{
		string filename = "";
		string filenameImage = "";
		InformationProgramm information;
		public InformProgram()
		{
			InitializeComponent();
		}
		public InformProgram(InformationProgramm information)
		{
			InitializeComponent();
			this.information = information;
			NameProg.Text = information.NameProgramm;
			DescriotionsProg.Text = information.DescriptionProgram;
			PatchProg.Text = information.LocationExeFile;
			ImgProg.Source = BitmapFrame.Create(new Uri(information.IconsProg));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.Filter = "Исполняемый файл (*.lnk)|*.lnk";
			dialog.FilterIndex = 1;
			bool? result = dialog.ShowDialog();

			if (result == true)
			{
				// Open document
				filename = dialog.FileName;
				try
				{
					PatchProg.Text = filename;
					using (Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(filename))
					{
						ImgProg.Source = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
					}
				}
				catch (Exception)
				{
					MessageBox.Show("Неверный формат");
					filename = "";
				}
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.Filter = "Изображения (*.png)|*.png|Иконки (*.ico)|*.ico|Всё (*.*)|*.*";
			dialog.FilterIndex = 1;
			bool? result = dialog.ShowDialog();

			if (result == true)
			{
				// Open document
				filenameImage = dialog.FileName;
				try
				{
					ImgProg.Source = BitmapFrame.Create(new Uri(filenameImage));
				}
				catch (Exception)
				{
					MessageBox.Show("Неверный формат изображения");
					filenameImage = "";
				}
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			if(NameProg.Text.Replace(" ", "") != ""&& PatchProg.Text.Replace(" ", "") != ""&& ImgProg.Source!=null)
			{
				InformationProgramm informationProgramm = new InformationProgramm(NameProg.Text, PatchProg.Text, CopyImg(), DescriotionsProg.Text);
				GlobalParam.GlobalInfoProg.Add(informationProgramm);
				this.Close();
			}
			else
			{
				MessageBox.Show("Проверьте правильность заполнения всех полей!");
			}
		}
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
		/// <summary>
		/// Метод копирующий картинку в каталог программы
		/// </summary>
		/// <returns> Полный путь к картинке</returns>
		private string CopyImg()
		{
			String filePath = AppDomain.CurrentDomain.BaseDirectory + @"ProgImage\" + NameProg.Text + ".jpg";
			var encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create((BitmapSource)ImgProg.Source));
			using (FileStream stream = new FileStream(filePath, FileMode.Create))
				encoder.Save(stream);
			return filePath;
		}
	}
}
