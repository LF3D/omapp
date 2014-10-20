using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Runtime;
using System.Workflow.ComponentModel;
using System.Collections.Specialized;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Implements workflow persistence service(file based).
    /// </summary>
	public class FilePersistenceService:WorkflowPersistenceService,IPendingWork
    {
        static readonly object _lock = new object();
        bool unloadOnIdle = true;
        /// <summary>
        /// Gets the directory of workflow persistence.
        /// </summary>
        public string FilePath
        {
            get
            {
                return AspNetConfig.Config["wfPersistenceFile"].ToString();
            }
        }
        /// <summary>
        /// Gets an identification collection of active workflows.
        /// </summary>
        /// <returns></returns>
        public List<Guid> LoadAllActiveWorkflows() {
            if (base.State == WorkflowRuntimeServiceState.Started)
                return XamlConvert.LoadWorkflowIds(FilePath);
            
            throw new InvalidOperationException("workflow file persistence for IMPWorkflow has not started.");
        }
        /// <summary>
        /// create a new instance of <see cref="OperatingManagement.Workflow.FilePersistenceService"/> class.
        /// </summary>
        public FilePersistenceService() { 
            
        }

        #region --Override WorkflowPersistenceService-
        protected override Activity LoadCompletedContextActivity(Guid scopeId, Activity outerActivity)
        {
            byte[] buffer = null;
            XamlConvert.LoadActivity(FilePath, scopeId, out buffer);
            return WorkflowPersistenceService.RestoreFromDefaultSerializedForm(buffer, outerActivity);
        }

        protected override Activity LoadWorkflowInstanceState(Guid instanceId)
        {
            byte[] buffer = null;
            XamlConvert.LoadActivity(FilePath, instanceId, out buffer);
            return WorkflowPersistenceService.RestoreFromDefaultSerializedForm(buffer, null);
        }

        protected override void SaveCompletedContextActivity(Activity activity)
        {
            PendingWorkItem workItem = new PendingWorkItem()
            {
                ItemType = WorkItemType.CompletedScope,
                BinaryData = WorkflowPersistenceService.GetDefaultSerializedForm(activity),
                InstanceId = WorkflowEnvironment.WorkflowInstanceId,
                StateId = (Guid)activity.GetValue(Activity.ActivityContextGuidProperty)
            };
            WorkflowEnvironment.WorkBatch.Add(this, workItem);
        }

        protected override void SaveWorkflowInstanceState(Activity rootActivity, bool unlock)
        {
            if (rootActivity == null)
                throw new ArgumentNullException("rootActivity");

            PendingWorkItem workItem = new PendingWorkItem()
            {
                Status = WorkflowPersistenceService.GetWorkflowStatus(rootActivity),
                IsBlocked = WorkflowPersistenceService.GetIsBlocked(rootActivity),
                Info = WorkflowPersistenceService.GetSuspendOrTerminateInfo(rootActivity),
                StateId = (Guid)rootActivity.GetValue(Activity.ActivityContextGuidProperty),
                Unlock=unlock
            };

            workItem.ItemType = WorkItemType.Instance;
            workItem.InstanceId = WorkflowEnvironment.WorkflowInstanceId;
            byte[] buffer = null;
            if ((workItem.Status != WorkflowStatus.Completed) && (workItem.Status != WorkflowStatus.Terminated))
                buffer = WorkflowPersistenceService.GetDefaultSerializedForm(rootActivity);
            else
                buffer = new byte[0];
            workItem.BinaryData = buffer;
            TimerEventSubscription subscription = ((TimerEventSubscriptionCollection)rootActivity.GetValue(TimerEventSubscriptionCollection.TimerCollectionProperty)).Peek();
            workItem.ExpiresAt = (subscription == null) ? DateTime.MaxValue : ((DateTime)subscription.ExpiresAt);
            if (workItem.Info == null)
                workItem.Info = string.Empty;

            WorkflowEnvironment.WorkBatch.Add(this, workItem);
        }

        protected override bool UnloadOnIdle(Activity activity)
        {
            return this.unloadOnIdle;
        }

        protected override void UnlockWorkflowInstanceState(Activity rootActivity)
        {
            PendingWorkItem workItem = new PendingWorkItem()
            {
                ItemType = WorkItemType.ActivationComplete,
                InstanceId = WorkflowEnvironment.WorkflowInstanceId
            };
            WorkflowEnvironment.WorkBatch.Add(this, workItem);
        }
        #endregion

        #region -IPendingWork members-

        public void Commit(System.Transactions.Transaction transaction, System.Collections.ICollection items)
        {
            foreach (PendingWorkItem item in items)
            {
                switch (item.ItemType)
                {
                    case WorkItemType.Instance:
                        if (item.Status == WorkflowStatus.Completed || item.Status == WorkflowStatus.Terminated)
                            XamlConvert.BakPendingWorkItem(this.FilePath, item.InstanceId);
                        else
                            XamlConvert.SavePendingWorkItem(this.FilePath, item);
                        break;
                    case WorkItemType.CompletedScope:
                        throw new NotSupportedException();
                    case WorkItemType.ActivationComplete:
                        XamlConvert.BakPendingWorkItem(this.FilePath, item.InstanceId);
                        break;
                }
            }
        }

        public void Complete(bool succeeded, System.Collections.ICollection items)
        {
            //throw new NotImplementedException();
        }

        public bool MustCommit(System.Collections.ICollection items)
        {
            return true;
        }

        #endregion
    }
}
