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
        public UserForUpsertDTO GetForUpsert(Guid id, Guid currentUserId)
        {
            using var context = new AppDbContext();


            var query = from s in context.Users
                        where (!s.IsDeleted)
                            && (s.Id == id)
                        select new UserForUpsertDTO
                        {
                            Id = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            UserName = s.UserName,
                            IsActive = s.IsActive,
                        };

            return query.AsNoTracking().FirstOrDefault();
        }

        public UserForViewDTO GetByUserName(string userName)
        {
            using var context = new AppDbContext();

            var query = from user in context.Users.Where(w => w.IsActive && !w.IsDeleted)
                        where user.UserName == userName
                        select new UserForViewDTO
                        {
                            Id = user.Id,
                            Name = $"{user.FirstName} {user.LastName}",
                        };

            return query.AsNoTracking().FirstOrDefault();
        }
    }
}
