using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProductionService.Model.Cuts
{
    public class CutTaskReader : ICutTaskReader
    {
        private static readonly Encoding Encoding = CodePagesEncodingProvider.Instance.GetEncoding("windows-1250");
        private readonly string basePath;

        public CutTaskReader(string basePath)
        {
            this.basePath = basePath;
        }

        public async Task<CutTask> Read(string taskId)
        {
            if (taskId == null)
            {
                throw new ArgumentNullException(nameof(taskId));
            }

            var path = Path.Combine(basePath, $"CT-{taskId}.txt");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Nie odnaleziono pliku {path} dla {taskId}");
            }

            using(var reader = new StreamReader(path, Encoding))
            {
                var result = new CutTask { TaskId = taskId };

                string currentCode = null;
                int currentCount = 0;

                string row;
                while ((row = await reader.ReadLineAsync()) != null)
                {
                    var code = row.Substring(0, 14).Trim();
                    if (code != currentCode)
                    {
                        if (currentCode != null && currentCount > 0)
                        {
                            result.Details.Add(new CutTaskDetails { ItemCode = currentCode, Count = currentCount });
                        }

                        currentCode = code;
                        currentCount = 0;
                    }

                    var count = row.Substring(27, 3).Trim();
                    if (count.EndsWith('N'))
                    {
                        currentCount++;
                    }
                }

                if (currentCode != null && currentCount > 0)
                {
                    result.Details.Add(new CutTaskDetails { ItemCode = currentCode, Count = currentCount });
                }

                return result;
            } 
        }
    }
}
