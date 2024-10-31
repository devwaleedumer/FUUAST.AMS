namespace AMS.MODELS.Filters;

public interface ISortMeta
{
    //
    /// <summary>
    /// field: field name to sort
    /// </summary>
    string field { set; get; }
    //
    /// <summary>
    /// order: accending/descending
    /// </summary>
    int order { set; get; }
    //
}
//
/// <summary>
/// a list sorting item, can be sorted on multiple fields
/// </summary>
public class SortMeta: ISortMeta
{
    //
    /// <summary>
    /// field: field name to sort
    /// </summary>
    public string field { set; get; }
    //
    /// <summary>
    /// order: accending/descending
    /// </summary>
    public int order { set; get; }
    //
}