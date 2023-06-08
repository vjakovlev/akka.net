using Akka.Actor;
using MovieStreamingFramework.Common.Messages;
using System;
using System.Collections.Generic;

namespace MovieStreamingFramework.Common.Actors
{
    public class UserCoordinatorActor : ReceiveActor
    {
        private readonly Dictionary<int, IActorRef> _users;

        public UserCoordinatorActor()
        {
            _users = new Dictionary<int, IActorRef>();

            Receive<PlayMovieMessage>(message =>
            {
                CreateChileIfNotExists(message.UserId);

                IActorRef childActorRef = _users[message.UserId];

                childActorRef.Tell(message);
            });

            Receive<StopMovieMessage>(message =>
            {
                CreateChileIfNotExists(message.UserId);

                IActorRef childActorRef = _users[message.UserId];

                childActorRef.Tell(message);
            });
        }

        private void CreateChileIfNotExists(int userId)
        {
            if (!_users.ContainsKey(userId)) 
            {
                IActorRef newChildActorRef =
                   Context.ActorOf(Props.Create(() => new UserActor(userId)), $"User{userId}");

                _users.Add(userId, newChildActorRef);

                ColorConsole.WriteLineCyan(
                    $"UserCoordinatorActor created a new child UserActor for {userId} (Total users: {_users.Count})");
            } 
        }


        //Actor Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineCyan("UserCoordinatorActor PreStart");
        }
        protected override void PostStop()
        {
            ColorConsole.WriteLineCyan("UserCoordinatorActor PostStop");
        }
        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineCyan($"UserCoordinatorActor PreRestart because: {reason}");
            base.PreRestart(reason, message);
        }
        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineCyan($"UserCoordinatorActor PostRestart because: {reason}");
            base.PostRestart(reason);
        }
    }
}
