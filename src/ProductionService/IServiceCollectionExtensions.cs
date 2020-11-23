using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace ProductionService
{
    public static class IServiceCollectionExtensions
    {
        public static void AddAllImplementing<TContract>(this IServiceCollection services, params Type[] typeIndicatingAssembly) 
            => services.AddAllImplementing<TContract>(typeIndicatingAssembly.Select(t => t.Assembly).ToArray());

        public static void AddAllImplementing<TContract>(this IServiceCollection services, params Assembly[] assemblies)
        {
            var contractType = typeof(TContract);
            assemblies
                .SelectMany(s => s.GetTypes())
                .Where(t => contractType.IsAssignableFrom(t) && !t.IsAbstract)
                .ToList()
                .ForEach(t => services.AddTransient(contractType, t));
        }
    }
}
