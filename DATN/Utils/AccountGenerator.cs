namespace DATN.Utils
{
    public class AccountGenerator
    {
        private static readonly Random random = new Random();

        public static (string Account, string Password) GenerateAccountAndRandomPassword(string name, string surname)
        {
            var account = string.Empty;
            if(name == "" && surname == "")
            {
                var surnameWords = surname.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                account = name.ToLower() + string.Concat(surnameWords.Select(word => word[0].ToString().ToLower()));
            }
           
            var password = GenerateRandomPassword();
            
            return (account, password);
        }

        private static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 8) // Change 8 to desired password length
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
