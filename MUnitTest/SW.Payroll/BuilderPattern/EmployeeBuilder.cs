using SW.Payroll.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Payroll.BuilderPattern
{
    public class EmployeeBuilder : IEmployeeBuilder
    {
        private Employee _employee = new Employee();

        public IEmployeeBuilder SetId(int id)
        {
            _employee.Id = id;
            return this;
        }

        public IEmployeeBuilder SetName(string name)
        {
            _employee.Name = name;
            return this;
        }

        public IEmployeeBuilder SetDutyStation(string dutyStation)
        {
            _employee.DutyStation = dutyStation;
            return this;
        }

        public IEmployeeBuilder SetWage(decimal wage)
        {
            _employee.Wage = wage;
            return this;
        }

        public IEmployeeBuilder SetWorkingDays(int days)
        {
            _employee.WorkingDays = days;
            return this;
        }

        public IEmployeeBuilder SetMaritalStatus(bool isMarried)
        {
            _employee.IsMarried = isMarried;
            return this;
        }

        public IEmployeeBuilder SetTotalDependancies(int count)
        {
            _employee.TotalDependancies = count;
            return this;
        }

        public IEmployeeBuilder SetDanger(bool isDanger)
        {
            _employee.IsDanger = isDanger;
            return this;
        }

        public IEmployeeBuilder SetPensionPlan(bool hasPensionPlan)
        {
            _employee.HasPensionPlan = hasPensionPlan;
            return this;
        }

        public IEmployeeBuilder SetHealthInsurance(HealthInsurancePackage? package)
        {
            _employee.HealthInsurancePackage = package;
            return this;
        }

        public IEmployeeBuilder SetWorkPlatform(WorkPlatform platform)
        {
            _employee.WorkPlatform = platform;
            return this;
        }

        public Employee Build()
        {
            return _employee;
        }
    }
}
