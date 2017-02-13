//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace GazeAwareElements
{
    using System.ComponentModel;

    public class MainWindowModel : INotifyPropertyChanged
    {
        private bool _showInstruction;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowModel()
        {
            ShowInstruction = false;
        }

        public string InstructionTeaser
        {
            get { return "Look here for instruction..."; }
        }

        public string Instruction
        {
            get { return "Instruction: The visual elements above respond to your eye-gaze. " +
                         "First, move away the mouse cursor. " +
                         "Now, look at one of the colored surfaces or the 'Hello!', and after a " +
                         "pre-defined delay they will change color from a dim to a clear color. " +
                         "As long as the eye-gaze falls within a child element its parent element " +
                         "will be considered looked at as well. " +
                         "Open MainWindow.xaml to see how it is done. (C)lose instruction. (Q)uit application."; }
        }

        public bool ShowInstruction
        {
            get { return _showInstruction; }
            private set
            {
                if (_showInstruction != value)
                {
                    _showInstruction = value;
                    NotifyPropertyChanged("ShowInstruction");
                }
            }
        }

        public void NotifyInstructionHasGazeChanged(bool hasGaze)
        {
            if (hasGaze)
            {
                ShowInstruction = true;
            }
        }

        public void CloseInstruction()
        {
            ShowInstruction = false;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
