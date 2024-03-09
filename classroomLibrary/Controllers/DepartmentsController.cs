using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using classroomLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace classroomLibrary.Controllers
{
	public class DepartmentsController : Controller
	{
		private readonly IDepartmentsService _departments;
		private readonly ICitiesService _cities;
        private readonly ILogger<DepartmentsController> _logger;
        public DepartmentsController(IDepartmentsService departments, ICitiesService cities, ILogger<DepartmentsController> logger)
		{
			_departments = departments;
			_cities = cities;
            _logger = logger;
		}
		// GET: CitiesController
		public ActionResult Index()
		{
            var response = _departments.GetDepartments();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// GET: CitiesController/Details/5
		public async Task<IActionResult> Details(int id)
		{
            var response = await _departments.GetDepartment(id);
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
			var departmentView = new DepartmentViewModel { AllCities=_cities.GetCities().Data};
			return View(departmentView);
		}

		// POST: CitiesController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(DepartmentViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _departments.Create(viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} создал объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при создании объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
            viewModel.AllCities = _cities.GetCities().Data;

			return View(viewModel);
		}

        // GET: CitiesController/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id)
		{
            var response = await _departments.GetDepartment(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
				response.Data.AllCities = _cities.GetCities().Data;
				return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// POST: CitiesController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _departments.Edit(id, viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} изменил объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при изменении объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
			viewModel.AllCities = _cities.GetCities().Data;
			return View(viewModel);
        }

		[HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> Delete(int id)
		{
            var response = await _departments.Delete(id);
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
			List<Department> departments = _departments.GetDepartments().Data.Where(c => c.title.ToLower().Contains(search.ToLower())).ToList();
            return PartialView("Lists/Departments", departments);
        }
    }
}
