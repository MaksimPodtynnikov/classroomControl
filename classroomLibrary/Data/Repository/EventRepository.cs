using Microsoft.EntityFrameworkCore;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
    public class EventRepository : IEvents
    {
        private readonly AppDBContent appDBContent;
        public EventRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<Event> Events => appDBContent.Events.Include(v => v.worker).Include(v => v.classroom);

        public async Task Create(Event @event)
        {
            await appDBContent.Events.AddAsync(@event);
            await appDBContent.SaveChangesAsync();
        }
        public IQueryable<Event> GetAll() => appDBContent.Events.Include(v => v.worker).Include(v => v.classroom);

        public async Task<Event> Edit(Event evento)
		{
            appDBContent.Events.Update(evento);
            await appDBContent.SaveChangesAsync();

            return evento;
        }

		public IEnumerable<Event> FindEventByClassroom(Classroom classroom)
		{
			return appDBContent.Events.Where(c => c.classroomId == classroom.id).ToList();
		}


		public IEnumerable<Event> FindEventByLesson(int lesson)
		{
			return appDBContent.Events.Where(c => c.lessonNumber == lesson).ToList();
		}

		public IEnumerable<Event> FindEventByTitle(string title)
		{
			return appDBContent.Events.Where(c => c.title == title).ToList();
		}

		public IEnumerable<Event> FindEventByType(string type)
		{
			return appDBContent.Events.Where(c => c.type == type).ToList();
		}

		public IEnumerable<Event> FindEventByWorker(Worker worker)
		{
			return appDBContent.Events.Where(c => c.workerId == worker.id).ToList();
		}

		public Event Get(int id) => appDBContent.Events.Include(v => v.worker).Include(v => v.classroom).Include(v => v.eventGroups).ThenInclude(c => c.group).FirstOrDefault(p => p.id == id);

        public IQueryable<Event> FindEventByGroupID(int idGroup)
        {
            return appDBContent.EventGroups.Where(c => c.groupId == idGroup).Include(c=>c.evento.classroom).Include(c => c.evento.worker).Select(c => c.evento).Where(c => c.date >= DateTime.Now && c.date < DateTime.Now.AddDays(7));
        }

		public async Task Remove(Event @event)
		{
            appDBContent.Events.Remove(@event);
            await appDBContent.SaveChangesAsync();
        }
	}
}
