using Akka.Actor;
using System;

namespace MovieStreamingFramework.Remote
{
    internal class Program
    {
        private static ActorSystem MovieStreamingActorSystem;
        static void Main(string[] args)
        {
            //ColorConsole.WrtieLineGray("Creating MovieStreamingSystem in remote process");

            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");

            MovieStreamingActorSystem.AwaitTermination();
        }
    }
}
