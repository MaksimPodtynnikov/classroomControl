using Microsoft.EntityFrameworkCore;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace classroomLibrary.Data.Repository
{
    public class CityRepository: ICities
    {
        private readonly AppDBContent appDBContent;
        public CityRepository(AppDBContent appDBContent)
        {
            this.appDBContent = appDBContent;
        }
        public IEnumerable<City> AllCities => appDBContent.City;

        public City Get(int id) => appDBContent.City.Include(c=>c.departments).FirstOrDefault(p => p.id == id);

        public IQueryable<City> GetAll()
        {
            return appDBContent.City;
        }
        public async Task Create(City city)
		{
			await appDBContent.City.AddAsync(city);
			await appDBContent.SaveChangesAsync();
		}

		public async Task<City> Edit(City city)
		{
            appDBContent.City.Update(city);
            await appDBContent.SaveChangesAsync();
            return city;
        }

		public IEnumerable<City> FindCityByTitle(string title)
		{
			return appDBContent.City.Where(c=>c.title == title).ToList();
		}

		public async Task Remove(City city)
		{
            appDBContent.City.Remove(city);
            await appDBContent.SaveChangesAsync();
        }
	}
}
