using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SchedulingFunction
{
    public static class RebuildSite
    {
        [FunctionName("rebuild-site")]
        public static async Task RunAsync(
            //[TimerTrigger("0/1 * * * * *")]TimerInfo timer,
            [TimerTrigger("0 30 6 * * *")]TimerInfo timer,
            ILogger log)
        {
            var githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            using (var client = new HttpClient())
            {
                var media = new MediaTypeWithQualityHeaderValue(
                    "application/vnd.github.mister-fantastic-preview");

                client.DefaultRequestHeaders.Accept.Add(media);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("token", githubToken);
                client.DefaultRequestHeaders.UserAgent.Add(
                    new ProductInfoHeaderValue(new ProductHeaderValue("vplauzon")));

                var response = await client.PostAsync(
                    "https://api.github.com/repos/vplauzon/vplauzon.github.io/pages/builds",
                    new StringContent(string.Empty));
                var content = await response.Content.ReadAsStringAsync();

                log.LogInformation($"Response status: {response.StatusCode}");
                log.LogInformation($"Response content: {content}");

                if ((int)response.StatusCode >= 300)
                {
                    throw new InvalidOperationException("Failure to rebuild site");
                }
            }
        }
    }
}