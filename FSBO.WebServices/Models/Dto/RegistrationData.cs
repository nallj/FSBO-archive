namespace FSBO.WebServices.Models.Dto
{
    public class RegistrationData
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public ulong Phone { get; set; }

        public string FirstZip { get; set; }
        public byte DayToCharge { get; set; }

        public string PaymentName { get; set; }
        public string CcName { get; set; }
        public ulong CcNumber { get; set; }
        public byte ExpMonth { get; set; }
        public ushort ExpYear { get; set; }

        public bool ReadPrivacyPolicy { get; set; }
        public bool ReadTermsOfService { get; set; }
    }
}
