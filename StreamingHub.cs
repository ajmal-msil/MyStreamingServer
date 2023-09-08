using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using MyStreamingServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
  using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

namespace MyStreamingServer
{
    public class StreamingHub : Hub
    {
        private readonly IHostApplicationLifetime _lifetime;
        private List<StreamingDatum> streamingDataList;
        private StreamingData streamingData; // Declare the variable

        private readonly string dataFilePath = "data/streamingData.json";
        private CancellationTokenSource _cancellationTokenSource;

        public StreamingHub(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
            LoadData();
            _cancellationTokenSource = new CancellationTokenSource();
            StartStreaming();
        }

        private void LoadData()
        {
            try
            {
                var dataJson = System.IO.File.ReadAllText(dataFilePath);
                streamingData = StreamingData.FromJson(dataJson);

                Console.WriteLine("StreamingHub loaded data.");
            }
            catch (Exception ex)
            {
                // Handle file read error
                streamingDataList = new List<StreamingDatum>();
                Console.WriteLine("Handle file read error: " + ex.Message);
            }
        }

        public async Task StartStreaming()

        {

        Console.WriteLine("StartStreaming.");

            while (true)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    break;
                }

                UpdateData();
                var updatedDataJson = JsonConvert.SerializeObject(streamingData); // Serialize to JSON

                await Clients.All.SendAsync("ReceiveStreamingData", updatedDataJson);
                        Console.WriteLine("ReceiveStreamingData.");

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _cancellationTokenSource.Cancel();
            await base.OnDisconnectedAsync(exception);
        }

        private void UpdateData()
{
    // Simulate updating the double properties with random values
    var random = new Random();

    if (streamingData?.StreamingDataStreamingData != null)
    {
        foreach (var data in streamingData.StreamingDataStreamingData)
        {
            if (data?.Data != null)
            {
                var dataProperties = data.Data.GetType().GetProperties();

                foreach (var property in dataProperties)
                {
                    if (property.PropertyType == typeof(double))
                    {
                        // Generate random double value
                        double newValue = random.NextDouble() * 100;
                        property.SetValue(data.Data, newValue);
                    }
                }
            }
        }
    }
}

    }
}
