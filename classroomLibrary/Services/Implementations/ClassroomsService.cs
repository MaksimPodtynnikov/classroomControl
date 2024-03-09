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
using System.Web;
using System.IO;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using classroomLibrary.Helpers;
using System.Net;

namespace classroomLibrary.Services.Implementations
{
	public class ClassroomsService : IClassroomsService
    {
        private readonly IClassrooms _classroomsRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IWorkers _workersRepository;
        public ClassroomsService(IClassrooms ClassroomRepository, IWebHostEnvironment AppEnvironment, IWorkers workersRepository)
        {
            _classroomsRepository = ClassroomRepository;
            _appEnvironment = AppEnvironment;
            _workersRepository = workersRepository;
        }

        public async Task<IBaseResponse<ClassViewModel>> GetClassroom(long id)
        {
            try
            {
                var Classroom = await _classroomsRepository.GetAll().Include(c => c.enviroments).ThenInclude(c => c.enviroment).Include(c => c.events).FirstOrDefaultAsync(x => x.id == id);
                if (Classroom == null)
                {
                    return new BaseResponse<ClassViewModel>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new ClassViewModel()
                {
                    title = Classroom.title,
                    capacity = Classroom.capacity,
                    photo = Classroom.photo,
                    departmentId = Classroom.departmentId,
                    id = Classroom.id,
                    department = Classroom.department,
                    classEnvironments = Classroom.enviroments,
                    enviroments = Classroom.enviroments,
                    events = Classroom.events
                };

                return new BaseResponse<ClassViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClassViewModel>()
                {
                    Description = $"[GetClassroom] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Classroom>> Create(ClassViewModel model)
        {
            try
            {
                var Classroom = new Classroom()
                {
                    title = model.title,
                    capacity = model.capacity,
                    photo = model.photo,
                    departmentId = model.departmentId,
                };
                await _classroomsRepository.Create(Classroom);
                string path = "";
                if (model.image != null)
                {
                    path = "/img/classes/" + Classroom.id + model.image.FileName.Substring(model.image.FileName.IndexOf(".")).Trim();
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.image.CopyToAsync(fileStream);
                    }
                    Classroom.photo = path;
                    await _classroomsRepository.Edit(Classroom);
                }
                return new BaseResponse<Classroom>()
                {
                    StatusCode = StatusCode.OK,
                    Data = Classroom
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Classroom>()
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
                var Classroom = await _classroomsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Classroom == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Classroom not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }
                string img = Classroom.photo == null || Classroom.photo == "" ? "-1.png" : Classroom.photo;
                string _imageToBeDeleted = $"{_appEnvironment.WebRootPath}{img.Replace("/","\\")}";
                if (File.Exists(_imageToBeDeleted))
                {
                    File.Delete(_imageToBeDeleted);
                }
                await _classroomsRepository.Remove(Classroom);

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

        public async Task<IBaseResponse<Classroom>> Edit(long id, ClassViewModel model)
        {
            try
            {
                var Classroom = await _classroomsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Classroom == null)
                {
                    return new BaseResponse<Classroom>()
                    {
                        Description = "Classroom not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }
                string path = "";
                if (model.image != null)
                {
                    path = "/img/classes/" + id + model.image.FileName.Substring(model.image.FileName.IndexOf(".")).Trim();
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.image.CopyToAsync(fileStream);
                    }
                    model.photo = path;
                }
                Classroom.title = model.title;
                Classroom.capacity = model.capacity;
                Classroom.photo = model.photo;
                Classroom.departmentId = model.departmentId;

                await _classroomsRepository.Edit(Classroom);


                return new BaseResponse<Classroom>()
                {
                    Data = Classroom,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<Classroom>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<List<Classroom>>> GetClassroomsForUser(string login)
        {
            try
            {
                var user = await _workersRepository.GetAll().Include(v => v.post).ThenInclude(c => c.department).Include(c=>c.events).FirstOrDefaultAsync(x => x.login == login);
                if (user == null)
                {
                    return new BaseResponse<List<Classroom>>()
                    {
                        Description = "Город пользователя не найден",
                    };
                }
                var classrooms = _classroomsRepository.GetAll().Where(x=>x.department.cityId == user.post.department.cityId).ToList();
                if (!classrooms.Any())
                {
                    return new BaseResponse<List<Classroom>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data = classrooms,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Classroom>>()
                {
                    Data = classrooms,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Classroom>>()
                {
                    Description = $"[Getclassrooms] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<List<Classroom>> GetClassrooms()
        {
            try
            {
                var classrooms = _classroomsRepository.GetAll().ToList();
                if (!classrooms.Any())
                {
                    return new BaseResponse<List<Classroom>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data =classrooms,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Classroom>>()
                {
                    Data = classrooms,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Classroom>>()
                {
                    Description = $"[Getclassrooms] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

		public async Task<IBaseResponse<bool>> sendClassroomsEnviroments(string title)
		{
            try
            {
                var classrooms = _classroomsRepository.GetAll().Include(c => c.department).ThenInclude(c => c.city).Include(c => c.enviroments).ThenInclude(c => c.enviroment).ToList();
                string filename = _appEnvironment.WebRootPath + "\\env\\test.csv";
                var result = await CSVWriteHelper.CreateCSVFile(classrooms, filename);
                if (result)
                {
                    FTPServerHelper.upload(filename, "504073-vepi2019.tmweb.ru", "dime", "vK6eU2vA5pkZ3c",title);
                    return new BaseResponse<bool>()
                    {
                        Data = result,
                        StatusCode = StatusCode.OK
                    };

                }
				return new BaseResponse<bool>()
				{
					Data = result,
					StatusCode = StatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Description = $"[sendClassrooms] : {ex.Message}",
					StatusCode = StatusCode.InternalServerError
				};
			}
		}
	}
}
