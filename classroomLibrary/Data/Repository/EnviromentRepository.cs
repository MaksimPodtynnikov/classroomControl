using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
    public class EnviromentRepository: IEnvironments
    {
        private readonly AppDBContent appDBContent;
        public EnviromentRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }
        public IQueryable<Enviroment> GetAll() => appDBContent.Enviroments;

        public IEnumerable<Enviroment> Enviroments => appDBContent.Enviroments;

		public async Task Create(Enviroment enviroment)
		{
            await appDBContent.Enviroments.AddAsync(enviroment);
            await appDBContent.SaveChangesAsync();
        }

		public async Task<Enviroment> Edit(Enviroment enviroment)
		{
            appDBContent.Enviroments.Update(enviroment);
            await appDBContent.SaveChangesAsync();

            return enviroment;
        }

		public IEnumerable<Enviroment> FindEnviromentByDescription(string description)
		{
			return appDBContent.Enviroments.Where(c => c.description == description).ToList();
		}

		public IEnumerable<Enviroment> FindEnviromentByTitle(string title)
		{
			return appDBContent.Enviroments.Where(c => c.title == title).ToList();
		}

		public Enviroment Get(int id)=> appDBContent.Enviroments.Include(v => v.classes).FirstOrDefault(p => p.id == id);

		public async Task Remove(Enviroment enviroment)
		{
            appDBContent.Enviroments.Remove(enviroment);
            await appDBContent.SaveChangesAsync();
        }
	}
}
