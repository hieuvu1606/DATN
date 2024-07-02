namespace DATN.Models
{
    public class RegisterUser
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string CitizenId { get; set; }
    }
}
