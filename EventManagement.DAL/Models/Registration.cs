namespace EventManagement.DAL.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }
        public DateTime RegistrationDate { get; set; }= DateTime.UtcNow;
        public bool IsConfirmed { get; set; }
    }
}
