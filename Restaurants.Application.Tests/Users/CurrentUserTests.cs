using FluentAssertions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;


namespace Restaurants.Application.Tests.Users
{
    public class CurrentUserTests
    {
        // TestMethod_Scenario_ExpectedResult
        [Fact()]
        public void IsInRoleTest_WithMatchingRole_ShouldReturnTrue()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);
            // act

            var isInRole = currentUser.IsInRole(UserRoles.Admin);
            // assert
            isInRole.Should().BeTrue();
        }

        [Fact()]
        public void IsInRoleTest_WithNoMatchingRole_ShouldReturnFalse()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);
            // act

            var isInRole = currentUser.IsInRole(UserRoles.Owner);
            // assert
            isInRole.Should().BeFalse();
        }

        [Fact()]
        public void IsInRoleTest_WithNoMatchingRoleCase_ShouldReturnFalse()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);
            // act

            var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());
            // assert
            isInRole.Should().BeFalse();
        }
    }

}