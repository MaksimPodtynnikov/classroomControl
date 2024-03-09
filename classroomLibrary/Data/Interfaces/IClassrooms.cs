using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;

namespace classroomLibrary.Data.Interfaces
{
	public interface IClassrooms
	{
		IEnumerable<Classroom> Classrooms { get; }
		Classroom Get(int id);
		IEnumerable<Classroom> FindClassroomsByCity(City city);
        Task Remove(Classroom classroom);
        Task Create(Classroom classroom);
        Task<Classroom> Edit(Classroom classroom);
        IQueryable<Classroom> GetAll();

    }
}
