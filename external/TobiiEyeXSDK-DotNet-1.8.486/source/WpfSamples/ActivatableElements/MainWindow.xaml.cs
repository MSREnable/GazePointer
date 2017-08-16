//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace ActivatableElements
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Note_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = e.Source as FrameworkElement;
            if (null == element) { return; }

            var parent = element.TemplatedParent as FrameworkElement;
            if (null == parent) { return; }

            var clickedNote = parent.DataContext as Note;
            ((MainWindowModel)DataContext).ActivateNote(clickedNote);
        }

        private void Note_OnEyeXActivate(object sender, RoutedEventArgs e)
        {
            var element = e.Source as FrameworkElement;
            if (null == element) { return; }

            var clickedNote = element.DataContext as Note;
            ((MainWindowModel)DataContext).ActivateNote(clickedNote);
        }

        private void Button_OnEyeXActivate(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (null == button) { return; }

            if (button.Command != null)
            {
                button.Command.Execute(button);
            }
            else
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button));
            }
        }

        private void RestoreWisdomButton_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowModel)DataContext).RestoreWisdom();
        }

        private void CloseCommand_OnExecuted(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CloseCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightShift)
            {
                var currentApp = Application.Current as App;
                if (currentApp != null) currentApp.EyeXHost.TriggerActivation();
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightShift)
            {
                var currentApp = Application.Current as App;
                if (currentApp != null) currentApp.EyeXHost.TriggerActivationModeOn();
            }
        }
    }
}
