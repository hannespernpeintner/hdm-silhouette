using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Silhouette.Engine.Manager
{
    static class DebugLogManager
    {

        public static void writeToLogFile(String message)
        {
            String currentLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            String Directory = System.IO.Path.GetDirectoryName(currentLocation);
            String LogfilePath = System.IO.Path.Combine(Directory, "error.log");

            File.AppendAllText(LogfilePath, System.DateTime.UtcNow.TimeOfDay + ": " + message + Environment.NewLine);
        }
    }
}
