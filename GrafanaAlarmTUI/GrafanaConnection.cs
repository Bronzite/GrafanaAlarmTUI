using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GrafanaAlarmTUI
{
    /// <summary>
    /// The information necessary to connect to a Grafana instance.
    /// </summary>
    public class GrafanaConnection
    {

        [JsonPropertyName("grafanaUrl")]
        public string GrafanaUrl { get; set; }
        [JsonPropertyName("grafanaApiKey")]
        public string GrafanaApiKey { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public DateTime LastUpdate { get; set; }

    }
}
