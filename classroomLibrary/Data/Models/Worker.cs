using classroomLibrary.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Models
{
	public class Worker:Person
	{
		public string login {  get; set; }
		public string password { get; set; }
		public string? mail { get; set; }
		public string? phone { set; get; }
		public string? oneC_id { set; get; }
		public int postId { set; get; }
		public virtual Post post { set; get; }
		public List<Event> events { set; get; }
		public Role role { set; get; }
	}
}
