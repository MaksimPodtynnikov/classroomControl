using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;

namespace classroomLibrary.Data.Interfaces
{
	public interface IPosts
	{
		IEnumerable<Post> Posts { get; }
		Post Get(int id);
        Task Create(Post post);
        Task Remove(Post post);
		IEnumerable<Post> FindPostByTitle(string title);
		IEnumerable<Post> FindPostByDepartment(Department department);
        Task<Post> Edit(Post post);
        IQueryable<Post> GetAll();
    }
}
