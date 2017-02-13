//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace GazeAwareForms
{
    using EyeXFramework.Forms;
    using System;
    using System.Windows.Forms;

    static class Program
    {
        private static FormsEyeXHost _eyeXHost = new FormsEyeXHost();

        /// <summary>
        /// Gets the singleton EyeX host instance.
        /// </summary>
        public static FormsEyeXHost EyeXHost
        {
            get { return _eyeXHost; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _eyeXHost.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GazeAwareForm());

            _eyeXHost.Dispose();
        }
    }
}
