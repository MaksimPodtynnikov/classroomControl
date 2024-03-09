using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;
using classroomLibrary.ViewModels.Account;
using System.Security.Claims;

namespace classroomLibrary.Services.Interfaces
{
    public interface IWorkersService
    {
        IBaseResponse<List<Worker>> GetWorkers();

        Task<IBaseResponse<WorkerViewModel>> GetWorker(long id);
        Task<IBaseResponse<WorkerViewModel>> GetWorker(string login);

        Task<IBaseResponse<Worker>> Create(WorkerViewModel post);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<Worker>> Edit(long id, WorkerViewModel model);
        public Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
    }
}
