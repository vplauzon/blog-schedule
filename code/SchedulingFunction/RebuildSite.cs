using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SchedulingFunction
{
    public static class RebuildSite
    {
        [FunctionName("RebuildSite")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo timer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}