using System.Collections.Generic;

namespace ProductionService.Model.Cuts
{
    public class CutTask
    {
        public string TaskId { get; set; }
        public IList<CutTaskDetails> Details { get; set; } = new List<CutTaskDetails>();
    }
}
