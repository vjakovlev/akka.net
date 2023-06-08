using Akka.Actor;
using MovieStreamingFramework.Common.Exceptions;
using MovieStreamingFramework.Common.Messages;
using System;
using System.Collections.Generic;

namespace MovieStreamingFramework.Common.Actors
{
    public class MoviePlayCounterActor : ReceiveActor
    {
        private readonly Dictionary<string, int> _moviePlayCounts;

        public MoviePlayCounterActor()
        {
            _moviePlayCounts = new Dictionary<string, int>();

            Receive<IncrementPlayCountMessage>(message => HandleIncrementMessage(message));
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            if (_moviePlayCounts.ContainsKey(message.MovieTitle))
            {
                _moviePlayCounts[message.MovieTitle]++;
            }
            else 
            {
                _moviePlayCounts.Add(message.MovieTitle, 1);
            }


            // simulate bugs

            if (_moviePlayCounts[message.MovieTitle] > 3)
            {
                throw new SimulatedCorruptStateException();
            }

            if (message.MovieTitle == "Partial Recoil")
            {
                throw new SimulatedTerribleMovieException();
            }


            ColorConsole.WriteLineMagenta(
                $"MoviePlayCounterActor {message.MovieTitle} has been watched {_moviePlayCounts[message.MovieTitle]} times");
        }



        //Actor Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineMagenta("PlaybackStatisticsActor PreStart");
        }
        protected override void PostStop()
        {
            ColorConsole.WriteLineMagenta("PlaybackStatisticsActor PostStop");
        }
        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineMagenta($"PlaybackStatisticsActor PreRestart because: {reason}");
            base.PreRestart(reason, message);
        }
        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineMagenta($"PlaybackStatisticsActor PostRestart because: {reason}");
            base.PostRestart(reason);
        }
    }
}
