using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;

namespace classroomLibrary.Services.Interfaces
{
    public interface IEnvironmentsService
    {
        IBaseResponse<List<Enviroment>> GetEnviroments();

        Task<IBaseResponse<EnvironmentViewModel>> GetEnviroment(long id);

        Task<IBaseResponse<Enviroment>> Create(EnvironmentViewModel enviroment);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<Enviroment>> Edit(long id, EnvironmentViewModel model);
    }
}
