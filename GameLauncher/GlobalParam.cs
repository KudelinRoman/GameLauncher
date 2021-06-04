using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace GameLauncher
{
	/// <summary>
	/// Класс в котором содержатся глобальные переменные
	/// </summary>
	static class GlobalParam
	{
		/// <summary>
		/// Список содержащий информацию о программах включенных в группу
		/// </summary>
		private static List<InformationProgramm> _infoProg = new List<InformationProgramm> { };
		/// <summary>
		/// Лист содержащий все созданные группы программ
		/// </summary>
		private static List<GroupProgram> _groupProgram = new List<GroupProgram> { };
		/// <summary>
		/// Переменная содержащая пароль
		/// </summary>
		private static string _password = "";
		/// <summary>
		/// Флаг указывающий является ли приложение оболочкой
		/// </summary>
		private static bool _shell = false;
		/// <summary>
		/// Флаг указывающий отображается ли контекстное меню у групп
		/// </summary>
		private static bool _contextMenuInGroup = false;
		/// <summary>
		/// Массив хранящий время начала и время завершения работы на ПК
		/// </summary>
		private static int[,] _masTime = new int[2, 2];



		/// <summary>
		/// Возвращает или задает список информации и программах включенных в группу
		/// </summary>
		public static List<InformationProgramm> GlobalInfoProg
		{
			get { return _infoProg; }
			set { _infoProg = value; }
		}
		/// <summary>
		/// Возвращает или задает список групп приложений
		/// </summary>
		public static List<GroupProgram> GlobalGroupProgram
		{
			get { return _groupProgram; }
			set { _groupProgram = value; }
		}
		/// <summary>
		/// Возвращает или задает текущий пароль
		/// </summary>
		public static string Password
		{
			get { return _password; }
			set { _password = value; }
		}
		/// <summary>
		/// Возвращает или задает является ли приложение текущей оболочкой ПК
		/// </summary>
		public static bool Shell
		{
			get { return _shell; }
			set { _shell = value; }
		}
		/// <summary>
		/// Возвращает или задает флаг, указывающий отображается ли контекстное меню у групп программ
		/// </summary>
		public static bool ContextMenuInGroup
		{
			get { return _contextMenuInGroup; }
			set { _contextMenuInGroup = value; }
		}
		/// <summary>
		/// Возвращает или задает массив времени начала и завершения работы за ПК
		/// </summary>
		public static int[,] MassTime
		{
			get { return _masTime; }
			set { _masTime = value; }
		}
		/// <summary>
		/// Метод записывающий все переменные в файлы
		/// </summary>
		public static void SaveList()
		{
			//сохранение листа с группами
			XmlSerializer formatter = new XmlSerializer(typeof(List<GroupProgram>));

			using (FileStream fs = new FileStream("SavedGroup.xml", FileMode.Create))
			{
				formatter.Serialize(fs, _groupProgram);
			}
			//сохранение пароля
			XmlSerializer formatterPass = new XmlSerializer(typeof(string));

			using (FileStream fs = new FileStream("SavedPass.xml", FileMode.Create))
			{
				formatterPass.Serialize(fs, _password);
			}
			//сохранение флага
			XmlSerializer formatterShell = new XmlSerializer(typeof(bool));

			using (FileStream fs = new FileStream("SavedShell.xml", FileMode.Create))
			{
				formatterShell.Serialize(fs, _shell);
			}
			//сохранение массива времени
			XmlSerializer formatterTime = new XmlSerializer(typeof(string));
			string str = _masTime[0, 0] + "+" + _masTime[0, 1] + "+" + _masTime[1, 0] + "+" + _masTime[1, 1];
			using (FileStream fs = new FileStream("SavedTime.xml", FileMode.Create))
			{
				formatterTime.Serialize(fs, str);
			}

		}
		/// <summary>
		/// Метод считывающий ранее записанные файлы в соответствующие переменные
		/// </summary>
		public static void LoadLists()
		{
			//запись листа с группами из файла
			const string savePathPrice = "SavedGroup.xml";
			if (File.Exists(savePathPrice))
			{
				XmlSerializer formatter = new XmlSerializer(typeof(List<GroupProgram>));
				try
				{
					using (FileStream fs = new FileStream(savePathPrice, FileMode.Open, FileAccess.Read))
					{
						_groupProgram = (List<GroupProgram>)formatter.Deserialize(fs);
					}
				}
				catch
				{

				}
			}
			//запись пароля из файла
			const string savePathPass = "SavedPass.xml";
			if (File.Exists(savePathPass))
			{
				XmlSerializer formatter = new XmlSerializer(typeof(string));
				try
				{
					using (FileStream fs = new FileStream(savePathPass, FileMode.Open, FileAccess.Read))
					{
						_password = (string)formatter.Deserialize(fs);
					}
				}
				catch
				{

				}
			}
			//запись флага из файла
			const string savePathShell = "SavedShell.xml";
			if (File.Exists(savePathShell))
			{
				XmlSerializer formatter = new XmlSerializer(typeof(bool));
				try
				{
					using (FileStream fs = new FileStream(savePathShell, FileMode.Open, FileAccess.Read))
					{
						_shell = (bool)formatter.Deserialize(fs);
					}
				}
				catch
				{

				}
			}
			//запись массива времени из файла
			const string savePathTime = "SavedTime.xml";
			if (File.Exists(savePathTime))
			{
				XmlSerializer formatter = new XmlSerializer(typeof(string));
				try
				{
					using (FileStream fs = new FileStream(savePathTime, FileMode.Open, FileAccess.Read))
					{
						string str = (string)formatter.Deserialize(fs);
						string[] mas = new string[4];
						mas =  str.Split('+');
						_masTime[0, 0] = Convert.ToInt32(mas[0]);
						_masTime[0, 1] = Convert.ToInt32(mas[1]);
						_masTime[1, 0] = Convert.ToInt32(mas[2]);
						_masTime[1, 1] = Convert.ToInt32(mas[3]);
					}
					
				}
				catch
				{

				}
			}
		}

	}
}
