using Xunit;
using KooliProjekt.Data;
using System.ComponentModel.DataAnnotations;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ModelTests
{
    public class TeamTests
    {
        [Fact]
        public void Team_NameRequired_ValidationFailsWhenEmpty()
        {
            // Arrange
            var team = new Team { Name = "" };
            var context = new ValidationContext(team);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(team, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
        }
    }
}