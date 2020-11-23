using System.Collections.Generic;

namespace ProductionService.Client
{
    public class GetActionsResponse : Response
    {
        public List<string> Actions { get; set; }
    }
}
