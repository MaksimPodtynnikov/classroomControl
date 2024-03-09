using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.Domain.Response;
using classroomLibrary.ViewModels;

namespace classroomLibrary.Services.Interfaces
{
     public interface IStudentsService
    {
        IBaseResponse<List<Student>> GetStudents();

        Task<IBaseResponse<StudentViewModel>> GetStudent(long id);

        Task<IBaseResponse<Student>> Create(StudentViewModel post);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<Student>> Edit(long id, StudentViewModel model);
    }
}
