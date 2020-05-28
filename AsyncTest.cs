using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageProcessingApiPerfTest
{
    public class AsyncTest : TestBase
    {
        public AsyncTest(string url) : base(url)
        {
        }

        public async Task RunResizeTests()
        {
            var file = await System.IO.File.ReadAllBytesAsync("Images/testImg.jpg");
            var byteArrayContent = new ByteArrayContent(file);
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(byteArrayContent, "Image", "filename.jpg");
            multipartContent.Add(new StringContent("960"), "ResizeWidth");
            multipartContent.Add(new StringContent("540"), "ResizeHeight");

            for (int j = 0; j < 10; j++)
            {
                var tasks = new List<Task<long>>();
                for (int i = 0; i < 12; i++)
                {
                    Console.WriteLine($"Running Resize {i}");
                    tasks.Add(resizeAsync(multipartContent));
                }
                await Task.WhenAll(tasks);

                foreach (var t in tasks)
                {
                    var ts = new Timestamps
                    {
                        total = t.GetAwaiter().GetResult()
                    };
                    resizeTimestamps.Add(ts);
                }
            }
        }

        private async Task<long> resizeAsync(MultipartFormDataContent multipartContent)
        {
            var sw = new Stopwatch();
            sw.Start();
            var postResponse = await client.PostAsync($"{baseUrl}/images/resize", multipartContent);
            await postResponse.Content.ReadAsByteArrayAsync();
            sw.Stop();
            if (postResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"{postResponse.StatusCode} {postResponse.Content}");
            }

            return sw.ElapsedMilliseconds;
        }

        public async Task RunTileTests()
        {
            var file = await System.IO.File.ReadAllBytesAsync("Images/testImg.jpg");
            var byteArrayContent = new ByteArrayContent(file);
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(byteArrayContent, "Image", "filename.jpg");

            for (int j = 0; j < 10; j++)
            {
                var tasks = new List<Task<long>>();
                for (int i = 0; i < 12; i++)
                {
                    Console.WriteLine($"Running Tile {i}");
                    tasks.Add(tileAsync(multipartContent));
                }
                await Task.WhenAll(tasks);

                foreach (var t in tasks)
                {
                    var ts = new Timestamps
                    {
                        total = t.GetAwaiter().GetResult()
                    };
                    tileTimestamps.Add(ts);
                }
            }
        }

        private async Task<long> tileAsync(MultipartFormDataContent multipartContent)
        {
            var sw = new Stopwatch();
            sw.Start();
            var postResponse = await client.PostAsync($"{baseUrl}/images/tile", multipartContent);
            await postResponse.Content.ReadAsByteArrayAsync();
            sw.Stop();
            if (postResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"{postResponse.StatusCode} {postResponse.Content}");
            }

            return sw.ElapsedMilliseconds;
        }
    }
}
