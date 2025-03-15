namespace SomerenApp.Models
{
    public class Activity
    {
        public int ActivityID { get; set; }
        public string ActivityName { get; set; } = string.Empty; // Ensure it has a default value
        public DateTime ActivityDateTime { get; set; }
    }
}
