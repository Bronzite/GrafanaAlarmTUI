using System.Dynamic;
using System.Reflection;

namespace GrafanaAlarmTUI
{
    internal class Program
    {
        public static string Title = "Grafana Alarm TUI";
        public static string Version = "1.0.0";
        static void Main(string[] args)
        {
            string sConfigurationFile = "configuration.json";
            bool bShowHelp = false;
            //Process Arguments
            for (int i =0;i<args.Length; i++)
            {
                if ((args[i] == "-c" || args[i] == "--config") && i+1 < args.Length)
                {
                    sConfigurationFile = args[i+1]; 
                }
                if (args[i] == "-h" || args[i] == "--help" || args[i] == "-?")
                {
                    bShowHelp = true;
                }
            }

            if(bShowHelp) 
            {
                //Show help and exit
                Console.WriteLine($"Grafana Alarm TUI v{Version}");
                Console.WriteLine("Usage: GrafanaAlarmTUI [options]");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("  -c, --config <file>  Configuration file to use. Default is configuration.json");
                Console.WriteLine("  -h, --help            Show this help");

                return;
            }
            
            //Load configuration.  We will only do this once.
            ConfigurationFile configurationFile = ConfigurationFile.LoadConfiguration(sConfigurationFile);
            
            //Clean up the Title.
            if(configurationFile.Title != null && configurationFile.Title.Trim() != "")
            {
                Title = configurationFile.Title;
            }

            //Set our initial color scheme to White on Black in case the terminal is
            //in a different color configuration.
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            //This clear counter is used to clear the screen every 10 seconds.
            //This is to make sure we don't have any artifacts left on the 
            //screen after an alarm is cleared.
            int iClear = 0;
            while (true)
            {
                //Refresh every second
                Thread.Sleep(1000);
                
                
                //Refresh the alerts from every connection.  We may want to
                //change this bit to handle a connection being down more
                //gracefully.
                List<GrafanaAlert> lstAlerts = new List<GrafanaAlert>();
                foreach (GrafanaConnection connection in configurationFile.Connections)
                {
                    GrafanaAlert[] alerts = GrafanaApi.GetAlerts(connection);
                    lstAlerts.AddRange(alerts);
                }

                //Star drawing the screen.
                Console.SetCursorPosition(0, 0);

                //Clear the screen every 10 seconds.
                if (iClear == 0) Console.Clear();
                iClear++;
                if (iClear == 10) iClear = 0;


                DrawScreen(lstAlerts);
            }
        }

        /// <summary>
        /// Draws the screen.  Presumes the cursor is already in the correct
        /// location.
        /// </summary>
        /// <param name="lstAlerts">List of all GrafanAlerts</param>
        static void DrawScreen(List<GrafanaAlert> lstAlerts)
        {
            //Store starting console colors.
            ConsoleColor cc = Console.ForegroundColor;
            ConsoleColor bc = Console.BackgroundColor;

            //Create the title line string of the correct size.
            string sTitleLine = Title;
            sTitleLine = new string(' ',(Console.WindowWidth/2)-(sTitleLine.Length/2)) + sTitleLine;
            sTitleLine = sTitleLine.PadRight(Console.WindowWidth,' ');


            //Draw the title line.
            Console.BackgroundColor = cc;
            Console.ForegroundColor = bc;
            Console.WriteLine(sTitleLine);
            Console.WriteLine();
            Console.BackgroundColor = bc;
            Console.ForegroundColor = cc;

            //Draw the alerts.
            foreach (GrafanaAlert alert in lstAlerts)
            {
                //We're assuming if we got an alert, it's a problem.
                //Change our font color to red.
                Console.ForegroundColor = ConsoleColor.Red;

                //If the alert is in the future, change the color to yellow.
                //This may indicate a timezone issue.
                if (alert.startsAt > DateTime.Now) Console.ForegroundColor = ConsoleColor.Yellow;

                //By default, we'll use the description.  If that's not available,
                string Description = alert.annotations.description;
                //we'll use the summary.  If that's not available,
                if (Description == null || Description.Trim() == "")
                {
                    Description = alert.annotations.summary;
                }
                //we'll use the alert name.
                if (Description == null || Description.Trim() == "")
                {
                    Description = alert.labels.alertname;
                }

                //Start creating the line to write.
                string sWrite = $"[{alert.connection.Name}] {Description}";
                
                //Create a string for the time since the alert started.
                string sTimeString = (DateTime.Now - alert.startsAt).ToString(@"dd\.hh\:mm\:ss");
                //Truncate the description if it's too long.
                if (Console.WindowWidth - sWrite.Length < sTimeString.Length)
                {
                    sWrite = sWrite.Substring(0, Console.WindowWidth - sTimeString.Length - 1);
                }
                sWrite = sWrite.PadRight(Console.WindowWidth - sTimeString.Length) + sTimeString;
                //Write out the line to the screen.
                Console.WriteLine(sWrite);

                //Reset the font color.
                Console.ForegroundColor = cc;

            }
        }
    }
}