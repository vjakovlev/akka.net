using System;
using Akka.Actor;
using MovieStreamingFramework.Actors;
using MovieStreamingFramework.Messages;

namespace MovieStreamingFramework
{
    internal class Program
    {
        private static ActorSystem MovieStreamingActorSystem;
        static void Main(string[] args)
        {
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");
            Console.WriteLine("Actor system created");

            // properties for creating an actor
            Props playbackActorProps = Props.Create<PlaybackActor>();
            Props userActorProps = Props.Create<UserActor>();

            // reference for the actor
            IActorRef playbackActorRef = MovieStreamingActorSystem.ActorOf(playbackActorProps, "PlaybackActor");
            IActorRef userActorRef = MovieStreamingActorSystem.ActorOf(userActorProps, "UserActor");

            //playbackActorRef.Tell("Akka.NET: The movie");
            //playbackActorRef.Tell(42);
            //playbackActorRef.Tell("c");
            //playbackActorRef.Tell('c');
            //playbackActorRef.Tell(new { Id = 1, FirstName = "James", LastName = "Bond" });

            playbackActorRef.Tell(new PlayMovieMessage("Akka.NET: The movie", 42));
            playbackActorRef.Tell(new PlayMovieMessage("Partial Recall", 99));
            playbackActorRef.Tell(new PlayMovieMessage("Boolean Lies", 77));
            playbackActorRef.Tell(new PlayMovieMessage("Codenan the Destroyer", 1));

            Console.ReadKey();
            Console.WriteLine("Sending PlayMovie message (Codenan the Destroyer)");
            userActorRef.Tell(new PlayMovieMessage("Codenan the Destroyer", 42));

            Console.ReadKey();
            Console.WriteLine("Sending PlayMovie message (Boolean Lies)");
            userActorRef.Tell(new PlayMovieMessage("Boolean Lies", 42));

            Console.ReadKey();
            Console.WriteLine("Sending StopMovieMessage");
            userActorRef.Tell(new StopMovieMessage());


            Console.ReadKey();
            Console.WriteLine("Sending another StopMovieMessage");
            userActorRef.Tell(new StopMovieMessage());


            // poison pill will terminate the actor and invoke the PostStop() hook
            //playbackActorRef.Tell(PoisonPill.Instance);

            Console.ReadKey();

            MovieStreamingActorSystem.Shutdown();
            MovieStreamingActorSystem.AwaitTermination();
            Console.WriteLine("Actor System Shutdown");

            Console.ReadKey();
        }
    }
}
