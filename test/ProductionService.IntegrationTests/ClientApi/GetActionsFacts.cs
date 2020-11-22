using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductionService.IntegrationTests.ClientApi
{
    [Trait("Category", "Integration")]
    public class GetActionsFacts : IntegrationTestsBase
    {
        [Theory]
        [InlineData("Saw1")]
        [InlineData("Saw2")]
        [InlineData("CoffeeMaker")]
        [InlineData("saw1")]
        [InlineData("saw2")]
        [InlineData("coffeemaker")]
        public async Task GetActions_ForValidMachine_ReturnsSuccess(string machine)
        {
            var response = await ClientApi.GetActions(machine, new DateTime(2020, 11, 03));
            
            response.IsSuccessful.ShouldBeTrue();
            response.ErrorMessage.ShouldBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Passat B5")]
        [InlineData("saw7")]
        [InlineData("Saw11")]
        public async Task GetActions_ForInvalidMachine_ReturnsFailure(string machine)
        {
            var response = await ClientApi.GetActions(machine, new DateTime(2020, 11, 03));

            response.IsSuccessful.ShouldBeFalse();
            response.ErrorMessage.ShouldStartWith("Nie ma takiej maszyny");
        }

        [Theory]
        [InlineData(2020, 11, 01)]
        [InlineData(2020, 11, 02)]
        [InlineData(2020, 11, 03)]
        public async Task GetActions_WhenSaw1_ReturnsValidActions(int year, int month, int day)
        {
            var response = await ClientApi.GetActions("Saw1", new DateTime(year, month, day));

            response.Actions.ShouldContain("Opróżnij pojemnik na odpady");
            response.Actions.ShouldContain("Sprawdź stan ostrza");
        }

        [Theory]
        [InlineData(2020, 11, 03)]
        [InlineData(2020, 11, 05)]
        [InlineData(2020, 11, 07)]
        public async Task GetActions_WhenSaw1AndWednesdayThursdayOrSaturday_ReturnsActionsWithCalibration(int year, int month, int day)
        {
            var response = await ClientApi.GetActions("Saw1", new DateTime(year, month, day));

            response.Actions.ShouldContain("Wykonaj kalibrację urządzenia");
        }

        [Theory]
        [InlineData(2020, 11, 04)]
        [InlineData(2020, 11, 06)]
        [InlineData(2020, 11, 08)]
        public async Task GetActions_WhenSaw1AndOtherDay_ReturnsActionsWithoutCalibration(int year, int month, int day)
        {
            var response = await ClientApi.GetActions("Saw1", new DateTime(year, month, day));

            response.Actions.ShouldNotContain("Wykonaj kalibrację urządzenia");
        }
    }
}
