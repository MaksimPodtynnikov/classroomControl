using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.RegularExpressions;
using classroomLibrary.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace classroomLibrary.Controllers
{
	public class ClassroomsController : Controller
	{
		private readonly IClassroomsService _classrooms;
		private readonly IDepartmentsService _departments;
		private readonly IEventsService _events;
		private readonly IEnvironmentsService _environments;
		private readonly IClassEnviromentsService _classEnvironments;
        private readonly ILogger<ClassroomsController> _logger;
        public ClassroomsController(IClassroomsService iClassrooms, IEnvironmentsService iEnvironments, IDepartmentsService iDepartments, IEventsService iEvents, IClassEnviromentsService iClassEnvironments, ILogger<ClassroomsController> logger)
		{
			_classrooms = iClassrooms;
			_departments = iDepartments;
			_environments = iEnvironments;	
			_events = iEvents;
			_classEnvironments = iClassEnvironments;
			_logger = logger;
		}
        [HttpGet]
        public async Task<IActionResult> Index(string filter)
        {
			if (filter == null)
			{
				var response = _classrooms.GetClassrooms();
				if (response.StatusCode == Domain.Enum.StatusCode.OK)
				{
					return View(response.Data);
				}
				return View("Error", $"{response.Description}");
			}
			else
			{
				var response = await _classrooms.GetClassroomsForUser(User.Identity.Name);

				if (response.StatusCode == Domain.Enum.StatusCode.OK)
				{
					return View(response.Data);
				}
				return View("Error", $"{response.Description}");
			}
        }
        // GET: ClassroomsController/Details/5
        public async Task<IActionResult> Details(int id)
		{
            var response = await _classrooms.GetClassroom(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        // GET: ClassroomsController/Create
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
		{
			var classCreate = new ClassViewModel
			{
				AllDepartments=_departments.GetDepartments().Data,
				AllEnviroments=_environments.GetEnviroments().Data,
				AllEvents=_events.GetEvents().Data

            };
			return View(classCreate);
		}

		// POST: ClassroomsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(ClassViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var response = await _classrooms.Create(viewModel);
				if (response.StatusCode == Domain.Enum.StatusCode.OK)
				{
					_logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} создал объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
					return RedirectToAction(nameof(Index));
				}
				_logger.LogError($"{DateTime.Now}: Ошибка при создании объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
				return View("Error", $"{response.Description}");
			}
			viewModel.AllDepartments = _departments.GetDepartments().Data;
			viewModel.AllEnviroments = _environments.GetEnviroments().Data;
			viewModel.AllEvents = _events.GetEvents().Data;
			return View(viewModel);
		}

        // GET: ClassroomsController/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id)
		{
            var response = await _classrooms.GetClassroom(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
				response.Data.AllDepartments = _departments.GetDepartments().Data;
				response.Data.AllEnviroments = _environments.GetEnviroments().Data;
				response.Data.AllEvents = _events.GetEvents().Data;

				return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// POST: ClassroomsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int id, ClassViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var response = await _classrooms.Edit(id, viewModel);
				if (response.StatusCode == Domain.Enum.StatusCode.OK)
				{
					_logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} изменил объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
					return RedirectToAction(nameof(Index));
				}
				_logger.LogError($"{DateTime.Now}: Ошибка при изменении объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
				return View("Error", $"{response.Description}");
			}
			viewModel.AllDepartments = _departments.GetDepartments().Data;
			viewModel.AllEnviroments = _environments.GetEnviroments().Data;
			viewModel.AllEvents = _events.GetEvents().Data;
			return View(viewModel);
        }
		// POST: ClassroomsController/Delete/5
		[HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> Delete(int id)
		{
            var response = await _classrooms.Delete(id);
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
			List<Classroom> classrooms = _classrooms.GetClassrooms().Data.Where(c => c.title.ToLower().Contains(search.ToLower())).ToList();
            return PartialView("Lists/Classrooms", classrooms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> AddEnviroment([FromBody] ClassEnvironment classEnviroment)
		{
			try
			{
				if (classEnviroment != null)
				{
					Classroom? classroom = _classrooms.GetClassrooms().Data.FirstOrDefault(c => c.id == classEnviroment.classroomId);
					await _classEnvironments.Create(classEnviroment);
					var enviroment = await _environments.GetEnviroment(classEnviroment.enviromentId);

                    return Results.Json(new { id = classEnviroment.id, count = classEnviroment.count,enviroment = enviroment.Data.title  });
				}
				else
				{
					throw new Exception("Некорректные данные");
				}
			}
			catch (Exception)
			{
				return Results.Json(false);
			}
		}
		[HttpDelete]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> DeleteEnviroment(int id)
		{
			try
			{
				var result = await _classEnvironments.Delete(id);

                if (result.Data)
					return Results.Json(true);
				else
				{
					throw new Exception("Некорректные данные");
				}
			}
			catch (Exception)
			{
				return Results.Json(false);
			}
		}
		[HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> DeleteEvent(int id)
		{
            var response = await _events.Delete(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Results.Json(true);
            }
            return Results.Json(false);
		}
		[HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> SendEnviroments(string title)
		{
			var response = await _classrooms.sendClassroomsEnviroments(title);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				_logger.LogWarning($"{DateTime.Now}: {User.Identity.Name} выгрузил аудиторное обеспечение");

				return RedirectToAction(nameof(Index));
			}
			return View("Error", $"{response.Description}");
		}
	}
}
