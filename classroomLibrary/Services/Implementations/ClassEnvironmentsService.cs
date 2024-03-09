using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Services.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;
using classroomLibrary.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace classroomLibrary.Services.Implementations
{
    public class ClassEnvironmentsService:IClassEnviromentsService
    {
        private readonly IClassEnvironments _classEnvironmentsRepository;
        public ClassEnvironmentsService(IClassEnvironments ClassEnvironmentRepository)
        {
            _classEnvironmentsRepository = ClassEnvironmentRepository;
        }

        public async Task<IBaseResponse<ClassEnvironment>> GetClassEnvironment(long id)
        {
            try
            {
                var model = await _classEnvironmentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (model == null)
                {
                    return new BaseResponse<ClassEnvironment>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new ClassEnvironment()
                {
                    id = model.id,
                    count = model.count,
                    classroomId = model.classroomId,
                    enviromentId = model.enviromentId,
                    classroom = model.classroom,
                    enviroment = model.enviroment,
                };

                return new BaseResponse<ClassEnvironment>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClassEnvironment>()
                {
                    Description = $"[GetClassEnvironment] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<ClassEnvironment>> Create(ClassEnvironment model)
        {
            try
            {
                var ClassEnvironment = new ClassEnvironment()
                {
                    count = model.count,
                    classroomId = model.classroomId,
                    enviromentId = model.enviromentId
                };
                await _classEnvironmentsRepository.Create(ClassEnvironment);

                return new BaseResponse<ClassEnvironment>()
                {
                    StatusCode = StatusCode.OK,
                    Data = ClassEnvironment
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClassEnvironment>()
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
                var ClassEnvironment = await _classEnvironmentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (ClassEnvironment == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "ClassEnvironment not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _classEnvironmentsRepository.Remove(ClassEnvironment);

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

        public async Task<IBaseResponse<ClassEnvironment>> Edit(long id, ClassEnvironment model)
        {
            try
            {
                var ClassEnvironment = await _classEnvironmentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (ClassEnvironment == null)
                {
                    return new BaseResponse<ClassEnvironment>()
                    {
                        Description = "ClassEnvironment not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                ClassEnvironment.count = model.count;
                ClassEnvironment.classroomId = model.classroomId;
                ClassEnvironment.enviromentId = model.enviromentId;

                await _classEnvironmentsRepository.Edit(ClassEnvironment);


                return new BaseResponse<ClassEnvironment>()
                {
                    Data = ClassEnvironment,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClassEnvironment>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<ClassEnvironment>> GetClassEnvironments()
        {
            try
            {
                var classEnvironments = _classEnvironmentsRepository.GetAll().ToList();
                if (!classEnvironments.Any())
                {
                    return new BaseResponse<List<ClassEnvironment>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data =classEnvironments,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<ClassEnvironment>>()
                {
                    Data = classEnvironments,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ClassEnvironment>>()
                {
                    Description = $"[GetclassEnvironments] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
