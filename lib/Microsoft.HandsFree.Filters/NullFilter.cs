using Microsoft.HandsFree.Sensors;

namespace Microsoft.HandsFree.Filters
{
    public class NullFilter : IFilter
    {
        public void Initialize()
        {
        }

        public void Terminate()
        {

        }

        public GazeEventArgs Update(GazeEventArgs gazeArgs)
        {
            return gazeArgs.Clone();
        }
    }
}
