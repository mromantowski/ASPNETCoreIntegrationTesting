using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionService.Service
{
    public class ResponseBase
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccessful { get => string.IsNullOrEmpty(ErrorMessage); }
    }
}
