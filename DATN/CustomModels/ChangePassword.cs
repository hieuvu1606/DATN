namespace DATN.CustomModels
{
    public class ChangePassword
    {
        public string Account { get; set; } = null!;

        public string OldPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;
    }
}
