﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncher
{
	static class GlobalParam
	{
		private static List<InformationProgramm> _infoProg = new List<InformationProgramm> { };
		private static List<GroupProgram> _groupProgram = new List<GroupProgram> { };
		private static String _nameAdmin;
		private static String _shift;
		private static List<String> _user = new List<String> { };
		private static String _timeBroni;
		private static String _timeBroniNoch;
		private static Boolean _OplataBroniNoch;

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
		

		
	}
}
