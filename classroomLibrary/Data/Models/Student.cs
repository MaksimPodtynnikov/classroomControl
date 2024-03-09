using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class Student: Person
	{
		public int? telegram_id { set; get; }
		public int groupId { set; get; }
		public virtual Group group { set; get; }
	}
}
