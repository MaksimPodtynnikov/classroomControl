using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;

namespace classroomLibrary.Data.Interfaces
{
	public interface IEnvironments
	{
		IEnumerable<Enviroment> Enviroments { get; }
		Enviroment Get(int id);
        Task Remove(Enviroment enviroment);
		IEnumerable<Enviroment> FindEnviromentByDescription(string description);
		IEnumerable<Enviroment> FindEnviromentByTitle(string title);
        Task Create(Enviroment enviroment);
        Task<Enviroment> Edit(Enviroment enviroment);
        IQueryable<Enviroment> GetAll();
    }
}
