// Connect to the server using gRPC channel.

using System.Diagnostics;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using MyApp.Shared;

Console.WriteLine("Hello, World!");
var channel = GrpcChannel.ForAddress("http://localhost:5244");
var ci = channel.CreateCallInvoker();
var client = MagicOnionClient.Create<IMyFirstService>(ci); // <-- Create a client only once.

var clientWithOptions = client.WithOptions(new CallOptions()
{
    Headers = { }
}); // <-- Create a client with new options here.
var result = await clientWithOptions.SumAsync(123, 456);

Console.WriteLine(result);