namespace GameConsole.ActorModel.Events
{
    internal class PlayerCreated
    {
        public string PlayerName { get; }

        public PlayerCreated(string playerName)
        {
            PlayerName = playerName;
        }
    }
}
