using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class Post
	{
		public int id { set; get; }
		public string title { set; get; }
		public int departmentId { set; get; }
		public Department department { set; get; }
		public List<Worker> workers { set; get; }
		public override string ToString()
		{
			return title;
		}
	}
}
