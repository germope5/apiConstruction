using apiConstruction.Application.DTOs.Requests;
using apiConstruction.Application.DTOs.Responses;
using apiConstruction.Application.Services.Interfaces;
using apiConstruction.Domain.Entities;
using apiConstruction.Domain.Enums;
using apiConstruction.Domain.Exceptions;
using apiConstruction.Domain.Interfaces;
using AutoMapper;

namespace apiConstruction.Application.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    public async Task<EmployeeResponse> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), id);
        }

        return _mapper.Map<EmployeeResponse>(employee);
    }

    public async Task<PaginatedResponse<EmployeeResponse>> GetAllAsync(QueryParameters queryParameters)
    {
        var employees = await _employeeRepository.GetAllAsync();
        
        // Aplicar filtros
        if (queryParameters.DepartmentId.HasValue)
        {
            employees = employees.Where(e => e.DepartmentId == queryParameters.DepartmentId.Value);
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
        {
            var searchTerm = queryParameters.SearchTerm.ToLower();
            employees = employees.Where(e => 
                e.FirstName.ToLower().Contains(searchTerm) ||
                e.LastName.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm));
        }

        // Aplicar ordenamiento
        if (!string.IsNullOrWhiteSpace(queryParameters.SortBy))
        {
            // Implementar lógica de ordenamiento dinámico si es necesario
        }

        var totalCount = employees.Count();
        var pagedEmployees = employees
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToList();

        var employeeResponses = _mapper.Map<IEnumerable<EmployeeResponse>>(pagedEmployees);

        return new PaginatedResponse<EmployeeResponse>
        {
            Data = employeeResponses,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request)
    {
        // Validar que el departamento existe
        var departmentExists = await _departmentRepository.ExistsAsync(request.DepartmentId);
        if (!departmentExists)
        {
            throw new NotFoundException(nameof(Department), request.DepartmentId);
        }

        var employee = _mapper.Map<Employee>(request);
        employee.CreatedAt = DateTime.UtcNow;
        employee.Status = (EmployeeStatus)request.Status;
        employee.ContractType = (ContractType)request.ContractType;

        var createdEmployee = await _employeeRepository.AddAsync(employee);
        return _mapper.Map<EmployeeResponse>(createdEmployee);
    }

    public async Task<EmployeeResponse> UpdateAsync(int id, UpdateEmployeeRequest request)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), id);
        }

        // Validar que el departamento existe
        var departmentExists = await _departmentRepository.ExistsAsync(request.DepartmentId);
        if (!departmentExists)
        {
            throw new NotFoundException(nameof(Department), request.DepartmentId);
        }

        _mapper.Map(request, employee);
        employee.UpdatedAt = DateTime.UtcNow;
        employee.Status = (EmployeeStatus)request.Status;
        employee.ContractType = (ContractType)request.ContractType;

        await _employeeRepository.UpdateAsync(employee);
        return _mapper.Map<EmployeeResponse>(employee);
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), id);
        }

        await _employeeRepository.DeleteAsync(id);
    }
}
