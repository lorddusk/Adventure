namespace Engine
{
    public class Creature
    {
        public int CurrHitPoints { get; set; }
        public int MaxHitPoints { get; set; }

        public Creature(int currHitPoints, int maxHitPoints)
        {
            CurrHitPoints = currHitPoints;
            MaxHitPoints = maxHitPoints;
        }
    }
}