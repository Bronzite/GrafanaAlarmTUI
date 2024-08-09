using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GrafanaAlarmTUI
{

    public class GrafanaAlert
    {
        public Annotations annotations { get; set; }
        public DateTime endsAt { get; set; }
        public string fingerprint { get; set; }
        public Receiver[] receivers { get; set; }
        public DateTime startsAt { get; set; }
        public Status status { get; set; }
        public DateTime updatedAt { get; set; }
        public string generatorURL { get; set; }
        public Labels labels { get; set; }
        [JsonIgnore]
        public GrafanaConnection connection { get; set; }
    }

    public class Annotations
    {
        public string __orgId__ { get; set; }
        public string __value_string__ { get; set; }
        public string __values__ { get; set; }
        public string description { get; set; }
        public string summary { get; set; }
    }

    public class Status
    {
        public object[] inhibitedBy { get; set; }
        public object[] silencedBy { get; set; }
        public string state { get; set; }
    }

    public class Labels
    {
        public string __alert_rule_uid__ { get; set; }
        public string __name__ { get; set; }
        public string alertname { get; set; }
        public string grafana_folder { get; set; }
        public string instance { get; set; }
        public string job { get; set; }
        public string slack { get; set; }
    }

    public class Receiver
    {
        public object active { get; set; }
        public object integrations { get; set; }
        public string name { get; set; }
    }

}
