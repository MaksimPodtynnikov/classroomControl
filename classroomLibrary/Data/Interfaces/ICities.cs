using System;
using classroomLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace classroomLibrary.Data.Interfaces
{
	public interface ICities
	{
		IEnumerable<City> AllCities { get; }
		City Get(int id);
		IEnumerable<City> FindCityByTitle(string title);
		Task Remove(City city);
		Task Create(City city);
		public IQueryable<City> GetAll();
        Task<City> Edit(City city);
	}
}
