namespace ClinicService.Models
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string EMail { get; set; }
        public bool Locked { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
    }
}
