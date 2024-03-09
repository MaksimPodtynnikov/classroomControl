using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class ClassEnvironment
	{
		public int id { set; get; }
		public int count { set; get; }
		public int classroomId { set; get; }
		public virtual Classroom classroom { set; get; }
		public int enviromentId { set; get; }
		public virtual Enviroment enviroment { set; get; }
		public override string ToString()
		{
			return $"{enviroment} ({count})";
		}
	}
}
