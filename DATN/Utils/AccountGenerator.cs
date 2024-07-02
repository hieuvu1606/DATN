using DATN.Models;

namespace DATN.Utils
{
    public class AccountGenerator
    {
        private readonly DeviceContext _db;
        public AccountGenerator(DeviceContext db)
        {
            _db = db;
        }

        private static readonly Random random = new Random();

        public static (string Account, string Password) GenerateAccountAndRandomPassword(string name, string surname, DeviceContext _db)
        {
            string account = string.Empty;
            if(name != "" &&  surname != "")
            {
                account = GenerateAccount(name, surname);
                account = EnsureUniqueAccount(account, _db);
            }

            var password = GenerateRandomPassword();

            return (account, password);
        }

        private static string GenerateAccount(string name, string surname)
        {
            var surnameWords = surname.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var account = NormalizeVietnamese(name.ToLower()) + string.Concat(surnameWords.Select(word => NormalizeVietnamese(word[0].ToString().ToLower())));

            return account;
        }

        private static string NormalizeVietnamese(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string[] vietnameseChars = new string[]
            {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
            };

            for (int i = 1; i < vietnameseChars.Length; i++)
            {
                for (int j = 0; j < vietnameseChars[i].Length; j++)
                {
                    input = input.Replace(vietnameseChars[i][j], vietnameseChars[0][i - 1]);
                }
            }

            return input;
        }

        private static string EnsureUniqueAccount(string account, DeviceContext _db)
        {
            var existingAccounts = _db.Users.Where(p => p.Account.StartsWith(account));
            string newAccount = string.Empty;

            if (existingAccounts.Any())
            {
                var latestAccount = existingAccounts.OrderByDescending(u => u.Account).FirstOrDefault();

                var suffix = latestAccount.Account.Substring(account.Length);

                int nextSuffix = 1;
                if (int.TryParse(suffix, out nextSuffix))
                {
                    nextSuffix++;
                }
                else
                {
                    nextSuffix = 1;
                }

                newAccount = $"{account}{nextSuffix}";
            }
            else
            {
                newAccount = account;
            }

            return newAccount;
        }

        private static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}