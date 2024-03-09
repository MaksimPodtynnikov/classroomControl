using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.Domain.Response;
using classroomLibrary.ViewModels;

namespace classroomLibrary.Services.Interfaces
{
    public interface IClassroomsService
    {
        IBaseResponse<List<Classroom>> GetClassrooms();

        Task<IBaseResponse<ClassViewModel>> GetClassroom(long id);

        Task<IBaseResponse<Classroom>> Create(ClassViewModel classroom);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<Classroom>> Edit(long id, ClassViewModel model);
        Task<IBaseResponse<List<Classroom>>> GetClassroomsForUser(string login);
		Task<IBaseResponse<bool>> sendClassroomsEnviroments(string title);
	}
}
