using System;
using System.Threading;
using Akka.Actor;
using MovieStreamingFramework.Common;
using MovieStreamingFramework.Common.Actors;
using MovieStreamingFramework.Common.Messages;

namespace MovieStreamingFramework
{
    internal class Program
    {
        private static ActorSystem MovieStreamingActorSystem;
        static void Main(string[] args)
        {
            //ActorsDemo();
            ActorHierarchy();
        }

        private static void ActorsDemo() 
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
            userActorRef.Tell(new StopMovieMessage(1));


            Console.ReadKey();
            Console.WriteLine("Sending another StopMovieMessage");
            userActorRef.Tell(new StopMovieMessage(1));


            // poison pill will terminate the actor and invoke the PostStop() hook
            //playbackActorRef.Tell(PoisonPill.Instance);

            Console.ReadKey();

            MovieStreamingActorSystem.Shutdown();
            MovieStreamingActorSystem.AwaitTermination();
            Console.WriteLine("Actor System Shutdown");

            Console.ReadKey();
        }
        private static void ActorHierarchy() 
        {
            // creating the system
            ColorConsole.WriteLineGray("Creating MovieStreamingActorSystem");
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");

            // creating actor in the system
            ColorConsole.WriteLineGray("Creating actor supervisory hierarchy");
            MovieStreamingActorSystem.ActorOf(Props.Create<PlaybackActorV2>(), "Playback");

            do
            {
                ShortPause(1);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                ColorConsole.WriteLineGray("enter a command and press enter");

                var command = Console.ReadLine();

                if (command.StartsWith("play")) 
                {
                    var userId = int.Parse(command.Split(',')[1]);
                    var movieTitle = command.Split(',')[2];

                    var message = new PlayMovieMessage(movieTitle, userId);
                    MovieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                }

                if (command.StartsWith("stop")) 
                {
                    var userId = int.Parse(command.Split(',')[1]);
                     
                    var message = new StopMovieMessage(userId);
                    MovieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                }

                if (command == "exit")
                {
                    MovieStreamingActorSystem.Shutdown();
                    MovieStreamingActorSystem.AwaitTermination();
                    ColorConsole.WriteLineGray("Actor system shutdown");
                    Console.ReadKey();
                    Environment.Exit(1);
                }

            } while (true);
        }

        private static void ShortPause(int time) 
        {
            Thread.Sleep(time * 1000);
        }
    }
}
