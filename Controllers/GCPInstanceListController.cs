using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Compute.beta;
using Google.Apis.Compute.beta.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ConnectGCPToGetComputeEngineLists.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GCPInstanceListController : ControllerBase
    {
        string projectId = "kubectltestprojectpburnwal1";

        private readonly ILogger<GCPInstanceListController> _logger;

        public GCPInstanceListController(ILogger<GCPInstanceListController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<string> Get()
        {
            ComputeService computeService = new ComputeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(),
                ApplicationName = "Google-ComputeSample/0.1",
            });
            // TODO: Update placeholder value.

            // The name of the zone for this request.
            string zone = "us-central1-f";  // TODO: Update placeholder value.

            InstancesResource.ListRequest request = computeService.Instances.List(projectId, zone);
            List<string> instances = new List<string>();
            InstanceList response;
            do
            {
                // To execute asynchronously in an async method, replace `request.Execute()` as shown:
                response = request.Execute();
                // response = await request.ExecuteAsync();

                if (response.Items == null)
                {
                    continue;
                }
                foreach (Instance instance in response.Items)
                {
                    // TODO: Change code below to process each `instance` resource:
                    instances.Add(JsonConvert.SerializeObject(instance));
                }
                request.PageToken = response.NextPageToken;
            } while (response.NextPageToken != null);
            return instances;
        }
        public static GoogleCredential GetCredential()
        {
            GoogleCredential credential = Task.Run(() => GoogleCredential.GetApplicationDefaultAsync()).Result;
            //if (credential.IsCreateScopedRequired)
            //{
            //    credential = credential.CreateScoped("https://www.googleapis.com/auth/cloud-platform");
            //}
            return credential;
        }
    }
}