namespace Application.DTOs.Response
{
    public class PagingResponse<T>
    {
        public int CurrentPage { get; set; }// redundant
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; } // redundant
        public ICollection<T> Results { get; set; } = new List<T>();
    }
}