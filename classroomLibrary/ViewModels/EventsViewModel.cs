using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using System.Collections.Generic;

namespace classroomLibrary.ViewModels
{
    public class EventsViewModel
    {
        public IEnumerable<Event> Events { get; set; }
        public Group group { get; set; }
		public IEnumerable<Group> AllGroups { get; set; }
        public IEnumerable<Classroom> AllClassrooms { get; set; }
        public DateTime date { get; set; }
    }
}
