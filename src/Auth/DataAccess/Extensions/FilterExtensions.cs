using Core.Utilities.Filtering.Parameters;

namespace DataAccess.Extensions
{
    public static class FilterExtensions
    {
        public static Criter GetCriter(this Core.Utilities.Filtering.Filter filter)
        {
            var criter = new Criter
            {
                Skip = filter.Skip,
                Take = filter.Take
            };

            if (filter.Sort != null)
            {
                criter.Sort = new List<Sort>
                {
                    new Sort
                    {
                        Field = filter.Sort.Field,
                        Dir = filter.Sort.Direction.ToLower() == "asc" ? Core.Utilities.Filtering.SortingDirection.Asc : Core.Utilities.Filtering.SortingDirection.Desc
                    }
                };
            }

            return criter;
        }
    }
}
