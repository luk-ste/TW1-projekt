namespace EventManagement.DAL.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
