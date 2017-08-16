//-----------------------------------------------------------------------
// Copyright 2015 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PannableElements
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var random = new Random(DateTime.Now.Millisecond);
            var margin = new Thickness(0, 25, 0, 25);
            var padding = new Thickness(150, 50, 150, 50);

            for (var index = 1; index <= 200; index++)
            {
                // Randomize a color for the button.
                var color = Color.FromRgb(
                    (byte) random.Next(0, 255), 
                    (byte) random.Next(0, 255),
                    (byte) random.Next(0, 255));

                // Add some buttons to the pannable list view.
                PannableListView.Items.Add(new Button
                {
                    Content = string.Concat("This is box number #", index),
                    Margin = margin,
                    Padding = padding,
                    Background = new SolidColorBrush(color),
                    Focusable = false,
                });
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightShift && !e.IsRepeat)
            {
                Console.WriteLine("OnKeyDown");
                var currentApp = Application.Current as App;
                if (currentApp != null) currentApp.EyeXHost.TriggerPanningBegin();
            }
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightShift)
            {
                Console.WriteLine("OnKeyUp");
                var currentApp = Application.Current as App;
                if (currentApp != null) currentApp.EyeXHost.TriggerPanningEnd();
            }
        }
    }
}
