using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
	public class DepartmentRepository : IDepartments
    {
        private readonly AppDBContent appDBContent;
        public DepartmentRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<Department> Departments => appDBContent.Departments.Include(c => c.city);

		public async Task Create(Department department)
		{
            await appDBContent.Departments.AddAsync(department);
            await appDBContent.SaveChangesAsync();

        }
        public IQueryable<Department> GetAll() => appDBContent.Departments.Include(c => c.city);
        public async Task<Department> Edit(Department department)
		{
            appDBContent.Departments.Update(department);
            await appDBContent.SaveChangesAsync();

            return department;
        }

		public IEnumerable<Department> FindDepartmentByTitle(string title)
		{
			return appDBContent.Departments.Where(c => c.title == title).ToList();
		}

		public IEnumerable<Department> FinDepartmentByCity(City city)
		{
			return appDBContent.Departments.Where(c => c.cityId == city.id).ToList();
		}

		public Department Get(int id)=> appDBContent.Departments.Include(v=>v.classrooms).Include(c=>c.posts).Include(c=>c.city).FirstOrDefault(p => p.id == id);

		public async Task Remove(Department department)
		{
            appDBContent.Departments.Remove(department);
            await appDBContent.SaveChangesAsync();
        }
	}
}
