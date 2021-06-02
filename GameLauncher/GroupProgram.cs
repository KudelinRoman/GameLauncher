using System;
using System.Collections.Generic;

namespace GameLauncher
{
	/// <summary>
	/// Класс представляющий группу программ
	/// </summary>
	[Serializable]
	public class GroupProgram
	{
		/// <summary>
		/// Переменная хранящая название группы
		/// </summary>
		public string NameGroup { get; set; }
		/// <summary>
		/// Переменная хранящая иконку для группы
		/// </summary>
		public string IconsGroup { get; set; }
		/// <summary>
		/// Переменная хранящяя краткую информацию о группе
		/// </summary>
		public string DescriptionGroup { get; set; }
		/// <summary>
		/// Лист, содержащий экземпляры класса сведений о программах, включенных в эту группу
		/// </summary>
		public List<InformationProgramm> ProgramInfo = new List<InformationProgramm>();
		/// <summary>
		/// Конструктор определяющий экземпляр данного класса
		/// </summary>
		/// <param name="Name">Название группы</param>
		/// <param name="Icon">Иконка для группы</param>
		/// <param name="Descriptions">Краткое описание</param>
		/// <param name="InformationProgramms">Список сведений о включенных в группу программ</param>
		public GroupProgram(string Name, string Icon, string Descriptions, List<InformationProgramm> InformationProgramms)
		{
			this.NameGroup = Name;
			this.IconsGroup = Icon;
			this.DescriptionGroup = Descriptions;
			this.ProgramInfo = InformationProgramms;
		}
		public GroupProgram() { }
		/// <summary>
		/// Конструктор определяющий экземпляр данного класса
		/// </summary>
		/// <param name="Name">Название группы</param>
		/// <param name="Icon">Иконка для группы</param>
		/// <param name="Descriptions">Краткое описание</param>
		public GroupProgram(string Name, string Icon, string Descriptions)
		{
			this.NameGroup = Name;
			this.IconsGroup = Icon;
			this.DescriptionGroup = Descriptions;
			this.ProgramInfo = null;
		}		
	}
}
