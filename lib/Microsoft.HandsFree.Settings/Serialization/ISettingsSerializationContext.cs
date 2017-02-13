using Microsoft.HandsFree.Settings.Nudgers;
using System.ComponentModel;

namespace Microsoft.HandsFree.Settings.Serialization
{
    /// <summary>
    /// Interface implemented by objects managing serialization.
    /// </summary>
    public interface ISettingsSerializationContext
    {
        /// <summary>
        /// The object being serialized.
        /// </summary>
        INotifyPropertyChanged Settings { get; }

        /// <summary>
        /// Basic CLR property/XML attribute serialization.
        /// </summary>
        /// <param name="name">Name of property and XML attribute.</param>
        /// <param name="factory">Factory for creating value nudger.</param>
        /// 
        void Serialize(string name, ValueNudgerFactory factory);

        /// <summary>
        /// Serialization of required and expected child object.
        /// </summary>
        /// <param name="name">Name of property containing child reference/XML element</param>
        void SerializeChild(string name);
    }
}
