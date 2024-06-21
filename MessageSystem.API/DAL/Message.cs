namespace MessageSystem.API.DAL
{
    public class Message
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string Greeting { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } // Nullable for type (A) messages (permanent)
    }
}
