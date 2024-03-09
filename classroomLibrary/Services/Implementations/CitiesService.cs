using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Services.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.ViewModels;
using classroomLibrary.Data.Repository;
using classroomLibrary.Domain.Response;
using classroomLibrary.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace classroomLibrary.Services.Implementations
{
    public class CitiesService: ICitiesService
    {
        private readonly ICities _citiesRepository;
        public CitiesService(ICities cityRepository)
        {
            _citiesRepository = cityRepository;
        }

        public async Task<IBaseResponse<CityViewModel>> GetCity(long id)
        {
            try
            {
                var city = await _citiesRepository.GetAll().Include(c=>c.departments).FirstOrDefaultAsync(x => x.id == id);
                if (city == null)
                {
                    return new BaseResponse<CityViewModel>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new CityViewModel()
                {
                    title = city.title,
                    city = city.city,
                    Id = city.id,
                    departments = city.departments,
				};

                return new BaseResponse<CityViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CityViewModel>()
                {
                    Description = $"[GetCity] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<City>> Create(CityViewModel model)
        {
            try
            {
                var city = new City()
                {
                    title = model.title,
                    city = model.city,
                };
                await _citiesRepository.Create(city);

                return new BaseResponse<City>()
                {
                    StatusCode = StatusCode.OK,
                    Data = city
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<City>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(long id)
        {
            try
            {
                var city = await _citiesRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (city == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "City not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _citiesRepository.Remove(city);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[Delete] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<City>> Edit(long id, CityViewModel model)
        {
            try
            {
                var city = await _citiesRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (city == null)
                {
                    return new BaseResponse<City>()
                    {
                        Description = "City not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                city.title = model.title;
                city.city = model.city;

                await _citiesRepository.Edit(city);


                return new BaseResponse<City>()
                {
                    Data = city,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<City>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<City>> GetCities()
        {
            try
            {
                var cities = _citiesRepository.GetAll().ToList();
                if (!cities.Any())
                {
                    return new BaseResponse<List<City>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data = cities,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<City>>()
                {
                    Data = cities,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<City>>()
                {
                    Description = $"[GetCities] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
