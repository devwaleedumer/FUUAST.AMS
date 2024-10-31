using System.Text.Json.Serialization;

namespace AMS.MODELS.Filters;
    /// <summary>
    /// PrimeNG structure, used by lazy loading feature.
    /// Class LazyLoadEvent ported from PrimeNG to this library.
    /// Generally, populated by the PrimeNG filter feature.
    /// <example>
    /// An example of the JSON:
    /// {"first":0,"rows":3,"sortOrder":1,
    ///   "filters":{"ServerId":{"value":1,"matchMode":"eq"},
    ///     "Mailed":{"value":"false","matchMode":"eq"},
    ///     "Closed":{"value":"false","matchMode":"eq"},
    ///     "Special":{"value":"false","matchMode":"eq"}},
    ///    "globalFilter":null}
    /// </example>
    /// </summary>
    public class LazyLoadEvent
    {
        // {"first":0,"rows":3,"sortOrder":1,
        // "filters":{"ServerId":{"value":1,"matchMode":"eq"},"Mailed":{"value":"false","matchMode":"eq"},"Closed":{"value":"false","matchMode":"eq"},"Special":{"value":"false","matchMode":"eq"}},
        // "globalFilter":null}
        /// <summary>
        /// First record #.
        /// </summary>
        [JsonPropertyName("first")]
        public long First { get; set; }
        /// <summary>
        /// # of rows to return (page size).
        /// </summary>
        [JsonPropertyName("rows")]
        public long Rows { get; set; }
        /// <summary>
        /// Sort field.
        /// </summary>
        [JsonPropertyName("sortField")]
        public string? SortField { get; set; }
        /// <summary>
        /// Ascending or desending sort order.
        /// </summary>
        /// <value> 1 = asc, -1 = desc</value>
        [JsonPropertyName("sortOrder")]
        public int SortOrder { get; set; }
        /// <summary>
        /// multiSortMeta, not implemented.
        /// </summary>
        [JsonPropertyName("multiSortMeta")]
        public object? MultiSortMeta  { get; set; }
        /// <summary>
        /// A dictionary of filters.
        /// Key of the dictionary is the field name, object is value(s)
        /// and match mode.
        /// </summary>
        /// <example>
        /// "filters":{"ServerId":{"value":1,"matchMode":"eq"},
        ///     "Mailed":{"value":"false","matchMode":"eq"},
        ///     "Closed":{"value":"false","matchMode":"eq"},
        ///     "Special":{"value":"false","matchMode":"eq"}},
        /// </example>
        [JsonPropertyName("filters")]
        public Dictionary<string, FilterMetadata>? Filters { get; set; }
        /// <summary>
        /// globalFilter, not implemented.
        /// </summary>
       [JsonPropertyName("globalFilter")]
        public string? GlobalFilter  { get; set; }
       
    }