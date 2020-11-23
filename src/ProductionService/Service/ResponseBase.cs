namespace ProductionService.Service
{
    public class ResponseBase
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccessful { get => string.IsNullOrEmpty(ErrorMessage); }
    }
}
