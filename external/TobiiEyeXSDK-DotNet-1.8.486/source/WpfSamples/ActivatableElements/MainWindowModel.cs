//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace ActivatableElements
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;
    using System.Windows.Media;

    public class MainWindowModel
    {
        private readonly ObservableCollection<Note> _notes;
        private readonly ICollectionView _notesView;

        public MainWindowModel()
        {
            _notes = CreateNotes();
            _notesView = CollectionViewSource.GetDefaultView(_notes);
        }

        /// <summary>
        /// A collection of notes to be displayed in the sample window.
        /// </summary>
        public ICollectionView Notes
        {
            get { return _notesView; }
        }

        /// <summary>
        /// Removes the activated note.
        /// </summary>
        /// <param name="activatedNote"></param>
        public void ActivateNote(Note activatedNote)
        {
            _notes.Remove(activatedNote);
        }

        /// <summary>
        /// Restores all notes.
        /// </summary>
        public void RestoreWisdom()
        {
            _notes.Clear();
            InitializeNotes(_notes);
        }

        private ObservableCollection<Note> CreateNotes()
        {
            var notes = new ObservableCollection<Note>();
            InitializeNotes(notes);
            return notes;
        }

        private void InitializeNotes(ObservableCollection<Note> notes)
        {
            var id = 0;
            notes.Add(new Note(id++, 2, 2, Colors.Aqua));
            notes.Add(new Note(id++, 3, 3, Colors.Aquamarine));
            notes.Add(new Note(id++, 4, 1, Colors.LightPink));
            notes.Add(new Note(id++, 4, 5, Colors.LightGoldenrodYellow));
            notes.Add(new Note(id++, 5, 4, Colors.Aqua));
            notes.Add(new Note(id++, 3, 7, Colors.Aquamarine));
            notes.Add(new Note(id++, 6, 6, Colors.LightPink));
            notes.Add(new Note(id++, 6, 2, Colors.LightGoldenrodYellow));
            notes.Add(new Note(id++, 2, 6, Colors.Aqua));
            notes.Add(new Note(id++, 1, 6, Colors.LightPink));
        }
    }

    /// <summary>
    /// Represents a note with a quote, and its placement in the sample window.
    /// </summary>
    public class Note
    {
        private readonly List<string> quotes = new List<string>()
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

        public Note(int id, int columnIndex, int rowIndex, Color backgroundColor)
        {
            Id = id;
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            Text = quotes[id];
            BackgroundColor = new SolidColorBrush(backgroundColor);
        }

        public int Id { get; set; }
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public string Text { get; set; }
        public SolidColorBrush BackgroundColor { get; set; }
    }
}
