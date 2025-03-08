using System;
using MagicOnion;
using MagicOnion.Client;
using MyApp.Shared;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    public event Action <int> OnResult; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        try
        {
            var channel = GrpcChannelx.ForAddress("http://localhost:5244");
            var client = MagicOnionClient.Create<IMyFirstService>(channel);

            var result = await client.SumAsync(100, 200);
            Debug.Log($"100 + 200 = {result}");
            OnResult?.Invoke(result);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}