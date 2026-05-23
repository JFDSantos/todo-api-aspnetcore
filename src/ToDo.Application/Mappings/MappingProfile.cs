using AutoMapper;
using ToDo.Application.Dtos;
using ToDo.Domain.Entities;

namespace ToDo.Application.Mappings
{
    /// <summary>
    /// Perfil de mapeamento AutoMapper
    /// Centraliza todas as conversões entre Entidades e DTOs
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapear TodoTask → TaskResponseDto
            CreateMap<TodoTask, TaskResponseDto>()
                .ReverseMap();

            // Mapear CreateTaskDto → TodoTask
            CreateMap<CreateTaskDto, TodoTask>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            // Mapear UpdateTaskDto → TodoTask
            CreateMap<UpdateTaskDto, TodoTask>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
