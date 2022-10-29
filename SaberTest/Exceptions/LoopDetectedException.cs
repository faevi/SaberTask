namespace SaberTest.Exceptions
{
    public class LoopDetectedException : System.Exception
    {
        public int StartPositionOfLoop { get; }

        public LoopDetectedException(string message, int startPositionOfLoop)
            : base (message)
        {
            StartPositionOfLoop = startPositionOfLoop;
        }
    }
}

