//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace ActivatableButtonsForms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using EyeXFramework;

    public partial class ActivatableButtonsForm : Form
    {
        const float HueStep = 0.075f;
        const float BrightnessStep = 0.1f;

        public ActivatableButtonsForm()
        {
            KeyPreview = true;
            InitializeComponent();
            UpdateButtonColors();

            // Make the buttons on the form direct clickable using the Activatable behavior.
            Program.EyeXHost.Connect(behaviorMap1);
            behaviorMap1.Add(buttonHueUp, new ActivatableBehavior(OnButtonActivated));
            behaviorMap1.Add(buttonHueDown, new ActivatableBehavior(OnButtonActivated));
            behaviorMap1.Add(buttonBrightnessUp, new ActivatableBehavior(OnButtonActivated));
            behaviorMap1.Add(buttonBrightnessDown, new ActivatableBehavior(OnButtonActivated));

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            Console.WriteLine("OnKeyUp: " + keyEventArgs.KeyCode);

            if (keyEventArgs.KeyCode == Keys.ShiftKey)
            {
                Console.WriteLine("TriggerActivation");
                Program.EyeXHost.TriggerActivation();
            }
            keyEventArgs.Handled = false;
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            // See PannableForms sample for an example how to disregard repeated KeyDown events.
            // We don't bother to do it in this example since most users do not press and hold down
            // the key for long, when clicking.
            Console.WriteLine("OnKeyDown: " + keyEventArgs.KeyCode);
            if (keyEventArgs.KeyCode == Keys.ShiftKey)
            {
                Console.WriteLine("TriggerActivationModeOn");
                Program.EyeXHost.TriggerActivationModeOn();
            }
            keyEventArgs.Handled = false;
        }

        /// <summary>
        /// Event handler invoked when a button is activated.
        /// </summary>
        /// <param name="sender">The control that received the gaze click.</param>
        private void OnButtonActivated(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                Console.WriteLine("OnButtonActivated");
                button.PerformClick();
            }
        }

        private void UpdateButtonColors()
        {
            buttonHueUp.BackColor = ModifyHue(panelColorSample.BackColor, +HueStep);
            buttonHueDown.BackColor = ModifyHue(panelColorSample.BackColor, -HueStep);
            buttonBrightnessUp.BackColor = ModifyBrightness(panelColorSample.BackColor, +BrightnessStep);
            buttonBrightnessDown.BackColor = ModifyBrightness(panelColorSample.BackColor, -BrightnessStep);
        }

        private Color ModifyHue(Color color, float change)
        {
            var hsb = new HsbColor(color);
            hsb.Hue += change;
            return hsb.ToRgb();
        }

        private Color ModifyBrightness(Color color, float change)
        {
            var hsb = new HsbColor(color);
            hsb.Brightness += change;
            return hsb.ToRgb();
        }

        private void buttonHueDown_Click(object sender, EventArgs e)
        {
            panelColorSample.BackColor = ModifyHue(panelColorSample.BackColor, -HueStep);
            UpdateButtonColors();
        }

        private void buttonHueUp_Click(object sender, EventArgs e)
        {
            panelColorSample.BackColor = ModifyHue(panelColorSample.BackColor, +HueStep);
            UpdateButtonColors();
        }

        private void buttonBrightnessUp_Click(object sender, EventArgs e)
        {
            panelColorSample.BackColor = ModifyBrightness(panelColorSample.BackColor, +BrightnessStep);
            UpdateButtonColors();
        }

        private void buttonBrightnessDown_Click(object sender, EventArgs e)
        {
            panelColorSample.BackColor = ModifyBrightness(panelColorSample.BackColor, -BrightnessStep);
            UpdateButtonColors();
        }
    }
}
