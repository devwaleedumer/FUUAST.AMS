namespace AMS.MODELS.Filters;

public class PaginationResponse<T>
{
    public PaginationResponse()
    {
        Data = new List<T>();
    }
    public List<T> Data { get; set; }
    public int Total { get; set; }
}