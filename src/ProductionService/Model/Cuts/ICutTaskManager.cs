namespace ProductionService.Model.Cuts
{
    public interface ICutTaskManager
    {
        void MarkCompleted(string taskId);
    }
}
