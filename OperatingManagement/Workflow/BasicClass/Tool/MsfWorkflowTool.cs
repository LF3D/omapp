using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Web.Script.Serialization;
using System.Collections.ObjectModel;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Provides implementation for Microsoft Windows Workflow management.
    /// </summary>
	public class MsfWorkflowTool
	{
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Workflow.MsfWorkflowTool"/> class.
        /// </summary>
        /// <param name="runtime">WorkflowRuntime host.</param>
        public MsfWorkflowTool(WorkflowRuntime runtime)
        {
            this.workflowRuntime = runtime;
        }
        /// <summary>
        /// Register the specified event and cached them.
        /// </summary>
        /// <param name="eventDic">EventHandler Dictionary.</param>
        /// <param name="parameters">WorkflowEventParameters</param>
        public void SetEvents(ref Dictionary<string, EventHandler<MsfWorkflowEventArgs>> eventDic, MsfWorkflowEventParameters parameters)
        {
            eventDic.Clear();
            if (parameters != null)
            {
                if (parameters.WorkflowCompleted != null)
                    eventDic.Add("WorkflowCompleted", parameters.WorkflowCompleted);
                if (parameters.WorkflowTerminated != null)
                    eventDic.Add("WorkflowTerminated", parameters.WorkflowTerminated);
            }
        }
        /// <summary>
        /// This event will be fired when client call the 'ResumeWorkflow' methods.
        /// </summary>
        public event EventHandler<MsfWorkflowEventArgs> OnSaving;

        #region -Properties-
        private WorkflowRuntime workflowRuntime;
        /// <summary>
        /// Gets the name of WorkflowRuntime.
        /// </summary>
        public static string WorkflowRuntimeName {
            get { return "MsfWorkflowRuntime"; }
        }
        #endregion

        #region -Methods-
        /// <summary>
        /// Runs the specified workflow instance, and returns the instance identification.
        /// </summary>
        /// <typeparam name="T">Workflow instance.</typeparam>
        /// <returns></returns>
        public Guid StartWorkflow<T>() where T : class
        {
            ManualWorkflowSchedulerService scheduler = workflowRuntime.GetService<ManualWorkflowSchedulerService>();

            WorkflowInstance workflowInstance = workflowRuntime.CreateWorkflow(typeof(T));
         
            workflowInstance.Start();

            scheduler.RunWorkflow(workflowInstance.InstanceId);

            return workflowInstance.InstanceId;
        }
        /// <summary>
        /// Gets the read-only collection that represents the Activities and 
        /// returns the specified event name by InstanceId and stateName.
        /// </summary>
        /// <param name="instanceId">The specified instance identification of workflow.</param>
        /// <param name="stateName">The name of current state.</param>
        /// <returns></returns>
        public string LoadEventName(Guid instanceId,string stateName) {
            StateMachineWorkflowInstance stateInstance = new StateMachineWorkflowInstance(workflowRuntime, instanceId);
            ReadOnlyCollection<Activity> acts = stateInstance.CurrentState.EnabledActivities;
            if (acts.Count == 0)
                throw new Exception("Can not found the EventDriven under this State.");
            if (!(acts[0] is EventDrivenActivity))
                throw new Exception("The first node under this State was not an EventDrivenActivity.");
            EventDrivenActivity eda = acts[0] as EventDrivenActivity;

            return eda.Name;
        }
        /// <summary>
        /// Gets the collection of nex workflow step.
        /// </summary>
        /// <param name="instanceId">The specified instance identification of workflow.</param>
        /// <returns></returns>
        public List<NextFlowStep> LoadFlowSteps(Guid instanceId)
        {
            StateMachineWorkflowInstance stateInstance = new StateMachineWorkflowInstance(workflowRuntime, instanceId);
            List<NextFlowStep> steps = new List<NextFlowStep>();

            foreach (Activity act in stateInstance.CurrentState.EnabledActivities)
            {
                if (act is EventDrivenActivity)
                {
                    EventDrivenActivity ed = (EventDrivenActivity)act;
                    JavaScriptSerializer jsSrlz = new JavaScriptSerializer();
                    if (ed.EnabledActivities.Count > 0 && ed.EnabledActivities[0] is HandleExternalEventActivity)
                    {
                        string desc = (ed.EnabledActivities[0] as HandleExternalEventActivity).Description;
                        if (string.IsNullOrEmpty(desc))
                            throw new Exception("The first HandleExternalEventActivity of this workflow Activity doesnt set Description[ROLE:VALUE] property.");
                        Dictionary<string, object> keyValues = (Dictionary<string, object>)jsSrlz.DeserializeObject(desc);
                           
                        object value = null;
                        keyValues.TryGetValue("Role",out value);
                        steps.Add(new NextFlowStep()
                        {
                            Name = ed.Name,
                            Text = ed.Description,
                            Role = value.ToString()
                        });
                    }
                }
            }
            return steps;
        }
        /// <summary>
        /// Retrieves the workflow from workflow runtime engine(Persistance) and runs it. 
        /// </summary>
        /// <typeparam name="T">Local service of workflow.</typeparam>
        /// <param name="instanceId">The specified instance identification of workflow.</param>
        /// <param name="eventDrivenName">Name of EventDriven.</param>
        public void ResumeWorkflow<T>(Guid instanceId, string eventDrivenName) where T : IService, new()
        {
            T service = workflowRuntime.GetService<T>();
            ManualWorkflowSchedulerService scheduler = workflowRuntime.GetService<ManualWorkflowSchedulerService>();

            StateMachineWorkflowInstance stateInstance = new StateMachineWorkflowInstance(workflowRuntime, instanceId);
            EventDrivenActivity ed = (EventDrivenActivity)stateInstance.CurrentState.Activities[eventDrivenName];
            HandleExternalEventActivity heeAct = (HandleExternalEventActivity)ed.EnabledActivities[0];

            //save data
            MsfWorkflowEventArgs args = new MsfWorkflowEventArgs()
                                         {
                                             InstanceID = instanceId,
                                             EventName = eventDrivenName,
                                             StateName = stateInstance.CurrentStateName

                                         };
            
            if (OnSaving != null)
            {
                OnSaving(null, args);
            }

            service.RaiseEvent(heeAct.EventName, instanceId);

            scheduler.RunWorkflow(instanceId);
        }
        /// <summary>
        /// Gets the current state by instance identification.
        /// </summary>
        /// <param name="instanceId">The specified instance identification of workflow.</param>
        /// <returns></returns>
        public MsfWorkflowState LoadCurrentState(Guid instanceId)
        {
            StateMachineWorkflowInstance stateInstance = new StateMachineWorkflowInstance(workflowRuntime, instanceId);
            return new MsfWorkflowState()
            {
                StateName = stateInstance.CurrentStateName,
                Description = stateInstance.CurrentState.Description
            };
        }
        /// <summary>
        /// Gets a history collection of workflow.
        /// </summary>
        /// <param name="instanceId">The specified instance identification of workflow.</param>
        /// <param name="dataKey">The specified datakey of key-value pairs.</param>
        /// <returns></returns>
        public List<MsfWorkflowState> LoadHistories(Guid instanceId, string dataKey)
        {
            List<string> query= FileTrackingQuery.GetTrackedWorkflowEvents(instanceId, dataKey);
            StateMachineWorkflowInstance stateInstance = new StateMachineWorkflowInstance(workflowRuntime, instanceId);
            ReadOnlyCollection<StateActivity> states = stateInstance.States;

            List<MsfWorkflowState> impWFStates = new List<MsfWorkflowState>();
            foreach (string s in query)
            {
                var state = states.Where(c => c.Name == s).FirstOrDefault();
                impWFStates.Add(new MsfWorkflowState()
                {
                    StateName = s,
                    Description = state.Description
                });
            }
            return impWFStates;
        }
        /// <summary>
        /// Gets an instanceId collection of all active workflows.
        /// </summary>
        /// <returns></returns>
        public List<Guid> LoadActiveWorkflows()
        {
            FilePersistenceService fps = workflowRuntime.GetService<FilePersistenceService>();
            List<Guid> ids = null;
            if (fps != null)
            {
                ids = fps.LoadAllActiveWorkflows();
            }
            return ids;
        }
        #endregion
    }
}
