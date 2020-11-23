using ProductionService.Model.ServiceActions;
using Shouldly;
using System;
using Xunit;

namespace ProductionService.UnitTests.Model.ServiceActions
{
    [Trait("Category", "Unit")]
    public class Saw1ActionProviderFActs
    {
        private readonly Saw1ActionProvider provider = new Saw1ActionProvider();
        
        [Fact]
        public void GetActions_WhenValidMachine_ReturnsActions()
        {
            var actions = provider.GetActions(Machine.Saw1, DateTime.Now);

            actions.ShouldContain("Opró¿nij pojemnik na odpady");
            actions.ShouldContain("SprawdŸ stan ostrza");
        }

        [Theory]
        [InlineData(Machine.Saw2)]
        [InlineData(Machine.CoffeeMaker)]
        public void GetActions_WhenInvalidMachine_ReturnsNoActions(Machine machine)
        {
            var actions = provider.GetActions(machine, DateTime.Now);

            actions.ShouldBeEmpty();
        }
    }
}
