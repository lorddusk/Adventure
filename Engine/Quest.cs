using System.Collections.Generic;

namespace Engine
{
    public class Quest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RewardXp { get; set; }
        public int RewardGold { get; set; }
        public List<QuestCompletionItem> QuestCompletionItems { get; set; }
        public Item RewardItem { get; set; }

        public Quest(int id, string name, string description, int rewardXp, int rewardGold)
        {
            ID = id;
            Name = name;
            Description = description;
            RewardXp = rewardXp;
            RewardGold = rewardGold;
            QuestCompletionItems = new List<QuestCompletionItem>();
        }
    }
}