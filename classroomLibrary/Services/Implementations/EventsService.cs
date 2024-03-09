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
using classroomLibrary.Data.Repository;
using classroomLibrary.Helpers;

namespace classroomLibrary.Services.Implementations
{
    public class EventsService: IEventsService
    {
        private readonly IEvents _eventsRepository;
        public EventsService(IEvents EventRepository)
        {
            _eventsRepository = EventRepository;
        }

        public async Task<IBaseResponse<EventViewModel>> GetEvent(long id)
        {
            try
            {
                var Event = await _eventsRepository.GetAll().Include(v => v.eventGroups).ThenInclude(c => c.group).FirstOrDefaultAsync(x => x.id == id);
                if (Event == null)
                {
                    return new BaseResponse<EventViewModel>()
                    {
                        Description = "Город не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new EventViewModel()
                {
                    title = Event.title,
                    type = Event.type,
                    date=Event.date,
                    lessonNumber=Event.lessonNumber,
                    workerId=Event.workerId,
                    classroomId=Event.classroomId,
                    id = Event.id,
                    classroom=Event.classroom,
                    eventGroups=Event.eventGroups,
                    worker=Event.worker,
                };

                return new BaseResponse<EventViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<EventViewModel>()
                {
                    Description = $"[GetEvent] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Event>> Create(EventViewModel model)
        {
            try
            {
                var @event = await _eventsRepository.GetAll().FirstOrDefaultAsync(x => x.date == model.date && x.lessonNumber==model.lessonNumber && x.classroomId==model.classroomId);
                if (@event != null)
                {
                    return new BaseResponse<Event>()
                    {
                        Description = "Данная аудитория занята в данное время",
                    };
                }
                else
                {
                    @event = await _eventsRepository.GetAll().FirstOrDefaultAsync(x => x.date == model.date && x.lessonNumber == model.lessonNumber && x.workerId == model.workerId);
                    if (@event != null)
                    {
                        return new BaseResponse<Event>()
                        {
                            Description = "Данный преподаватель занят в данное время",
                        };
                    }
                    else
                    {
                        var Event = new Event()
                        {
                            title = model.title,
                            type = model.type,
                            date = model.date,
                            lessonNumber = model.lessonNumber,
                            workerId = model.workerId,
                            classroomId = model.classroomId
                        };
                        await _eventsRepository.Create(Event);

                        return new BaseResponse<Event>()
                        {
                            StatusCode = StatusCode.OK,
                            Data = Event
                        };
                    }
                }                
            }
            catch (Exception ex)
            {
                return new BaseResponse<Event>()
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
                var Event = await _eventsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Event == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Event not found",
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }

                await _eventsRepository.Remove(Event);
                MailHelper.sendMail(Event.worker.mail, Event);
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

        public async Task<IBaseResponse<Event>> Edit(long id, EventViewModel model)
        {
            try
            {
                var Event = await _eventsRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);
                if (Event == null)
                {
                    return new BaseResponse<Event>()
                    {
                        Description = "Event not found",
                        StatusCode = StatusCode.ObjectNotFound
                    };
                }
                if (model.workerId != Event.workerId)
                    await MailHelper.sendMail(Event.worker.mail, Event);
                Event.title = model.title;
                Event.type = model.type;
                Event.date = model.date;
                Event.lessonNumber = model.lessonNumber;
                Event.workerId = model.workerId;
                Event.classroomId = model.classroomId;

                await _eventsRepository.Edit(Event);
                

                return new BaseResponse<Event>()
                {
                    Data = Event,
                    StatusCode = StatusCode.OK,
                };
                // TypeCar
            }
            catch (Exception ex)
            {
                return new BaseResponse<Event>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Event>> GetEvents()
        {
            try
            {
                var events = _eventsRepository.GetAll().ToList();
                if (!events.Any())
                {
                    return new BaseResponse<List<Event>>()
                    {
                        Description = "Найдено 0 элементов",
                        Data=events,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Event>>()
                {
                    Data = events,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Event>>()
                {
                    Description = $"[Getevents] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Event>> FindEventByGroupId(int id)
        {
            try
            {
                var events = _eventsRepository.FindEventByGroupID(id).ToList();
                if (!events.Any())
                {
                    return new BaseResponse<List<Event>>()
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Event>>()
                {
                    Data = events,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Event>>()
                {
                    Description = $"[Getevents] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Duplicate(int id, DateTime dateTime, int repeats)
        {
            try
            {
                var @event = await _eventsRepository.GetAll().Include(v => v.eventGroups).ThenInclude(c => c.group).FirstOrDefaultAsync(x => x.id == id);
                DateTime dateEvent = @event.date.AddDays(7 * repeats);
                while (dateEvent <= dateTime)
                {
                    var @eventTemp = await _eventsRepository.GetAll().FirstOrDefaultAsync(x => x.date.Date == dateEvent.Date && x.lessonNumber == @event.lessonNumber && x.classroomId == @event.classroomId);
                    if (@eventTemp != null)
                    {
                        return new BaseResponse<bool>()
                        {
                            Description = "Данная аудитория занята в данное время",
                        };
                    }
                    else
                    {
                        @eventTemp = await _eventsRepository.GetAll().FirstOrDefaultAsync(x => x.date.Date == dateEvent.Date && x.lessonNumber == @event.lessonNumber && x.workerId == @event.workerId);
                        if (@eventTemp != null)
                        {
                            return new BaseResponse<bool>()
                            {
                                Description = "Данный преподаватель занят в данное время",
                            };
                        }
                        else
                        {
                            var Event = new Event()
                            {
                                title = @event.title,
                                type = @event.type,
                                date = dateEvent,
                                lessonNumber = @event.lessonNumber,
                                workerId = @event.workerId,
                                classroomId = @event.classroomId,
                                eventGroups = @event.eventGroups,
                            };
                            await _eventsRepository.Create(Event);
                        }
                    }
                    dateEvent = dateEvent.AddDays(7 * repeats);
                }
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
                    Description = $"[Duplicate] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
