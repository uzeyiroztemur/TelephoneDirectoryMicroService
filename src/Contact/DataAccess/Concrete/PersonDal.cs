﻿using Core.DataAccess.Concrete.EntityFramework;
using Core.Entities.DTOs;
using Core.Utilities.Filtering;
using DataAccess.Abstract;
using DataAccess.DataContext.EntityFramework.Context;
using Core.Extensions;
using Entities.Concrete;
using Entities.Constants;
using Entities.DTOs.Results;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class PersonDal : EfEntityRepositoryBase<Person, AppDbContext>, IPersonDal
    {
        public async Task<IDataResult<IList<PersonForViewDTO>>> ListAsync(Filter filter)
        {
            using var context = new AppDbContext();

            var query = from person in context.Persons
                        select new PersonForViewDTO
                        {
                            Id = person.Id,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            Company = person.Company,
                            IsActive = person.IsActive,
                            CreatedOn = person.CreatedOn,
                            ModifiedOn = person.ModifiedOn,
                        };

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(w => w.FirstName.ToLower().Contains(filter.Search.ToLower())
                || w.LastName.ToLower().Contains(filter.Search.ToLower())
                || w.Company.ToLower().Contains(filter.Search.ToLower()));

            return await query.AsNoTracking().ToFilteredListAsync(filter.GetCriter());
        }

        public async Task<PersonForPreviewDTO> GetAsync(Guid id)
        {
            using var context = new AppDbContext();

            var query = from person in context.Persons
                        where (person.Id == id)
                        select new PersonForPreviewDTO
                        {
                            Id = person.Id,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            Company = person.Company,
                            IsActive = person.IsActive,
                            CreatedOn = person.CreatedOn,
                            ModifiedOn = person.ModifiedOn,
                            ContactInfos = person.PersonContacts.Select(s => new PersonContactInfoForViewDTO
                            {
                                Id = s.Id,
                                InfoType = s.InfoType,
                                InfoValue = s.InfoValue,
                                IsActive = s.IsActive,
                                CreatedOn = s.CreatedOn,
                                ModifiedOn = s.ModifiedOn,
                            }).ToList(),
                        };

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IList<PersonReportForViewDTO>> ReportAsync()
        {
            using var context = new AppDbContext();

            return await context.Persons
                .SelectMany(x => x.PersonContacts)
                .Where(y => y.InfoType == ContactInfoType.Location)
                .GroupBy(y => y.InfoValue)
                .Select(group => new PersonReportForViewDTO
                {
                    Location = group.Key,
                    PersonCount = context.Persons
                                    .Where(p => p.PersonContacts.Any(pc => pc.InfoType == ContactInfoType.Location && pc.InfoValue == group.Key))
                                    .Count(),
                    PhoneNumberCount = group.SelectMany(g => g.Person.PersonContacts)
                                           .Where(pc => pc.InfoType == ContactInfoType.Phone)
                                           .Count(),
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
