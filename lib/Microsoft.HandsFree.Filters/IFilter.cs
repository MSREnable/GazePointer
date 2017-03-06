using Microsoft.HandsFree.Sensors;

namespace Microsoft.HandsFree.Filters
{
    public interface IFilter
    {
        void Initialize();
        void Terminate();
        GazeEventArgs Update(GazeEventArgs gazeArgs);
    }
}
