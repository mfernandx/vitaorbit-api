namespace VitaOrbitApi.Models
{
    public class User
    {
        public int UserId { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Gender { get; private set; }
        public string UserDescription { get; private set; }
        public string CurrentLocation { get; private set; }
        public string PhoneNumber { get; private set; }
        public string EmergencyContact { get; private set; }
        public DateTime RegisteredAt { get; private set; }
        public ICollection<HealthRecord> HealthRecords { get; private set; }
        public ICollection<SymptomRecord> SymptomRecords { get; private set; }
        public EnvironmentalCondition? EnvironmentalCondition { get; private set; }
        public ICollection<Emergency> Emergencies { get; private set; }


        protected User() { 

            HealthRecords = new List<HealthRecord>();
            SymptomRecords = new List<SymptomRecord>();
            Emergencies = new List<Emergency>();

        }

        public User (string fullName, string email, string password, DateTime birthDate, string gender, string userDescription, string currentLocation, string phoneNumber, string emergencyContact)
        {

            FullName = fullName;
            Email = email;
            Password = password;
            BirthDate = birthDate;
            Gender = gender;
            UserDescription = userDescription;
            CurrentLocation = currentLocation;
            PhoneNumber = phoneNumber;
            EmergencyContact = emergencyContact;
            RegisteredAt = DateTime.UtcNow;
            HealthRecords = new List<HealthRecord>();
            SymptomRecords = new List<SymptomRecord>();
            Emergencies = new List<Emergency>();

        }

        public void UpdateEmail(string newEmail)
        {
            Email = newEmail;

        }

        public void UpdateCurrentLocation(string newCurrentLocation)
        {
            CurrentLocation = newCurrentLocation;
        }

        public void UpdatePhoneNumber(string newPhoneNumber)
        {
            PhoneNumber = newPhoneNumber;

        }

        public void UpdateEmergencyContact(string newEmergencyContact)
        {
            EmergencyContact = newEmergencyContact;

        }




    }
}
