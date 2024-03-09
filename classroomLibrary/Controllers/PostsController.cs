using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using classroomLibrary.Services.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace classroomLibrary.Controllers
{
	public class PostsController : Controller
	{
		private readonly IPostsService _posts;
		private readonly IDepartmentsService _departments;
        private readonly ILogger<PostsController> _logger;
        public PostsController(IPostsService posts, IDepartmentsService departments, ILogger<PostsController> logger)
		{
			_posts = posts;
			_departments = departments;
            _logger = logger;
		}
		// GET: CitiesController
		public ActionResult Index()
		{
            ViewBag.Title = "Должности";
            var response = _posts.GetPosts();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// GET: CitiesController/Details/5
		public async Task<ActionResult> Details(int id)
		{
            var response = await _posts.GetPost(id);
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
            var groupViewModel = new PostViewModel
            {
                 AllDepartments =_departments.GetDepartments().Data
            };
            return View(groupViewModel);
        }

		// POST: CitiesController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PostViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _posts.Create(viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} создал объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при создании объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
            viewModel.AllDepartments = _departments.GetDepartments().Data;

			return View(viewModel);
        }

        // GET: CitiesController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
		{
            var response = await _posts.GetPost(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                response.Data.AllDepartments = _departments.GetDepartments().Data;

				return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// POST: CitiesController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, PostViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _posts.Edit(id, viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} изменил объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при изменении объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
			viewModel.AllDepartments = _departments.GetDepartments().Data;
			return View(viewModel);
        }

		[HttpDelete]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IResult> Delete(int id)
		{
            var response = await _posts.Delete(id);
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
            List<Post> cities = _posts.GetPosts().Data.Where(c => c.title.ToLower().Contains(search.ToLower())).ToList();
            return PartialView("Lists/Posts", cities);
        }
    }
}
