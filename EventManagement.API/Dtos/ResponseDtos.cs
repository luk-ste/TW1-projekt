namespace EventManagement.API.Dtos
{
    // User DTOs
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class EventResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public int MaxCapacity { get; set; }
        public string OrganizerUsername { get; set; }
        public string LocationName { get; set; }
        public string CategoryName { get; set; }
    }

    public class EventDetailResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public int MaxCapacity { get; set; }
        public string OrganizerUsername { get; set; }
        public string LocationName { get; set; }
        public string CategoryName { get; set; }
        public List<RegistrationResponseDto> Registrations { get; set; } = new();
        public List<CommentResponseDto> Comments { get; set; } = new();
    }

    public class RegistrationResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int EventId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int EventId { get; set; }
    }
}