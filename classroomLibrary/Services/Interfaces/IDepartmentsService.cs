using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;

namespace classroomLibrary.Services.Interfaces
{
    public interface IDepartmentsService
    {
        IBaseResponse<List<Department>> GetDepartments();

        Task<IBaseResponse<DepartmentViewModel>> GetDepartment(long id);

        Task<IBaseResponse<Department>> Create(DepartmentViewModel department);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<Department>> Edit(long id, DepartmentViewModel model);
    }
}
