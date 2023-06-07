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
                //message => message.UserId == 422
            );
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message) 
        {
            Console.WriteLine($"Recieved movie title {message.MovieTitle}");
            Console.WriteLine($"Recieved user ID {message.UserId}");
        }

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
