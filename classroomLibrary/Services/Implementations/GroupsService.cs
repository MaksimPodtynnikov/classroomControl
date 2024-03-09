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
    public class GroupsService:IGroupsService
    {
        private readonly IGroups _groupsRepository;
        public GroupsService(IGroups GroupRepository)
        {
            _groupsRepository = GroupRepository;
        }

        public async Task<IBaseResponse<GroupViewModel>> GetGroup(long id)
        {
            try
            {
                var Group = await _groupsRepository.GetAll().Include(c => c.students).Include(v => v.eventGroups).FirstOrDefaultAsync(x => x.id == id);
                if (Group == null)
                {
                    return new BaseResponse<GroupViewModel>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new GroupViewModel()
                {
                    title = Group.title,
                    direction = Group.direction,
                    faculty = Group.faculty,
                    yearStart = Group.yearStart,
                    yearEnd = Group.yearEnd,
                    universityId = Group.universityId,
                    id = Group.id,
                    students=Group.students,
                    university=Group.university,
                };

                return new BaseResponse<GroupViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<GroupViewModel>()
                {
                    Description = $"[GetGroup] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Group>> Create(GroupViewModel model)
        {
            try
            {
                var Group = new Group()
                {
                    title = model.title,
                    direction = model.direction,
                    faculty = model.faculty,
                    yearStart = model.yearStart,
                    yearEnd = model.yearEnd,
                    universityId = model.universityId
                };
                await _groupsRepository.Create(Group);

                return new BaseResponse<Group>()
                {
                    StatusCode = StatusCode.OK,
                    Data = Group
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Group>()
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
                var Group = await _groupsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Group == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Group not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _groupsRepository.Remove(Group);

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

        public async Task<IBaseResponse<Group>> Edit(long id, GroupViewModel model)
        {
            try
            {
                var Group = await _groupsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Group == null)
                {
                    return new BaseResponse<Group>()
                    {
                        Description = "Group not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                Group.title = model.title;
                Group.direction = model.direction;
                Group.faculty = model.faculty;
                Group.yearStart = model.yearStart;
                Group.yearEnd = model.yearEnd;
                Group.universityId = model.universityId;

                await _groupsRepository.Edit(Group);


                return new BaseResponse<Group>()
                {
                    Data = Group,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<Group>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Group>> GetGroups()
        {
            try
            {
                var groups = _groupsRepository.GetAll().ToList();
                if (!groups.Any())
                {
                    return new BaseResponse<List<Group>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data=groups ,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Group>>()
                {
                    Data = groups,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Group>>()
                {
                    Description = $"[Getgroups] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
