using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;

namespace classroomLibrary.Data.Interfaces
{
	public interface IGroups
	{
		IEnumerable<Group> Groups { get; }
		Group Get(int id);
        Task Create(Group group);
        Task Remove(Group group);
		IEnumerable<Group> FindGrouptByTitle(string title);
		IEnumerable<Group> FindGroupByDirection(string direction);
		IEnumerable<Group> FindGroupByFaculty(string faculty);
		IEnumerable<Group> FindGroupByEnd(DateTime dateTime);
		IEnumerable<Group> FindGroupByStart(DateTime dateTime);
		IEnumerable<Group> FindGroupByClassroom(University university);
        Task<Group> Edit(Group group);
        IQueryable<Group> GetAll();
    }
}
