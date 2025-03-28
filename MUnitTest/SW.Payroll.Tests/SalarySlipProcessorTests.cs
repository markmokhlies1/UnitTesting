using Moq;
using SW.Payroll.BuilderPattern;
using SW.Payroll.Calculation;
using SW.Payroll.Enums;
using SW.Payroll.Service;

namespace SW.Payroll.Tests
{
    public class SalarySlipProcessorTests
    {

        #region CalculateBasicSalaryTests
        [Fact]
        public void CalculateBasicSalary_ForEmployeeWageAndWorkingDayes_ReturnBasicSalary()
        {
            //Arrange
            var employee = new EmployeeBuilder().SetWage(500).SetWorkingDays(20).Build();

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateBasicSalary(employee);
            var expected = 10000m;

            //Assert

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void CalculateBasicSalary_EmployeeIsNull_ThrowArgumentNullException()
        {
            //Arrange

            Employee employee = null;

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculateBasicSalary(employee);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }
        #endregion

        #region CalculateSpouseAllowanceTests

        [Fact]
        public void CalculateSpouseAllowance_EmployeeIsNull_ReturnArgumentNullException()
        {
            //Arrange
            Employee employee =null;

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculateSpouseAllowance(employee);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }

        [Fact]
        public void CalculateSpouseAllowance_EmployeeIsMarried_ReturnSpouseAllowanceAmount()
        {
            //Arrange
            var employee = new EmployeeBuilder().SetMaritalStatus(true).Build();

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateSpouseAllowance(employee);
            var expected = Constants.SpouseAllowanceAmount;

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void CalculateSpouseAllowance_EmployeeIsNotMarried_ReturnZero()
        {
            //Arrange
            var employee = new EmployeeBuilder().SetMaritalStatus(false).Build();

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateSpouseAllowance(employee);
            var expected = 0m;

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region CalculateDependancyAllowanceTests
        [Fact]
        public void CalculateDependancyAllowance_EmployeeIsNull_ReturnArgumentNullException()
        {
            //Arrange
            Employee employee = null;

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculateDependancyAllowance(employee);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }
        [Fact]
        public void CalculateDependancyAllowance_EmployeeTotalDependanciesLessThanZero_ReturnArgumentOutOfRangeException()
        {
            //Arrange
            var employee = new EmployeeBuilder().SetTotalDependancies(-2).Build();

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee,decimal> func = (e) => salarySlipProcessor.CalculateDependancyAllowance((employee));

            //Assert

            Assert.Throws<ArgumentOutOfRangeException>(() => func(employee));
        }

        [Fact]
        public void CalculateDependancyAllowance_EmployeeTotalDependanciesMoreThanMaxDependantsFactor_ReturnMaxDependancyAllowanceAmount()
        {
            //Arrange

            var employee = new EmployeeBuilder()
                .SetTotalDependancies(Constants.MaxDependantsFactor + 10)
                .Build();

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateDependancyAllowance(employee);
            var expected = Constants.MaxDependancyAllowanceAmount;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CalculateDependancyAllowance_EmployeeTotalDependanciesEqualToZero_ReturnZero()
        {
            //Arrange

            var employee = new EmployeeBuilder()
                .SetTotalDependancies(0)
                .Build(); 

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateDependancyAllowance(employee);
            var expected = 0m;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CalculateDependancyAllowance_EmployeeTotalDependanciesIsMoreThanZeroAndLessThanorEqualMaxDependantsFactor_ReturneTotalDependanciesDotDependancyAllowancePerChildAmount()
        {
            //Arrange

            var employee = new EmployeeBuilder()
                .SetTotalDependancies(Constants.MaxDependantsFactor - 2)
                .Build();

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateDependancyAllowance(employee);
            var expected = employee.TotalDependancies * Constants.DependancyAllowancePerChildAmount;

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region CalculatePensionTests
        [Fact]
        public void CalculatePension_EmployeeIsNull_ThrowArgumentNullException()
        {
            //Arrange

            Employee employee = null;

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculatePension(employee);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }

        [Fact]
        public void CalculatePension_EmployeeHasNotPension_ReturnZero()
        {
            //Arrange
            var employee = new EmployeeBuilder()
                .SetPensionPlan(false)
                .Build(); 

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            var actual = salarySlipProcessor.CalculatePension(employee);
            var excepexted = 0m;

            //Assert

            Assert.Equal(excepexted,actual);
        }
        [Fact]
        public void CalculatePension_EmployeeHasPension_ReturnPensionRateDotBasicSalary()
        {
            //Arrange
            var employee = new EmployeeBuilder()
                .SetPensionPlan(true)
                .Build();

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            var actual = salarySlipProcessor.CalculatePension(employee);
            var excepexted = Constants.PensionRate * salarySlipProcessor.CalculateBasicSalary(employee);

            //Assert

            Assert.Equal(excepexted, actual);
        }
        #endregion

        #region CalculateTaxTests
        [Fact]
        public void CalculateTax_EmployeeBasicSalaryMoreThanOrEqualMediumSalaryThreshold_ReturnBasicSalaryDotHighSalaryTaxFactor()
        {
            //Arrange
            var employee = new EmployeeBuilder()
                .SetWage(Constants.MediumSalaryThreshold)
                .SetWorkingDays(2)
                .Build();

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            var actual = salarySlipProcessor.CalculateBasicSalary(employee) * Constants.HighSalaryTaxFactor;
            var excepexted = salarySlipProcessor.CalculateTax(employee);

            //Assert
            Assert.Equal(excepexted, actual);
        }

        [Fact]
        public void CalculateTax_EmployeeBasicSalaryMoreThanOrEqualLowSalaryThreshold_ReturnBasicSalaryDotMediumSalaryTaxFactor()
        {
            //Arrange
            var employee = new EmployeeBuilder()
                .SetWage(Constants.LowSalaryThreshold)
                .SetWorkingDays(1)
                .Build();

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            var actual = salarySlipProcessor.CalculateBasicSalary(employee) * Constants.MediumSalaryTaxFactor;
            var excepexted = salarySlipProcessor.CalculateTax(employee);

            //Assert
            Assert.Equal(excepexted, actual);
        }
        [Fact]
        public void CalculateTax_EmployeeBasicSalaryLessThanLowSalaryThreshold_ReturnBasicSalaryDotLowSalaryTaxFactor()
        {
            //Arrange
            var employee = new EmployeeBuilder()
                .SetWage(500)
                .SetWorkingDays(1)
                .Build();

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            var actual = salarySlipProcessor.CalculateBasicSalary(employee) * Constants.LowSalaryTaxFactor;
            var excepexted = salarySlipProcessor.CalculateTax(employee);

            //Assert
            Assert.Equal(excepexted, actual);
        }
        #endregion

        #region CalculateDangerPayTests

        [Fact]
        public void CalculateDangerPay_EmployeeIsNull_ThrowArgumentNullException()
        {
            //Arrange

            Employee employee = null;

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculateDangerPay(employee);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }
        [Fact]
        public void CalculateDangerPay_EmployeeIsDanger_ReturnDangerPayAmount()
        {
            //Arrange

            var employee = new EmployeeBuilder()
                .SetDanger(true)
                .Build(); 

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateDangerPay(employee);
            var expected = Constants.DangerPayAmount;

            //Assert

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void CalculateDangerPay_EmployeeIsDangerOffAndInDangerZone_ReturnDangerPayAmount()
        {
            //Arrange
            var employee = new EmployeeBuilder()
                            .SetDanger(false)
                            .SetDutyStation("ukran")
                            .Build();

            var mock = new Mock<IZoneService>();
            var setup = mock.Setup(z=>z.IsDangerZone(employee.DutyStation)).Returns(true);

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(mock.Object);

            var actual = salarySlipProcessor.CalculateDangerPay(employee);
            var expected = Constants.DangerPayAmount;

            //Assert

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void CalculateDangerPay_EmployeeIsDangerOffAndNotInDangerZone_ReturnZero()
        {
            //Arrange

            var employee = new EmployeeBuilder()
                .SetDanger(false)
                .SetDutyStation("ukran")
                .Build();
            var mock = new Mock<IZoneService>();
            var setup = mock.Setup(z => z.IsDangerZone(employee.DutyStation)).Returns(false);

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(mock.Object);

            var actual = salarySlipProcessor.CalculateDangerPay(employee);
            var expected = 0m;

            //Assert

            Assert.Equal(expected, actual);
        }
        #endregion

        #region CalculateHealthInsuranceTests

        [Fact]
        public void CalculateHealthInsurance_EmployeeIsNull_ThrowArgumentNullException()
        {
            //Arrange

            Employee employee = null;

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculateHealthInsurance(employee);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }

        [Fact]
        public void CalculateHealthInsurance_EmployeeHealthInsuranceHasValue_ReturnZero()
        {
            //Arrange
            var employee = new Employee { HealthInsurancePackage = null };

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);
            var excepected = 0m;
            var actual = salarySlipProcessor.CalculateHealthInsurance(employee);

            //Assert
            Assert.Equal(excepected, actual);
        }
        [Fact]
        public void CalculateHealthInsurance_EmployeeHealthInsuranceIsBasic_ReturnBasicHealthInsurance()
        {
            //Arrange
            var employee = new Employee { HealthInsurancePackage = HealthInsurancePackage.Basic };

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);
            var excepected = Constants.BasicHealthCareAmount;
            var actual = salarySlipProcessor.CalculateHealthInsurance(employee);

            //Assert
            Assert.Equal(excepected, actual);
        }
        [Fact]
        public void CalculateHealthInsurance_EmployeeHealthInsuranceIsFair_ReturnFairHealthInsurance()
        {
            //Arrange
            var employee = new Employee { HealthInsurancePackage = HealthInsurancePackage.Fair };

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);
            var excepected = Constants.FairHealthCareAmount;
            var actual = salarySlipProcessor.CalculateHealthInsurance(employee);

            //Assert
            Assert.Equal(excepected, actual);
        }

        [Fact]
        public void CalculateHealthInsurance_EmployeeHealthInsuranceIsPremium_ReturnPremiumHealthInsurance()
        {
            //Arrange
            var employee = new Employee { HealthInsurancePackage = HealthInsurancePackage.Premium };

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);
            var excepected = Constants.PremiumHealthCareAmount;
            var actual = salarySlipProcessor.CalculateHealthInsurance(employee);

            //Assert
            Assert.Equal(excepected, actual);
        }
        #endregion

        #region CalculateTransportationAlloweceTests
        [Fact]
        public void CalculateTransportationAllowece_EmployeeWorkFromOffice_ReturnTransportationAllowece()
        {
            //Arrange

            var employee = new EmployeeBuilder()
                .SetWorkPlatform(WorkPlatform.Office)
                .Build(); 

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateTransportationAllowece(employee);
            var expected = Constants.TransportationAllowanceAmount;

            //Assert

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void CalculateTransportationAllowece_EmployeeIsNull_ThrowArgumentNullException()
        {
            //Arrange

            Employee employee = null;

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculateTransportationAllowece(employee);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }
        [Fact]
        public void CalculateTransportationAllowece_EmployeeWorkRemote_ReturnTransportationAllowece()
        {
            //Arrange

            var employee = new EmployeeBuilder()
                .SetWorkPlatform(WorkPlatform.Remote)
                .Build();

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateTransportationAllowece(employee);
            var expected = 0m;

            //Assert

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void CalculateTransportationAllowece_EmployeeWorkHybridMode_ReturnTransportationAllowece()
        {
            //Arrange

            var employee = new EmployeeBuilder()
                .SetWorkPlatform(WorkPlatform.Hybrid)
                .Build();

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.CalculateTransportationAllowece(employee);
            var expected = Constants.TransportationAllowanceAmount/2;

            //Assert

            Assert.Equal(expected, actual);
        }
        #endregion

        #region calculateNetSalaryTests

        [Fact]
        public void calculateNetSalary_EmployeeIsNull_ReturnArgumentNullException()
        {
            //Arrange
            Employee employee = null;

            //Act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.calculateNetSalary(employee);

            //Assert
            Assert.Throws<ArgumentNullException>(() => func(employee));
        }
        [Fact]
        public void calculateNetSalary_EmployeeNotNull_ReturnNetSallary()
        {
            //Arrange
            var employee = new EmployeeBuilder()
                .SetDutyStation("New York")
                .SetWage(5000)
                .SetWorkingDays(30)
                .SetMaritalStatus(true)
                .SetDanger(true)
                .SetPensionPlan(true)
                .SetHealthInsurance(HealthInsurancePackage.Basic)
                .SetWorkPlatform(WorkPlatform.Hybrid)
                .Build();

            //Act

            var salarySlipProcessor = new SalarySlipProcessor(null);

            var actual = salarySlipProcessor.calculateNetSalary(employee);

            var totalEarnings =
                salarySlipProcessor.CalculateBasicSalary(employee) +
                salarySlipProcessor.CalculateTransportationAllowece(employee) +
                salarySlipProcessor.CalculateSpouseAllowance(employee) +
                salarySlipProcessor.CalculateDangerPay(employee) +
                salarySlipProcessor.CalculateDependancyAllowance(employee);

            var totalDeductions =
                salarySlipProcessor.CalculatePension(employee) +
                salarySlipProcessor.CalculateHealthInsurance(employee) +
                salarySlipProcessor.CalculateTax(employee);

            var expected = totalEarnings - totalDeductions;

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}