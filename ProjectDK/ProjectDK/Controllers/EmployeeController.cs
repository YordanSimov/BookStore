using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.Interfaces;
using ProjectDK.Models.Models.Users;

namespace ProjectDK.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet(nameof(GetEmployeeDetails))]
        [Authorize(AuthenticationSchemes ="Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeDetails()
        {
            var employees = await employeeService.GetEmployeeDetails();
            if (employees.Count() <= 0)
            {
                return NotFound("There aren't any employees in the collection");
            }
            return Ok(employees);
        }
        [HttpPost(nameof(AddEmployee))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (await employeeService.GetEmployeeDetails(employee.EmployeeID) != null)
            {
                return BadRequest("Employee already exists");
            }
            await employeeService.AddEmployee(employee);
            return Ok("Successfully added employee");
        }

        [HttpGet("GetId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than 0");
            }
            var result = await employeeService.GetEmployeeDetails(id);
            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [HttpPut(nameof(UpdateEmployee))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            if (await employeeService.GetEmployeeDetails(employee.EmployeeID) == null)
            {
                return BadRequest("Invalid employee");
            }
            var result = employeeService.UpdateEmployee(employee);

            return Ok(result);
        }
        [HttpGet(nameof(CheckEmployee))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckEmployee(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than 0");
            }
            var result = await employeeService.CheckEmployee(id);
            if (!result) return NotFound("ID not found - " + id);

            return Ok(result);
        }
    }
}
