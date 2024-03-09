using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class City
	{
		public int id { set; get; }
		public string title { set; get; }
		public string? city { set; get; }
		public List<Department> departments { set; get; }
		public override string ToString()
		{
			return title;
		}
	}

}
