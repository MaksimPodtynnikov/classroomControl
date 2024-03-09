using classroomLibrary.Data.Models;
using System.Collections.Generic;

namespace classroomLibrary.Data.Interfaces
{
    public interface IEventGroups
    {
        IEnumerable<EventGroup> EventGroups { get; }
        EventGroup Get(int id);
        Task Remove(EventGroup eventGroup);
		IEnumerable<EventGroup> FindEventGroupByGroup(Group group);
		IEnumerable<EventGroup> FindEventGroupByEvent(Event evento);
        Task Create(EventGroup eventGroup);
        Task<EventGroup> Edit(EventGroup eventGroup);
		Task ClearForEvent(int id);
        IQueryable<EventGroup> GetAll();
    }
}
