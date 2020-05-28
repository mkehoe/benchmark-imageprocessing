using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageProcessingApiPerfTest
{
    public class SyncronousTest : TestBase
    {
        public SyncronousTest(string url): base(url)
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

            for (int i = 0; i < 100; i++)
            {
                var sw = new Stopwatch();
                sw.Start();
                Console.WriteLine($"Running Resize {i}");
                var postResponse = await client.PostAsync($"{baseUrl}/images/resize", multipartContent);
                var resp = await postResponse.Content.ReadAsByteArrayAsync();
                sw.Stop();
                if (postResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"{postResponse.StatusCode} {postResponse.Content}");
                }

                var ts = new Timestamps
                {
                    total = sw.ElapsedMilliseconds,
                };
                resizeTimestamps.Add(ts);
            }
        }

        public async Task RunTileTests()
        {
            var file = await System.IO.File.ReadAllBytesAsync("Images/testImg.jpg");
            var byteArrayContent = new ByteArrayContent(file);
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(byteArrayContent, "Image", "filename.jpg");

            for (int i = 0; i < 100; i++)
            {
                var sw = new Stopwatch();
                sw.Start();
                Console.WriteLine($"Running Tile {i}");
                var postResponse = await client.PostAsync($"{baseUrl}/images/tile", multipartContent);
                var resp = await postResponse.Content.ReadAsByteArrayAsync();
                sw.Stop();
                if (postResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"{postResponse.StatusCode} {postResponse.Content}");
                }

                var ts = new Timestamps
                {
                    total = sw.ElapsedMilliseconds
                };
                tileTimestamps.Add(ts);
            }
        }
    }
}
