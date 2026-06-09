namespace VitaOrbitApi.Models
{
    public class Alert
    {
        public int AlertId { get; private set; }
        public int UserId { get; private set; }
        public User? User { get; private set; }
        public int? HealthRecordId { get; private set; }
        public HealthRecord? HealthRecord { get; private set; }
        public int? SymptomRecordId {  get; private set; }
        public SymptomRecord? SymptomRecord { get; private set; }
        public string TypeAlert { get; private set; }
        public string Message { get; private set; }
        public string RiskLevel { get; private set; }
        public DateTime RegisteredAt { get; private set; }


        protected Alert() { }

        public Alert (int userId, string typeAlert, string message, string riskLevel, int? healthRecordId = null, int? symptomRecordId = null)
        {
            UserId = userId;
            TypeAlert = typeAlert;
            Message = message;
            RiskLevel = riskLevel;
            HealthRecordId = healthRecordId;
            SymptomRecordId = symptomRecordId;
            RegisteredAt = DateTime.UtcNow;

        }
    }
}
