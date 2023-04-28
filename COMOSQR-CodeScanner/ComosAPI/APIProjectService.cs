using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COMOSQR_CodeScanner.ComosAPI
{
    public class APIProjectService
    {
        public async Task<HttpResponseMessage> GetProjects(HttpClient client, string databaseID)
        {
            HttpResponseMessage response;
            string adress = $"/public/api/projects/v1/dbs/{databaseID}/projects";
            response = await client.GetAsync(adress);

            return response;

        }
    }
}
