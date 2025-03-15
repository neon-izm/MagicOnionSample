using System;
using MagicOnion;
using MagicOnion.Client;
using MyApp;
using MyApp.Shared;
using UnityEngine;

public class GrpcClientUnity : MonoBehaviour
{
    public event Action <int> OnResult; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        try
        {
            var channel = GrpcChannelx.ForAddress(SystemConstants.ServerUrl);
            var client = MagicOnionClient.Create<IMyFirstService>(channel);

            var result = await client.SumAsync(100, 200);
            Debug.Log($"100 + 200 = {result}");
            OnResult?.Invoke(result);
            
            var hello = await client.SayHelloAsync("World");
            Debug.Log(hello);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        
    }
}