namespace Tasker.Models
{
    public class NewTask
    {
       public int AssignerID { get; set; }
        public string[] Assignees { get; set; }
        public string TaskTitle { get; set; }

        public DateTime DueDate { get; set; }
    }
}
