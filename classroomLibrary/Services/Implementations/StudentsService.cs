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
    public class StudentsService:IStudentsService
    {
        private readonly IStudents _studentsRepository;
        public StudentsService(IStudents StudentRepository)
        {
            _studentsRepository = StudentRepository;
        }

        public async Task<IBaseResponse<StudentViewModel>> GetStudent(long id)
        {
            try
            {
                var model = await _studentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (model == null)
                {
                    return new BaseResponse<StudentViewModel>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new StudentViewModel()
                {
                    name = model.name,
                    family = model.family,
                    patronymic = model.patronymic,
                    telegram_id = model.telegram_id,
                    groupId = model.groupId,
                    id = model.id,
                    group = model.group,
                    
                };

                return new BaseResponse<StudentViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudentViewModel>()
                {
                    Description = $"[GetStudent] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Student>> Create(StudentViewModel model)
        {
            try
            {
                var Student = new Student()
                {
                    name = model.name,
                    family = model.family,
                    patronymic = model.patronymic,
                    telegram_id = model.telegram_id,
                    groupId = model.groupId
                };
                await _studentsRepository.Create(Student);

                return new BaseResponse<Student>()
                {
                    StatusCode = StatusCode.OK,
                    Data = Student
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Student>()
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
                var Student = await _studentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Student == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Student not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _studentsRepository.Remove(Student);

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

        public async Task<IBaseResponse<Student>> Edit(long id, StudentViewModel model)
        {
            try
            {
                var Student = await _studentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Student == null)
                {
                    return new BaseResponse<Student>()
                    {
                        Description = "Student not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                Student.name = model.name;
                Student.family = model.family;
                Student.patronymic = model.patronymic;
                Student.telegram_id = model.telegram_id;
                Student.groupId = model.groupId;

                await _studentsRepository.Edit(Student);


                return new BaseResponse<Student>()
                {
                    Data = Student,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<Student>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Student>> GetStudents()
        {
            try
            {
                var students = _studentsRepository.GetAll().ToList();
                if (!students.Any())
                {
                    return new BaseResponse<List<Student>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data=students,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Student>>()
                {
                    Data = students,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Student>>()
                {
                    Description = $"[Getstudents] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
