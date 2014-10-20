using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime.Tracking;
using OperatingManagement.Framework;
using System.IO;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Represents the file tracking chanel.
    /// </summary>
	public class FileTrackingChannel:TrackingChannel
    {
        #region -Constructor-
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Workflow.FileTrackingChannel"/> class.
        /// </summary>
        public FileTrackingChannel() { }
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Workflow.FileTrackingChannel"/> class.
        /// </summary>
        /// <param name="trackingParameters">Contains information about the workflow instance associated with a <see cref="TrackingChannel"/> class.</param>
        public FileTrackingChannel(TrackingParameters trackingParameters)
        {
            this._trackingParameters = trackingParameters;
            this._trackingFile = TrackingDirectory + this._trackingParameters.InstanceId.ToString() + ".xaml";
        }
        #endregion

        #region -Property-
        private TrackingParameters _trackingParameters;
        private string _trackingFile;


        /// <summary>
        /// Gets the directory of workflow tracking logs.
        /// </summary>
        public static string TrackingDirectory
        {
            get { return AspNetConfig.Config["wfTrackingFile"].ToString(); }
        }
        #endregion

        #region -Override-
        protected override void InstanceCompletedOrTerminated()
        {
            WriteToFile("Finished: Workflow is completed or teminated @" + DateTime.Now.ToString());
        }
        protected override void Send(TrackingRecord record)
        {
            if (record is WorkflowTrackingRecord)
            {
                WriteWorkflowTrackingRecord((WorkflowTrackingRecord)record);
            }
            if (record is ActivityTrackingRecord)
            {
                WriteActivityTrackingRecord((ActivityTrackingRecord)record);
            }
            if (record is UserTrackingRecord)
            {
                WriteUserTrackingRecord((UserTrackingRecord)record);
            }
        }
        #endregion

        #region -Private methods-
        private void WriteWorkflowTrackingRecord(WorkflowTrackingRecord workflowTrackingRecord)
        {
            WriteToFile("Workflow: " + workflowTrackingRecord.TrackingWorkflowEvent.ToString());
        }

        private void WriteActivityTrackingRecord(ActivityTrackingRecord activityTrackingRecord)
        {
            WriteToFile("Activity: " + activityTrackingRecord.QualifiedName.ToString() + " " + activityTrackingRecord.ExecutionStatus.ToString());
        }

        private void WriteUserTrackingRecord(UserTrackingRecord userTrackingRecord)
        {
            string key = "User Data";
            if (!string.IsNullOrEmpty(userTrackingRecord.UserDataKey)) {
                key = userTrackingRecord.UserDataKey;
            }
            WriteToFile(key + ": " + userTrackingRecord.UserData.ToString());
        }
        private void WriteToFile(string toWrite)
        {
            try
            {
                StreamWriter sw;
                if (File.Exists(this._trackingFile))
                {
                    sw = File.AppendText(this._trackingFile);
                }
                else
                    sw = File.CreateText(this._trackingFile);
                sw.WriteLine(toWrite);
                sw.Close();
                sw.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}