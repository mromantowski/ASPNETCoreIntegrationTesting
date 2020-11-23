using System.Threading.Tasks;

namespace ProductionService.Model.Cuts
{
    public interface ICutTaskReader
    {
        Task<CutTask> Read(string taskId);
    }
}
