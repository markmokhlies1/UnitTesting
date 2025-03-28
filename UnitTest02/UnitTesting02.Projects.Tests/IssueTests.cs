using System.Reflection;
using UnitTest02.Projects.Enums;
using UnitTest02.Projects.Exceptions;
using UnitTest02.Projects.Model;

namespace UnitTesting02.Projects.Tests
{
    public class IssueTests
    {
        [Fact]
        public void Constractuor_WithIsuueDescriptionIsNull_ThrowInvalidIssueDescriptionException()
        {
            //Arrange
            //Act
            var ctor = () => new Issue(null, Priority.Low, Category.Hardware, DateTime.Now);

            //Assert
            Assert.Throws<InvalidIssueDescriptionException>(() => ctor());
        }
        [Fact]
        public void Constractuor_WithIsuueDescriptionIsEmpty_ThrowInvalidIssueDescriptionException()
        {
            //Arrange
            //Act
            var ctor = () => new Issue("   ", Priority.Low, Category.Hardware, DateTime.Now);

            //Assert
            Assert.Throws<InvalidIssueDescriptionException>(() => ctor());
        }
        [Fact]
        public void Constractuor_WithIsuueDateTimeIsNull_ReturnDateTimeNow()
        {
            //Arrange
            var sut = new Issue("Number#1", Priority.Low, Category.Hardware);
            //Act
            var actual = sut.CreatedAt;
            //Assert
            Assert.NotEqual(default(DateTime), actual);
        }
        [Fact]
        public void GenerateKey_WithIsuueValiedPropertise_ReturnIssueKeyFirstSegment()
        {
            //Arrange
            var sut = new Issue("Number#1", Priority.Low, Category.Hardware,new DateTime(2022,10,11,12,30,00));
            
            //Act

            MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var actual = methodInfo.Invoke(sut, null).ToString();
            var excpected = "Hw-2022-L-ABCD1234";

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(excpected.Length, actual.Length);
        }
        [Fact]
        public void GenerateKey_WithIsuueHardware_ReturnIssueKeyFirstSegmentHw()
        {
            //Arrange
            var sut = new Issue("Number#1", Priority.High, Category.Hardware, new DateTime(2022, 10, 11, 12, 30, 00));

            //Act

            MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var actual = methodInfo.Invoke(sut, null).ToString();
            var excpected = "HW-2022-H-ABCD1234";

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(excpected.Split("-")[0], actual.Split("-")[0]);
        }
        [Fact]
        public void GenerateKey_WithIsuueProiorityLow_ReturnIssueKeyThirdSegmentL()
        {
            //Arrange
            var sut = new Issue("Number#1", Priority.Low, Category.Hardware, new DateTime(2022, 10, 11, 12, 30, 00));

            //Act

            MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var actual = methodInfo.Invoke(sut, null).ToString();
            var excpected = "L";

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(excpected, actual.Split("-")[2]);
        }
        [Fact]
        public void GenerateKey_WithIsuueCreatedAt_ReturnIssueKeySecondSegmentYYYY()
        {
            //Arrange
            var sut = new Issue("Number#1", Priority.Low, Category.Hardware, new DateTime(2022, 10, 11, 12, 30, 00));

            //Act

            MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var actual = methodInfo.Invoke(sut, null).ToString();
            var excpected = "2022";

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(excpected, actual.Split("-")[1]);
        }
        [Fact]
        public void GenerateKey_WithIsuueValiedProperties_ReturnIssueKeyFourAlphaNumeric()
        {
            //Arrange
            var sut = new Issue("Number#1", Priority.Low, Category.Hardware, new DateTime(2022, 10, 11, 12, 30, 00));

            //Act

            MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var FourthSegment = methodInfo.Invoke(sut, null).ToString().Split("-")[3];
            var IsAlphaNumeric = FourthSegment.All(x => char.IsLetterOrDigit(x));

            //Assert
            Assert.True(IsAlphaNumeric);
        }

        [Theory]
        [InlineData("Issue #1", Priority.Urgent, Category.Software, "2000-10-10", "SW-2000-U-ABCD1234")]
        [InlineData("issue #1", Priority.Low, Category.Software, "2022-10-10", "SW-2022-L-ABCD1234")]
        [InlineData("issue #1", Priority.Low, Category.UnKnown, "2018-10-10", "NA-2018-L-ABCD1234")]
        [InlineData("issue #1", Priority.Low, Category.Hardware, "1992-10-10", "HW-1992-L-ABCD1234")]
        [InlineData("issue #1", Priority.Medium, Category.Hardware, "2003-10-10", "HW-2003-M-ABCD1234")]
        [InlineData("issue #1", Priority.High, Category.Hardware, "2015-10-10", "HW-2015-H-ABCD1234")]
        [InlineData("issue #1", Priority.Urgent, Category.Hardware, "1980-10-10", "HW-1980-U-ABCD1234")]
        public void GenerateKey_WithValidIssueProperties_ReturnsExpectedKey
            (string desc, Priority priority, Category category, string createdAt, string expected)
        {
            // Arrange 
            var sut = new Issue(desc, priority, category, DateTime.Parse(createdAt));

            // Act
            MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var actual = methodInfo.Invoke(sut, null).ToString();



            // Assert 
            Assert.Equal(expected.Substring(0, 10), actual.Substring(0, 10));

        }
    }

}
