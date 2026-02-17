using apiConstruction.Application.DTOs.Requests;
using apiConstruction.Application.DTOs.Responses;
using apiConstruction.Domain.Entities;
using apiConstruction.Domain.Enums;
using AutoMapper;

namespace apiConstruction.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Employee mappings
        CreateMap<CreateEmployeeRequest, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => (ContractType)src.ContractType))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (EmployeeStatus)src.Status));

        CreateMap<UpdateEmployeeRequest, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => (ContractType)src.ContractType))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (EmployeeStatus)src.Status));

        CreateMap<Employee, EmployeeResponse>()
            .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));

        // Department mappings
        CreateMap<Department, DepartmentResponse>()
            .ForMember(dest => dest.EmployeeCount, opt => opt.Ignore());
    }
}
