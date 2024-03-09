using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using classroomLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace classroomLibrary.Controllers
{
	public class EnvironmentsController : Controller
	{
		private readonly IEnvironmentsService _environments;
        private readonly ILogger<EnvironmentsController> _logger;
        public EnvironmentsController(IEnvironmentsService environments, ILogger<EnvironmentsController> logger)
		{
			_environments = environments;
            _logger = logger;
		}
		// GET: CitiesController
		public ActionResult Index()
		{
            var response = _environments.GetEnviroments();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        // GET: CitiesController/Details/5
        public async Task<ActionResult> Details(int id)
		{
            var response = await _environments.GetEnviroment(id);
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
			var view = new EnvironmentViewModel();
			return View(view);
		}

		// POST: CitiesController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(EnvironmentViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _environments.Create(viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} создал объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при создании объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
            return View(viewModel);
        }

        // GET: CitiesController/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int id)
		{
			var enviroment = await _environments.GetEnviroment(id);
			if (enviroment != null)
			{
				var environmentView = new EnvironmentViewModel
				{
					classes = enviroment.Data.classes,
                    description = enviroment.Data.description,
                    id = id,
                    title = enviroment.Data.title
                };
				return View(environmentView);
			}
			return NotFound();
		}

		// POST: CitiesController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id, EnvironmentViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _environments.Edit(id, viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} изменил объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при изменении объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
            return View(viewModel);
        }

		[HttpDelete]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> Delete(int id)
		{
            var response = await _environments.Delete(id);
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
            List<Enviroment> environments = _environments.GetEnviroments().Data.Where(c => c.title.ToLower().Contains(search.ToLower())).ToList();
            return PartialView("Lists/Environments", environments);
        }
    }
}
