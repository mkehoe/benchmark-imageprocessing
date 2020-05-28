using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace ImageProcessingApiPerfTest
{
    public class TestBase
    {
        protected readonly HttpClient client = new HttpClient();
        protected IList<Timestamps> tileTimestamps = new List<Timestamps>();
        protected IList<Timestamps> resizeTimestamps = new List<Timestamps>();
        protected string baseUrl;

        public TestBase(string url)
        {
            baseUrl = url;
        }
        
        public async Task ResetStats()
        {
            var result = await client.PostAsync($"{baseUrl}/images/reset_stats", null);
        }

        public async Task GetStats()
        {
            var result = await client.GetStringAsync($"{baseUrl}/images/stats");
            //Console.WriteLine($"{result}");

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };

            var stats = JsonSerializer.Deserialize<ResultsDTO>(result, options);

            Console.WriteLine($"ResizeTimestampCount={stats.resizeTimestamps.Count} TileTimestampCount={stats.tileTimestamps.Count}");
            for (int i = 0; i < resizeTimestamps.Count; i++)
            {
                resizeTimestamps[i].serverTimestamps = stats.resizeTimestamps[i];
            }

            for (int i = 0; i < tileTimestamps.Count; i++)
            {
                tileTimestamps[i].serverTimestamps = stats.tileTimestamps[i];
            }

        }

        public void PrintStats()
        {
            {
                var one = resizeTimestamps.Select(rs => rs.total).ToList();
                var two = resizeTimestamps.Select(rs => rs.serverTimestamps.t1).ToList();
                var three = resizeTimestamps.Select(rs => rs.serverTimestamps.t2).ToList();
                var four = resizeTimestamps.Select(rs => rs.serverTimestamps.t3).ToList();
                var five = resizeTimestamps.Select(rs => rs.serverTimestamps.t4).ToList();
                var six = resizeTimestamps.Select(rs => rs.serverTimestamps.t5).ToList();

                Console.WriteLine("Resize Timing");

                Console.WriteLine($"{one.Average()} - {two.Average()} - {three.Average()} - {four.Average()} - {five.Average()} - {six.Average()}");
            }
            {
                var one = tileTimestamps.Select(rs => rs.total).ToList();
                var two = tileTimestamps.Select(rs => rs.serverTimestamps.t1).ToList();
                var three = tileTimestamps.Select(rs => rs.serverTimestamps.t2).ToList();
                var four = tileTimestamps.Select(rs => rs.serverTimestamps.t3).ToList();
                var five = tileTimestamps.Select(rs => rs.serverTimestamps.t4).ToList();
                var six = tileTimestamps.Select(rs => rs.serverTimestamps.t5).ToList();

                Console.WriteLine("Tile Timing");

                Console.WriteLine($"{one.Average()} - {two.Average()} - {three.Average()} - {four.Average()} - {five.Average()} - {six.Average()}");
            }

        }
    }
}