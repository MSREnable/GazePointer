using Microsoft.HandsFree.Settings.Nudgers;
using Microsoft.HandsFree.Settings.Serialization;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.HandsFree.Settings.UI
{
    /// <summary>
    /// Interaction logic for SettingControl.xaml
    /// </summary>
    public partial class SettingControl : UserControl
    {
        public static DependencyProperty NudgerProperty = DependencyProperty.Register("Nudger", typeof(IValueNudger), typeof(SettingControl));

        INotifyPropertyChanged _settings;
        string _propertyName;

        public SettingControl()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        public IValueNudger Nudger { get { return (IValueNudger)GetValue(NudgerProperty); } set { SetValue(NudgerProperty, value); } }

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var expression = GetBindingExpression(DataContextProperty);

            if (expression != null)
            {
                var settings = expression.ResolvedSource as INotifyPropertyChanged;
                var propertyName = expression.ResolvedSourcePropertyName;

                if (settings != _settings || propertyName != _propertyName)
                {
                    _settings = settings;
                    _propertyName = propertyName;

                    if (settings != null && propertyName != null)
                    {
                        var nudger = SettingsSerializer.FindNudger(settings, propertyName);

                        Nudger = nudger;
                    }
                }
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            var panel = VisualParent as StackPanel;
            if (panel != null)
            {
                var index = panel.Children.IndexOf(this);
                Background = index % 2 == 0 ? Brushes.Transparent : (Brush)FindResource("AlternateLine");
            }
        }
    }
}
