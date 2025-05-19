using AutoMapper;
using TaskManagerAPI.Models;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.BusinessLogic
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskItem, TaskItemDTO>().ReverseMap();
        }
    }
}
