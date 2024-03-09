using classroomLibrary.Data.Models;
using System.Collections.Generic;

namespace classroomLibrary.Data.Interfaces
{
    public interface IUniversities
    {
        IEnumerable<University> AllUniversities { get; }
        University Get(int id);
        Task Create(University university);
        Task Remove(University university);
		IEnumerable<University> FindUniversityByTitle(string title);
        Task<University> Edit(University university);
        IQueryable<University> GetAll();
    }
}
