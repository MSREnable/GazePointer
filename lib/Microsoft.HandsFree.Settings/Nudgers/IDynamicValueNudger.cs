namespace Microsoft.HandsFree.Settings.Nudgers
{
    /// <summary>
    /// Nudger for values that are defined by environemnt.
    /// </summary>
    public interface IDynamicValueNudger : IValueNudger
    {
        /// <summary>
        /// The values defined by the environment.
        /// </summary>
        DynamicValueSetting[] Values { get; set; }
    }
}
