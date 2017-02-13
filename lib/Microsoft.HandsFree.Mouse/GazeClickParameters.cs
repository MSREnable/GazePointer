namespace Microsoft.HandsFree.Mouse
{
    public class GazeClickParameters
    {
        public uint MouseDownDelay;
        public uint MouseUpDelay;
        public uint RepeatMouseDownDelay;

        public override string ToString()
        {
            return string.Format("MouseDownDelay:{0}, MouseUpDelay:{1}, RepeatMouseDownDelay:{2}",
                MouseDownDelay, MouseUpDelay, RepeatMouseDownDelay);
        }
    }
}
