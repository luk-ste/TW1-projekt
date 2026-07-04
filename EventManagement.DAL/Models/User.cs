namespace EventManagement.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }




        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
