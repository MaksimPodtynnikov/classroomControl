using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
    public class EventGroupRepository: IEventGroups
    {
        private readonly AppDBContent appDBContent;
        public EventGroupRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<EventGroup> EventGroups => appDBContent.EventGroups.Include(v => v.group).Include(v => v.evento);

		public async Task ClearForEvent(int id)
		{
			appDBContent.EventGroups.RemoveRange(appDBContent.EventGroups.Where(v => v.eventoId == id));
			await appDBContent.SaveChangesAsync();
		}
		public IQueryable<EventGroup> GetAll() => appDBContent.EventGroups.Include(v => v.group).Include(v => v.evento);

		public async Task Create(EventGroup eventGroup)
		{
            await appDBContent.EventGroups.AddAsync(eventGroup);
            await appDBContent.SaveChangesAsync();
        }

		public async Task<EventGroup> Edit(EventGroup eventGroup)
		{
            appDBContent.EventGroups.Update(eventGroup);
            await appDBContent.SaveChangesAsync();

            return eventGroup;
        }

		public IEnumerable<EventGroup> FindEventGroupByEvent(Event evento)
		{
			return appDBContent.EventGroups.Where(c => c.eventoId == evento.id).ToList();
		}

		public IEnumerable<EventGroup> FindEventGroupByGroup(Group group)
		{
			return appDBContent.EventGroups.Where(c => c.groupId == group.id).ToList();
		}

		public EventGroup Get(int id)=> appDBContent.EventGroups.Include(v => v.group).Include(v => v.evento).FirstOrDefault(p => p.id == id);

		public async Task Remove(EventGroup eventGroup)
		{
            appDBContent.EventGroups.Remove(eventGroup);
            await appDBContent.SaveChangesAsync();
        }
	}
}
