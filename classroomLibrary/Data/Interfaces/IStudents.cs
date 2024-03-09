using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;

namespace classroomLibrary.Data.Interfaces
{
	public interface IStudents
	{
		IEnumerable<Student> Students { get; }
		Student Get(int id);
        Task Create(Student student);
        Task Remove(Student student);
		IEnumerable<Student> FindStudentByName(string name, string family, string patronymic);
		IEnumerable<Student> FindStudentByGroup(Group group);
        Task<Student> Edit(Student student);
        IQueryable<Student> GetAll();
    }
}
