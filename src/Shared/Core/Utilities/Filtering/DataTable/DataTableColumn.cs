namespace Core.Utilities.Filtering.DataTable
{
    public class DataTableColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Sortable { get; set; }
        public DataTableColumnSearch Search { get; set; }

        public DataTableColumn() { }
    }
}
