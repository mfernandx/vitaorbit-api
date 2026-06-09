namespace VitaOrbitApi.Models
{
    public class EnvironmentalCondition
    {
        public int EnvironmentalConditionId { get; private set; }
        public int UserId { get; private set; }
        public User? User { get; private set; }
        public decimal ExternalTemperature { get; private set; }
        public decimal Humidity { get; private set; } 
        public decimal Altitude { get; private set; }
        public decimal AtmosphericPressure { get; private set; }
        public string AirQuality { get; private set; }
        public decimal RadiationLevel { get; private set; }
        public string EnvironmentType { get; private set; }
        public DateTime RegisteredAt { get; private set; }

        protected EnvironmentalCondition() { }

        public EnvironmentalCondition(int userId, decimal externalTemperature, decimal humidity, decimal altitude, decimal atmosphericPressure, string airQuality, decimal radiationLevel,  string environmentType)
        {
            UserId = userId;
            ExternalTemperature = externalTemperature;
            Humidity = humidity;
            Altitude = altitude;
            AtmosphericPressure = atmosphericPressure;
            AirQuality = airQuality;
            RadiationLevel = radiationLevel;
            EnvironmentType = environmentType;
            RegisteredAt = DateTime.UtcNow;
        }

        public void UpdateEnvironmentalCondition(decimal externalTemperature,decimal humidity,decimal altitude,decimal atmosphericPressure,string airQuality,decimal radiationLevel,string environmentType)
        {
            ExternalTemperature = externalTemperature;
            Humidity = humidity;
            Altitude = altitude;
            AtmosphericPressure = atmosphericPressure;
            AirQuality = airQuality;
            RadiationLevel = radiationLevel;
            EnvironmentType = environmentType;
            RegisteredAt = DateTime.UtcNow;
        }


    }
}
