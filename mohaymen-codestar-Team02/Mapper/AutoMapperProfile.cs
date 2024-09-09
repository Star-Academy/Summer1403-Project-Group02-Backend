using AutoMapper;
using mohaymen_codestar_Team02.CleanArch1.Dtos.Dataset;
using mohaymen_codestar_Team02.CleanArch1.Dtos.Dataset.Attributes;
using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.Role;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Dto.UserDtos;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Models.EdgeEAV;
using mohaymen_codestar_Team02.Models.VertexEAV;

namespace mohaymen_codestar_Team02.CleanArch1.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, GetUserDto>()
            .ForMember(dto => dto.Roles, c =>
                c.MapFrom(u => u.UserRoles.Select(ur => ur.Role)));
        
        CreateMap<Role, GetRoleDto>();

        CreateMap<User, CreateUserDto>();
        CreateMap<User, UpdateUserDto>();
        
        CreateMap<DataGroup, GetDataGroupDto>()
            .ForMember(dest => dest.EdgeEntity, opt =>
                opt.MapFrom(src => src.EdgeEntity))
            .ForMember(dest => dest.VertexEntity, opt =>
                opt.MapFrom(src => src.VertexEntity))
            .ForMember(dest => dest.Name, opt => 
                opt.MapFrom(src => src.Name));

        CreateMap<EdgeEntity, GetEdgeEntityDto>();
        CreateMap<VertexEntity, GetVertexEntityDto>();

        CreateMap<DataGroup, GetDatasetPreviewDto>();
        CreateMap<DataGroup, GetDitailedDatasetDto>()
            .ForMember(dto => dto.EdgeEntity, opt => opt.MapFrom(src => src.EdgeEntity))
            .ForMember(dto => dto.VertexEntity, opt => opt.MapFrom(src => src.VertexEntity))
            .ForMember(dto => dto.EdgeAttributes, opt => opt.MapFrom(src => src.EdgeEntity.EdgeAttributes.Select(ea => new GetEdgeAttributeDto
            {
                Id = ea.Id,
                Name = ea.Name
            })))
            .ForMember(dto => dto.VertexAttributes, opt => opt.MapFrom(src => src.VertexEntity.VertexAttributes.Select(va => new GetVertexAttributeDto
            {
                Id = va.Id,
                Name = va.Name
            })));
    }
}