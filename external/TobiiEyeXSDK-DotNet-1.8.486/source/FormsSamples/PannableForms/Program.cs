//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Windows.Forms;
using EyeXFramework.Forms;

namespace PannableForms
{
    static class Program
    {
        public static FormsEyeXHost EyeXHost { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (EyeXHost = new FormsEyeXHost())
            {
                // Start the EyeX host.
                EyeXHost.Start();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new PannableForm());   
            }
        }
    }
}
