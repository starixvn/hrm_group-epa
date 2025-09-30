namespace SMS.EPA.Criteria.UnitTests
{
    using Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class GetAllTargetEmployeeSettingCriteriaTests
    {
        [Test]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetAllTargetEmployeeSettingCriteria();
            string expected = "EpaTargetTemplateEmployee_Get";

            // Act
            string actual = criteria.GetSettingKey();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void IsValid_ReturnTrue()
        {
            // Arrange
            var criteria = new GetAllTargetEmployeeSettingCriteria();

            // Act
            bool actual = criteria.IsValid();

            // Assert
            Assert.That(actual, Is.True);
        }
    }
}
