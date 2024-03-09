using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;

namespace classroomLibrary.Data.Interfaces
{
	public interface IDepartments
	{
		IEnumerable<Department> Departments { get; }
		Department Get(int id);
        Task Remove(Department department);
		IEnumerable<Department> FinDepartmentByCity(City city);
		IEnumerable<Department> FindDepartmentByTitle(string title);
        Task Create(Department department);
        Task<Department> Edit(Department department);
        IQueryable<Department> GetAll();
    }
}
