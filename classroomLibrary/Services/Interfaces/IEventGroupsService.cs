using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;

namespace classroomLibrary.Services.Interfaces
{
    public interface IEventGroupsService
    {
        IBaseResponse<List<EventGroup>> GetEventGroups();

        Task<IBaseResponse<EventGroup>> GetEventGroup(long id);

        Task<IBaseResponse<EventGroup>> Create(EventGroup eventGroup);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<EventGroup>> Edit(long id, EventGroup model);
		Task ClearForEvent(int id);
	}
}
