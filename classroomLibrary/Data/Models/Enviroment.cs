using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class Enviroment
	{
		public int id { set; get; }
		public string title { set; get; }
		public string? description { set; get; }
		public List<ClassEnvironment> classes { set; get; }
		public override string ToString()
		{
			return title;
		}
	}
}
