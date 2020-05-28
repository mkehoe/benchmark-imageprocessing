using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageProcessingApiPerfTest
{
    public class TestHelpers
    {
        public static async Task ApiWarmUp(string baseUrl)
        {
            var client = new HttpClient();
            var file = await System.IO.File.ReadAllBytesAsync("Images/testImg.jpg");
            var byteArrayContent = new ByteArrayContent(file);
            {
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(byteArrayContent, "Image", "filename.jpg");
                multipartContent.Add(new StringContent("960"), "ResizeWidth");
                multipartContent.Add(new StringContent("540"), "ResizeHeight");
                for (int i = 0; i < 5; i++)
                {
                    var postResponse = await client.PostAsync($"{baseUrl}/images/resize", multipartContent);
                    var resp = await postResponse.Content.ReadAsByteArrayAsync();
                }
            }

            {
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(byteArrayContent, "Image", "filename.jpg");

                for (int i = 0; i < 5; i++)
                {
                    var postResponse = await client.PostAsync($"{baseUrl}/images/tile", multipartContent);
                    var resp = await postResponse.Content.ReadAsByteArrayAsync();
                }
            }
        }
    }
}
