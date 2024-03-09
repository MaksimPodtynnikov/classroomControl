using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.Domain.Response;
using classroomLibrary.ViewModels;

namespace classroomLibrary.Services.Interfaces
{
    public interface ICitiesService
    {
        IBaseResponse<List<City>> GetCities();

        Task<IBaseResponse<CityViewModel>> GetCity(long id);

        Task<IBaseResponse<City>> Create(CityViewModel city);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<City>> Edit(long id, CityViewModel model);
    }
}
