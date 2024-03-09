using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using classroomLibrary.Domain.Response;

namespace classroomLibrary.Services.Interfaces
{
    public interface IGroupsService
    {
        IBaseResponse<List<Group>> GetGroups();

        Task<IBaseResponse<GroupViewModel>> GetGroup(long id);

        Task<IBaseResponse<Group>> Create(GroupViewModel group);

        Task<IBaseResponse<bool>> Delete(long id);

        Task<IBaseResponse<Group>> Edit(long id, GroupViewModel model);
    }
}
