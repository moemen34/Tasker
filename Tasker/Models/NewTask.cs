namespace Tasker.Models
{
    public class NewTask
    {
       public int AssignerID { get; set; }
        public string[] Assignees { get; set; }
        public string DueDate { get; set; }
    }
}
