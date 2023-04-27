using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COMOSQR_CodeScanner.ViewModel
{
    internal class HTTPBaseViewModel : BaseViewModel
    {
        public static string BaseDBID;
        public static HttpClient BaseHttpClient;
        public static Model.ProjectModel BaseProject;
        public static Model.DocSearchModel BaseQueryId;
        public static string BaseQRUID;

        private static System.Timers.Timer _hearbeatTimer = new System.Timers.Timer();

        public static ComosAPI.APISessionService SessionService = new ComosAPI.APISessionService();
        public static ComosAPI.APIProjectService ProjectService = new ComosAPI.APIProjectService();
        public static ComosAPI.APIDocService DocService = new ComosAPI.APIDocService();

        public void InitalizeHeartBeat(int heartBeat)
        {
            _hearbeatTimer.Interval = heartBeat * 1000;
            _hearbeatTimer.Elapsed += async (s, el) =>
            {
                var response = await SessionService.SendHeartbeat(BaseHttpClient);
            };
            _hearbeatTimer.Start();
        }

        public string ManageResponseCodes(HttpResponseMessage response)
        {
            switch (response.ReasonPhrase)
            {
                case "Unauthorized":
                    return "failed, wrong username or password was provided.";

                case "Forbidden":
                    return "failed, missing licence: restart the licence server.";

                default:
                    return "failed, API responded: " + response.ReasonPhrase + ".";
            }
        }

    }
}
