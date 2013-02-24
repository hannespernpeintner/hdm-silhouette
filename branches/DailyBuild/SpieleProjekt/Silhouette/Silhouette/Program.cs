using System;

namespace Silhouette
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameLoop game = new GameLoop())
            {
                for (int i = 0; i < args.LongLength; i++)
                {
                    if (args[i].Equals("--novideo"))
                    {
                        game.parameterNoVideo = true;
                    }
                    if (args[i].Equals("--loadlevel"))
                    {
                        game.parameterLevelToLoad = args[i + 1];
                    }
                }

                game.Run();
            }
        }
    }
#endif
}

