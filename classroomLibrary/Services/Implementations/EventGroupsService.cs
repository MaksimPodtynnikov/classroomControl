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

namespace classroomLibrary.Services.Implementations
{
    public class EventGroupsService: IEventGroupsService
    {
        private readonly IEventGroups _eventGroupsRepository;
        public EventGroupsService(IEventGroups EventGroupRepository)
        {
            _eventGroupsRepository = EventGroupRepository;
        }

        public async Task<IBaseResponse<EventGroup>> GetEventGroup(long id)
        {
            try
            {
                var EventGroup = await _eventGroupsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (EventGroup == null)
                {
                    return new BaseResponse<EventGroup>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new EventGroup()
                {
                    eventoId = EventGroup.eventoId,
                    groupId = EventGroup.groupId,
                    id = EventGroup.id,
                    evento = EventGroup.evento,
                    group = EventGroup.group,
                };

                return new BaseResponse<EventGroup>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<EventGroup>()
                {
                    Description = $"[GetEventGroup] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<EventGroup>> Create(EventGroup model)
        {
            try
            {
                var EventGroup = new EventGroup()
                {
                    eventoId = model.eventoId,
                    groupId = model.groupId
                };
                await _eventGroupsRepository.Create(EventGroup);

                return new BaseResponse<EventGroup>()
                {
                    StatusCode = StatusCode.OK,
                    Data = EventGroup
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<EventGroup>()
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
                var EventGroup = await _eventGroupsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (EventGroup == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "EventGroup not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _eventGroupsRepository.Remove(EventGroup);

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

        public async Task<IBaseResponse<EventGroup>> Edit(long id, EventGroup model)
        {
            try
            {
                var EventGroup = await _eventGroupsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (EventGroup == null)
                {
                    return new BaseResponse<EventGroup>()
                    {
                        Description = "EventGroup not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }

                EventGroup.eventoId = model.eventoId;
                EventGroup.groupId = model.groupId;

                await _eventGroupsRepository.Edit(EventGroup);


                return new BaseResponse<EventGroup>()
                {
                    Data = EventGroup,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<EventGroup>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<EventGroup>> GetEventGroups()
        {
            try
            {
                var eventGroups = _eventGroupsRepository.GetAll().ToList();
                if (!eventGroups.Any())
                {
                    return new BaseResponse<List<EventGroup>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data=eventGroups,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<EventGroup>>()
                {
                    Data = eventGroups,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<EventGroup>>()
                {
                    Description = $"[GeteventGroups] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

		public async Task ClearForEvent(int id)
		{
			await _eventGroupsRepository.ClearForEvent(id);
		}
	}
}
