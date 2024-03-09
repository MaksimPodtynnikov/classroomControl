using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using Microsoft.Extensions.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using Microsoft.IdentityModel.Tokens;
using classroomLibrary.Services.Interfaces;
using Azure;
using Microsoft.AspNetCore.Authorization;

namespace classroomLibrary.Controllers
{
	public class EventsController: Controller
	{
		private readonly IEventsService _events;
		private readonly IWorkersService _workers;
		private readonly IClassroomsService _classrooms;
		private readonly IGroupsService _groups;
		private readonly IEventGroupsService _eventgroups;
        private readonly ILogger<EventsController> _logger;
        public EventsController(IEventsService iEvents, IWorkersService workers, IClassroomsService classrooms, IGroupsService groups, IEventGroupsService eventGroups, ILogger<EventsController> logger)
		{
			_events = iEvents;
			_workers = workers;
			_classrooms = classrooms;
			_groups = groups;
			_eventgroups = eventGroups;
			_logger = logger;
		}
		public ActionResult Index()
		{
            var response = _events.GetEvents();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
		public async Task<ActionResult> Details(int id)
		{
            var response = await _events.GetEvent(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// GET: ClassroomsController/Create
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Create(int lessonNumber, int classroom, DateTime date)
		{
			var response = await _workers.GetWorker(User.Identity.Name);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{ 
				var eventCreate = new EventViewModel
				{
					AllWorkers = _workers.GetWorkers().Data,
					AllClassrooms = _classrooms.GetClassrooms().Data,
					AllGroups = _groups.GetGroups().Data,
					lessonNumber = lessonNumber,
					classroomId = classroom,
					workerId = response.Data.id.Value,
					fromTable =true,
					date = date
				};
				return View(eventCreate);
			}
            _logger.LogError($"{DateTime.Now}: Ошибка при создании объекта Event пользователем {User.Identity.Name}");
            return View("Error", $"{response.Description}");
        }

		// POST: ClassroomsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(EventViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var response = await _events.Create(viewModel);
				if (response.StatusCode == Domain.Enum.StatusCode.OK)
				{
					if(viewModel.eventGroupsId!=null)
						foreach (int group in viewModel.eventGroupsId)
							await _eventgroups.Create(new EventGroup { eventoId = response.Data.id, groupId = group });
					_logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} создал объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
					if(!viewModel.fromTable)
						return RedirectToAction(nameof(Index));
					else return RedirectToAction(nameof(Classrooms));
				}
				_logger.LogError($"{DateTime.Now}: Ошибка при создании объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
				return View("Error", $"{response.Description}");
			}
			viewModel.AllWorkers = _workers.GetWorkers().Data;
			viewModel.AllClassrooms = _classrooms.GetClassrooms().Data;
			viewModel.AllGroups = _groups.GetGroups().Data;
			return View(viewModel);
        }

        // GET: ClassroomsController/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
		{
            var response = await _events.GetEvent(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
				response.Data.AllWorkers = _workers.GetWorkers().Data;
				response.Data.AllClassrooms = _classrooms.GetClassrooms().Data;
				response.Data.AllGroups = _groups.GetGroups().Data;

				return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// POST: ClassroomsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(int id, EventViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var response = await _events.Edit(id, viewModel);
				if (response.StatusCode == Domain.Enum.StatusCode.OK)
				{
					await _eventgroups.ClearForEvent(id);
					if (viewModel.eventGroupsId != null)
						foreach (int group in viewModel.eventGroupsId.DefaultIfEmpty())
							await _eventgroups.Create(new EventGroup { eventoId = response.Data.id, groupId = group });
					_logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} изменил объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
					return RedirectToAction(nameof(Index));
				}
				_logger.LogError($"{DateTime.Now}: Ошибка при изменении объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
				return View("Error", $"{response.Description}");
			}
			viewModel.AllWorkers = _workers.GetWorkers().Data;
			viewModel.AllClassrooms = _classrooms.GetClassrooms().Data;
			viewModel.AllGroups = _groups.GetGroups().Data;
			return View(viewModel);
        }
		// POST: ClassroomsController/Delete/5
		[HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IResult> Delete(int id)
		{
            var response = await _events.Delete(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} удалил объект {GetType().Name.Replace("sController", "")}");
                return Results.Json(true);
            }
            _logger.LogError($"{DateTime.Now}: Ошибка при удалении объекта {GetType().Name.Replace("sController", "")} пользователем {User.Identity.Name}");
            return Results.Json(false);
        }
        [HttpPost]
        public IActionResult Search(string search)
        {
            if (search == null) search = "";
            List<Event> cities = _events.GetEvents().Data.Where(c => c.title.ToLower().Contains(search.ToLower())).ToList();
            return PartialView("Lists/Events", cities);
        }
		[HttpPost]
        [Authorize]
        public async Task<IActionResult> Duplicate(IFormCollection collection)
		{
			if (collection["events"].Any() && collection["dateEnd"].Any())
			{
				foreach (string id in collection["events"])
				{
					var response = await _events.Duplicate(int.Parse(id), DateTime.ParseExact(collection["dateEnd"], "yyyy-MM-dd", null), int.Parse(collection["week"]));
                    if (response.StatusCode != Domain.Enum.StatusCode.OK)
                        return View("Error", $"{response.Description}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View("Error", "Непредвиденная ошибка");
        }
		[Authorize]
		public async Task<IActionResult> Classrooms()
		{
			var response = await _classrooms.GetClassroomsForUser(User.Identity.Name);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				var events = _events.GetEvents().Data.Where(c => response.Data.Contains(c.classroom) && c.date.Date == DateTime.Today);

				return View(new EventsViewModel
				{
					Events = events,
					AllClassrooms = response.Data,
					date = DateTime.Today,
				});
			}
			return View("Error", $"{response.Description}");
		}
		[HttpPost]
        [Authorize]
        public async Task<IActionResult> Classrooms(DateTime date)
        {

            var response = await _classrooms.GetClassroomsForUser(User.Identity.Name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var events = _events.GetEvents().Data.Where(c => response.Data.Contains(c.classroom) && c.date.Date==date);

                var view= new EventsViewModel
                {
                    Events = events,
                    AllClassrooms = response.Data,
                    date = date,
                };
                return PartialView("Lists/EventTable", view);
            }
            return View("Error", $"{response.Description}");
        }
    }
}

