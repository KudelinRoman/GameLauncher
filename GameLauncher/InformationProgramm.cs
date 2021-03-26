using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncher
{
	/// <summary>
	/// Класс, представляющий сведеия о программе
	/// </summary>
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
		public Image IconsProg { get; set; }
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
		public InformationProgramm(string Name, string LocationExeFile, Image Icons, string Desription)
		{
			this.NameProgramm = Name;
			this.LocationExeFile = LocationExeFile;
			if (Icons == null)
			{
				this.IconsProg = Bitmap.FromFile(LocationExeFile);
			}
			else
			{
				this.IconsProg = Icons;
			}
			this.DescriptionProgram = Desription;
		}
	}
}
