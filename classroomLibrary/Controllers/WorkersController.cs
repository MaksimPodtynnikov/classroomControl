using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using classroomLibrary.Services.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace classroomLibrary.Controllers
{
	public class WorkersController : Controller
	{
		private readonly IWorkersService _workers;
		private readonly IPostsService _posts;
        private readonly ILogger<WorkersController> _logger;
        public WorkersController(IWorkersService iworkers, IPostsService iposts, ILogger<WorkersController> logger)
		{
			_workers = iworkers;
			_posts = iposts;
            _logger = logger;
		}
		// GET: CitiesController
		public ActionResult Index()
		{
            ViewBag.Title = "Должности";
            var response = _workers.GetWorkers();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// GET: CitiesController/Details/5
		public async Task<IActionResult> Details(int id)
		{
            var response = await _workers.GetWorker(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        // GET: CitiesController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
		{
            var groupViewModel = new WorkerViewModel
            {
                AllPosts = _posts.GetPosts().Data
            };
            return View(groupViewModel);
        }

		// POST: CitiesController/Create
		[HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(WorkerViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _workers.Create(viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} создал объект \"{viewModel.getNameShort()}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при создании объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
			viewModel.AllPosts = _posts.GetPosts().Data;
			return View(viewModel);
        }

        // GET: CitiesController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
		{
            var response = await _workers.GetWorker(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                response.Data.AllPosts = _posts.GetPosts().Data;
                response.Data.password = "";
				return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// POST: CitiesController/Edit/5
		[HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, WorkerViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _workers.Edit(id, viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} изменил объект \"{viewModel.getNameShort()}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при изменении объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
			viewModel.AllPosts = _posts.GetPosts().Data;
			return View(viewModel);
        }

		[HttpDelete]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IResult> Delete(int id)
		{
            var response = await _workers.Delete(id);
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
            List<Worker> cities = _workers.GetWorkers().Data.Where(c => (c.name + c.family + c.patronymic).ToLower().Contains(search.ToLower())).ToList();
            return PartialView("Lists/Workers", cities);
        }
    }
}
