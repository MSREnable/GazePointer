using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;

namespace Microsoft.HandsFree.Settings.Serialization
{
    /// <summary>
    /// Class managing the lifetime of a settings object, maintaining the relationship between the XML and the 
    /// </summary>
    /// <typeparam name="T">The type of the root settings object.</typeparam>
    public sealed class SettingsStore<T> : IDisposable
        where T : INotifyPropertyChanged, new()
    {
        static readonly Encoding _xmlEncoding = Encoding.UTF8;

        XmlDocument _document;

        readonly FileSystemWatcher _watcher;

        readonly string _path;

        /// <summary>
        /// Settings have be changed externally.
        /// </summary>
        public event EventHandler Changed;

        SettingsStore(XmlDocument document, T settings, string path, FileSystemWatcher watcher)
        {
            _document = document;
            _settings = settings;
            _path = path;
            _watcher = watcher;

            if (_watcher != null)
            {
                _watcher.Created += OnFileChanged;
                _watcher.Changed += OnFileChanged;
                _watcher.Deleted += OnFileChanged;
                _watcher.Renamed += OnFileChanged;
                _watcher.EnableRaisingEvents = true;
            }
        }

        void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            XmlDocument document;

            try
            {
                var newContent = File.ReadAllText(_path);
                document = new XmlDocument();
                document.LoadXml(newContent);
            }
            catch
            {
                document = null;
            }

            if (document != null)
            {
                SettingsSerializer.MergeXmlDocument(document, _settings);
                _document = document;

                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        SettingsStore(XmlDocument document, T settings)
            : this(document, settings, null, null)
        {
        }

        /// <summary>
        /// The managed settings object.
        /// </summary>
        public T Settings { get { return _settings; } }
        readonly T _settings;

        /// <summary>
        /// Create store from an xml string.
        /// </summary>
        /// <param name="xmlString">The XML.</param>
        /// <returns>The object managing the settings.</returns>
        public static SettingsStore<T> CreateFromXml(string xmlString)
        {
            var document = new XmlDocument();
            document.LoadXml(xmlString);

            var settings = SettingsSerializer.FromXmlDocument<T>(document);
            var store = new SettingsStore<T>(document, settings);
            return store;
        }

        /// <summary>
        /// Create a live settings store attached to a settings file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SettingsStore<T> Create(string path)
        {
            var document = new XmlDocument();

            var directory = Path.GetDirectoryName(path);
            var file = Path.GetFileName(path);
            var watcher = new FileSystemWatcher(directory, file);

            try
            {
                var xml = File.ReadAllText(path, _xmlEncoding);
                document.LoadXml(xml);
            }
            catch
            {
                // Errors can be ignored.
                document = new XmlDocument();
                document.LoadXml("<" + typeof(T).Name + "/>");
            }

            T settings;
            try
            {
                settings = SettingsSerializer.FromXmlDocument<T>(document);
            }
            catch
            {
                settings = SettingsSerializer.CreateDefault<T>();
            }

            var store = new SettingsStore<T>(document, settings, path, watcher);
            return store;
        }

        /// <summary>
        /// Save the current settings.
        /// </summary>
        public void Save()
        {
            var xml = ToXmlString();

            File.WriteAllText(_path, xml, _xmlEncoding);
        }

        /// <summary>
        /// Save the updated settings to an XML string.
        /// </summary>
        /// <returns>The XML string.</returns>
        public string ToXmlString()
        {
            var context = new Rewriter(_document, _document.DocumentElement, Settings);
            Settings.Serialize(context);

            var builder = new StringBuilder();
            var xmlWriterSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                NewLineOnAttributes = true
            };
            using (var writer = XmlWriter.Create(builder, xmlWriterSettings))
            {
                _document.WriteTo(writer);
            }
            var xml = builder.ToString();

            return xml;
        }

        /// <summary>
        /// Reset content to default.
        /// </summary>
        public void Reset()
        {
            SettingsSerializer.ResetDefault(_settings);

            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="isDisposing">Called from Dispose().</param>
        void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _watcher?.Dispose();
            }
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
