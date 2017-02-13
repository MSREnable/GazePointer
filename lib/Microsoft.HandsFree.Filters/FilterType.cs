using System.ComponentModel;

namespace Microsoft.HandsFree.Filters
{
    public enum FilterType
    {
        [Description("Gain Filter")]
        GainFilter,
        [Description("Stampe Filter")]
        StampeFilter,
        [Description("One Euro Filter")]
        OneEuroFilter,
        [Description("None")]
        NullFilter
    }
}
