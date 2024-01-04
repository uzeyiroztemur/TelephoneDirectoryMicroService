using Core.Extensions;
using Core.Utilities.Filtering.DataTable;

namespace Core.Utilities.Filtering
{
    public class Filter
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public Sort Sort { get; set; }
        public string Search { get; set; }

        public Filter() { }
        public Filter(DataTableOptions options)
        {
            #region Paging
            Skip = options.Start;
            if (options.Length > 0)
            {
                Take = options.Length;
            }
            else
            {
                Take = int.MaxValue;
            }
            #endregion

            #region Sorting
            Sort = new Sort { Field = (!string.IsNullOrEmpty(options.SortColumn) ? options.SortColumn.FirstCharToUpper() : "Id"), Direction = options.SortDesc ? "desc" : "asc" };
            #endregion

            #region Searching
            Search = options.Search;

            #endregion

        }
    }
}