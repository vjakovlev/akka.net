using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using MovieStreamingFramework.Messages;

namespace MovieStreamingFramework.Actors
{
    public class PlaybackActor : ReceiveActor //UntypedActor
    {
        public PlaybackActor()
        {
            Console.WriteLine("Creating a PlaybackActor");

            Receive<PlayMovieMessage>(
                message => HandlePlayMovieMessage(message)
                //message => message.UserId == 1
            );
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            ColorConsole.WriteLineYellow(
                $"PlayMovieMessage {message.MovieTitle} for user {message.UserId}");
        }

        //Actor Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineGreen("PlaybackActor PreStart");
        }
        protected override void PostStop()
        {
            ColorConsole.WriteLineGreen("PlaybackActor PostStop");
        }
        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineGreen($"PlaybackActor PreRestart because: {reason}");
            base.PreRestart(reason, message);
        }
        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineGreen($"PlaybackActor PostRestart because: {reason}");
            base.PostRestart(reason);
        }

        //private void HandlePlayMovieMessage(PlayMovieMessage message) 
        //{
        //    Console.WriteLine($"Recieved movie title {message.MovieTitle}");
        //    Console.WriteLine($"Recieved user ID {message.UserId}");
        //}

        //protected override void OnReceive(object message) 
        //{
        //    if (message is PlayMovieMessage)
        //    {
        //        var m = message as PlayMovieMessage;

        //        Console.WriteLine($"Recieved movie title {m.MovieTitle}");
        //        Console.WriteLine($"Recieved user ID {m.UserId}");
        //    }
        //    else 
        //    {
        //        Unhandled(message);
        //    }
        //}

        //protected override void OnReceive(object message)
        //{
        //    if (message is string)
        //    {
        //        Console.WriteLine($"Recieved movie title {message}");
        //    }
        //    else if (message is int)
        //    {
        //        Console.WriteLine($"Recieved user ID {message}");
        //    }
        //    //else if (message is object)
        //    //{
        //    //    Console.WriteLine($"Recieved Object {message}");
        //    //}
        //    else 
        //    {
        //        // possible solution to convert unhandled message in to debug messages
        //        Console.WriteLine($"Unhandled: {message}");
        //        Unhandled(message);
        //    }
        //}
    }
}
