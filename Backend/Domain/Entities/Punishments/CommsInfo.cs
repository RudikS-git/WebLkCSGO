using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entities.Punishments
{
    public class CommsInfo : PunishmentsInfo
    {
        [JsonProperty("lengthRows")]
        public long length;

        [JsonProperty("sourcecomms")]
        public List<CommInfo> commsInfo { get; set; } = new List<CommInfo>();
    }
}
