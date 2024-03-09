using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
	public class UniversityRepository : IUniversities
    {
        private readonly AppDBContent appDBContent;
        public UniversityRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }
        public IQueryable<University> GetAll() => appDBContent.Universities;
        public IEnumerable<University> AllUniversities => appDBContent.Universities;

		public async Task Create(University university)
		{
            await appDBContent.Universities.AddAsync(university);
            await appDBContent.SaveChangesAsync();
        }

		public async Task<University> Edit(University university)
		{
            appDBContent.Universities.Update(university);
            await appDBContent.SaveChangesAsync();

            return university;
        }

		public IEnumerable<University> FindUniversityByTitle(string title)
		{
			return appDBContent.Universities.Where(c => c.title == title);
		}

		public University Get(int id) => appDBContent.Universities.Include(v => v.groups).FirstOrDefault(p => p.id == id);

		public async Task Remove(University university)
		{
            appDBContent.Universities.Remove(university);
            await appDBContent.SaveChangesAsync();
        }
	}
}
