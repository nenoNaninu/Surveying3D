using System.Threading.Tasks;
using MicroBatchFramework;

namespace Surveying3D.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await BatchHost.CreateDefaultBuilder().RunBatchEngineAsync<CommandBase>(args);
        }
    }
}