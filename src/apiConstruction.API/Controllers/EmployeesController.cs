using apiConstruction.Application.DTOs.Requests;
using apiConstruction.Application.DTOs.Responses;
using apiConstruction.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace apiConstruction.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EmployeeResponse>>> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        return Ok(ApiResponse<EmployeeResponse>.SuccessResponse(employee));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<EmployeeResponse>>>> GetAll(
        [FromQuery] QueryParameters queryParameters)
    {
        var result = await _employeeService.GetAllAsync(queryParameters);
        return Ok(ApiResponse<PaginatedResponse<EmployeeResponse>>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Create(
        [FromBody] CreateEmployeeRequest request)
    {
        var employee = await _employeeService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, 
            ApiResponse<EmployeeResponse>.SuccessResponse(employee, "Empleado creado exitosamente"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Update(
        int id, 
        [FromBody] UpdateEmployeeRequest request)
    {
        var employee = await _employeeService.UpdateAsync(id, request);
        return Ok(ApiResponse<EmployeeResponse>.SuccessResponse(employee, "Empleado actualizado exitosamente"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        await _employeeService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Empleado eliminado exitosamente"));
    }
}
