using System;
using classroomLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace classroomLibrary.Data.Interfaces
{
	public interface IEvents
	{
		IEnumerable<Event> Events { get; }
        IQueryable<Event> FindEventByGroupID(int idGroup);
		Event Get(int id);
        Task Create(Event @event);
        Task Remove(Event @event);
		IEnumerable<Event> FindEventByTitle(string title);
		IEnumerable<Event> FindEventByType(string type);
		IEnumerable<Event> FindEventByLesson(int lesson);
		IEnumerable<Event> FindEventByWorker(Worker worker);
		IEnumerable<Event> FindEventByClassroom(Classroom classroom);
        Task<Event> Edit(Event evento);
        IQueryable<Event> GetAll();
    }
}
