using ConsoleApp1;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Tasker.OpenFGA
{
    public class FGAMethods
    {
        public static string AuthorizationModelId = "01H5T4C03KQDC7YR67C8F3VAQC";//"01H5B0VND3034JA8BJP4GBMWH7";
        public static string StoreId = "01H1AM5QQYN9VZTJ8MNW2HXAJV";//https://localhost:7293/api/canviewtask?employeeId=2
        public static OpenFgaApi CreateStoreClient(/*String StoreId*/)
        {
            var configuration = new Configuration()
            {
                ApiScheme = "http",
                ApiHost = "localhost:8080",
                StoreId = StoreId
            };
            var fgaClient = new OpenFgaApi(configuration);

            return fgaClient;
        }


        public static async Task<FgaTaskResult> AddRelationAsync(string source, string destination, string relation)
        {
            var fgaClient = CreateStoreClient();

            FgaTaskResult addResult = await AddNewRelationshipTuple(fgaClient, source, relation, destination);


            return addResult;
        }


        public static async Task<ListObjectsResponse> ListCheck(OpenFgaApi fgaClient, String tupleUser, String tupleRelation, String objectType)
        {
            var body = new ListObjectsRequest
            {
                AuthorizationModelId = AuthorizationModelId,
                User = tupleUser,
                Relation = tupleRelation,
                Type = objectType,
            };
            var response = await fgaClient.ListObjects(body);

            return response;
        }

        public static async Task<FgaTaskResult> AddNewRelationshipTuple(OpenFgaApi fgaClient, string tupleUser, string tupleRelation, string tupleObject)
        {
            try
            {
                Console.WriteLine(tupleUser + " " + tupleRelation + " " + tupleObject);
                var response = await fgaClient.Write(new WriteRequest
                {
                    Writes = new TupleKeys(new List<TupleKey>() {
                    new() { User = tupleUser, Relation = tupleRelation, Object = tupleObject}
                }),
                    AuthorizationModelId = AuthorizationModelId
                });
                return new FgaTaskResult() { Status = true };//"Success";

            }
            catch (Exception ex)
            {
                return new FgaTaskResult() { Status = false, Message = ex.Message };
            }
        }
    }
}
