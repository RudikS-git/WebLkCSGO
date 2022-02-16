using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entities.Punishments
{
    public class BansInfo : PunishmentsInfo
    {
        [JsonProperty("lengthRows")]
        public long length;

        [JsonProperty("sourcebans")]
        public List<BanInfo> bansInfo { get; set; } = new List<BanInfo>();
    }
}
