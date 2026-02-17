using apiConstruction.Application.DTOs.Responses;
using apiConstruction.Application.Services.Interfaces;
using apiConstruction.Domain.Entities;
using apiConstruction.Domain.Exceptions;
using apiConstruction.Domain.Interfaces;
using AutoMapper;

namespace apiConstruction.Application.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public DepartmentService(
        IDepartmentRepository departmentRepository,
        IEmployeeRepository employeeRepository,
        IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<DepartmentResponse> GetByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new NotFoundException(nameof(Department), id);
        }

        var response = _mapper.Map<DepartmentResponse>(department);
        var employees = await _employeeRepository.GetByDepartmentIdAsync(id);
        response.EmployeeCount = employees.Count();
        
        return response;
    }

    public async Task<IEnumerable<DepartmentResponse>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        var responses = _mapper.Map<IEnumerable<DepartmentResponse>>(departments);

        foreach (var response in responses)
        {
            var employees = await _employeeRepository.GetByDepartmentIdAsync(response.Id);
            response.EmployeeCount = employees.Count();
        }

        return responses;
    }

    public async Task<DepartmentResponse> CreateAsync(string name, string description)
    {
        var department = new Department
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };

        var createdDepartment = await _departmentRepository.AddAsync(department);
        return _mapper.Map<DepartmentResponse>(createdDepartment);
    }

    public async Task<DepartmentResponse> UpdateAsync(int id, string name, string description)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new NotFoundException(nameof(Department), id);
        }

        department.Name = name;
        department.Description = description;
        department.UpdatedAt = DateTime.UtcNow;

        await _departmentRepository.UpdateAsync(department);
        return _mapper.Map<DepartmentResponse>(department);
    }

    public async Task DeleteAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new NotFoundException(nameof(Department), id);
        }

        // Verificar si hay empleados asociados
        var employees = await _employeeRepository.GetByDepartmentIdAsync(id);
        if (employees.Any())
        {
            throw new BusinessRuleException($"No se puede eliminar el departamento porque tiene {employees.Count()} empleado(s) asociado(s).");
        }

        await _departmentRepository.DeleteAsync(id);
    }
}
