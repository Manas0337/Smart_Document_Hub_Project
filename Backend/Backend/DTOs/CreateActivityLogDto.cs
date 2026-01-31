namespace Backend.DTOs
{
    public class CreateActivityLogDto
    {
        public string Action { get; set; }
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        public int UserId { get; set; }
    }
}
