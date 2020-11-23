using System;
using System.IO;

namespace ProductionService.Model.Cuts
{
    public class CutTaskManager : ICutTaskManager
    {
        private readonly string sourcePath;
        private readonly string completedPath;

        public CutTaskManager(string sourcePath, string completedPath)
        {
            this.sourcePath = sourcePath;
            this.completedPath = completedPath;
        }

        public void MarkCompleted(string taskId)
        {
            if (taskId == null)
            {
                throw new ArgumentNullException(nameof(taskId));
            }

            var source = Path.Combine(sourcePath, $"CT-{taskId}.txt");
            if (!File.Exists(source))
            {
                throw new FileNotFoundException($"Nie odnaleziono pliku {source} dla {taskId}");
            }

            var destination = Path.Combine(completedPath, $"CT-{taskId}.txt");
            
            File.Move(source, destination);
        }
    }
}
