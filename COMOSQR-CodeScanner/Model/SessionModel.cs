using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMOSQR_CodeScanner.Model
{
    public class SessionModel
    {
        [JsonProperty("SessionId")]
        public string Id { get; set; }

        [JsonProperty("UserName")]
        public string UserId { get; set; }

        [JsonProperty("HeartBeatInterval")]
        public int HeartBeat { get; set; }
    }
}
