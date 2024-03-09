using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class Group
	{
		public int id { set; get; }
		public string title { set; get; }
		public string? direction { set; get; }
		public string? faculty { set; get; }
		public DateTime yearStart { set; get; }
		public DateTime yearEnd { set; get; }
		public List<Student> students { set; get; }
		public List<EventGroup> eventGroups { set; get; }
        public int universityId { set; get; }
        public virtual University university { set; get; }
		public override string ToString()
		{
			return title;
		}
	}
}
