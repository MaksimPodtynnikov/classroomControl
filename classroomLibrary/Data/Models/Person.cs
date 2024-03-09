using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class Person
	{
		public int id { set; get; }
		public string name { set; get; }
		public string family { set; get; }
		public string? patronymic { set; get; }
		public string getNameShort()
		{
			return family+" "+name.FirstOrDefault()+"."+patronymic.FirstOrDefault()+".";
		}
		public override string ToString()
		{
			return getNameShort();
		}
	}
}
