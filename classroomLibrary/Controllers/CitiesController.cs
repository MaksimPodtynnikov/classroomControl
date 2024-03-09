using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using classroomLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using classroomLibrary.Services.Implementations;
using System;
using classroomLibrary.Domain.Enum;

namespace classroomLibrary.Controllers
{
	public class CitiesController : Controller
	{
		private readonly ICitiesService _cities;
        private readonly ILogger<CitiesController> _logger;
        public CitiesController(ICitiesService cities, ILogger<CitiesController> logger) { 
			_cities = cities;
            _logger = logger;
		}

		// GET: CitiesController/Details/5
		public ActionResult Details(int id)
		{
            var response = _cities.GetCity(id);
            if (response.Result.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Result.Data);
            }
            return View("Error", $"{response.Result.Description}");
            
        }

        // GET: CitiesController/Create
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
		{
			var cityView = new CityViewModel();
			return View(cityView);
		}

		// POST: CitiesController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(CityViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _cities.Create(viewModel);
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
        public async Task<IActionResult> Edit(int id)
		{
            var response = await _cities.GetCity(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
            
        }

		// POST: CitiesController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int id, CityViewModel viewModel)
		{
            if (ModelState.IsValid)
            {
                var response = await _cities.Edit(id, viewModel);
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
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> Delete(int id)
		{
            var response = await _cities.Delete(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                _logger.LogInformation($"{DateTime.Now}: {User.Identity.Name} удалил объект {GetType().Name.Replace("sController", "")}");
                return Results.Json(true);
            }
            _logger.LogError($"{DateTime.Now}: Ошибка при удалении объекта City пользователем {User.Identity.Name}");
            return Results.Json(false);
        }
		[HttpPost]
		public IActionResult Search(string search)
		{
            if (search == null) search = "";
			List<City> cities = _cities.GetCities().Data.Where(c => c.title.ToLower().Contains(search.ToLower())).ToList();
            return PartialView("Lists/Cities", cities);
        }
        [HttpGet]
        public ActionResult Index()
        {
            var response = _cities.GetCities();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
    }
}
