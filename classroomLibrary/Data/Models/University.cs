using System.Collections.Generic;

namespace classroomLibrary.Data.Models
{
    public class University
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<Group> groups { set; get; }
		public override string ToString()
		{
			return title;
		}
	}
}
