namespace Engine
{
    public class InventoryItem
    {
        public Item Details { get; set; }
        public int Amount { get; set; }
        
        public InventoryItem(Item details, int amount)
        {
            Details = details;
            Amount = amount;
        }

    }
}