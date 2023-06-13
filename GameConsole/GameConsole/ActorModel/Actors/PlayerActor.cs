﻿using System;
using Akka.Actor;
using Akka.Persistence;
using GameConsole.ActorModel.Commands;
using GameConsole.ActorModel.Events;

namespace GameConsole.ActorModel.Actors
{
    class PlayerActor : ReceivePersistentActor
    {
        private readonly string _playerName;
        private int _health;

        public override string PersistenceId => $"player-{_playerName}";

        public PlayerActor(string playerName, int startingHealth)
        {
            _playerName = playerName;
            _health = startingHealth;

            DisplayHelper.WriteLine($"{_playerName} created");

            Command<HitPlayer>(message => HitPlayer(message));
            Command<DisplayStatus>(message => DisplayPlayerStatus());
            Command<SimulateError>(message => SimulateError());

            Recover<PlayerHit>(message => 
            {
                DisplayHelper.WriteLine($"{_playerName} replacing PlayerHit event from event journal");
                _health -= message.DamageTaken;
            });
        }

        private void HitPlayer(HitPlayer command)
        {
            DisplayHelper.WriteLine($"{_playerName} received HitPlayer command");
            var @event = new PlayerHit(command.Damage);

            DisplayHelper.WriteLine($"{_playerName} persist PlayerHit event");
            Persist(@event, playerHitEvent => 
            {
                DisplayHelper.WriteLine($"{_playerName} persisted PlayerHit event ok, updating actor state");
                _health -= command.Damage;
            }); 
        }

        private void DisplayPlayerStatus()
        {
            DisplayHelper.WriteLine($"{_playerName} received DisplayStatus");

            Console.WriteLine($"{_playerName} has {_health} health");
        }

        private void SimulateError()
        {
            DisplayHelper.WriteLine($"{_playerName} received SimulateError");

            throw new ApplicationException($"Simulated exception in player: {_playerName}");
        }
    }
}
