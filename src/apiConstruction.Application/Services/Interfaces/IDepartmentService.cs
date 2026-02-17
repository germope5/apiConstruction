using apiConstruction.Application.DTOs.Responses;

namespace apiConstruction.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentResponse> GetByIdAsync(int id);
    Task<IEnumerable<DepartmentResponse>> GetAllAsync();
    Task<DepartmentResponse> CreateAsync(string name, string description);
    Task<DepartmentResponse> UpdateAsync(int id, string name, string description);
    Task DeleteAsync(int id);
}
