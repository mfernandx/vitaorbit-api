namespace VitaOrbitApi.Models
{
    public class HealthRecord
    {
        public int HealthRecordId { get; private set; }
        public int UserId { get; private set; }
        public User? User { get; private set; }
        public int HeartRate { get; private set; }
        public string BloodPressure { get; private set; }
        public decimal BodyTemperature { get; private set; }
        public int OxygenSaturation { get; private set; }
        public string Mood {  get; private set; }
        public decimal HydrationLevel { get; private set; }
        public decimal SleepHours { get; private set; }
        public string Notes { get; private set; }
        public string RiskClassification { get; private set; }
        public DateTime RegisteredAt { get; private set; }
        public ICollection<Alert> Alerts { get; private set; }


        protected HealthRecord() {

            Alerts = new List<Alert>();

        }

        public HealthRecord(int userId, int heartRate, string bloodPressure, decimal bodyTemperature, int oxygenSaturation, string mood, decimal hydrationLevel, decimal sleepHours, string notes)
        {
            UserId = userId;
            HeartRate = heartRate;
            BloodPressure = bloodPressure;
            BodyTemperature = bodyTemperature;
            OxygenSaturation = oxygenSaturation;
            Mood = mood;
            HydrationLevel = hydrationLevel;
            SleepHours = sleepHours;
            Notes = notes;
            RiskClassification = "Baixo";
            RegisteredAt = DateTime.UtcNow;
            Alerts = new List<Alert>();

        }

        public void UpdateRiskClassification(string riskClassification)
        {
            RiskClassification = riskClassification;
        }

    }
}
