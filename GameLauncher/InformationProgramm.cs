using System;
using System.Drawing;

namespace GameLauncher
{
	/// <summary>
	/// Класс, представляющий сведеия о программе
	/// </summary>
	[Serializable]
	public class InformationProgramm
	{
		/// <summary>
		/// Переменная хранящая название программы
		/// </summary>
		public string NameProgramm { get; set; }
		/// <summary>
		/// Переменная хранящая расположение exe-файла для запуска программы
		/// </summary>
		public string LocationExeFile { get; set; }
		/// <summary>
		/// Переменная хранящяя изображение ярлыка для программы.
		/// </summary>
		public string IconsProg { get; set; }
		/// <summary>
		/// Переменная хранящая краткое описание программы.
		/// </summary>
		public string DescriptionProgram { get; set; }
		/// <summary>
		/// Конструктор определяющий экземпляр данного класса
		/// </summary>
		/// <param name="Name">Название</param>
		/// <param name="LocationExeFile">Расположение exe-файла программы</param>
		/// <param name="Icons">Изображение, если null, то применяется изображение exe-файла</param>
		/// <param name="Desription">Краткое описание</param>
		public InformationProgramm(string Name, string LocationExeFile, string Icons, string Desription)
		{
			this.NameProgramm = Name;
			this.LocationExeFile = LocationExeFile;
			if (Icons == null)
			{
				string filePath = AppDomain.CurrentDomain.BaseDirectory + @"ProgImage\" + Name + ".jpg";
				Bitmap b = (Bitmap)Bitmap.FromFile(LocationExeFile);
				b.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			else
			{
				this.IconsProg = Icons;
			}
			this.DescriptionProgram = Desription;
		}
		/// <summary>
		/// Пустой конструктор класса
		/// </summary>
		public InformationProgramm() { }
	}
}
