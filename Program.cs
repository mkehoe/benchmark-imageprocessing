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
    public class Timestamps
    {
        public long total { get; set; }
        public TimestampDTO serverTimestamps { get; set; }
    }
    class Program
    {

        static async Task Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Base Url is Needed");
                return;
            }
            var baseUrl = args[0];

            await TestHelpers.ApiWarmUp(baseUrl);

            var syncTest = new SyncronousTest(baseUrl);
            await syncTest.ResetStats();

            await syncTest.RunResizeTests();

            await syncTest.RunTileTests();

            await syncTest.GetStats();

            var asyncTest = new AsyncTest(baseUrl);
            await asyncTest.ResetStats();

            await asyncTest.RunResizeTests();

            await asyncTest.RunTileTests();

            await asyncTest.GetStats();

            syncTest.PrintStats();
            asyncTest.PrintStats();

        }

    }
}
