using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime.Tracking;
using System.IO;
using System.Xml.Schema;
using System.Workflow.ComponentModel;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Logs between a tracking service and the run-time tracking infrastructure.
    /// </summary>
    public class FileTrackingService : TrackingService
    {
        private TrackingProfile defaultProfile;

        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Workflow.FileTrackingService"/> class.
        /// </summary>
        public FileTrackingService()
            : base()
        {
            defaultProfile = GetDefaultProfile();
        }

        protected override TrackingChannel GetTrackingChannel(TrackingParameters parameters)
        {
            return new FileTrackingChannel(parameters);
        }

        protected override TrackingProfile GetProfile(Guid workflowInstanceId)
        {
            return defaultProfile;
        }

        protected override TrackingProfile GetProfile(Type workflowType, Version profileVersionId)
        {
            return defaultProfile;
        }

        protected override bool TryGetProfile(Type workflowType, out TrackingProfile profile)
        {
            profile = defaultProfile;
            return true;
        }

        protected override bool TryReloadProfile(Type workflowType, Guid workflowInstanceId,
                                                 out TrackingProfile profile)
        {
            profile = null;
            return false;
        }

        private TrackingProfile GetDefaultProfile()
        {
            TrackingProfile profile = new TrackingProfile();
            WorkflowTrackPoint workflowPoint = new WorkflowTrackPoint();
            List<TrackingWorkflowEvent> workflowEvents = new List<TrackingWorkflowEvent>();
            workflowEvents.AddRange(Enum.GetValues(typeof(TrackingWorkflowEvent))
                        as IEnumerable<TrackingWorkflowEvent>);
            WorkflowTrackingLocation workflowLocation = new WorkflowTrackingLocation(workflowEvents);
            workflowPoint.MatchingLocation = workflowLocation;
            profile.WorkflowTrackPoints.Add(workflowPoint);
            ActivityTrackPoint activityPoint = new ActivityTrackPoint();

            List<ActivityExecutionStatus> activityStatus = new List<ActivityExecutionStatus>();
            activityStatus.AddRange(Enum.GetValues(typeof(ActivityExecutionStatus))
                        as IEnumerable<ActivityExecutionStatus>);
            ActivityTrackingLocation activityLocation = new ActivityTrackingLocation(
                        typeof(Activity), true, activityStatus);
            activityPoint.MatchingLocations.Add(activityLocation);
            profile.ActivityTrackPoints.Add(activityPoint);

            UserTrackPoint userPoint = new UserTrackPoint();
            UserTrackingLocation userLocation = new UserTrackingLocation(
                        typeof(Object), typeof(Activity));
            userLocation.MatchDerivedActivityTypes = true;
            userLocation.MatchDerivedArgumentTypes = true;
            userPoint.MatchingLocations.Add(userLocation);
            profile.UserTrackPoints.Add(userPoint);
            profile.Version = new Version(1, 0, 0);
            return profile;
        }
    }
}
