using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models.Users;

namespace ProjectDK.BL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task AddEmployee(Employee employee)
        {
            var checkEmployee = await employeeRepository.CheckEmployee(employee.EmployeeID);
            if (!checkEmployee)
            {
                await employeeRepository.AddEmployee(employee);
            }
        }
        public async Task<bool> CheckEmployee(int id)
        {
            return await employeeRepository.CheckEmployee(id);
        }

        public async Task DeleteEmployee(int id)
        {
            if ((await employeeRepository.GetEmployeeDetails()).Any(x=>x.EmployeeID == id))
            {
                await employeeRepository.DeleteEmployee(id);
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployeeDetails()
        {
            return await employeeRepository.GetEmployeeDetails();
        }

        public async Task<Employee?> GetEmployeeDetails(int id)
        {
            return await employeeRepository.GetEmployeeDetails(id);
        }

        public async Task<UserInfo?> GetUserInfoAsync(string email, string password)
        {
            return await employeeRepository.GetUserInfoAsync(email, password);
        }
        public async Task UpdateEmployee(Employee employee)
        {
            var employeeCheck = await employeeRepository.GetEmployeeDetails(employee.EmployeeID);
            if (employeeCheck != null)
            {
                await employeeRepository.UpdateEmployee(employee);
            }
        }
    }
}
