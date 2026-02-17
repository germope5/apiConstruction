using apiConstruction.Domain.Enums;

namespace apiConstruction.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public ContractType ContractType { get; set; }
    public EmployeeStatus Status { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
