//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using EyeXFramework;

namespace ActivatableNotesForms
{
    public partial class ActivatableNotesForm : Form
    {
        private readonly List<string> _quotes = new List<string>
        {
            "You miss 100 percent of the shots you never take.\n—Wayne Gretzky",
            "I always wanted to be somebody, but now I realize I should have been more specific.\n—Lily Tomlin",
            "To the man who only has a hammer, everything he encounters begins to look like a nail.\n—Abraham Maslow",
            "I am extraordinarily patient, provided I get my own way in the end.\n—Margaret Thatcher",
            "Those who believe in telekinetics, raise my hand.\n—Kurt Vonnegut",
            "It is more important to know where you are going than to get there quickly.\n—Mabel Newcomber",
            "If you eat a frog first thing in the morning, the rest of your day will be wonderful.\n—Mark Twain",
            "Age is not important unless you're a cheese.\n—Helen Hayes",
            "If you have to eat a frog, don’t look at it for too long.\n—Mark Twain",
            "A thing is mighty big when time and distance cannot shrink it.\n—Zora Neale Hurston",
        };

        private readonly List<Color> _colorsApprovedByTheNotesAuthority = new List<Color>
        {
            Color.LightYellow,
            Color.Lime,
            Color.IndianRed,
            Color.MediumSlateBlue,
            Color.MediumOrchid,
            Color.Linen,
            Color.MintCream
        };

        private readonly Random _random = new Random();

        public ActivatableNotesForm()
        {
            InitializeComponent();

            KeyPreview = true;

            // Make the buttons on the form direct clickable using the Activatable behavior.
            Program.EyeXHost.Connect(behaviorMap1);

            // More wisdom button has activatable behavior and activation focus changed behavior.
            var activatableBehaviorForMoreWisdomButton = new ActivatableBehavior(OnButtonActivated,
                ActivationFocusChangedHandler) {IsTentativeFocusEnabled = true};
            behaviorMap1.Add(moreWisdomButton, activatableBehaviorForMoreWisdomButton);

            // Close button has activateble behavior
            behaviorMap1.Add(closeButton, new ActivatableBehavior(OnButtonActivated));

            AddNotes();
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.ShiftKey)
            {
                Console.WriteLine("TriggerActivation");
                Program.EyeXHost.TriggerActivation();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            // See PannableForms sample for an example how to disregard repeated KeyDown events.
            // We don't bother to do it in this example since most users do not press and hold down
            // the key for long, when clicking.
            if (keyEventArgs.KeyCode == Keys.ShiftKey)
            {
                Console.WriteLine("TriggerActivationModeOn");
                Program.EyeXHost.TriggerActivationModeOn();
            }
        }

        private void ActivationFocusChangedHandler(object sender,
            ActivationFocusChangedEventArgs activationFocusChangedEventArgs)
        {
            var button = sender as Button;
            Debug.Assert(button != null);

            Console.WriteLine();
            Console.WriteLine("Button {0}", button.Name);
            Console.WriteLine("Has activation focus = {0}",
                activationFocusChangedEventArgs.Focus == ActivationFocus.HasActivationFocus);
            Console.WriteLine("Has tentative activation focus = {0}",
                activationFocusChangedEventArgs.Focus == ActivationFocus.HasTentativeActivationFocus);
            Console.WriteLine("Has no activation focus = {0}",
                activationFocusChangedEventArgs.Focus == ActivationFocus.None);
        }

        private void OnButtonActivated(object sender, EventArgs e)
        {
            ((Button) sender).PerformClick();
        }

        private void AddNotes()
        {
            Size size = ClientSize;

            foreach (string quote in _quotes)
            {
                var note = new NoteControl();
                note.Text = quote;
                note.BackColor =
                    _colorsApprovedByTheNotesAuthority[_random.Next(_colorsApprovedByTheNotesAuthority.Count)];
                note.Location = new Point(_random.Next(size.Width - note.Width), _random.Next(size.Height - note.Height));

                Controls.Add(note);
            }
        }

        private void moreWisdomButton_Click(object sender, EventArgs e)
        {
            AddNotes();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
