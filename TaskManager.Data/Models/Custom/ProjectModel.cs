using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Data.Models.Custom
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        public string Project { get; set; }
        public int NoOfTasks { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public Nullable<int> Priority { get; set; }
        public int CompletedTasks { get; set; }
        public bool IsActive { get; set; }
        public string ManagerId { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
    }
}
