using apiConstruction.Application.DTOs.Requests;
using apiConstruction.Application.DTOs.Responses;
using apiConstruction.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace apiConstruction.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DepartmentResponse>>> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        return Ok(ApiResponse<DepartmentResponse>.SuccessResponse(department));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentResponse>>>> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<DepartmentResponse>>.SuccessResponse(departments));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DepartmentResponse>>> Create(
        [FromBody] CreateDepartmentRequest request)
    {
        var department = await _departmentService.CreateAsync(request.Name, request.Description);
        return CreatedAtAction(nameof(GetById), new { id = department.Id }, 
            ApiResponse<DepartmentResponse>.SuccessResponse(department, "Departamento creado exitosamente"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DepartmentResponse>>> Update(
        int id, 
        [FromBody] UpdateDepartmentRequest request)
    {
        var department = await _departmentService.UpdateAsync(id, request.Name, request.Description);
        return Ok(ApiResponse<DepartmentResponse>.SuccessResponse(department, "Departamento actualizado exitosamente"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        await _departmentService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Departamento eliminado exitosamente"));
    }
}

public class CreateDepartmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateDepartmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
