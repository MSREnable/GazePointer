//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace GazeAwareElements
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EyeXFramework.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Message = string.Empty;
        }

        public string Message { get; private set; }

        /// <summary>
        /// Handler for Behavior.HasGazeChanged events for the instruction text block.
        /// </summary>
        private void Instruction_OnHasGazeChanged(object sender, RoutedEventArgs e)
        {
            var textBlock = e.Source as TextBlock;
            if (null == textBlock) { return; }

            var model = (MainWindowModel) DataContext;
            var hasGaze = textBlock.GetHasGaze();
            model.NotifyInstructionHasGazeChanged(hasGaze);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q)
            {
                Close();
            }
            else if (e.Key == Key.C)
            {
                var model = (MainWindowModel) DataContext;
                model.CloseInstruction();
            }
        }

        private void Instruction_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var model = (MainWindowModel) DataContext;
            model.NotifyInstructionHasGazeChanged(true);
        }
    }
}
