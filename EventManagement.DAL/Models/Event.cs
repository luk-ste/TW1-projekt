namespace EventManagement.DAL.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }= DateTime.UtcNow;
        public int MaxCapacity { get; set; }

        public int OrganizerId { get; set; }
        public User? Organizer { get; set; }

        public int LocationId { get; set; }
        public Location? Location { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Registration> Registrations { get; set; }=new List<Registration>();
        public ICollection<Comment> Comments { get; set; }=new List<Comment>();
    }
}
