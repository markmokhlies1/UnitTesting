using SW.Payroll.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Payroll.BuilderPattern
{
    public interface IEmployeeBuilder
    {
        IEmployeeBuilder SetId(int id);
        IEmployeeBuilder SetName(string name);
        IEmployeeBuilder SetDutyStation(string dutyStation);
        IEmployeeBuilder SetWage(decimal wage);
        IEmployeeBuilder SetWorkingDays(int days);
        IEmployeeBuilder SetMaritalStatus(bool isMarried);
        IEmployeeBuilder SetTotalDependancies(int count);
        IEmployeeBuilder SetDanger(bool isDanger);
        IEmployeeBuilder SetPensionPlan(bool hasPensionPlan);
        IEmployeeBuilder SetHealthInsurance(HealthInsurancePackage? package);
        IEmployeeBuilder SetWorkPlatform(WorkPlatform platform);
        Employee Build();
    }
}
