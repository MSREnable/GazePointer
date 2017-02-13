namespace Microsoft.HandsFree.MVVM
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    public static class NotifyPropertyChangedHelper
    {
        public static void AttachPropertyChangedAction(this INotifyPropertyChanged container, string propertyName, Action action)
        {
            Debug.Assert(container.GetType().GetProperty(propertyName) != null, "Must provide valid propertyName");

            container.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == null || e.PropertyName == propertyName)
                    {
                        action();
                    }
                };
        }
    }
}
