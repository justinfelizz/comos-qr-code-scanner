using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COMOSQR_CodeScanner.ComosAPI
{
    internal class APIDocService
    {
        public async Task<HttpResponseMessage> GetDocuments(HttpClient client, string databaseID, string projectID, string queryID)
        {
            HttpResponseMessage response;
            string address = $"/public/api/projects/v1/dbs/{databaseID}/projects/{projectID}/queries/{queryID}/result";

            response = await client.GetAsync(address);

            return response;
        }

        public async Task<HttpResponseMessage> GetDocumentStream(HttpClient client, string databaseID, string projectID, string docID)
        {
            string address = $"/public/api/projects/v1/dbs/{databaseID}/projects/{projectID}/documents/{docID}";

            var response = await client.GetAsync(address);

            return response;
        }
    }
}
