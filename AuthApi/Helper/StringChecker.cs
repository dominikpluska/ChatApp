using System.Text.RegularExpressions;

namespace AuthApi.Helper
{
    public static class StringChecker
    {
        private static readonly string _passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?"":{}|<>]).+$";
        private static readonly string _userNamePattern = @"^[a-zA-Z0-9_]+$";
        private static readonly string _emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public static bool PasswordChecker(this string password)
        {
            Regex regex = new Regex(_passwordPattern);
            return regex.IsMatch(password); 
        }

        public static bool UserNameChecker(this string userName)
        {
            Regex regex = new Regex(_userNamePattern);
            return regex.IsMatch(userName);
        }


        public static bool EmailChecker(this string email)
        {
            Regex regex = new Regex(_emailPattern);
            return regex.IsMatch(email);
        }
    }
}
