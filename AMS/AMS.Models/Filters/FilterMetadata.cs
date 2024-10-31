namespace AMS.MODELS.Filters;

//
/// <summary>
/// Json of interface:
///  {"value":"S","matchMode":"startsWith","operator":"and"}
/// </summary>
public interface IFilterMetadata
{
    //
    /// <summary>
    /// value: any string/numeric/etc
    /// </summary>
    object? Value { set; get; }
    //
    /// <summary>
    /// matchMode: string, with values of:
    ///  gt/lt/equals/lte/gte/notequals/contains/startswith/endswith
    /// </summary>
    string? MatchMode { set; get; }
    //
    /// <summary>
    /// operator: string
    ///  and/or
    /// </summary>
    string? @operator { set; get; }
}
//
/// <summary>
/// Json of class:
///  {"value":"S","matchMode":"startsWith","operator":"and"}
/// </summary>
public class FilterMetadata: IFilterMetadata
{
    //
    /// <summary>
    /// value: any string/numeric/etc
    /// </summary>
    public object? Value { set; get; }
    //
    /// <summary>
    /// matchMode: string, with values of:
    ///  gt/lt/equals/lte/gte/notequals/contains/startswith/endswith
    /// </summary>
    public string? MatchMode { set; get; }
    //
    /// <summary>
    /// operator: string
    ///  and/or
    /// </summary>
    public string? @operator { set; get; }
    //
}