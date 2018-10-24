namespace TDDMicroExercises.TurnTicketDispenser
{
    // Move to Singelton? http://csharpindepth.com/Articles/General/Singleton.aspx
    public static class TurnNumberSequence
    {
        private static int _turnNumber = 0;
        private static object _lock = new object();

        public static int GetNextTurnNumber()
        {
            lock (_lock)
            {
                return _turnNumber++;
            }
        }
    }
}
