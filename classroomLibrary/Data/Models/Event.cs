using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class Event
	{
		public int id { set; get; }
		public string title { set; get; }
		public string? type { set; get; }
		public DateTime date { set; get; }
		public int lessonNumber { set; get; }
		public int workerId { set; get; }
		public virtual Worker worker { set; get; }
		public int classroomId { set; get; }
		public virtual Classroom classroom { get; set; }
		public List<EventGroup> eventGroups { set; get; }
		public override string ToString()
		{
			return $"{title} ({type})\n{date:d} {getLesson()}\n{worker.getNameShort()} - {classroom}";
		}
		public string getLesson()
		{
			switch(lessonNumber)
			{
				case 1: return "8:30 - 10:05";
				case 2: return "10:15 - 11:50";
				case 3: return "12:10 - 13:45";
				case 4: return "13:55 - 15:30";
				case 5: return "15:50 - 17:25";
				case 6: return "17:35 - 19:10";
				default: return "не установлено";
			}
		}
	}
}
