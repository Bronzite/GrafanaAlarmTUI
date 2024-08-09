using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GrafanaAlarmTUI
{
    public class ConfigurationFile
    {
        /// <summary>
        /// Load the configuration file from disk.
        /// </summary>
        /// <param name="sConfigurationFile">Path to the configuration file</param>
        /// <returns>Configuration File object</returns>
        public static ConfigurationFile LoadConfiguration(string sConfigurationFile)
        {
            string json = System.IO.File.ReadAllText(sConfigurationFile);
            ConfigurationFile configurationFile = System.Text.Json.JsonSerializer.Deserialize<ConfigurationFile>(json);
            return configurationFile;
        }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("connections")]
        public GrafanaConnection[] Connections { get; set; }


    }
}
