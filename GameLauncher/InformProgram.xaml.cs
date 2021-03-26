using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;

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
				//не факт что работает)))
				BitmapSource bitmap1 = (BitmapSource)ImgProg.Source;
				var width = bitmap1.PixelWidth;
				var height = bitmap1.PixelHeight;
				var stride = width * ((bitmap1.Format.BitsPerPixel + 7) / 8);
				var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
				bitmap1.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
				var bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, memoryBlockPointer);
				InformationProgramm informationProgramm = new InformationProgramm(NameProg.Text, PatchProg.Text, bitmap, DescriotionsProg.Text);
				GlobalParam.GlobalInfoProg.Add(informationProgramm);
				this.Close();
			}
			else
			{
				MessageBox.Show("Проверьте правильность заполнения всех полей!");
			}
		}
	}
}
