using apiConstruction.Domain.Entities;

namespace apiConstruction.Domain.Interfaces;

public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(int id);
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department> AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
