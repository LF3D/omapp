using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Workflow.Runtime;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Implements methods to enabled serialize xoml to file, and load xoml from file(.xaml).
    /// </summary>
	public sealed class XamlConvert
	{
        private XamlConvert() { }
        /// <summary>
        /// Load all workflow identifications from the directory.
        /// </summary>
        /// <param name="filePath">Directory of FilePersistence</param>
        /// <returns></returns>
        public static List<Guid> LoadWorkflowIds(string filePath)
        {
            List<Guid> list = new List<Guid>();
            DirectoryInfo dirInfo = new DirectoryInfo(filePath);
            if (dirInfo.Exists)
            {
                foreach (FileInfo fi in dirInfo.GetFiles("*.xaml"))
                {
                    list.Add(new Guid(Path.GetFileNameWithoutExtension(fi.Name)));
                }
            }

            return list;
        }
        /// <summary>
        /// Save pending workitem to disk.
        /// </summary>
        /// <param name="filePath">Directory of FilePersistence</param>
        /// <param name="item"></param>
        public static void SavePendingWorkItem(string filePath, PendingWorkItem item)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            using (StreamWriter writer = new StreamWriter(Path.Combine(filePath, item.InstanceId.ToString() + ".xaml"), false))
            {
                XmlSerializer xs = new XmlSerializer(item.GetType());
                xs.Serialize(writer, item);
            }
        }
        /// <summary>
        /// Backup pending workitem to disk.
        /// </summary>
        /// <param name="filePath">Directory of FilePersistence</param>
        /// <param name="instanceId">Instance identification of workflow</param>
        public static void BakPendingWorkItem(string filePath, Guid instanceId)
        {
            string fileName = Path.Combine(filePath, instanceId + ".xaml");
            try
            {
                if (File.Exists(fileName))
                    File.Move(fileName, fileName + ".bak");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load pending workitems from disk file.
        /// </summary>
        /// <param name="filePath">Directory of FilePersistence</param>
        /// <param name="instanceId">Instance identification of workflow</param>
        /// <param name="item">Pending workitem</param>
        public static void LoadPendingWorkItem(string filePath, Guid instanceId, out PendingWorkItem item)
        {
            item = null;
            using (StreamReader read = new StreamReader(Path.Combine(filePath, instanceId + ".xaml")))
            {
                XmlSerializer xs = new XmlSerializer(typeof(PendingWorkItem));
                object o = xs.Deserialize(read);
                item = o as PendingWorkItem;
            }
        }
        /// <summary>
        /// Load activity from disk file.
        /// </summary>
        /// <param name="filePath">Directory of FilePersistence</param>
        /// <param name="instanceId">Instance identification of workflow</param>
        /// <param name="buffer">Binary data</param>
        public static void LoadActivity(string filePath, Guid instanceId, out byte[] buffer)
        {
            buffer = new byte[0];
            PendingWorkItem item = null;
            LoadPendingWorkItem(filePath, instanceId, out item);
            if (item != null)
                buffer = item.BinaryData;


        }
	}
}
/// <summary>
/// implements workitem entity.
/// </summary>
[Serializable]
public class PendingWorkItem
{
    /// <summary>
    /// create a new instance of <see cref="OperatingManagement.Workflow.PendingWorkItem"/> class.
    /// </summary>
    public PendingWorkItem() { }

    public bool IsBlocked { get; set; }
    public string Info { get; set; }
    public Guid InstanceId { get; set; }
    public DateTime ExpiresAt { get; set; }

    public byte[] BinaryData { get; set; }
    public Guid StateId { get; set; }
    public WorkflowStatus Status { get; set; }
    public WorkItemType ItemType { get; set; }
    public bool Unlock { get; set; }

}
/// <summary>
/// Implements enumeration for WorkItem type.
/// </summary>
[Serializable]
public enum WorkItemType
{
    /// <summary>
    /// workflow instance.
    /// </summary>
    Instance,
    /// <summary>
    /// scope for activities.
    /// </summary>
    CompletedScope,
    /// <summary>
    /// activation complete state.
    /// </summary>
    ActivationComplete
}
