using Core.DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using DataAccess.DataContext.EntityFramework.Context;
using Entities.Concrete;
using Entities.DTOs.Params;
using Entities.DTOs.Results;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class UserDal : EfEntityRepositoryBase<User, AppDbContext>, IUserDal
    {
        public async Task<UserForUpsertDTO> GetForUpsertAsync(Guid id)
        {
            using var context = new AppDbContext();


            var query = from s in context.Users
                        where (s.Id == id)
                        select new UserForUpsertDTO
                        {
                            Id = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            UserName = s.UserName,
                            IsActive = s.IsActive,
                        };

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<UserForViewDTO> GetByUserNameAsync(string userName)
        {
            using var context = new AppDbContext();

            var query = from user in context.Users.Where(w => w.IsActive)
                        where user.UserName == userName
                        select new UserForViewDTO
                        {
                            Id = user.Id,
                            Name = $"{user.FirstName} {user.LastName}",
                        };

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
