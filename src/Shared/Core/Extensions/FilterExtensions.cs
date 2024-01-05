using Core.Utilities.Filtering.Parameters;

namespace Core.Extensions
{
    public static class FilterExtensions
    {
        public static Criter GetCriter(this Utilities.Filtering.Filter filter)
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
                        Dir = filter.Sort.Direction.ToLower() == "asc" ? Utilities.Filtering.SortingDirection.Asc : Utilities.Filtering.SortingDirection.Desc
                    }
                };
            }

            return criter;
        }
    }
}
