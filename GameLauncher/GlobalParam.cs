using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameLauncher
{
	static class GlobalParam
	{
		private static List<InformationProgramm> _infoProg = new List<InformationProgramm> { };
		private static List<GroupProgram> _groupProgram = new List<GroupProgram> { };
		private static string _password = "";
		private static bool _shell = false;
		

		public static List<InformationProgramm> GlobalInfoProg
		{
			get { return _infoProg; }
			set { _infoProg = value; }
		}
		public static List<GroupProgram> GlobalGroupProgram
		{
			get { return _groupProgram; }
			set { _groupProgram = value; }
		}
		public static string Password
		{
			get { return _password; }
			set { _password = value; }
		}
		public static bool Shell
		{
			get { return _shell; }
			set { _shell = value; }
		}

		public static void SaveList()
		{
			//сохранение листа с группами
			XmlSerializer formatter = new XmlSerializer(typeof(List<GroupProgram>));

			using (FileStream fs = new FileStream("SavedGroup.xml", FileMode.Create))
			{
				formatter.Serialize(fs, _groupProgram);
			}

		}

		public static void LoadLists()
		{
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
		}

	}
}
