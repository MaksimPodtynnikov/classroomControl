using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.ViewModels;
using classroomLibrary.Data.Models;
using classroomLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace classroomLibrary.Controllers
{
    public class GroupsController : Controller
    {
		private readonly IGroupsService _groups;
		private readonly IUniversities _universities;
        private readonly ILogger<GroupsController> _logger;
        public GroupsController(IGroupsService iGroups, IUniversities universities, ILogger<GroupsController> logger)
		{
			_groups = iGroups;
			_universities = universities;
            _logger = logger;
		}
		public ActionResult Index()
		{
			ViewBag.Title = "Учебные группы";
            var response = _groups.GetGroups();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
        public async Task<ActionResult> Details(int id)
		{
            var response = await _groups.GetGroup(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
		{
			var groupViewModel = new GroupViewModel
			{
				AllUniversities = _universities.AllUniversities
			};
			return View(groupViewModel);
		}

		// POST: ClassroomsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(GroupViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _groups.Create(viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} создал объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при создании объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
            viewModel.AllUniversities = _universities.AllUniversities;

			return View(viewModel);
        }

        // GET: ClassroomsController/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int id)
		{
            var response = await _groups.GetGroup(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                response.Data.AllUniversities = _universities.AllUniversities;

				return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

		// POST: ClassroomsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int id, GroupViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _groups.Edit(id, viewModel);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} изменил объект \"{viewModel.title}\"-{nameof(viewModel).Replace("ViewModel", "")}");
                    return RedirectToAction(nameof(Index));
                }
                _logger.LogError($"{DateTime.Now}: Ошибка при изменении объекта {nameof(viewModel).Replace("ViewModel", "")} пользователем {User.Identity.Name}");
                return View("Error", $"{response.Description}");
            }
			viewModel.AllUniversities = _universities.AllUniversities;
			return View(viewModel);
        }
		// POST: ClassroomsController/Delete/5
		[HttpDelete]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> Delete(int id)
		{
            var response = await _groups.Delete(id);
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
            List<Group> cities = _groups.GetGroups().Data.Where(c => c.title.ToLower().Contains(search.ToLower())).ToList();
            return PartialView("Lists/Groups", cities);
        }
    }
}