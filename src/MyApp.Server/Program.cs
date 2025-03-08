

using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Kestrel の設定で HTTPS エンドポイントを上書きし、HTTP のみで待ち受ける
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5244, listenOptions =>
    {
        // gRPC を使う場合は HTTP/2 が必要ですが、HTTP/2 は暗号化なしでも動作させる設定です
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
// Add services to the container.
builder.Services.AddMagicOnion();

var app = builder.Build();
// 明示的にポート5244でリッスンするように指定（全てのネットワークインターフェースで待ち受け）
app.Urls.Add("http://0.0.0.0:5244");
// Configure the HTTP request pipeline.
app.MapMagicOnionService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
