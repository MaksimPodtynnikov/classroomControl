using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Services.Interfaces;
using classroomLibrary.Data.Models;
using classroomLibrary.Data.Interfaces;
using classroomLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using classroomLibrary.Domain.Response;
using classroomLibrary.Domain.Enum;

namespace classroomLibrary.Services.Implementations
{
    public class EnvironmentsService:IEnvironmentsService
    {
        private readonly IEnvironments _environmentsRepository;
        public EnvironmentsService(IEnvironments EnviromentRepository)
        {
            _environmentsRepository = EnviromentRepository;
        }

        public async Task<IBaseResponse<EnvironmentViewModel>> GetEnviroment(long id)
        {
            try
            {
                var Enviroment = await _environmentsRepository.GetAll().Include(v => v.classes).FirstOrDefaultAsync(x => x.id == id);
                if (Enviroment == null)
                {
                    return new BaseResponse<EnvironmentViewModel>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new EnvironmentViewModel()
                {
                    title = Enviroment.title,
                    description = Enviroment.description,
                    id = Enviroment.id,
                    classes = Enviroment.classes,
                };

                return new BaseResponse<EnvironmentViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<EnvironmentViewModel>()
                {
                    Description = $"[GetEnviroment] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Enviroment>> Create(EnvironmentViewModel model)
        {
            try
            {
                var Enviroment = new Enviroment()
                {
                    title = model.title,
                    description = model.description
                };
                await _environmentsRepository.Create(Enviroment);

                return new BaseResponse<Enviroment>()
                {
                    StatusCode = StatusCode.OK,
                    Data = Enviroment
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Enviroment>()
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
                var Enviroment = await _environmentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Enviroment == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Enviroment not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _environmentsRepository.Remove(Enviroment);

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

        public async Task<IBaseResponse<Enviroment>> Edit(long id, EnvironmentViewModel model)
        {
            try
            {
                var Enviroment = await _environmentsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Enviroment == null)
                {
                    return new BaseResponse<Enviroment>()
                    {
                        Description = "Enviroment not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                Enviroment.title = model.title;
                Enviroment.description = model.description;

                await _environmentsRepository.Edit(Enviroment);


                return new BaseResponse<Enviroment>()
                {
                    Data = Enviroment,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<Enviroment>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Enviroment>> GetEnviroments()
        {
            try
            {
                var environments = _environmentsRepository.GetAll().ToList();
                if (!environments.Any())
                {
                    return new BaseResponse<List<Enviroment>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data = environments,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Enviroment>>()
                {
                    Data = environments,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Enviroment>>()
                {
                    Description = $"[Getenvironments] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
