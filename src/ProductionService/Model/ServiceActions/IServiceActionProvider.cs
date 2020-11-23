using System;
using System.Collections.Generic;

namespace ProductionService.Model.ServiceActions
{
    public interface IServiceActionProvider
    {
        IEnumerable<string> GetActions(Machine machine, DateTime day);
    }
}
