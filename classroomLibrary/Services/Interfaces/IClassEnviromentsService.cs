using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.Domain.Response;
using classroomLibrary.ViewModels;

namespace classroomLibrary.Services.Interfaces
{
    public interface IClassEnviromentsService
    {
        IBaseResponse<List<ClassEnvironment>> GetClassEnvironments();

        Task<IBaseResponse<ClassEnvironment>> GetClassEnvironment(long id);

        Task<IBaseResponse<ClassEnvironment>> Create(ClassEnvironment classEnvironment);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<ClassEnvironment>> Edit(long id, ClassEnvironment model);
    }
}
