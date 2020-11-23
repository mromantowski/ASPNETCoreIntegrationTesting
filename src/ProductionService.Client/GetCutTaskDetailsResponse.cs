using System.Collections.Generic;

namespace ProductionService.Client
{
    public class GetCutTaskDetailsResponse : Response
    {
        public List<CutTaskDetails> Details { get; set; }
    }
}
