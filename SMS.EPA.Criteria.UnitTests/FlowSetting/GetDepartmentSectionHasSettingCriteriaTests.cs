namespace SMS.EPA.Criteria.UnitTests
{
    using Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class GetDepartmentSectionHasSettingCriteriaTests
    {
        [Test]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetDepartmentSectionHasSettingCriteria();
            string expected = "FlowTemplate_GetDepartmentHasSetting";

            // Act
            string actual = criteria.GetSettingKey();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(1, true)]
        [TestCase(0, false)]
        public void IsValid_CheckParameter_ReturnAsExpected(int flowTemplateId, bool expected)
        {
            // Arrange
            var criteria = new GetDepartmentSectionHasSettingCriteria
            {
                FlowTemplateId = flowTemplateId
            };

            // Act
            bool actual = criteria.IsValid();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
