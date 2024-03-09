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
using classroomLibrary.Helpers;
using System.Security.Claims;
using classroomLibrary.ViewModels.Account;
using classroomLibrary.Data.Repository;

namespace classroomLibrary.Services.Implementations
{
    public class WorkersService: IWorkersService
    {
        private readonly IWorkers _workersRepository;
        public WorkersService(IWorkers WorkerRepository)
        {
            _workersRepository = WorkerRepository;
        }

        public async Task<IBaseResponse<WorkerViewModel>> GetWorker(long id)
        {
            try
            {
                var model = await _workersRepository.GetAll().Include(v => v.events).FirstOrDefaultAsync(x => x.id == id);
                if (model == null)
                {
                    return new BaseResponse<WorkerViewModel>()
                    {
                        Description = "Сотрудник не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new WorkerViewModel()
                {
                    name = model.name,
                    family = model.family,
                    patronymic = model.patronymic,
                    mail = model.mail,
                    login = model.login,
                    password = model.password,
                    phone = model.phone,
                    oneC_id = model.oneC_id,
                    postId = model.postId,
                    role = model.role,
                    id=model.id,
                    events= model.events,
                    post=model.post,
                };

                return new BaseResponse<WorkerViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<WorkerViewModel>()
                {
                    Description = $"[GetWorker] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(Worker user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.role.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
        public async Task<IBaseResponse<bool>> Delete(long id)
        {
            try
            {
                var Worker = await _workersRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Worker == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Worker not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _workersRepository.Remove(Worker);

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

        public async Task<IBaseResponse<Worker>> Edit(long id, WorkerViewModel model)
        {
            try
            {
                var Worker = await _workersRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Worker == null)
                {
                    return new BaseResponse<Worker>()
                    {
                        Description = "Worker not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                Worker.name = model.name;
                Worker.family = model.family;
                Worker.patronymic = model.patronymic;
                Worker.mail = model.mail;
                Worker.login = model.login;
                if(model.password != "") 
                    Worker.password = HashPasswordHelper.HashPassword(model.password);
                Worker.phone = model.phone;
                Worker.oneC_id = model.oneC_id;
                Worker.postId = model.postId;
                Worker.role = model.role;

                await _workersRepository.Edit(Worker);


                return new BaseResponse<Worker>()
                {
                    Data = Worker,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<Worker>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<Worker>> Create(WorkerViewModel model)
        {
            try
            {
                var user = await _workersRepository.GetAll().FirstOrDefaultAsync(x => x.login == model.login);
                if (user != null)
                {
                    return new BaseResponse<Worker>()
                    {
                        Description = "Пользователь с таким логином уже есть",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                user = new Worker()
                {
                    name = model.name,
                    family = model.family,
                    patronymic = model.patronymic,
                    mail = model.mail,
                    login = model.login,
                    password = HashPasswordHelper.HashPassword(model.password),
                    phone = model.phone,
                    oneC_id = model.oneC_id,
                    postId = model.postId,
                    role = model.role,
                };

                await _workersRepository.Create(user);
                return new BaseResponse<Worker>()
                {
                    StatusCode = StatusCode.OK,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Worker>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _workersRepository.GetAll().FirstOrDefaultAsync(x => x.login == model.Login);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                if (user.password != HashPasswordHelper.HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль или логин"
                    };
                }
                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<List<Worker>> GetWorkers()
        {
            try
            {
                var workers = _workersRepository.GetAll().ToList();
                if (!workers.Any())
                {
                    return new BaseResponse<List<Worker>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data=workers,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Worker>>()
                {
                    Data = workers,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Worker>>()
                {
                    Description = $"[Getworkers] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<WorkerViewModel>> GetWorker(string login)
        {
            try
            {
                var model = await _workersRepository.GetAll().Include(v => v.events).FirstOrDefaultAsync(x => x.login == login);
                if (model == null)
                {
                    return new BaseResponse<WorkerViewModel>()
                    {
                        Description = "Сотрудник не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new WorkerViewModel()
                {
                    name = model.name,
                    family = model.family,
                    patronymic = model.patronymic,
                    mail = model.mail,
                    login = model.login,
                    password = model.password,
                    phone = model.phone,
                    oneC_id = model.oneC_id,
                    postId = model.postId,
                    role = model.role,
                    id = model.id,
                    events = model.events,
                    post = model.post,
                };

                return new BaseResponse<WorkerViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<WorkerViewModel>()
                {
                    Description = $"[GetWorker] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
