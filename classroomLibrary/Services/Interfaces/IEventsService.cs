using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;

namespace classroomLibrary.Services.Interfaces
{
    public interface IEventsService
    {
        IBaseResponse<List<Event>> GetEvents();

        Task<IBaseResponse<EventViewModel>> GetEvent(long id);

        Task<IBaseResponse<Event>> Create(EventViewModel evento);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<Event>> Edit(long id, EventViewModel model);
        IBaseResponse<List<Event>> FindEventByGroupId(int id);
        Task<IBaseResponse<bool>> Duplicate(int id, DateTime dateTime, int repeats);
    }
}
