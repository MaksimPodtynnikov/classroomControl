using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using classroomLibrary.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;

namespace classroomLibrary.Controllers
{
	public class StudentsController : Controller
	{
		private readonly IStudentsService _students;
		private readonly IGroupsService _groups;
        private readonly ILogger<StudentsController> _logger;
        public StudentsController(IStudentsService students, IGroupsService groups, ILogger<StudentsController> logger)
		{
			_students = students;
			_groups = groups;
            _logger = logger;
		}
        // GET: CitiesController
        
        public ActionResult Index()
		{
            ViewBag.Title = "Должности";
            var response = _students.GetStudents();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// GET: CitiesController/Details/5
		public async Task<IActionResult> Details(int id)
		{
            var response = await _students.GetStudent(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        // GET: CitiesController/Create
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
		{
            var groupViewModel = new StudentViewModel
            {

                AllGroups = _groups.GetGroups().Data
            };
            return View(groupViewModel);
        }

		// POST: CitiesController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(StudentViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _students.Create(viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} создал объект \"{viewModel.getNameShort()}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при создании объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
            viewModel.AllGroups = _groups.GetGroups().Data;

			return View(viewModel);
        }

        // GET: CitiesController/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id)
		{
            var response = await _students.GetStudent(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                response.Data.AllGroups = _groups.GetGroups().Data;

				return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// POST: CitiesController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id, StudentViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _students.Edit(id, viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} изменил объект \"{viewModel.getNameShort()}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при изменении объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
			viewModel.AllGroups = _groups.GetGroups().Data;
			return View(viewModel);
        }

		[HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> Delete(int id)
		{
            var response = await _students.Delete(id);
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
            List<Student> cities = _students.GetStudents().Data.Where(c => (c.name+c.family+c.patronymic).ToLower().Contains(search.ToLower())).ToList();
            return PartialView("Lists/Students", cities);
        }
    }
}
