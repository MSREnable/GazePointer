using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.HandsFree.Settings.Nudgers
{
    /// <summary>
    /// Interface for describing a setting that can be nudged up and down a value.
    /// </summary>
    public interface IValueNudger : INotifyPropertyChanged
    {
        /// <summary>
        /// Description to display to user describing setting.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Display string for value.
        /// </summary>
        string ValueString { get; }

        /// <summary>
        /// Boolean version of value.
        /// </summary>
        bool ValueBool { get; }

        /// <summary>
        /// Visibility of Up/Down interaction.
        /// </summary>
        Visibility UpDownInterfaceVisibility { get; }

        /// <summary>
        /// Visibility of boolean interaction.
        /// </summary>
        Visibility BooleanInterfaceVisibility { get; }

        /// <summary>
        /// Command for incrementing the value.
        /// </summary>
        ICommand NudgeUp { get; }

        /// <summary>
        /// Command for decrementing the value.
        /// </summary>
        ICommand NudgeDown { get; }
    }
}
