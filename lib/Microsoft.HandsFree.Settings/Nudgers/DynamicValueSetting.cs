namespace Microsoft.HandsFree.Settings.Nudgers
{
    /// <summary>
    /// Environmentally defined setting value. (E.g. Selection of device installed on host.)
    /// </summary>
    public abstract class DynamicValueSetting
    {
        /// <summary>
        /// Stored value for setting.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// User friendly version of setting.
        /// </summary>
        public string ValueString { get; set; }
    }
}
