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

        public static void AddCuts(this IServiceCollection services, string sourceCutsPath, string completedCutsPath)
        {
            services.AddTransient<ICutTaskReader, CutTaskReader>((s) => new CutTaskReader(sourceCutsPath));
            services.AddTransient<ICutTaskManager, CutTaskManager>((s) => new CutTaskManager(sourceCutsPath, completedCutsPath));
        }
    }
}
