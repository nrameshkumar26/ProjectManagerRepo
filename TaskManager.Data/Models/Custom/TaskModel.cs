using System;

namespace ProjectManager.Data.Models.Custom
{
    public class TaskModel
    {
        public int TaskId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public int ProjectId { get; set; }
        public string ParentTask { get; set; }
        public string Task { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Priority { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public bool IsParent { get; set; }
        public int UserId { get; set; }
        public string Project { get; set; }
        public string ManagerId { get; set; }
    }
}
