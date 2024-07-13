namespace DATN.CustomModels
{
    public class ChangePassword
    {
        public int UserID { get; set; }

        public string OldPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;
    }
}
