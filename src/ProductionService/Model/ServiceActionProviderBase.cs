using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionService.Model
{
    public abstract class ServiceActionProviderBase : IServiceActionProvider
    {
        public abstract Machine Machine { get; }
        public abstract IList<string> Actions { get; }
        public virtual bool DateMatches(DateTime day) => true;
        public IEnumerable<string> GetActions(Machine machine, DateTime day)
            => machine == Machine && DateMatches(day) ? Actions: Enumerable.Empty<string>();
    }
}
