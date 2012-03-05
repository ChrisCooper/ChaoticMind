using System;

namespace ChaoticMind
{
#if WINDOWS || XBOX
    static class Program
    {

        static ChaoticMindGame _sharedGame;
        public static ChaoticMindGame SharedGame
        {
            get { return _sharedGame; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (_sharedGame = new ChaoticMindGame())
            {
                _sharedGame.Run();
            }
        }
    }
#endif
}

