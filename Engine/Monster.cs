using System.Collections.Generic;

namespace Engine
{
    public class Monster : Creature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaxDamage { get; set; }
        public int RewardXp { get; set; }
        public int RewardGold { get; set; }
        public List<LootItem> LootTable { get; set; }

        public Monster(int currHitPoints, int maxHitPoints, int id, string name, int maxDamage, int rewardXp,
            int rewardGold) : base(currHitPoints, maxHitPoints)
        {
            ID = id;
            Name = name;
            MaxDamage = maxDamage;
            RewardXp = rewardXp;
            RewardGold = rewardGold;
            LootTable = new List<LootItem>();
        }
    }
}