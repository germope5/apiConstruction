namespace apiConstruction.Application.DTOs.Requests;

public class CreateEmployeeRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public int ContractType { get; set; }
    public int Status { get; set; }
    public int DepartmentId { get; set; }
}
