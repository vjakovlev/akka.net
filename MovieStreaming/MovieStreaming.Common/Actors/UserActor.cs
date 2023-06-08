using Akka.Actor;
using MovieStreamingFramework.Common.Messages;
using System;

namespace MovieStreamingFramework.Common.Actors
{
    public class UserActor : ReceiveActor
    {
        private readonly int _userId;
        private string _currentlyWatching;

        public UserActor(int userId)
        {
            _userId = userId;

            //Console.WriteLine("Createing a UserActor");

            //Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message));
            //Receive<StopMovieMessage>(message => HandleStopMovieMessage());

            ColorConsole.WriteLineCyan("Setting initial behaviour to stopped");
            StoppedState();        
        }

        //behaviour
        private void PlayingState() 
        {
            Receive<PlayMovieMessage>(message =>
                ColorConsole.WriteLineRed("Error: cannot start playing another movie before stopping the existing one"));

            Receive<StopMovieMessage>(message => StopPlayingCurrentMovie());

            ColorConsole.WriteLineCyan("UserActor has now become Playing");
        }
        //behaviour
        private void StoppedState() 
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));
            Receive<StopMovieMessage>(message => 
                ColorConsole.WriteLineRed("Error: cannot stop if nothing is playing"));

            ColorConsole.WriteLineCyan("UserActor has now become Stoppd");
        }

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;
            ColorConsole.WriteLineYellow($"User is currently watching {_currentlyWatching}");

            Context.ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter")
                .Tell(new IncrementPlayCountMessage(title));

            Become(PlayingState);
        }
        private void StopPlayingCurrentMovie() 
        {
            ColorConsole.WriteLineYellow($"User has stopped watching {_currentlyWatching}");
            _currentlyWatching = null;

            Become(StoppedState);
        }

        //hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineGreen("UserActor PreStart");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineGreen("UserActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineGreen($"UserActor PreRestart because: {reason}");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineGreen($"PlaybackActor PostRestart because: {reason}");
            base.PostRestart(reason);
        }
    }
}
