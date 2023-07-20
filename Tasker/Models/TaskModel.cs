namespace Tasker.Models
{
    public class TaskModel
    {
        public string TaskTitle { get; set; }
        public string Assigner { get; set; }
        public bool Complete { get; set; }
        public DateTime AssignedOn { get; set; }
        public DateTime DueOn { get; set; }
    }
}
