using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COMOSQR_CodeScanner.ComosAPI
{
    internal class APISessionService
    {
        public async Task<HttpResponseMessage> Login(HttpClient client)
        {
            HttpResponseMessage response;
            string adress = "/public/api/sessions/v1/sessions/actions/login";
            response = await client.PostAsync(adress, null);

            return response;

        }

        public async Task<HttpResponseMessage> SendHeartbeat(HttpClient client)
        {
            HttpResponseMessage response;
            string address = "/public/api/sessions/v1/sessions/actions/send_heartbeat";

            response = await client.PostAsync(address, null);

            return response;
        }
    }
}
