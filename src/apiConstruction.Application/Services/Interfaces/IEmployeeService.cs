using apiConstruction.Application.DTOs.Requests;
using apiConstruction.Application.DTOs.Responses;

namespace apiConstruction.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeResponse> GetByIdAsync(int id);
    Task<PaginatedResponse<EmployeeResponse>> GetAllAsync(QueryParameters queryParameters);
    Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request);
    Task<EmployeeResponse> UpdateAsync(int id, UpdateEmployeeRequest request);
    Task DeleteAsync(int id);
}
