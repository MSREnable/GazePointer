//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace WpfCalibrationSample
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Data converter for mapping enumeration values to display names.
    /// </summary>
    [ContentProperty("Items")]
    internal class EnumDisplayNameConverter : IValueConverter
    {
        private ObservableCollection<EnumDisplayNameItem> _items = new ObservableCollection<EnumDisplayNameItem>();

        /// <summary>
        /// Gets the look-up table: enumeration value to resource identifier.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Invoked from Xaml.")]
        public ObservableCollection<EnumDisplayNameItem> Items
        {
            get { return _items; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _items
                .Where(x => x.Value.Equals(value))
                .Select(x => x.DisplayName)
                .FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Represents an (enumeration value, display name) pair. Used by the EnumDisplayNameConverter.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Invoked from Xaml.")]
    internal class EnumDisplayNameItem
    {
        /// <summary>
        /// Gets or sets the enumeration value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }
    }
}
