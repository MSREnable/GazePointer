using Microsoft.HandsFree.MVVM;
using System.Windows;
using System.Windows.Input;

namespace GazePointerTest
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        public static readonly DependencyProperty QuitProperty = DependencyProperty.Register(nameof(Quit), typeof(ICommand), typeof(SettingsWindow),
            new PropertyMetadata(OnPropertyChanged));
        public static readonly DependencyProperty QuitVisibilityProperty = DependencyProperty.Register(nameof(QuitVisibility), typeof(Visibility), typeof(SettingsWindow),
            new PropertyMetadata(Visibility.Hidden));
        public static readonly DependencyProperty RecalibrateProperty = DependencyProperty.Register(nameof(Recalibrate), typeof(ICommand), typeof(SettingsWindow),
            new PropertyMetadata(OnPropertyChanged));
        public static readonly DependencyProperty RecalibrateVisibilityProperty = DependencyProperty.Register(nameof(RecalibrateVisibility), typeof(Visibility), typeof(SettingsWindow),
            new PropertyMetadata(Visibility.Hidden));
        public static readonly DependencyProperty ResetSettingsCommandProperty = DependencyProperty.Register(nameof(ResetSettingsCommand), typeof(ICommand), typeof(SettingsWindow));
        public static readonly DependencyProperty CloseSettingsCommandProperty = DependencyProperty.Register(nameof(CloseSettingsCommand), typeof(ICommand), typeof(SettingsWindow));

        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = AppSettings.Instance;

            Loaded += (s, e) => Owner.IsEnabled = false;
            Unloaded += (s, e) => Owner.IsEnabled = true;

            ResetSettingsCommand = new RelayCommand((p) => { AppSettings.Store.Reset(); });
            CloseSettingsCommand = new RelayCommand((p) =>
            {
                AppSettings.Store.Save();
                Close();
            });
        }

        static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (SettingsWindow)d;
            window.QuitVisibility = window.Quit != null ? Visibility.Visible : Visibility.Hidden;
            window.RecalibrateVisibility = window.Recalibrate != null ? Visibility.Visible : Visibility.Hidden;
        }

        public ICommand Quit { get { return (ICommand)GetValue(QuitProperty); } set { SetValue(QuitProperty, value); } }

        public Visibility QuitVisibility { get { return (Visibility)GetValue(QuitVisibilityProperty); } set { SetValue(QuitVisibilityProperty, value); } }

        public ICommand Recalibrate { get { return (ICommand)GetValue(RecalibrateProperty); } set { SetValue(RecalibrateProperty, value); } }

        public Visibility RecalibrateVisibility { get { return (Visibility)GetValue(RecalibrateVisibilityProperty); } set { SetValue(RecalibrateVisibilityProperty, value); } }

        public ICommand ResetSettingsCommand { get { return (ICommand)GetValue(ResetSettingsCommandProperty); } set { SetValue(ResetSettingsCommandProperty, value); } }

        public ICommand CloseSettingsCommand { get { return (ICommand)GetValue(CloseSettingsCommandProperty); } set { SetValue(CloseSettingsCommandProperty, value); } }
    }
}
