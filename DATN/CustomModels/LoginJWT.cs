namespace DATN.CustomModels
{
    public class LoginJWT
    {
        public int UserId { get; set; }

        public string Surname { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string CitizenId { get; set; } = null!;

        public string Role { get; set; }

        public string Account { get; set; } = null!;
        public bool RandomPassword { get; set; }

    }
}
