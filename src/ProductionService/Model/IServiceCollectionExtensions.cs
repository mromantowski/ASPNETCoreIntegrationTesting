using Microsoft.Extensions.DependencyInjection;
using ProductionService.Model.Cuts;
using ProductionService.Model.ServiceActions;

namespace ProductionService.Model
{
    public static class IServiceCollectionExtensions
    {
        public static void AddServiceActions(this IServiceCollection services)
        {
            services.AddAllImplementing<IServiceActionProvider>(typeof(IServiceActionProvider));
        }

        public static void AddCuts(this IServiceCollection services, string cutsPath)
        {
            services.AddTransient<ICutTaskReader, CutTaskReader>((s) => new CutTaskReader(cutsPath));
        }
    }
}
