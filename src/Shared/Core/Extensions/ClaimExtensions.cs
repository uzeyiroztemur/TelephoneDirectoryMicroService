using System.Security.Claims;

namespace Core.Extensions
{
    public static class ClaimExtensions
    {
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
        }

        public static void AddPhoneNumber(this ICollection<Claim> claims, string phoneNumber)
        {
            claims.Add(new Claim(ClaimTypes.MobilePhone, phoneNumber));
        }

        public static void AddAvatar(this ICollection<Claim> claims, string avatar)
        {
            claims.Add(new Claim(ClaimTypes.UserData, avatar));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }

        public static void AddCoreRole(this ICollection<Claim> claims, bool hasCore)
        {
            claims.Add(new Claim(ClaimTypes.AuthenticationMethod, hasCore ? "1" : "0"));
        }
    }
}
