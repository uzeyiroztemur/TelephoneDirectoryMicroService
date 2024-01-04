using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        //Otorasyon iş ihtiyaçlarına dayalıdır. Mesela proje için veritabanına bağlanılacaksa; (o projeye özgü olacaksa) bu business içerisine yazılmalıdır.
        //Veya özel bir veritabanına bağlanılmayacak veya sistemin temel bir Identity altyapısı vardır, o zaman Core'a yazılır.
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var query = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value);
            return query.ToList();
        } 

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }
    }
}
