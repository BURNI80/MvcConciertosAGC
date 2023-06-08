using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using MvcConciertosAGC.Models;
using Newtonsoft.Json;

namespace MvcConciertosAGC.Helpers
{
    public static class HelperSecret
    {

        public static async Task<SecretAWS> GetSecret()
        {
            string secretName = "secreto";
            string region = "us-east-1";

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
            };


            GetSecretValueResponse response;

            try
            {
                response = await client.GetSecretValueAsync(request);
            }
            catch (Exception e)
            {
                // For a list of the exceptions thrown, see
                // https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
                throw e;
            }

            string secret = response.SecretString;

            return JsonConvert.DeserializeObject<SecretAWS>(secret);

            // Your code goes here
        }


    }
}
