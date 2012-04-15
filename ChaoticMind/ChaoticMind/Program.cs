using System;

namespace ChaoticMind
{
#if WINDOWS || XBOX
    static class Program
    {

        public static ChaoticMindGame SharedGame {
            get;
            set;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SharedGame = new ChaoticMindGame())
            {
                SharedGame.Run();
            }
        }
    }
#endif
}

