using AestheticEMR.Core.Models.Employees;
using EmployeeEntity = AestheticEMR.Core.Models.Employees.Employees;

namespace AestheticEMR.Core.Services.Employees.Interfaces;

public interface IEmployeeService
{
    Task<string> GenerateEmpIdAsync();
    Task<IEnumerable<EmployeeEntity>> GetAllAsync();
    Task<IEnumerable<QryEmployees>> GetReportRowsAsync();
    Task<EmployeeEntity?> GetByIdAsync(string empId);
    Task<EmployeeEntity> CreateAsync(EmployeeEntity employee);
    Task<EmployeeEntity> UpdateAsync(EmployeeEntity employee);
    Task DeleteAsync(string empId);
    Task<IEnumerable<Designation>> GetDesignationsAsync();
    Task<IEnumerable<EmpDepartments>> GetDepartmentsAsync();
}
