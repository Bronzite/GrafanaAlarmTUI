using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GrafanaAlarmTUI
{
    public class GrafanaApi
    {

        /// <summary>
        /// Get the alerts from a Grafana instance.
        /// </summary>
        /// <param name="connection">The connection to Grafana</param>
        /// <returns>An array of Grafana Alerts.  May have zero elements.</returns>
        public static GrafanaAlert[] GetAlerts(GrafanaConnection connection)
        {
            string baseUrl = connection.GrafanaUrl;
            while(baseUrl.EndsWith("/")) { baseUrl = baseUrl.Substring(0, baseUrl.Length - 1); }
            HttpWebRequest hwr = HttpWebRequest.CreateHttp($"{baseUrl}/api/alertmanager/grafana/api/v2/alerts");
            hwr.Headers.Add("Authorization", $"Bearer {connection.GrafanaApiKey}");
            WebResponse response = hwr.GetResponse();
            string responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
            GrafanaAlert[] retval = System.Text.Json.JsonSerializer.Deserialize<GrafanaAlert[]>(responseString);
            //Need to compute the timespan difference because the JSON deserializer is converting
            //the Datetime to UTC with the wrong offset.
            TimeSpan tsDifference = (DateTime.Now - DateTime.UtcNow);
            
            foreach (GrafanaAlert alert in retval)
            {
                alert.connection = connection;
                //Adjust start and end times.
                alert.startsAt = alert.startsAt.Add(tsDifference);
                alert.endsAt = alert.endsAt.Add(tsDifference);
            }
            return retval;
        }
    }
}
