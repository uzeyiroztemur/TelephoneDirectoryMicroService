namespace Core.Utilities.Filtering.DataTable
{
    public static class DataTableHelper
    {
        public static Filter GetFilter(this DataTableOptions options)
        {
            return new Filter(options);
        }
    }
}
