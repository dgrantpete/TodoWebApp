using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TodoTask, TaskDTO>().ReverseMap();
        CreateMap<TodoTask, CreateTaskDTO>().ReverseMap();
        CreateMap<TodoUser, UserDTO>().ReverseMap();
        CreateMap<TodoUser, CreateAccountDTO>().ReverseMap();
    }
}
