using Core.DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using DataAccess.DataContext.EntityFramework.Context;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class UserPasswordResetDal : EfEntityRepositoryBase<UserPasswordReset, AppDbContext>, IUserPasswordResetDal
    {
        public async Task<UserPasswordReset> GetLastRequestAsync(Guid userId)
        {
            using var context = new AppDbContext();

            var query = from reset in context.UserPasswordResets
                        join user in context.Users on reset.UserId equals user.Id
                        where reset.CreatedOn <= DateTime.UtcNow && DateTime.UtcNow <= reset.ValidUntil &&
                              user.IsActive &&
                              user.Id == userId
                        orderby reset.CreatedOn descending
                        select reset;

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
