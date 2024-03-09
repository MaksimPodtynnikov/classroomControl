using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
    public class ClassEnviromentRepository : IClassEnvironments
    {
        private readonly AppDBContent appDBContent;
        public ClassEnviromentRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<ClassEnvironment> ClassEnvironments => appDBContent.ClassEnvironments.Include(c => c.classroom).Include(c => c.enviroment);

		public async Task Create(ClassEnvironment classEnviroment)
		{
            await appDBContent.ClassEnvironments.AddAsync(classEnviroment);
            await appDBContent.SaveChangesAsync();
        }


		public async Task<ClassEnvironment> Edit(ClassEnvironment classEnviroment)
		{
            appDBContent.ClassEnvironments.Update(classEnviroment);
            await appDBContent.SaveChangesAsync();

            return classEnviroment;
        }

		public IEnumerable<ClassEnvironment> FindClassEnvironmentByClass(Classroom classroom)
		{
			return appDBContent.ClassEnvironments.Where(c=>c.classroom.id == classroom.id);
		}

		public IEnumerable<ClassEnvironment> FindClassEnvironmentByEnvironment(Enviroment enviroment)
		{
			return appDBContent.ClassEnvironments.Where(c => c.enviroment.id == enviroment.id);
		}

		public ClassEnvironment Get(int id) => appDBContent.ClassEnvironments.Include(c=>c.classroom).Include(c => c.enviroment).FirstOrDefault(p => p.id == id);

        public IQueryable<ClassEnvironment> GetAll() => appDBContent.ClassEnvironments.Include(c => c.classroom).Include(c => c.enviroment);

        public async Task<bool> Remove(ClassEnvironment classEnvironment)
		{
            try
            {
                appDBContent.ClassEnvironments.Remove(classEnvironment);
                await appDBContent.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
	}
}
