using Microsoft.EntityFrameworkCore;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
    public class GroupRepository: IGroups
    {
        private readonly AppDBContent appDBContent;
        public GroupRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }
        public IEnumerable<Group> Groups => appDBContent.Groups.Include(v => v.university);

		public async Task Create(Group group)
		{
            await appDBContent.Groups.AddAsync(group);
            await appDBContent.SaveChangesAsync();
        }
        public IQueryable<Group> GetAll() => appDBContent.Groups.Include(v => v.university);

        public async Task<Group> Edit(Group group)
		{
            appDBContent.Groups.Update(group);
            await appDBContent.SaveChangesAsync();

            return group;
        }

		public IEnumerable<Group> FindGroupByClassroom(University university)
		{
			return appDBContent.Groups.Where(c => c.universityId == university.id).ToList();
		}

		public IEnumerable<Group> FindGroupByDirection(string direction)
		{
			return appDBContent.Groups.Where(c => c.direction == direction).ToList();
		}

		public IEnumerable<Group> FindGroupByEnd(DateTime dateTime)
		{
			return appDBContent.Groups.Where(c => c.yearEnd.Year == dateTime.Year).ToList();
		}

		public IEnumerable<Group> FindGroupByFaculty(string faculty)
		{
			return appDBContent.Groups.Where(c => c.faculty == faculty).ToList();
		}

		public IEnumerable<Group> FindGroupByStart(DateTime dateTime)
		{
			return appDBContent.Groups.Where(c => c.yearStart.Year == dateTime.Year).ToList();
		}

		public IEnumerable<Group> FindGrouptByTitle(string title)
		{
			return appDBContent.Groups.Where(c => c.title == title).ToList();
		}

		public Group Get(int id)=>appDBContent.Groups.Include(c=>c.students).Include(v => v.university).Include(v => v.eventGroups).FirstOrDefault(p=>p.id == id);

		public async Task Remove(Group group)
		{
            appDBContent.Groups.Remove(group);
            await appDBContent.SaveChangesAsync();
        }
	}
}
