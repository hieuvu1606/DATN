namespace DATN.CustomModels
{
    public class UserBasicInfo
    {
        public UserBasicInfo(int userId, string surname, string name, string email, string phoneNumber, string roleDescr)
        {
            UserId = userId;
            Surname = surname;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            RoleDescr = roleDescr;
        }

        public int UserId { get; set; }

        public string Surname { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string RoleDescr { get; set; }
    }
}
