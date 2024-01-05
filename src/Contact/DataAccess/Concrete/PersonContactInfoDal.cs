using Core.DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using DataAccess.DataContext.EntityFramework.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class PersonContactInfoDal : EfEntityRepositoryBase<PersonContactInfo, AppDbContext>, IPersonContactInfoDal
    {

    }
}
