using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class Department
	{
		public int id { set; get; }
		public string title { set; get; }
        public int cityId { set; get; }
        public virtual City city { set; get; }
        public List<Post> posts { set; get; }
        public List<Classroom> classrooms { set; get; }
		public override string ToString()
		{
			return title;
		}
	}
}
