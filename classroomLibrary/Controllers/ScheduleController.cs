using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using classroomLibrary.Services.Interfaces;

namespace classroomLibrary.Controllers
{
    public class ScheduleController:Controller
    {
        private readonly IEventsService _events;
		private readonly IGroupsService _groups;
        private readonly ILogger<ScheduleController> _logger;
        public ScheduleController(IEventsService iEvents, IGroupsService iGroups, ILogger<ScheduleController> logger)
        {
            _events = iEvents;
			_groups = iGroups;
            _logger = logger;
		}
		[HttpPost]
		public IActionResult Week(int group)
        {
            
            if (group != 0)
            {
                List<Event> events = _events.FindEventByGroupId(group).Data;

				return PartialView("Lists/ListShedule", events);
            }
			return PartialView("Error", $"неверная группа");

        }
		[HttpGet]
		public ViewResult Week()
		{
            var response = _groups.GetGroups();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var groupSearch = response.Data.FirstOrDefault();
                var homeClasses = new EventsViewModel
                {
                    Events = _events.FindEventByGroupId(groupSearch.id).Data,
                    AllGroups = _groups.GetGroups().Data,
                    group = groupSearch
                };
                return View(homeClasses);
            }
            return View("Error", $"{response.Description}");
		}
	}
}
