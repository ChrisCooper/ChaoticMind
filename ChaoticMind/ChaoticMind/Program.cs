using System;

namespace ChaoticMind
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ChaoticMindGame game = new ChaoticMindGame())
            {
                game.Run();
            }
        }
    }
#endif
}

