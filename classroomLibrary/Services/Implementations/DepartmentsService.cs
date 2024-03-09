using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Services.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;
using Microsoft.EntityFrameworkCore;
using classroomLibrary.Domain.Enum;

namespace classroomLibrary.Services.Implementations
{
    public class DepartmentsService:IDepartmentsService
    {
        private readonly IDepartments _departmentsRepository;
        public DepartmentsService(IDepartments DepartmentRepository)
        {
            _departmentsRepository = DepartmentRepository;
        }

        public async Task<IBaseResponse<DepartmentViewModel>> GetDepartment(long id)
        {
            try
            {
                var Department = await _departmentsRepository.GetAll().Include(v => v.classrooms).Include(c => c.posts).FirstOrDefaultAsync(x => x.id == id);
                if (Department == null)
                {
                    return new BaseResponse<DepartmentViewModel>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new DepartmentViewModel()
                {
                    title = Department.title,
                    cityId = Department.cityId,
                    id = Department.id,
                    city = Department.city,
                    classrooms = Department.classrooms,
                    posts = Department.posts,
                    
                };

                return new BaseResponse<DepartmentViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<DepartmentViewModel>()
                {
                    Description = $"[GetDepartment] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Department>> Create(DepartmentViewModel model)
        {
            try
            {
                var Department = new Department()
                {
                    title = model.title,
                    cityId = model.cityId,
                };
                await _departmentsRepository.Create(Department);

                return new BaseResponse<Department>()
                {
                    StatusCode = StatusCode.OK,
                    Data = Department
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Department>()
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
                var Department = await _departmentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Department == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Department not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _departmentsRepository.Remove(Department);

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

        public async Task<IBaseResponse<Department>> Edit(long id, DepartmentViewModel model)
        {
            try
            {
                var Department = await _departmentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Department == null)
                {
                    return new BaseResponse<Department>()
                    {
                        Description = "Department not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                Department.title = model.title;
                Department.cityId = model.cityId;

                await _departmentsRepository.Edit(Department);


                return new BaseResponse<Department>()
                {
                    Data = Department,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<Department>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Department>> GetDepartments()
        {
            try
            {
                var departments = _departmentsRepository.GetAll().ToList();
                if (!departments.Any())
                {
                    return new BaseResponse<List<Department>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data=departments,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Department>>()
                {
                    Data = departments,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Department>>()
                {
                    Description = $"[Getdepartments] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
