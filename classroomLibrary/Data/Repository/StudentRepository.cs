using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
	public class StudentRepository : IStudents
    {
        private readonly AppDBContent appDBContent;
        public StudentRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<Student> Students =>appDBContent.Students.Include(v => v.group);
        public IQueryable<Student> GetAll() => appDBContent.Students.Include(v => v.group);
        public async Task Create(Student student)
		{
            await appDBContent.Students.AddAsync(student);
            await appDBContent.SaveChangesAsync();
        }

		public async Task<Student> Edit(Student student)
		{
            appDBContent.Students.Update(student);
            await appDBContent.SaveChangesAsync();

            return student;
        }

		public IEnumerable<Student> FindStudentByGroup(Group group)
		{
			return appDBContent.Students.Where(c=>c.groupId==group.id);
		}

		public IEnumerable<Student> FindStudentByName(string name, string family, string patronymic)
		{
			return appDBContent.Students.Where(c => c.name == name || c.family == family || c.patronymic == patronymic).ToList();
		}

		public Student Get(int id) => appDBContent.Students.Include(v => v.group).FirstOrDefault(p => p.id == id);

		public async Task Remove(Student student)
		{
            appDBContent.Students.Remove(student);
            await appDBContent.SaveChangesAsync();
        }
	}
}
