using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
	public class PostRepository : IPosts
    {
        private readonly AppDBContent appDBContent;
        public PostRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<Post> Posts => appDBContent.Posts.Include(v => v.department);

		public async Task Create(Post post)
		{
            await appDBContent.Posts.AddAsync(post);
            await appDBContent.SaveChangesAsync();
        }

		public async Task<Post> Edit(Post post)
		{
            appDBContent.Posts.Update(post);
            await appDBContent.SaveChangesAsync();

            return post;
        }

		public IEnumerable<Post> FindPostByDepartment(Department department)
		{
			return appDBContent.Posts.Where(c => c.departmentId == department.id).ToList();
		}

		public IEnumerable<Post> FindPostByTitle(string title)
		{
			return appDBContent.Posts.Where(c => c.title == title).ToList();
		}

		public Post Get(int id) => appDBContent.Posts.Include(v => v.workers).Include(v => v.department).FirstOrDefault(p => p.id == id);

		public async Task Remove(Post post)
		{
            appDBContent.Posts.Remove(post);
            await appDBContent.SaveChangesAsync();
        }
        public IQueryable<Post> GetAll() => appDBContent.Posts.Include(v => v.department);
    }
}
