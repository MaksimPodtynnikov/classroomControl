using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class Classroom
	{
		public int id { set; get; }
		public string title { set; get; }
		public string? description { set; get; }
		public int capacity { set; get; }
		public string? photo { set; get; }
		public List<Event> events { set; get; }
		public List<ClassEnvironment> enviroments { set; get; }
        public int departmentId { set; get; }
        public virtual Department department { set; get; }
		public string getAllEnvironment()
		{
			StringBuilder stringBuilder = new StringBuilder("");
			if(enviroments!=null)
				for (int i=0;i< enviroments.Count;i++) {
					stringBuilder.Append(enviroments[i].enviroment.title);
					if(i < enviroments.Count-1) 
						stringBuilder.Append(';');
				}
			return stringBuilder.ToString();
		}
		public override string ToString()
		{
			return title;
		}
	}
}
