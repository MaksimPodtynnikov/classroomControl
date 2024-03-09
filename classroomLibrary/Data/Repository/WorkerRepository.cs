using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
	public class WorkerRepository : IWorkers
    {
        private readonly AppDBContent appDBContent;
        public WorkerRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<Worker> Workers => appDBContent.Workers.Include(v => v.post);

        public IQueryable<Worker> GetAll() => appDBContent.Workers.Include(v => v.post);
        public async Task Create(Worker worker)
		{
			await appDBContent.Workers.AddAsync(worker);
            await appDBContent.SaveChangesAsync();
		}

		public async Task<Worker> Edit(Worker worker)
		{
            appDBContent.Workers.Update(worker);
            await appDBContent.SaveChangesAsync();
			return worker;
		}

		public IEnumerable<Worker> FindWorkerByName(string name, string family, string patronymic)
		{
			return appDBContent.Workers.Where(c => c.name == name || c.family == family || c.patronymic == patronymic).ToList();
		}

		public IEnumerable<Worker> FindWorkerByPost(Post post)
		{
			return appDBContent.Workers.Where(c => c.postId == post.id);
		}

		public Worker Get(int id) => appDBContent.Workers.Include(v => v.events).Include(v => v.post).FirstOrDefault(p=>p.id==id);

        public async Task Remove(Worker worker)
		{
			appDBContent.Workers.Remove(worker);
			await appDBContent.SaveChangesAsync();
		}
	}
}
