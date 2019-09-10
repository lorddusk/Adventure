namespace Engine
{
    public class QuestCompletionItem
    {
        public Item Details { get; set; }
        public int Amount { get; set; }

        public QuestCompletionItem(Item details, int amount)
        {
            Details = details;
            Amount = amount;
        }
    }
}