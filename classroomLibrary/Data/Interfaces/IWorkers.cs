using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace classroomLibrary.Data.Interfaces
{
	public interface IWorkers
	{
		IEnumerable<Worker> Workers { get; }
		Worker Get(int id);
        Task Create(Worker worker);
        Task Remove(Worker worker);
		IEnumerable<Worker> FindWorkerByPost(Post post);
        Task<Worker> Edit(Worker worker);
		IEnumerable<Worker> FindWorkerByName(string name, string family, string patronymic);
        IQueryable<Worker> GetAll();
    }
}
