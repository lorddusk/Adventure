using System.Collections.Generic;
using System.Net.Configuration;
using System.Net.NetworkInformation;

namespace Engine
{
    public class Player : Creature
    {
        public int Gold { get; set; }
        public int XpPoints { get; set; }
        public int Level { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }
        public Location CurrentLocation { get; set; }

        public Player(int currHitPoints, int maxHitPoints, int gold, int xpPoints, int level) : base(currHitPoints,
            maxHitPoints)
        {
            Gold = gold;
            XpPoints = xpPoints;
            Level = level;
            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredtoEnter == null)
                return true;

            foreach (InventoryItem inventoryItem in Inventory)
            {
                if (inventoryItem.Details.ID == location.ItemRequiredtoEnter.ID)
                    return true;
            }

            return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                    return true;
            }

            return false;
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                    return playerQuest.isCompleted;
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem questQuestCompletionItem in quest.QuestCompletionItems)
            {
                bool foundItemInPlayersInventory = false;
                foreach (InventoryItem inventoryItem in Inventory)
                {
                    if (inventoryItem.Details.ID == questQuestCompletionItem.Details.ID)
                    {
                        foundItemInPlayersInventory = true;
                        if (inventoryItem.Amount < questQuestCompletionItem.Amount)
                            return false;
                    }
                }

                if (!foundItemInPlayersInventory)
                    return false;
            }

            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem questQuestCompletionItem in quest.QuestCompletionItems)
            {
                foreach (InventoryItem inventoryItem in Inventory)
                {
                    if (inventoryItem.Details.ID == questQuestCompletionItem.Details.ID)
                    {
                        inventoryItem.Amount -= questQuestCompletionItem.Amount;
                        break;
                    }
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (InventoryItem inventoryItem in Inventory)
            {
                if (inventoryItem.Details.ID == itemToAdd.ID)
                {
                    inventoryItem.Amount++;
                    return;
                }
            }

            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }

        public void MarkQuestCompleted(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Details.ID == quest.ID)
                {
                    playerQuest.isCompleted = true;
                    return;
                }
            }
        }
    }
}