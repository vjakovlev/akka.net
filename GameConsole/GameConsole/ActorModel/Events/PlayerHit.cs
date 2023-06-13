namespace GameConsole.ActorModel.Events
{
    internal class PlayerHit
    {
        public int DamageTaken { get; }

        public PlayerHit(int damageTaken)
        {
            DamageTaken = damageTaken;
        }
    }
}
