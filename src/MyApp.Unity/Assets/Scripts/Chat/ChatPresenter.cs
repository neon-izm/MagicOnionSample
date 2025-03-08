using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Client;
using MagicOnion;
using MyApp;
using R3;
using MyApp.Shared.Hubs;
using MyApp.Shared.MessagePackObjects;
using MyApp.Shared.Services;
using UnityEngine;

public interface IChatPresenter
{
    ReadOnlyReactiveProperty<string> ChatText { get; }
    ReadOnlyReactiveProperty<long> Rtt { get; }
    bool IsJoined { get; }
    Task InitializeAsync();
    Task JoinOrLeaveAsync(string userName);
    Task SendMessageAsync(string message);
    Task SendReportAsync(string report);
    Task DisconnectAsync();
    Task GenerateExceptionAsync();
    Task UnaryGenerateExceptionAsync();
}

public class ChatPresenter : IChatPresenter, IChatHubReceiver, IDisposable
{
    private CancellationTokenSource shutdownCancellation = new CancellationTokenSource();
    private ChannelBase channel;
    private IChatHub streamingClient;
    private IChatService client;

    // UniRxによるUI更新用ReactiveProperty
    private readonly ReactiveProperty<string> chatText = new ReactiveProperty<string>(string.Empty);
    private readonly ReactiveProperty<long> rtt = new ReactiveProperty<long>(0);

    public ReadOnlyReactiveProperty<string> ChatText => chatText;
    public ReadOnlyReactiveProperty<long> Rtt => rtt;
    public bool IsJoined { get; private set; } = false;

    public async Task InitializeAsync()
    {
        channel = GrpcChannelx.ForAddress(SystemConstants.ServerUrl);
        // 接続の試行ループ
        while (!shutdownCancellation.IsCancellationRequested)
        {
            try
            {
                var options = StreamingHubClientOptions.CreateWithDefault()
                    .WithClientHeartbeatResponseReceived(x => { rtt.Value = (long)x.RoundTripTime.TotalMilliseconds; })
                    .WithClientHeartbeatInterval(TimeSpan.FromSeconds(10))
                    .WithClientHeartbeatTimeout(TimeSpan.FromSeconds(5));
                streamingClient = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(
                    channel, this, options, cancellationToken: shutdownCancellation.Token);
                _ = RegisterDisconnectEventAsync(streamingClient);
                break;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            await Task.Delay(5000);
        }
        client = MagicOnionClient.Create<IChatService>(channel);
    }

    private async Task RegisterDisconnectEventAsync(IChatHub client)
    {
        try
        {
            await client.WaitForDisconnect();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            Debug.Log("Disconnected from server.");
            // 必要に応じた再接続処理などを実装可能
        }
    }

    public async Task JoinOrLeaveAsync(string userName)
    {
        if (IsJoined)
        {
            await streamingClient.LeaveAsync();
            IsJoined = false;
            chatText.Value += "\n<color=grey>部屋から退出しました。</color>";
        }
        else
        {
            var request = new JoinRequest { RoomName = "SampleRoom", UserName = userName };
            await streamingClient.JoinAsync(request);
            IsJoined = true;
            chatText.Value += $"\n<color=grey>{userName}として入室しました。</color>";
        }
    }

    public async Task SendMessageAsync(string message)
    {
        if (!IsJoined) return;
        await streamingClient.SendMessageAsync(message);
    }

    public async Task GenerateExceptionAsync()
    {
        if (!IsJoined) return;
        await streamingClient.GenerateException("client exception(streaminghub)!");
    }

    public async Task SendReportAsync(string report)
    {
        await client.SendReportAsync(report);
    }

    public async Task UnaryGenerateExceptionAsync()
    {
        await client.GenerateException("client exception(unary)!");
    }

    public async Task DisconnectAsync()
    {
        shutdownCancellation.Cancel();
        if (IsJoined)
        {
            await streamingClient.LeaveAsync();
        }
        if (streamingClient != null)
            await streamingClient.DisposeAsync();
        if (channel != null)
            await channel.ShutdownAsync();
    }

    // IChatHubReceiver の実装
    public void OnJoin(string name)
    {
        chatText.Value += $"\n<color=grey>{name} entered the room.</color>";
    }

    public void OnLeave(string name)
    {
        chatText.Value += $"\n<color=grey>{name} left the room.</color>";
    }

    public void OnSendMessage(MessageResponse message)
    {
        chatText.Value += $"\n{message.UserName}：{message.Message}";
    }

    public Task<string> HelloAsync(string name, int age)
    {
        return Task.FromResult($"Hello {name} ({age}) from Client");
    }

    public void Dispose()
    {
        shutdownCancellation.Cancel();
    }
}
