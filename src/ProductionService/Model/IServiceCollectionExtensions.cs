using Microsoft.Extensions.DependencyInjection;
using ProductionService.Model.ServiceActions;

namespace ProductionService.Model
{
    public static class IServiceCollectionExtensions
    {
        public static void AddProductionServiceModel(this IServiceCollection services)
        {
            services.AddAllImplementing<IServiceActionProvider>(typeof(IServiceActionProvider));
        }
    }
}
