using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OperatingManagement.Framework;
using System.Collections.ObjectModel;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Implements tracking service log query functions. 
    /// </summary>
	public sealed class FileTrackingQuery
    {
        /// <summary>
        /// Gets the directory of workflow tracking logs.
        /// </summary>
        public static string TrackingDirectory
        {
            get { return AspNetConfig.Config["wfTrackingFile"].ToString(); }
        }
        /// <summary>
        /// Gtes a collection of all tracked events(name only).
        /// </summary>
        /// <param name="instanceId">Instance identification</param>
        /// <param name="dataKey">Key of key-value pairs, i.e: User Data, Workflow, Activity</param>
        /// <returns></returns>
        public static List<string> GetTrackedWorkflowEvents(Guid instanceId,string dataKey)
        {
            List<string> workflowEvents = new List<string>();

            using (StreamReader sr = File.OpenText(Path.Combine(TrackingDirectory, instanceId + ".xaml")))
            {
                string line;
                // read from the tracking file 
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.Contains(dataKey + ":"))
                    {
                        int nSeparatorIndex = line.IndexOf(":", StringComparison.OrdinalIgnoreCase);
                        if (0 <= nSeparatorIndex)
                        {
                            string eventName = line.Substring(nSeparatorIndex + 1).Trim();
                            workflowEvents.Add(eventName);
                        }
                    }

                }
                sr.Close();
            }
            return workflowEvents;
        }
	}
}
