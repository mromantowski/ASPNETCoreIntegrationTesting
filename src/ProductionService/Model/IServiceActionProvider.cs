using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionService.Model
{
    public interface IServiceActionProvider
    {
        IEnumerable<string> GetActions(Machine machine, DateTime day);
    }
}
