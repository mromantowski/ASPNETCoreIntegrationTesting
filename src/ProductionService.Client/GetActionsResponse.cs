using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionService.Client
{
    public class GetActionsResponse : Response
    {
        public List<string> Actions { get; set; }
    }
}
