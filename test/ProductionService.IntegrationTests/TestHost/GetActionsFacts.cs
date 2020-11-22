using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductionService.IntegrationTests.TestHost
{
    [Trait("Category", "Integration")]
    public class GetActionsFacts : IntegrationTestsBase
    {
        public class Query
        {
            public string Machine { get; set; }
            public string Day { get; set; }
        }

        public class Response
        {
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
            public List<string> Actions { get; set; }
        }

        [Theory]
        [InlineData("Saw1")]
        [InlineData("Saw2")]
        [InlineData("CoffeeMaker")]
        [InlineData("saw1")]
        [InlineData("saw2")]
        [InlineData("coffeemaker")]
        public async Task GetActions_ForValidMachine_ReturnsSuccess(string machine)
        {
            var response = await GetAsync<Response>("/api/actions", new Query { Machine = machine, Day = "2020-11-03" });
            
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
            var response = await GetAsync<Response>("/api/actions", new Query { Machine = machine, Day = "2020-11-03" });

            response.IsSuccessful.ShouldBeFalse();
            response.ErrorMessage.ShouldStartWith("Nie ma takiej maszyny");
        }

        [Theory]
        [InlineData("2020-11-01")]
        [InlineData("2020-11-02")]
        [InlineData("2020-11-03")]
        public async Task GetActions_WhenSaw1_ReturnsValidActions(string date)
        { 
            var response = await GetAsync<Response>("/api/actions", new Query { Machine = "Saw1", Day = date });

            response.Actions.ShouldContain("Opróżnij pojemnik na odpady");
            response.Actions.ShouldContain("Sprawdź stan ostrza");
        }

        [Theory]
        [InlineData("2020-11-03")]
        [InlineData("2020-11-05")]
        [InlineData("2020-11-07")]
        public async Task GetActions_WhenSaw1AndWednesdayThursdayOrSaturday_ReturnsActionsWithCalibration(string date)
        {
            var response = await GetAsync<Response>("/api/actions", new Query { Machine = "Saw1", Day = date });

            response.Actions.ShouldContain("Wykonaj kalibrację urządzenia");
        }

        [Theory]
        [InlineData("2020-11-04")]
        [InlineData("2020-11-06")]
        [InlineData("2020-11-08")]
        public async Task GetActions_WhenSaw1AndOtherDay_ReturnsActionsWithoutCalibration(string date)
        { 
            var response = await GetAsync<Response>("/api/actions", new Query { Machine = "Saw1", Day = date });

            response.Actions.ShouldNotContain("Wykonaj kalibrację urządzenia");
        }
    }
}
