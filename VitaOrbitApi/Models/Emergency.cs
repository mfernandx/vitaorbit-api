namespace VitaOrbitApi.Models
{
    public class Emergency
    {
        public int EmergencyId { get; private set; }
        public int UserId { get; private set; }
        public User? User { get; private set; }
        public string Location { get; private set; }
        public string Message { get; private set; }
        public string Status { get; private set; }
        public DateTime RequestDate { get; private set; }

        protected Emergency() { }

        public Emergency (int userId, string location, string message)
        {
            UserId = userId;
            Location = location;
            Message = message;
            Status = "Aberta";
            RequestDate = DateTime.UtcNow;
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }


    }
}
