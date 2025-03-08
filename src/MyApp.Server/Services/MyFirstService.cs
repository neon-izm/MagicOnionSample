using MagicOnion;
using MagicOnion.Server;
using MyApp.Shared;

namespace MyApp.Server.Services;

// Implements RPC service in the server project.
// The implementation class must inherit `ServiceBase<IMyFirstService>` and `IMyFirstService`
public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
{
    // `UnaryResult<T>` allows the method to be treated as `async` method.
    public UnaryResult<int> SumAsync(int x, int y)
    {
        Console.WriteLine($"Received:{x}, {y}");
        return UnaryResult.FromResult(x + y);
    }

    public async UnaryResult<string> SayHelloAsync(string name)
    {
        // sleep 1 second
        await Task.Delay(1000);
        return $"Hello {name} from MagicOnion.";
    }
}